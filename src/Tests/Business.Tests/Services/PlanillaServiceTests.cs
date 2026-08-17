namespace Business.Tests.Services;

using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Business.Application.DTOs.Rrhh;
using Business.Application.Interfaces;
using Business.Application.Services;
using Business.Domain.Entities.Rrhh;
using Business.Domain.Interfaces;

public class PlanillaServiceTests
{
    private readonly Mock<IRepository<Planilla>> _repo = new();
    private readonly Mock<IRepository<Trabajador>> _trabRepo = new();
    private readonly Mock<IRepository<ConceptoPlanilla>> _conceptoRepo = new();
    private readonly Mock<IRepository<TasaAfp>> _tasaAfpRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ICorrelativoService> _correlativos = new();
    private readonly Mock<IAsientoContableService> _asientos = new();
    private readonly Mock<IParametroService> _parametros = new();
    private readonly Mock<IConfiguracionContableService> _configContable = new();
    private readonly PlanillaService _service;

    public PlanillaServiceTests()
    {
        _repo.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Planilla, bool>>>())).ReturnsAsync(false);
        _repo.Setup(r => r.AddAsync(It.IsAny<Planilla>())).ReturnsAsync((Planilla p) => p);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        _correlativos.Setup(c => c.SiguienteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("PLA-000001");
        _conceptoRepo.Setup(c => c.GetAllAsync()).ReturnsAsync(ConceptosBase());
        _tasaAfpRepo.Setup(t => t.GetAllAsync()).ReturnsAsync(new List<TasaAfp>());
        _asientos.Setup(a => a.GenerarAsientoAsync(
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(),
                It.IsAny<IEnumerable<(string, decimal, decimal)>>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _parametros.Setup(p => p.GetRmvAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1025m);
        _parametros.Setup(p => p.GetTopeAseguradoAfpAsync(It.IsAny<CancellationToken>())).ReturnsAsync(10878m);
        _configContable.Setup(c => c.ObtenerMapaAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Business.Domain.Common.ConceptosContables.Defaults);
        _service = new PlanillaService(_repo.Object, _trabRepo.Object, _conceptoRepo.Object, _tasaAfpRepo.Object, _uow.Object, _correlativos.Object, _asientos.Object, _parametros.Object, _configContable.Object);
    }

    private static List<ConceptoPlanilla> ConceptosBase() => new()
    {
        new() { Codigo = "SUELDO", Nombre = "Sueldo", Tipo = "INGRESO", MetodoCalculo = "MANUAL", AfectaAfp = true, AfectaEssalud = true },
        new() { Codigo = "ASIGFAM", Nombre = "Asig. familiar", Tipo = "INGRESO", MetodoCalculo = "FIJO", MontoFijo = 102.50m, AfectaAfp = true, AfectaEssalud = true },
        new() { Codigo = "ONP", Nombre = "ONP", Tipo = "DESCUENTO", MetodoCalculo = "PORCENTUAL", Porcentaje = 0.13m },
        new() { Codigo = "AFP_FONDO", Nombre = "AFP fondo", Tipo = "DESCUENTO", MetodoCalculo = "PORCENTUAL", Porcentaje = 0.10m },
        new() { Codigo = "AFP_COMISION", Nombre = "AFP comisión", Tipo = "DESCUENTO", MetodoCalculo = "PORCENTUAL", Porcentaje = 0.016m },
        new() { Codigo = "AFP_SEGURO", Nombre = "AFP seguro", Tipo = "DESCUENTO", MetodoCalculo = "PORCENTUAL", Porcentaje = 0.0135m },
        new() { Codigo = "ESSALUD", Nombre = "EsSalud", Tipo = "APORTE", MetodoCalculo = "PORCENTUAL", Porcentaje = 0.09m },
    };

    private void SetTrabajadores(params Trabajador[] t)
        => _trabRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Trabajador, bool>>>())).ReturnsAsync(t.ToList());

    [Fact]
    public async Task Procesar_TrabajadorOnp_CalculaDescuento13yEsSalud9()
    {
        SetTrabajadores(new Trabajador { Id = 1, SueldoBasico = 3000, RegimenPension = "ONP", Estado = "ACTIVO" });

        var p = await _service.ProcesarAsync(2026, 8, "tester");
        var b = p.Boletas.Single();

        b.TotalIngresos.Should().Be(3000);
        b.TotalDescuentos.Should().Be(390);      // 3000 * 13%
        b.TotalAportes.Should().Be(270);         // 3000 * 9%
        b.NetoPagar.Should().Be(2610);           // 3000 - 390
        p.TotalNeto.Should().Be(2610);
    }

    [Fact]
    public async Task Procesar_TrabajadorAfpConAsigFamiliar_SumaTresDescuentos()
    {
        SetTrabajadores(new Trabajador { Id = 2, SueldoBasico = 2000, RegimenPension = "AFP", TieneAsignacionFamiliar = true, Estado = "ACTIVO" });

        var p = await _service.ProcesarAsync(2026, 8, "tester");
        var b = p.Boletas.Single();

        b.TotalIngresos.Should().Be(2102.50m);   // 2000 + 102.50
        // base 2102.50: fondo 210.25 + comisión 33.64 + seguro 28.38 = 272.27
        b.TotalDescuentos.Should().Be(272.27m);
        b.TotalAportes.Should().Be(189.23m);     // 2102.50 * 9% = 189.225 -> 189.23
        b.NetoPagar.Should().Be(1830.23m);
        b.Conceptos.Count(c => c.Tipo == "DESCUENTO").Should().Be(3);
    }

    [Fact]
    public async Task Procesar_TrabajadorConAfpRegistrada_UsaTasaRealDeLaAfp()
    {
        // Integra: comisión 1.55% (en vez del 1.6% genérico del catálogo), seguro 1.35%.
        _tasaAfpRepo.Setup(t => t.GetAllAsync()).ReturnsAsync(new List<TasaAfp>
        {
            new() { Nombre = "Integra", AporteFondo = 0.10m, ComisionFlujo = 0.0155m, PrimaSeguro = 0.0135m }
        });
        SetTrabajadores(new Trabajador { Id = 3, SueldoBasico = 2000, RegimenPension = "AFP", AfpNombre = "Integra", Estado = "ACTIVO" });

        var p = await _service.ProcesarAsync(2026, 8, "tester");
        var b = p.Boletas.Single();

        // base 2000: fondo 200 + comisión 31.00 (2000*1.55%) + seguro 27.00 = 258.00
        b.TotalDescuentos.Should().Be(258.00m);
        var comision = b.Conceptos.Single(c => c.ConceptoCodigo == "AFP_COMISION");
        comision.Monto.Should().Be(31.00m);
    }

    [Fact]
    public async Task Procesar_TrabajadorAfpSueldoSuperaTope_SeguroSeCalculaSoloSobreElTope()
    {
        // Tope 10878: fondo y comisión se calculan sobre el sueldo completo (15000);
        // la prima de seguro se limita al tope.
        _tasaAfpRepo.Setup(t => t.GetAllAsync()).ReturnsAsync(new List<TasaAfp>
        {
            new() { Nombre = "Integra", AporteFondo = 0.10m, ComisionFlujo = 0.0155m, PrimaSeguro = 0.0135m }
        });
        SetTrabajadores(new Trabajador { Id = 4, SueldoBasico = 15000, RegimenPension = "AFP", AfpNombre = "Integra", Estado = "ACTIVO" });

        var p = await _service.ProcesarAsync(2026, 8, "tester");
        var b = p.Boletas.Single();

        var fondo = b.Conceptos.Single(c => c.ConceptoCodigo == "AFP_FONDO");
        var comision = b.Conceptos.Single(c => c.ConceptoCodigo == "AFP_COMISION");
        var seguro = b.Conceptos.Single(c => c.ConceptoCodigo == "AFP_SEGURO");

        fondo.Monto.Should().Be(1500.00m);      // 15000 * 10% (sin tope)
        comision.Monto.Should().Be(232.50m);    // 15000 * 1.55% (sin tope)
        seguro.Monto.Should().Be(146.85m);      // 10878 (tope) * 1.35%, no 15000 * 1.35% = 202.50
    }

    [Fact]
    public async Task Procesar_GeneraAsientoContableDePlanilla()
    {
        SetTrabajadores(new Trabajador { Id = 1, SueldoBasico = 3000, RegimenPension = "ONP", Estado = "ACTIVO" });

        await _service.ProcesarAsync(2026, 8, "tester");

        _asientos.Verify(a => a.GenerarAsientoAsync(
            It.IsAny<DateTime>(), It.IsAny<string>(), "PLANILLA", It.IsAny<string?>(),
            It.IsAny<IEnumerable<(string, decimal, decimal)>>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Procesar_SinTrabajadores_Lanza()
    {
        SetTrabajadores();

        var act = () => _service.ProcesarAsync(2026, 8, "t");

        await act.Should().ThrowAsync<Business.Application.Common.BusinessRuleException>();
    }

    [Fact]
    public async Task Procesar_PeriodoDuplicado_Lanza()
    {
        _repo.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Planilla, bool>>>())).ReturnsAsync(true);
        SetTrabajadores(new Trabajador { Id = 1, SueldoBasico = 1000, RegimenPension = "ONP", Estado = "ACTIVO" });

        var act = () => _service.ProcesarAsync(2026, 8, "t");

        await act.Should().ThrowAsync<Business.Application.Common.BusinessRuleException>().WithMessage("*Ya existe una planilla*");
    }
}

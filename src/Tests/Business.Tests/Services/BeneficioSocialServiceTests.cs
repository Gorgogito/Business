namespace Business.Tests.Services;

using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Business.Application.Interfaces;
using Business.Application.Services;
using Business.Domain.Entities.Rrhh;
using Business.Domain.Interfaces;

public class BeneficioSocialServiceTests
{
    private readonly Mock<IRepository<BeneficioSocial>> _repo = new();
    private readonly Mock<IRepository<Trabajador>> _trabRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IParametroService> _parametros = new();
    private readonly Mock<IAsientoContableService> _asientos = new();
    private readonly Mock<IConfiguracionContableService> _configContable = new();
    private readonly BeneficioSocialService _service;
    private readonly List<BeneficioSocial> _guardados = new();

    public BeneficioSocialServiceTests()
    {
        _repo.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<BeneficioSocial, bool>>>())).ReturnsAsync(false);
        _repo.Setup(r => r.AddAsync(It.IsAny<BeneficioSocial>()))
            .ReturnsAsync((BeneficioSocial b) => { _guardados.Add(b); return b; });
        // CargarQuery().Where(...).ToListAsync() sobre lo guardado en memoria.
        _repo.Setup(r => r.Query()).Returns(() => _guardados.AsQueryable());
        _repo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<BeneficioSocial, bool>>>()))
            .ReturnsAsync((Expression<Func<BeneficioSocial, bool>> pred) => _guardados.Where(pred.Compile()).ToList());
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        _parametros.Setup(p => p.GetRmvAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1025m);
        _asientos.Setup(a => a.GenerarAsientoAsync(
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(),
                It.IsAny<IEnumerable<(string, decimal, decimal)>>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _configContable.Setup(c => c.ObtenerMapaAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Business.Domain.Common.ConceptosContables.Defaults);
        _service = new BeneficioSocialService(_repo.Object, _trabRepo.Object, _uow.Object, _parametros.Object, _asientos.Object, _configContable.Object);
    }

    private void SetTrabajadores(params Trabajador[] t)
        => _trabRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Trabajador, bool>>>())).ReturnsAsync(t.ToList());

    private static Trabajador Trab(decimal sueldo, bool asigFam = false) => new()
    {
        Id = 1, SueldoBasico = sueldo, Estado = "ACTIVO", TieneAsignacionFamiliar = asigFam,
        FechaIngreso = new DateTime(2020, 1, 1), Nombres = "Ana", ApellidoPaterno = "Lopez", ApellidoMaterno = "Diaz"
    };

    [Fact]
    public async Task Gratificacion_SemestreCompleto_UnSueldoMasBonificacion9()
    {
        SetTrabajadores(Trab(3000));

        var r = (await _service.CalcularGratificacionAsync(2026, 1, "t")).Single();

        r.MesesComputables.Should().Be(6);
        r.Monto.Should().Be(3000);                 // 3000 * 6/6
        r.BonificacionExtraordinaria.Should().Be(270); // 9% de 3000
        r.TotalPagar.Should().Be(3270);
    }

    [Fact]
    public async Task Cts_SemestreCompleto_IncluyeUnSextoDeGratificacion()
    {
        SetTrabajadores(Trab(3000));

        var r = (await _service.CalcularCtsAsync(2026, 1, "t")).Single();

        r.RemuneracionComputable.Should().Be(3500);  // 3000 * 7/6
        r.Monto.Should().Be(1750);                    // 3500 * 6/12
    }

    [Fact]
    public async Task Vacaciones_AnioCompleto_UnaRemuneracion()
    {
        SetTrabajadores(Trab(2400));

        var r = (await _service.CalcularVacacionesAsync(2026, "t")).Single();

        r.MesesComputables.Should().Be(12);
        r.Monto.Should().Be(2400);
    }

    [Fact]
    public async Task Gratificacion_GeneraAsientoDeProvisionPorElTotalAPagar()
    {
        SetTrabajadores(Trab(3000));

        await _service.CalcularGratificacionAsync(2026, 1, "t");

        // Monto 3000 + bonificación 270 = 3270 (gasto por beneficios / beneficios por pagar).
        _asientos.Verify(a => a.GenerarAsientoAsync(
            It.IsAny<DateTime>(), It.IsAny<string>(), "BENEFICIOS", It.IsAny<string?>(),
            It.Is<IEnumerable<(string, decimal, decimal)>>(lineas =>
                lineas.Sum(l => l.Item2) == 3270m && lineas.Sum(l => l.Item3) == 3270m),
            It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task RegistrarPago_GeneraAsientoDePagoYMarcaComoPagado()
    {
        SetTrabajadores(Trab(2400));
        var creado = (await _service.CalcularVacacionesAsync(2026, "t")).Single();

        var pagado = await _service.RegistrarPagoAsync(creado.Id, "EFECTIVO", "t");

        pagado!.EstadoPago.Should().Be("PAGADO");
        pagado.FechaPago.Should().NotBeNull();
        _asientos.Verify(a => a.GenerarAsientoAsync(
            It.IsAny<DateTime>(), It.IsAny<string>(), "PAGO", It.IsAny<string?>(),
            It.Is<IEnumerable<(string, decimal, decimal)>>(l => l.Sum(x => x.Item2) == 2400m && l.Sum(x => x.Item3) == 2400m),
            It.IsAny<string?>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task RegistrarPago_YaPagado_Lanza()
    {
        SetTrabajadores(Trab(2400));
        var creado = (await _service.CalcularVacacionesAsync(2026, "t")).Single();
        await _service.RegistrarPagoAsync(creado.Id, "EFECTIVO", "t");

        var act = () => _service.RegistrarPagoAsync(creado.Id, "EFECTIVO", "t");

        await act.Should().ThrowAsync<Business.Application.Common.BusinessRuleException>().WithMessage("*ya fue pagado*");
    }

    [Fact]
    public async Task Calcular_PeriodoDuplicado_Lanza()
    {
        _repo.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<BeneficioSocial, bool>>>())).ReturnsAsync(true);
        SetTrabajadores(Trab(1000));

        var act = () => _service.CalcularGratificacionAsync(2026, 1, "t");

        await act.Should().ThrowAsync<Business.Application.Common.BusinessRuleException>().WithMessage("*Ya se calculó*");
    }
}

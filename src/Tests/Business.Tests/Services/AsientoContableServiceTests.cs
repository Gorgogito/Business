namespace Business.Tests.Services;

using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Business.Application.Common;
using Business.Application.DTOs.Contabilidad;
using Business.Application.Interfaces;
using Business.Application.Services;
using Business.Domain.Entities.Contabilidad;
using Business.Domain.Interfaces;

public class AsientoContableServiceTests
{
    private readonly Mock<IRepository<AsientoContable>> _repo = new();
    private readonly Mock<IRepository<CuentaContable>> _cuentaRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ICorrelativoService> _correlativos = new();
    private readonly AsientoContableService _service;

    public AsientoContableServiceTests()
    {
        _repo.Setup(r => r.AddAsync(It.IsAny<AsientoContable>())).ReturnsAsync((AsientoContable a) => a);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        _correlativos.Setup(c => c.SiguienteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("ASI-00000001");
        _service = new AsientoContableService(_repo.Object, _cuentaRepo.Object, _uow.Object, _correlativos.Object);
    }

    private void SetCuentas(params CuentaContable[] cuentas)
        => _cuentaRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<CuentaContable, bool>>>())).ReturnsAsync(cuentas.ToList());

    private static CreateAsientoContableDto Asiento(params (int cuenta, decimal debe, decimal haber)[] lineas) => new()
    {
        Fecha = DateTime.UtcNow, Glosa = "prueba",
        Detalles = lineas.Select(l => new CreateAsientoDetalleDto { CuentaContableId = l.cuenta, Debe = l.debe, Haber = l.haber }).ToList()
    };

    [Fact]
    public async Task Create_AsientoCuadrado_Registra()
    {
        SetCuentas(
            new CuentaContable { Id = 1, Codigo = "121", EsMovimiento = true },
            new CuentaContable { Id = 2, Codigo = "701", EsMovimiento = true });

        var result = await _service.CreateAsync(Asiento((1, 118, 0), (2, 0, 118)), "tester");

        result.TotalDebe.Should().Be(118);
        result.TotalHaber.Should().Be(118);
        result.Numero.Should().Be("ASI-00000001");
        _repo.Verify(r => r.AddAsync(It.IsAny<AsientoContable>()), Times.Once);
    }

    [Fact]
    public async Task Create_NoCuadra_Lanza()
    {
        SetCuentas(
            new CuentaContable { Id = 1, EsMovimiento = true },
            new CuentaContable { Id = 2, EsMovimiento = true });

        var act = () => _service.CreateAsync(Asiento((1, 100, 0), (2, 0, 118)), "t");

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*no cuadra*");
    }

    [Fact]
    public async Task Create_LineaConDebeYHaber_Lanza()
    {
        SetCuentas(new CuentaContable { Id = 1, EsMovimiento = true }, new CuentaContable { Id = 2, EsMovimiento = true });

        var act = () => _service.CreateAsync(Asiento((1, 50, 50), (2, 0, 0)), "t");

        await act.Should().ThrowAsync<BusinessRuleException>();
    }

    [Fact]
    public async Task Create_CuentaNoMovimiento_Lanza()
    {
        SetCuentas(
            new CuentaContable { Id = 1, Codigo = "12", Nombre = "Agrupadora", EsMovimiento = false },
            new CuentaContable { Id = 2, EsMovimiento = true });

        var act = () => _service.CreateAsync(Asiento((1, 118, 0), (2, 0, 118)), "t");

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*no admite movimientos*");
    }

    [Fact]
    public async Task Create_UnaSolaLinea_Lanza()
    {
        SetCuentas(new CuentaContable { Id = 1, EsMovimiento = true });

        var act = () => _service.CreateAsync(Asiento((1, 100, 0)), "t");

        await act.Should().ThrowAsync<BusinessRuleException>().WithMessage("*al menos dos*");
    }
}

namespace Business.Tests.Services;

using FluentAssertions;
using Moq;
using Business.Application.DTOs.Ventas;
using Business.Application.Interfaces;
using Business.Application.Services;
using Business.Domain.Entities.Inventario;
using Business.Domain.Entities.Ventas;
using Business.Domain.Interfaces;

/// <summary>
/// Verifica que los servicios de documentos toman su número del servicio de correlativos
/// (numeración segura) en lugar de esquemas basados en Count()+1 o Random().
/// </summary>
public class CorrelativoUsageTests
{
    private static Mock<ICorrelativoService> CorrelativoMock(string valorDevuelto)
    {
        var mock = new Mock<ICorrelativoService>();
        mock.Setup(c => c.SiguienteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(valorDevuelto);
        return mock;
    }

    private static Mock<IInventarioService> InventarioMock()
    {
        var mock = new Mock<IInventarioService>();
        mock.Setup(i => i.RegistrarMovimientoAsync(
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<decimal>(),
                It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MovimientoInventario());
        return mock;
    }

    private static Mock<IParametroService> ParametrosMock()
    {
        var mock = new Mock<IParametroService>();
        mock.Setup(p => p.GetIgvRateAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0.18m);
        return mock;
    }

    private static Mock<ICuentaPorCobrarService> CxcMock() => new();

    private static Mock<IAsientoContableService> AsientosMock()
    {
        var mock = new Mock<IAsientoContableService>();
        mock.Setup(a => a.GenerarAsientoAsync(
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(),
                It.IsAny<IEnumerable<(string, decimal, decimal)>>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        return mock;
    }

    private static Mock<IConfiguracionContableService> ConfiguracionContableMock()
    {
        var mock = new Mock<IConfiguracionContableService>();
        mock.Setup(c => c.ObtenerMapaAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Business.Domain.Common.ConceptosContables.Defaults);
        return mock;
    }

    [Fact]
    public async Task Cotizacion_CreateAsync_UsaNumeroDeCorrelativo()
    {
        var repo = new Mock<IRepository<Cotizacion>>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.AddAsync(It.IsAny<Cotizacion>())).ReturnsAsync((Cotizacion c) => c);
        uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        var correlativos = CorrelativoMock("COT-00000042");
        var parametros = ParametrosMock();
        var service = new CotizacionService(repo.Object, uow.Object, correlativos.Object, parametros.Object);

        var dto = new CreateCotizacionDto
        {
            ClienteId = 1,
            FechaVencimiento = DateTime.UtcNow.AddDays(15),
            Detalles = new List<CreateCotizacionDetalleDto>
            {
                new() { ProductoId = 1, Cantidad = 2, PrecioUnitario = 100, Descuento = 0 }
            }
        };

        var result = await service.CreateAsync(dto, "tester");

        result.Numero.Should().Be("COT-00000042");
        correlativos.Verify(c => c.SiguienteAsync("COTIZACION", "COT", 1, default), Times.Once);
    }

    [Fact]
    public async Task Factura_CreateAsync_Factura_UsaSerieF001()
    {
        var repo = new Mock<IRepository<Factura>>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.AddAsync(It.IsAny<Factura>())).ReturnsAsync((Factura f) => f);
        uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        var correlativos = CorrelativoMock("00000007");
        var inventario = InventarioMock();
        var pedidoRepo = new Mock<IRepository<Pedido>>();
        var parametros = ParametrosMock();
        var cxc = CxcMock();
        var asientos = AsientosMock();
        var configContable = ConfiguracionContableMock();
        var service = new FacturaService(repo.Object, pedidoRepo.Object, uow.Object, correlativos.Object, inventario.Object, parametros.Object, cxc.Object, asientos.Object, configContable.Object);

        var dto = new CreateFacturaDto
        {
            ClienteId = 1,
            TipoDocumento = "FACTURA",
            Detalles = new List<CreateFacturaDetalleDto>
            {
                new() { ProductoId = 1, Cantidad = 1, PrecioUnitario = 100, Descuento = 0 }
            }
        };

        var result = await service.CreateAsync(dto, "tester");

        result.Serie.Should().Be("F001");
        result.Numero.Should().Be("00000007");
        correlativos.Verify(c => c.SiguienteAsync("FACTURA", "F001", 1, default), Times.Once);
    }

    [Fact]
    public async Task Factura_CreateAsync_Boleta_UsaSerieB001()
    {
        var repo = new Mock<IRepository<Factura>>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.AddAsync(It.IsAny<Factura>())).ReturnsAsync((Factura f) => f);
        uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        var correlativos = CorrelativoMock("00000003");
        var inventario = InventarioMock();
        var pedidoRepo = new Mock<IRepository<Pedido>>();
        var parametros = ParametrosMock();
        var cxc = CxcMock();
        var asientos = AsientosMock();
        var configContable = ConfiguracionContableMock();
        var service = new FacturaService(repo.Object, pedidoRepo.Object, uow.Object, correlativos.Object, inventario.Object, parametros.Object, cxc.Object, asientos.Object, configContable.Object);

        var dto = new CreateFacturaDto
        {
            ClienteId = 1,
            TipoDocumento = "BOLETA",
            Detalles = new List<CreateFacturaDetalleDto>
            {
                new() { ProductoId = 1, Cantidad = 1, PrecioUnitario = 100, Descuento = 0 }
            }
        };

        var result = await service.CreateAsync(dto, "tester");

        result.Serie.Should().Be("B001");
        result.Numero.Should().Be("00000003");
        correlativos.Verify(c => c.SiguienteAsync("BOLETA", "B001", 1, default), Times.Once);
    }

    [Fact]
    public async Task Factura_CreateAsync_DescuentaStockDeCadaDetalle()
    {
        var repo = new Mock<IRepository<Factura>>();
        var uow = new Mock<IUnitOfWork>();
        repo.Setup(r => r.AddAsync(It.IsAny<Factura>())).ReturnsAsync((Factura f) => f);
        uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        var correlativos = CorrelativoMock("00000010");
        var inventario = InventarioMock();
        var pedidoRepo = new Mock<IRepository<Pedido>>();
        var parametros = ParametrosMock();
        var cxc = CxcMock();
        var asientos = AsientosMock();
        var configContable = ConfiguracionContableMock();
        var service = new FacturaService(repo.Object, pedidoRepo.Object, uow.Object, correlativos.Object, inventario.Object, parametros.Object, cxc.Object, asientos.Object, configContable.Object);

        var dto = new CreateFacturaDto
        {
            ClienteId = 1,
            AlmacenId = 1,
            TipoDocumento = "FACTURA",
            Detalles = new List<CreateFacturaDetalleDto>
            {
                new() { ProductoId = 5, Cantidad = 3, PrecioUnitario = 100, Descuento = 0 },
                new() { ProductoId = 8, Cantidad = 2, PrecioUnitario = 50, Descuento = 0 }
            }
        };

        await service.CreateAsync(dto, "tester");

        // Cada detalle genera una salida de stock validando disponibilidad.
        inventario.Verify(i => i.RegistrarMovimientoAsync(
            "SALIDA", 5, 1, 3m, 100m, It.IsAny<string?>(), It.IsAny<string?>(), "tester", true, It.IsAny<CancellationToken>()), Times.Once);
        inventario.Verify(i => i.RegistrarMovimientoAsync(
            "SALIDA", 8, 1, 2m, 50m, It.IsAny<string?>(), It.IsAny<string?>(), "tester", true, It.IsAny<CancellationToken>()), Times.Once);
    }
}

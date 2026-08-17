namespace Business.Tests.Services;

using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Business.Application.Common;
using Business.Application.Services;
using Business.Domain.Entities.Inventario;
using Business.Domain.Interfaces;

/// <summary>
/// Verifica la lógica central de movimientos de inventario: ajuste de stock en entradas
/// y salidas, validación de disponibilidad y creación de stock inexistente.
/// </summary>
public class InventarioServiceTests
{
    private readonly Mock<IRepository<MovimientoInventario>> _movRepo = new();
    private readonly Mock<IRepository<Stock>> _stockRepo = new();
    private readonly InventarioService _service;

    public InventarioServiceTests()
    {
        _movRepo.Setup(r => r.AddAsync(It.IsAny<MovimientoInventario>())).ReturnsAsync((MovimientoInventario m) => m);
        _stockRepo.Setup(r => r.AddAsync(It.IsAny<Stock>())).ReturnsAsync((Stock s) => s);
        _service = new InventarioService(_movRepo.Object, _stockRepo.Object);
    }

    private void SetStock(Stock? stock)
    {
        var lista = stock is null ? new List<Stock>() : new List<Stock> { stock };
        _stockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Stock, bool>>>())).ReturnsAsync(lista);
    }

    [Fact]
    public async Task Salida_ConStockSuficiente_Decrementa()
    {
        var stock = new Stock { ProductoId = 1, AlmacenId = 1, CantidadActual = 10 };
        SetStock(stock);

        var mov = await _service.RegistrarMovimientoAsync("SALIDA", 1, 1, 4, 100, "ref", null, "u", validarDisponibilidad: true);

        stock.CantidadActual.Should().Be(6);
        mov.Tipo.Should().Be("SALIDA");
        mov.Cantidad.Should().Be(4);
    }

    [Fact]
    public async Task Salida_ConStockInsuficiente_LanzaExcepcion()
    {
        SetStock(new Stock { ProductoId = 1, AlmacenId = 1, CantidadActual = 2 });

        var act = async () => await _service.RegistrarMovimientoAsync("SALIDA", 1, 1, 5, 100, "ref", null, "u", validarDisponibilidad: true);

        await act.Should().ThrowAsync<BusinessRuleException>();
    }

    [Fact]
    public async Task Salida_SinValidar_PermiteNegativo()
    {
        var stock = new Stock { ProductoId = 1, AlmacenId = 1, CantidadActual = 2 };
        SetStock(stock);

        await _service.RegistrarMovimientoAsync("SALIDA", 1, 1, 5, 100, "ref", null, "u", validarDisponibilidad: false);

        stock.CantidadActual.Should().Be(-3);
    }

    [Fact]
    public async Task Entrada_Incrementa()
    {
        var stock = new Stock { ProductoId = 1, AlmacenId = 1, CantidadActual = 10 };
        SetStock(stock);

        await _service.RegistrarMovimientoAsync("ENTRADA", 1, 1, 7, 50, "ref", null, "u", validarDisponibilidad: false);

        stock.CantidadActual.Should().Be(17);
    }

    [Fact]
    public async Task Entrada_SinStockPrevio_CreaStock()
    {
        SetStock(null);

        await _service.RegistrarMovimientoAsync("ENTRADA", 9, 1, 5, 50, "ref", null, "u", validarDisponibilidad: false);

        _stockRepo.Verify(r => r.AddAsync(It.Is<Stock>(s => s.ProductoId == 9 && s.CantidadActual == 5)), Times.Once);
    }

    [Fact]
    public async Task Entrada_RecalculaCostoPromedioPonderado()
    {
        // 10 uds a 100 (promedio 100) + 10 uds a 200 => promedio (1000+2000)/20 = 150
        var stock = new Stock { ProductoId = 1, AlmacenId = 1, CantidadActual = 10, CostoPromedio = 100 };
        SetStock(stock);

        await _service.RegistrarMovimientoAsync("ENTRADA", 1, 1, 10, 200, "ref", null, "u", validarDisponibilidad: false);

        stock.CantidadActual.Should().Be(20);
        stock.CostoPromedio.Should().Be(150m);
    }

    [Fact]
    public async Task Salida_ValorizaAlCostoPromedio_SinCambiarlo()
    {
        var stock = new Stock { ProductoId = 1, AlmacenId = 1, CantidadActual = 20, CostoPromedio = 150 };
        SetStock(stock);

        var mov = await _service.RegistrarMovimientoAsync("SALIDA", 1, 1, 5, 999, "ref", null, "u", validarDisponibilidad: true);

        stock.CantidadActual.Should().Be(15);
        stock.CostoPromedio.Should().Be(150m);      // la salida no altera el promedio
        mov.CostoUnitario.Should().Be(150m);        // se valoriza al promedio, no al precio pasado
        mov.CostoTotal.Should().Be(750m);
    }

    [Fact]
    public async Task Entrada_PrimerIngreso_FijaCostoPromedioAlPrecio()
    {
        SetStock(null);

        await _service.RegistrarMovimientoAsync("ENTRADA", 9, 1, 4, 80, "ref", null, "u", validarDisponibilidad: false);

        _stockRepo.Verify(r => r.AddAsync(It.Is<Stock>(s => s.CostoPromedio == 80m && s.CantidadActual == 4)), Times.Once);
    }
}

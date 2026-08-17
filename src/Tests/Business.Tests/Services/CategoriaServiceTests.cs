namespace Business.Tests.Services;

using FluentAssertions;
using Moq;
using Business.Application.DTOs.Maestros;
using Business.Application.Services;
using Business.Domain.Entities.Maestros;
using Business.Domain.Interfaces;

public class CategoriaServiceTests
{
    private readonly Mock<IRepository<Categoria>> _repoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly CategoriaService _service;

    public CategoriaServiceTests()
    {
        _repoMock = new Mock<IRepository<Categoria>>();
        _uowMock = new Mock<IUnitOfWork>();
        _service = new CategoriaService(_repoMock.Object, _uowMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCategorias()
    {
        var categorias = new List<Categoria>
        {
            new() { Id = 1, Nombre = "Electrónicos", IsActive = true },
            new() { Id = 2, Nombre = "Oficina", IsActive = true }
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categorias);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_CreatesCategoria()
    {
        var dto = new CreateCategoriaDto { Nombre = "Tecnología", Descripcion = "Productos tecnológicos" };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Categoria>())).ReturnsAsync((Categoria c) => c);
        _uowMock.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _service.CreateAsync(dto);

        result.Nombre.Should().Be("Tecnología");
        result.Descripcion.Should().Be("Productos tecnológicos");
    }

    [Fact]
    public async Task UpdateAsync_NonExisting_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Categoria?)null);

        var result = await _service.UpdateAsync(999, new CreateCategoriaDto { Nombre = "X" });

        result.Should().BeNull();
    }
}

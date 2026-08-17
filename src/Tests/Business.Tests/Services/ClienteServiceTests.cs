namespace Business.Tests.Services;

using FluentAssertions;
using Moq;
using Business.Application.DTOs.Maestros;
using Business.Application.Services;
using Business.Domain.Entities.Maestros;
using Business.Domain.Interfaces;

public class ClienteServiceTests
{
    private readonly Mock<IRepository<Cliente>> _repoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly ClienteService _service;

    public ClienteServiceTests()
    {
        _repoMock = new Mock<IRepository<Cliente>>();
        _uowMock = new Mock<IUnitOfWork>();
        _service = new ClienteService(_repoMock.Object, _uowMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllClientes()
    {
        var clientes = new List<Cliente>
        {
            new() { Id = 1, RazonSocial = "Empresa A", RUC = "20000000001", IsActive = true },
            new() { Id = 2, RazonSocial = "Empresa B", RUC = "20000000002", IsActive = true }
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(clientes);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.First().RazonSocial.Should().Be("Empresa A");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsCliente()
    {
        var cliente = new Cliente { Id = 1, RazonSocial = "Test SA", RUC = "20000000001", IsActive = true };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cliente);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.RazonSocial.Should().Be("Test SA");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Cliente?)null);

        var result = await _service.GetByIdAsync(99);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsCreatedCliente()
    {
        var dto = new CreateClienteDto
        {
            RazonSocial = "Nueva Empresa SAC",
            RUC = "20123456789",
            Email = "test@test.com",
            Telefono = "999999999",
            TipoDocumento = "RUC",
            LimiteCredito = 10000
        };

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Cliente>())).ReturnsAsync((Cliente c) => c);
        _uowMock.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.RazonSocial.Should().Be("Nueva Empresa SAC");
        result.RUC.Should().Be("20123456789");
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Cliente>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_ReturnsTrue()
    {
        var cliente = new Cliente { Id = 1, IsActive = true };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cliente);
        _uowMock.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _service.DeleteAsync(1);

        result.Should().BeTrue();
        cliente.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ReturnsFalse()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Cliente?)null);

        var result = await _service.DeleteAsync(99);

        result.Should().BeFalse();
    }
}

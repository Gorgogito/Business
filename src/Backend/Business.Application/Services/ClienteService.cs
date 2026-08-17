namespace Business.Application.Services;

using Business.Application.DTOs.Maestros;
using Business.Application.Interfaces;
using Business.Domain.Entities.Maestros;
using Business.Domain.Interfaces;

public class ClienteService : IClienteService
{
    private readonly IRepository<Cliente> _repo;
    private readonly IUnitOfWork _unitOfWork;

    public ClienteService(IRepository<Cliente> repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ClienteDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<ClienteDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<ClienteDto> CreateAsync(CreateClienteDto dto)
    {
        var entity = new Cliente
        {
            RazonSocial = dto.RazonSocial, RUC = dto.RUC,
            Direccion = dto.Direccion, Telefono = dto.Telefono,
            Email = dto.Email, TipoDocumento = dto.TipoDocumento,
            LimiteCredito = dto.LimiteCredito, IsActive = true, CreatedAt = DateTime.UtcNow
        };
        await _repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<ClienteDto?> UpdateAsync(int id, CreateClienteDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;
        entity.RazonSocial = dto.RazonSocial; entity.RUC = dto.RUC;
        entity.Direccion = dto.Direccion; entity.Telefono = dto.Telefono;
        entity.Email = dto.Email; entity.LimiteCredito = dto.LimiteCredito;
        entity.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        entity.IsActive = false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static ClienteDto MapToDto(Cliente c) => new()
    {
        Id = c.Id, RazonSocial = c.RazonSocial, RUC = c.RUC,
        Direccion = c.Direccion, Telefono = c.Telefono, Email = c.Email,
        TipoDocumento = c.TipoDocumento, LimiteCredito = c.LimiteCredito, IsActive = c.IsActive
    };
}

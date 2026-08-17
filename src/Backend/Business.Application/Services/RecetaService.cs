namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Produccion;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Produccion;
using Business.Domain.Interfaces;

public class RecetaService : IRecetaService
{
    private readonly IRepository<Receta> _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICorrelativoService _correlativos;

    public RecetaService(IRepository<Receta> repo, IUnitOfWork unitOfWork, ICorrelativoService correlativos)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _correlativos = correlativos;
    }

    public async Task<IEnumerable<RecetaDto>> GetAllAsync()
    {
        var items = await CargarQuery().OrderBy(r => r.Codigo).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<RecetaDto?> GetByIdAsync(int id)
    {
        var item = await CargarQuery().FirstOrDefaultAsync(r => r.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<RecetaDto?> GetByProductoAsync(int productoId)
    {
        var item = await CargarQuery().FirstOrDefaultAsync(r => r.ProductoId == productoId && r.Estado == EstadoReceta.Activa);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<RecetaDto> CreateAsync(CreateRecetaDto dto, string userName)
    {
        ValidarDetalles(dto);
        if (await _repo.ExistsAsync(r => r.ProductoId == dto.ProductoId && r.Estado == EstadoReceta.Activa))
            throw new BusinessRuleException("El producto ya tiene una receta activa.");
        if (dto.CantidadProducida <= 0)
            throw new BusinessRuleException("La cantidad producida debe ser mayor a cero.");

        var codigo = await _correlativos.SiguienteAsync("RECETA", "RCT");
        var receta = new Receta
        {
            Codigo = codigo, ProductoId = dto.ProductoId, Descripcion = dto.Descripcion,
            CantidadProducida = dto.CantidadProducida, Estado = EstadoReceta.Activa,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };
        foreach (var d in dto.Detalles)
            receta.Detalles.Add(new RecetaDetalle { InsumoId = d.InsumoId, Cantidad = d.Cantidad, IsActive = true, CreatedAt = DateTime.UtcNow });

        await _repo.AddAsync(receta);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(receta);
    }

    public async Task<RecetaDto?> UpdateAsync(int id, CreateRecetaDto dto, string userName)
    {
        var receta = await CargarQuery().FirstOrDefaultAsync(r => r.Id == id);
        if (receta == null) return null;
        ValidarDetalles(dto);
        if (dto.CantidadProducida <= 0)
            throw new BusinessRuleException("La cantidad producida debe ser mayor a cero.");

        receta.Descripcion = dto.Descripcion;
        receta.CantidadProducida = dto.CantidadProducida;
        receta.UpdatedAt = DateTime.UtcNow; receta.UpdatedBy = userName;

        // Reemplaza los insumos.
        foreach (var det in receta.Detalles.ToList())
            det.IsActive = false;
        foreach (var d in dto.Detalles)
            receta.Detalles.Add(new RecetaDetalle { InsumoId = d.InsumoId, Cantidad = d.Cantidad, IsActive = true, CreatedAt = DateTime.UtcNow });

        await _unitOfWork.SaveChangesAsync();
        return MapToDto(receta);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var receta = await _repo.GetByIdAsync(id);
        if (receta == null) return false;
        receta.IsActive = false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static void ValidarDetalles(CreateRecetaDto dto)
    {
        if (dto.Detalles.Count == 0)
            throw new BusinessRuleException("La receta debe tener al menos un insumo.");
        if (dto.Detalles.Any(d => d.Cantidad <= 0))
            throw new BusinessRuleException("La cantidad de cada insumo debe ser mayor a cero.");
    }

    private IQueryable<Receta> CargarQuery() =>
        _repo.Query().Include(r => r.Producto).Include(r => r.Detalles).ThenInclude(d => d.Insumo);

    private static RecetaDto MapToDto(Receta r) => new()
    {
        Id = r.Id, Codigo = r.Codigo, ProductoId = r.ProductoId, ProductoNombre = r.Producto?.Nombre,
        Descripcion = r.Descripcion, CantidadProducida = r.CantidadProducida, Estado = r.Estado,
        Detalles = r.Detalles.Where(d => d.IsActive).Select(d => new RecetaDetalleDto
        {
            Id = d.Id, InsumoId = d.InsumoId, InsumoNombre = d.Insumo?.Nombre, Cantidad = d.Cantidad
        }).ToList()
    };
}

namespace Business.Application.Interfaces;

using Business.Application.DTOs.Rrhh;

public interface IConceptoPlanillaService
{
    Task<IEnumerable<ConceptoPlanillaDto>> GetAllAsync();
    Task<ConceptoPlanillaDto?> GetByIdAsync(int id);
    Task<ConceptoPlanillaDto> CreateAsync(CreateConceptoPlanillaDto dto, string userName);
    Task<ConceptoPlanillaDto?> UpdateAsync(int id, CreateConceptoPlanillaDto dto, string userName);
    Task<bool> DeleteAsync(int id);
}

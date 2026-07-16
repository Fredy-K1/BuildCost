using Shared.Contracts.DTOs.Propuestas;
using Shared.Contracts.Entidades;

namespace OrderService.Interfaces
{
    public interface IPropuestaRepository
    {
        Task<IEnumerable<Propuesta>> GetbyProyectoIdAsync(int proyectoId);
        Task<Propuesta?> GetByIdAsync(int propuestaId);
        Task<Propuesta> CreateAsync(Propuesta propuesta);
        Task<Propuesta?> UpdateAsync(int propuestaId, UpdatePropuestaDto updatePropuestaDto);
        
        Task<bool> DeleteAsync(int propuestaId);
    }
}

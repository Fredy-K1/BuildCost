using Shared.Contracts.DTOs.Propuestas;

namespace OrderService.Interfaces
{
    public interface IPropuestaService
    {
        Task<IEnumerable<PropuestaDto>> GetByContratistaIdAsync(Guid contratistaId);
        Task<IEnumerable<PropuestaDto>> GetByProyectoIdAsync(int proyectoId);
        Task<PropuestaDto?> GetByIdAsync(int propuestaId);
        Task<PropuestaDto> CreateAsync(CreatePropuestaDto createPropuestaDto);
        Task<PropuestaDto?> UpdateAsync(int propuestaId, UpdatePropuestaDto updatePropuestaDto);
        Task<bool> DeleteAsync(int propuestaId);
        Task<PropuestaDto?> ResponderPropuestaAsync(int id, bool aceptada);
    }
}

using Shared.Contracts.DTOs.Projec;
using Shared.Contracts.Enums;

namespace OrderService.Interfaces
{
    public interface IProyectoService
    {
        Task<IEnumerable<ProyectoDto>> GetAllProyectosAsync(Guid userId);
        Task<ProyectoDto?> GetProyectoByIdAsync(int id, Guid userId);
        Task<ProyectoDto> CreateAsync(CreateProyectoDto dto);
        Task<ProyectoDto?> UpdateAsync(int id, UpdateProyectoDto dto);
        Task<ProyectoDto?> DeleteAsync(int id);
        Task<ProyectoDto?> CambiarEstadoAsync(int id, ProjectStatus estado, Guid userId);
    }
}


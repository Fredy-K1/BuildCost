using Shared.Contracts.DTOs.Projec;

namespace OrderService.Interfaces
{
    public interface IProyectoService
    {
        Task<IEnumerable<ProyectoDto>> GetAllProyectosAsync();
        Task<ProyectoDto?> GetProyectoByIdAsync(int id);
        Task<ProyectoDto> CreateAsync(CreateProyectoDto dto);
        Task<ProyectoDto?> UpdateAsync(int id, UpdateProyectoDto dto);
        Task<ProyectoDto?> DeleteAsync(int id);


    }
}

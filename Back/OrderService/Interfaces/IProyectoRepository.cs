using Shared.Contracts.DTOs.Projec;
using Shared.Contracts.Entidades;
using Shared.Contracts.Enums;

namespace OrderService.Interfaces
{
    public interface IProyectoRepository
    {
        Task<IEnumerable<Proyecto>> GetByUserIdAsync(Guid userId);
        Task<Proyecto?> GetAsyncID(int id);
        Task<Proyecto?> CreateAsync(Proyecto proyecto);
        Task<Proyecto?> UpdateAsync(int id, UpdateProyectoDto dto);
        Task<Proyecto?> DeleteAsync(int id);
        Task<Proyecto?> UpdateEstadoAsync(int id, ProjectStatus estado);
    }
}

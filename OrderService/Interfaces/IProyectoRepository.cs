using Shared.Contracts.DTOs.Projec;
using Shared.Contracts.Entidades;

namespace OrderService.Interfaces
{
    public interface IProyectoRepository
    {
        Task<IEnumerable<Proyecto>> GetAllAsync();
        Task<Proyecto?> GetAsyncID(int id);
        Task<Proyecto?> CreateAsync(Proyecto proyecto);
        Task<Proyecto?> UpdateAsync(int id, UpdateProyectoDto dto);
        Task<Proyecto?> DeleteAsync(int id);
    }
}

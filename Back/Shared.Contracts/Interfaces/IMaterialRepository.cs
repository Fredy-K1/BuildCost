using Shared.Contracts.Entidades;

namespace Shared.Contracts.Interfaces
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> GetAllByContratistaAsync(Guid contratistaId);

        Task<Material?> GetByIdAsync(int id,Guid contratistaId);

        Task<Material> CreateAsync(Material material);

        Task<Material?> UpdateAsync(int id,Guid contratistaId,Material material);

        Task<bool> DeleteAsync(int id,Guid contratistaId);
    }
}
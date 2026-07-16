

using Shared.Contracts.Entidades;

namespace Shared.Contracts.Interfaces
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> GetAll();
        Task<Material?> GetById(int id);
        Task<Material?> Create(Material material);
        Task<Material?> Update(Material material);
        Task<bool> DeleteById(int id);

    }
}

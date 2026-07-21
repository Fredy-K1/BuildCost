using Shared.Contracts.Entidades;

namespace Shared.Contracts.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllByContratistaAsync(
            Guid contratistaId);

        Task<Product?> GetByIdAsync(int id,Guid contratistaId);

        Task<Product> CreateAsync(Product product);

        Task<Product?> UpdateAsync(int id,Guid contratistaId,Product product);

        Task<bool> DeleteAsync(int id,Guid contratistaId);
    }
}
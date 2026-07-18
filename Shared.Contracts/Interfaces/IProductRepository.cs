

using Shared.Contracts.Entidades;

namespace Shared.Contracts.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product?> GetById(Guid id);
        Task <Product> Create (Product product);
        Task<Product?> UpdateById (Guid id, Product product);
        Task<bool> Delete (Guid id);
    }
}

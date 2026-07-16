

using Shared.Contracts.Entidades;

namespace Shared.Contracts.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product?> GetById(int id);
        Task <Product> Create (Product product);
        Task<Product?> UpdateById (int id, Product product);
        Task<bool> Delete (int id);
    }
}

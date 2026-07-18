using Shared.Contracts.Entidades;
using Shared.Contracts.DbContext;
using Shared.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Repositorios
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Product>> GetAll() =>
            await _db.Products.ToListAsync();
        public async Task<Product?> GetById(Guid id) =>
            await _db.Products.FindAsync(id);
        public async Task<Product>Create(Product product)
        {
            if (product.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero.");
            if(product.Id == Guid.Empty)
                product.Id = Guid.NewGuid();

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateById(Guid id, Product product)
        {
            var existingProduct = await _db.Products.FindAsync(id);
            if (existingProduct == null) return null;

            if (product.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero.");

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;

            await _db.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> Delete(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return false;

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }



    }
}

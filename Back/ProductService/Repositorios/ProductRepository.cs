using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Data;
using Shared.Contracts.Entidades;
using Shared.Contracts.Interfaces;

namespace ProductService.Repositorios
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> GetAllByContratistaAsync(Guid contratistaId)
        {
            return await _db.Products.AsNoTracking().Where(product => product.ContratistaId == contratistaId)
                .OrderBy(product => product.Name)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id,Guid contratistaId)
        {
            return await _db.Products.AsNoTracking().FirstOrDefaultAsync(product =>
                    product.Id == id &&
                    product.ContratistaId == contratistaId);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            if (product.ContratistaId == Guid.Empty)
                throw new ArgumentException("El identificador del contratista es obligatorio.");

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("El nombre del producto es obligatorio.");

            if (product.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero.");

            product.Name = product.Name.Trim();
            product.Description = product.Description?.Trim() ?? string.Empty;

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> UpdateAsync(int id,Guid contratistaId,Product product)
        {
            var existing = await _db.Products.FirstOrDefaultAsync(item =>item.Id == id && item.ContratistaId == contratistaId);

            if (existing == null)
                return null;

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("El nombre del producto es obligatorio.");

            if (product.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero.");

            existing.Name = product.Name.Trim();
            existing.Description = product.Description?.Trim() ?? string.Empty;
            existing.Price = product.Price;

            await _db.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(int id,Guid contratistaId)
        {
            var product = await _db.Products.FirstOrDefaultAsync(item =>item.Id == id && item.ContratistaId == contratistaId);

            if (product == null)
                return false;

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
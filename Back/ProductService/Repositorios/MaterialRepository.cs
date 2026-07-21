using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Data;
using Shared.Contracts.Entidades;
using Shared.Contracts.Interfaces;

namespace ProductService.Repositorios
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AppDbContext _db;

        public MaterialRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Material>> GetAllByContratistaAsync(Guid contratistaId)
        {
            return await _db.Materials.AsNoTracking().Where(material => material.ContratistaId == contratistaId)
                .OrderByDescending(material =>material.CreatedAt)
                .ToListAsync();
        }

        public async Task<Material?> GetByIdAsync(int id,Guid contratistaId)
        {
            return await _db.Materials.AsNoTracking().FirstOrDefaultAsync(material =>material.Id == id && material.ContratistaId == contratistaId);
        }

        public async Task<Material> CreateAsync(Material material)
        {
            if (material.ContratistaId == Guid.Empty)
                throw new ArgumentException("El identificador del contratista es obligatorio.");

            if (string.IsNullOrWhiteSpace(material.Name))
                throw new ArgumentException("El nombre del material es obligatorio.");

            if (string.IsNullOrWhiteSpace(material.Unit))
                throw new ArgumentException("La unidad del material es obligatoria.");

            if (material.Price <= 0)
                throw new ArgumentException("El precio del material debe ser mayor a cero.");

            material.Name = material.Name.Trim();
            material.Unit = material.Unit.Trim();
            material.CreatedAt = DateTime.UtcNow;

            _db.Materials.Add(material);
            await _db.SaveChangesAsync();

            return material;
        }

        public async Task<Material?> UpdateAsync(int id,Guid contratistaId,Material material)
        {
            var existing = await _db.Materials
                .FirstOrDefaultAsync(item =>item.Id == id && item.ContratistaId == contratistaId);

            if (existing == null)
                return null;

            if (string.IsNullOrWhiteSpace(material.Name))
                throw new ArgumentException("El nombre del material es obligatorio.");

            if (string.IsNullOrWhiteSpace(material.Unit))
                throw new ArgumentException("La unidad del material es obligatoria.");

            if (material.Price <= 0)
                throw new ArgumentException("El precio del material debe ser mayor a cero.");

            existing.Name = material.Name.Trim();
            existing.Unit = material.Unit.Trim();
            existing.Price = material.Price;

            await _db.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(int id,Guid contratistaId)
        {
            var material = await _db.Materials
                .FirstOrDefaultAsync(item =>item.Id == id && item.ContratistaId == contratistaId);

            if (material == null)
                return false;

            _db.Materials.Remove(material);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
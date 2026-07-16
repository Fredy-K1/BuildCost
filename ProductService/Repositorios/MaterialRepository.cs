using Microsoft.EntityFrameworkCore;
using Shared.Contracts.DbContext;
using Shared.Contracts.Entidades;
using Shared.Contracts.Interfaces;

namespace ProductService.Repositorios
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AppDbContext _db;

        public MaterialRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Material>> GetAll()
            => await _db.Materials.ToListAsync();

        public async Task<Material?> GetById(int id)
            => await _db.Materials.FindAsync(id);

        public async Task<Material?> Create(Material material)
        {

            if (material.Price <= 0)
                throw new ArgumentException("El precio del material debe ser mayor a cero.");

            _db.Materials.Add(material);
            await _db.SaveChangesAsync();
            return material;
        }

        public async Task<Material?> Update(Material material)
        {
            var existing = await _db.Materials.FindAsync(material.Id);
            if (existing == null) return null;

            if (material.Price <= 0)
                throw new ArgumentException("El precio del material debe ser mayor a cero.");

            existing.Name = material.Name;
            existing.Unit = material.Unit;
            existing.Price = material.Price;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteById(int id)
        {
            var material = await _db.Materials.FindAsync(id);
            if (material == null) return false;

            _db.Materials.Remove(material);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

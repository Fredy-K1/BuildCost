using Microsoft.EntityFrameworkCore;
using Shared.Contracts.DbContext;
using Shared.Contracts.Entidades;
using Shared.Contracts.Interfaces;

namespace ProductService.Repositorios
{
    public class DatoM2Repository : IDatoM2Repository
    {
        private readonly AppDbContext _db;

        public DatoM2Repository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<DatosM2>> GetAll()
            => await _db.DatosM2.ToListAsync();

        public async Task<DatosM2?> GetById(int id)
            => await _db.DatosM2.FindAsync(id);

        public async Task<DatosM2> Create(DatosM2 datosM2)
        {
            if (datosM2.Value <= 0)
                throw new ArgumentException("El valor de m2 debe ser mayor a cero.");

            _db.DatosM2.Add(datosM2);
            await _db.SaveChangesAsync();
            return datosM2;
        }

        public async Task<DatosM2?> Update(int id, DatosM2 datosM2)
        {
            var existing = await _db.DatosM2.FindAsync(id);
            if (existing == null) return null;

            if (datosM2.Value <= 0)
                throw new ArgumentException("El valor de m2 debe ser mayor a cero.");

            existing.DataType = datosM2.DataType;
            existing.Value = datosM2.Value;
            existing.Description = datosM2.Description;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _db.DatosM2.FindAsync(id);
            if (entity == null) return false;

            _db.DatosM2.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

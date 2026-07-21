using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Data;
using Shared.Contracts.Entidades;
using Shared.Contracts.Interfaces;

namespace ProductService.Repositorios
{
    public class DatoM2Repository : IDatoM2Repository
    {
        private readonly AppDbContext _db;

        public DatoM2Repository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<DatosM2>> GetAllByContratistaAsync(Guid contratistaId)
        {
            return await _db.DatosM2.AsNoTracking().Where(dato =>dato.ContratistaId == contratistaId)
                .OrderBy(dato => dato.DataType).ToListAsync();
        }

        public async Task<DatosM2?> GetByIdAsync(int id,Guid contratistaId)
        {
            return await _db.DatosM2.AsNoTracking().FirstOrDefaultAsync(dato =>
                    dato.Id == id && dato.ContratistaId == contratistaId);
        }

        public async Task<DatosM2> CreateAsync(DatosM2 datosM2)
        {
            if (datosM2.ContratistaId == Guid.Empty)
                throw new ArgumentException("El identificador del contratista es obligatorio.");

            if (string.IsNullOrWhiteSpace(datosM2.DataType))
                throw new ArgumentException("El tipo de dato es obligatorio.");

            if (datosM2.Value <= 0)
                throw new ArgumentException("El valor de m² debe ser mayor a cero.");

            datosM2.DataType = datosM2.DataType.Trim();
            datosM2.Description = datosM2.Description?.Trim() ?? string.Empty;

            _db.DatosM2.Add(datosM2);
            await _db.SaveChangesAsync();

            return datosM2;
        }

        public async Task<DatosM2?> UpdateAsync(int id, Guid contratistaId,DatosM2 datosM2)
        {
            var existing = await _db.DatosM2.FirstOrDefaultAsync(dato =>
                    dato.Id == id && dato.ContratistaId == contratistaId);

            if (existing == null)
                return null;

            if (string.IsNullOrWhiteSpace(datosM2.DataType))
                throw new ArgumentException("El tipo de dato es obligatorio.");

            if (datosM2.Value <= 0)
                throw new ArgumentException("El valor de m² debe ser mayor a cero.");

            existing.DataType = datosM2.DataType.Trim();
            existing.Value = datosM2.Value;
            existing.Description = datosM2.Description?.Trim() ?? string.Empty;

            await _db.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(int id,Guid contratistaId)
        {
            var datoM2 = await _db.DatosM2.FirstOrDefaultAsync(dato => dato.Id == id && dato.ContratistaId == contratistaId);

            if (datoM2 == null)
                return false;

            _db.DatosM2.Remove(datoM2);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
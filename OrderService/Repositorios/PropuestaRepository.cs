using Microsoft.EntityFrameworkCore;
using OrderService.Interfaces;
using Shared.Contracts.Data;
using Shared.Contracts.DTOs.Propuestas;
using Shared.Contracts.Entidades;


namespace OrderService.Repositorios
{
    public class PropuestaRepository : IPropuestaRepository
    {
        private readonly AppDbContext _appDbContext;

        public PropuestaRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Propuesta>> GetbyProyectoIdAsync(int proyectoId)
        {
            return await _appDbContext.Propuestas
                .Where(p => p.ProyectoId == proyectoId)
                .ToListAsync();
        }

        public async Task<Propuesta?> GetByIdAsync(int propuestaId)
        {
            return await _appDbContext.Propuestas.FindAsync(propuestaId);
        }

        public async Task<Propuesta> CreateAsync(Propuesta propuesta)
        {
            await _appDbContext.Propuestas.AddAsync(propuesta);
            await _appDbContext.SaveChangesAsync();
            return propuesta;
        }

        public async Task<Propuesta?> UpdateAsync(int propuestaId, UpdatePropuestaDto updatePropuestaDto)
        {
            var propuesta = await _appDbContext.Propuestas.FindAsync(propuestaId);
            if (propuesta == null) return null;
            

            propuesta.Costo = updatePropuestaDto.Costo ?? propuesta.Costo;
            propuesta.Detalles = updatePropuestaDto.Detalles ?? propuesta.Detalles;
            propuesta.PdfUrl = updatePropuestaDto.PdfUrl ?? propuesta.PdfUrl;

            if (updatePropuestaDto.Estado.HasValue)
                propuesta.Estado = updatePropuestaDto.Estado.Value;

            await _appDbContext.SaveChangesAsync();
            return propuesta;
        }
        
        public async Task<bool> DeleteAsync(int propuestaId)
        {
            var propuesta = await _appDbContext.Propuestas.FindAsync(propuestaId);
            if (propuesta == null) return false;


            _appDbContext.Propuestas.Remove(propuesta);

            var c = await _appDbContext.SaveChangesAsync();
            return c > 0; 
        }
    }
}

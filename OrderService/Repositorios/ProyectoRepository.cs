using Microsoft.EntityFrameworkCore;
using OrderService.Interfaces;
using Shared.Contracts.Data;
using Shared.Contracts.DTOs.Projec;
using Shared.Contracts.Entidades;


namespace OrderService.Repositorios
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly AppDbContext _context;

        public ProyectoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proyecto>> GetAllAsync()
        {
            return await _context.Proyectos.ToListAsync();
        }

        public async Task<Proyecto?> GetAsyncID(int id)
        {
            return await _context.Proyectos.FindAsync(id);
        }

        public async Task<Proyecto?> CreateAsync(Proyecto proyecto)
        {
            _context.Proyectos.Add(proyecto);
            await _context.SaveChangesAsync();
            return proyecto;
        }

        public async Task<Proyecto?> UpdateAsync(int id, UpdateProyectoDto dto)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null)
                return null;

            proyecto.Titulo = dto.Titulo ?? proyecto.Titulo;
            proyecto.Descripcion = dto.Descripcion ?? proyecto.Descripcion;
            proyecto.Municipio = dto.Municipio ?? proyecto.Municipio;

            if (dto.Estado.HasValue)
            {
                proyecto.Estado = dto.Estado.Value;
            }

            await _context.SaveChangesAsync();
            return proyecto;
        }

        public async Task<Proyecto?> DeleteAsync(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null)
            {
                return null;
            }
            _context.Proyectos.Remove(proyecto);
            await _context.SaveChangesAsync();
            return proyecto;
        }
    }
}

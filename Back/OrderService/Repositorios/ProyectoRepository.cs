using Microsoft.EntityFrameworkCore;
using OrderService.Interfaces;
using Shared.Contracts.Data;
using Shared.Contracts.DTOs.Projec;
using Shared.Contracts.Entidades;
using Shared.Contracts.Enums;

namespace OrderService.Repositorios
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly AppDbContext _context;

        public ProyectoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proyecto>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Proyectos
                                 .Where(p => p.UserOwnerId == userId)
                                 .ToListAsync();
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

            if (!string.IsNullOrWhiteSpace(dto.Titulo))
                proyecto.Titulo = dto.Titulo;

            if (!string.IsNullOrWhiteSpace(dto.Descripcion))
                proyecto.Descripcion = dto.Descripcion;

            if (!string.IsNullOrWhiteSpace(dto.Municipio))
                proyecto.Municipio = dto.Municipio;

            if (dto.Estado.HasValue)
                proyecto.Estado = dto.Estado.Value;

            await _context.SaveChangesAsync();
            return proyecto;
        }

        public async Task<Proyecto?> UpdateEstadoAsync(int id, ProjectStatus estado)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null) return null;

            proyecto.Estado = estado;
            await _context.SaveChangesAsync();
            return proyecto;
        }


        public async Task<Proyecto?> DeleteAsync(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null)
                return null;

            _context.Proyectos.Remove(proyecto);
            await _context.SaveChangesAsync();
            return proyecto;
        }
    }
}

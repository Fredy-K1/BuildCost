using OrderService.Interfaces;
using Shared.Contracts.DTOs.Projec;
using Shared.Contracts.Entidades;
using Shared.Contracts.Enums;

namespace OrderService.Services
{
    public class ProyectoService : IProyectoService
    {
        private readonly IProyectoRepository _proyectoRepository;
        public ProyectoService(IProyectoRepository proyectoRepository)
        {
            _proyectoRepository = proyectoRepository;
        }

        public async Task<IEnumerable<ProyectoDto>> GetAllProyectosAsync(Guid userId)
        {
            var proyectos = await _proyectoRepository.GetByUserIdAsync(userId);
            return proyectos.Select(p => MapToDto(p)!);
        }

        public async Task<ProyectoDto?> GetProyectoByIdAsync(int id, Guid userId)
        {
            var proyecto = await _proyectoRepository.GetAsyncID(id);
            if (proyecto == null || proyecto.UserOwnerId != userId)
                return null;

            return MapToDto(proyecto);
        }

        public async Task<ProyectoDto> CreateAsync(CreateProyectoDto dto)
        {
            var proyecto = new Proyecto
            {
                UserOwnerId = dto.UserOwnerId,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Municipio = dto.Municipio,
                Estado = ProjectStatus.Disponible,
                CreatedAt = DateTime.UtcNow
            };
            var createdProyecto = await _proyectoRepository.CreateAsync(proyecto);
            return MapToDto(createdProyecto)!;
        }

        public async Task<ProyectoDto?> UpdateAsync(int id, UpdateProyectoDto dto)
        {
            var actualizadoProyecto = await _proyectoRepository.UpdateAsync(id, dto);
            return MapToDto(actualizadoProyecto);
        }

        public async Task<ProyectoDto?> CambiarEstadoAsync(int id, ProjectStatus estado, Guid userId)
        {
            var proyecto = await _proyectoRepository.GetAsyncID(id);
            if (proyecto == null || proyecto.UserOwnerId != userId)
                return null;

            var actualizado = await _proyectoRepository.UpdateEstadoAsync(id, estado);
            return MapToDto(actualizado);
        }



        public async Task<ProyectoDto?> DeleteAsync(int id)
        {
            var proyecto = await _proyectoRepository.DeleteAsync(id);
            return proyecto == null ? null : MapToDto(proyecto);
        }

        private ProyectoDto? MapToDto(Proyecto? proyecto)
        {
            if (proyecto == null) return null;

            return new ProyectoDto
            {
                Id = proyecto.Id,
                UserOwnerId = proyecto.UserOwnerId,
                Titulo = proyecto.Titulo,
                Descripcion = proyecto.Descripcion,
                Municipio = proyecto.Municipio,
                Estado = proyecto.Estado,
                CreatedAt = proyecto.CreatedAt
            };
        }
    }
}

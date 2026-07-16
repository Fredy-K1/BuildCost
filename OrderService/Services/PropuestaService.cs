using OrderService.Interfaces;
using Shared.Contracts.DTOs.Propuestas;
using Shared.Contracts.Enums;
using Shared.Contracts.Entidades;

namespace OrderService.Services
{
    public class PropuestaService : IPropuestaService
    {
        private readonly IPropuestaRepository _repository;

        public PropuestaService(IPropuestaRepository repository)
        {
            _repository = repository;
        }


        public async Task<IEnumerable<PropuestaDto>> GetByProyectoIdAsync(int proyectoId)
        {
            var propuestas = await _repository.GetbyProyectoIdAsync(proyectoId);
            return propuestas.Select(p => MapToDto(p)!);
        }

        public async Task<PropuestaDto?> GetByIdAsync(int propuestaId)
        {
            var propuesta = await _repository.GetByIdAsync(propuestaId);
            return MapToDto(propuesta);
        }

        public async Task<PropuestaDto> CreateAsync(CreatePropuestaDto createPropuestaDto)
        {
            if (createPropuestaDto.Costo <= 0)
                throw new ArgumentException("El costo debe ser mayor a cero.");

            var propuesta = new Propuesta
            {
                ProyectoId = createPropuestaDto.ProyectoId,
                ContratistaId = createPropuestaDto.ContratistaId,
                Costo = createPropuestaDto.Costo,
                Detalles = createPropuestaDto.Detalles,
                Estado = PropoalStatus.Pendiente,
                PdfUrl = createPropuestaDto.PdfUrl
            };

            var createdPropuesta = await _repository.CreateAsync(propuesta);
            return MapToDto(createdPropuesta);
        }

        public async Task<PropuestaDto?> UpdateAsync(int propuestaId, UpdatePropuestaDto updatePropuestaDto)
        {
            var updatedPropuesta = await _repository.UpdateAsync(propuestaId, updatePropuestaDto);
            return MapToDto(updatedPropuesta);
        }

        public async Task<PropuestaDto?> ResponderPropuestaAsync(int id, bool aceptada)
        {
            var updateDto = new UpdatePropuestaDto
            {
                Estado = aceptada ? PropoalStatus.Aceptado : PropoalStatus.Rechazado
            };

            var actualizada = await _repository.UpdateAsync(id, updateDto);
            return MapToDto(actualizada);
        }


        public async Task<bool> DeleteAsync(int propuestaId)
        {
            return await _repository.DeleteAsync(propuestaId);
        }

        private PropuestaDto MapToDto(Propuesta propuesta)
        {
            return new PropuestaDto
            {
                Id = propuesta.id,
                ProyectoId = propuesta.ProyectoId,
                ContratistaId = propuesta.ContratistaId,
                Costo = propuesta.Costo,
                Detalles = propuesta.Detalles,
                Estado = propuesta.Estado,
                PdfUrl = propuesta.PdfUrl
            };
        }
    }
}

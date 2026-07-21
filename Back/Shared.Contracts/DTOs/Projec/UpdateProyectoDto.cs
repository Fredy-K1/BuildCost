
using Shared.Contracts.Enums;

namespace Shared.Contracts.DTOs.Projec
{
    public class UpdateProyectoDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public ProjectStatus? Estado { get; set; } 
    }
}
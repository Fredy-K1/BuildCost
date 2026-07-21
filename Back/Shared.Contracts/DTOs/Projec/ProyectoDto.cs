using Shared.Contracts.Enums;
namespace Shared.Contracts.DTOs.Projec
{
    public class ProyectoDto
    {
        public int Id { get; set; }
        public Guid UserOwnerId { get; set; } 
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public ProjectStatus Estado { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}

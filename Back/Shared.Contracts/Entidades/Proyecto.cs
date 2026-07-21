using Shared.Contracts.Enums;

namespace Shared.Contracts.Entidades
{
    public class Proyecto
    {
        public int Id { get; set; }
        public Guid UserOwnerId { get; set; } // ID del usuario propietario del proyecto
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public ProjectStatus Estado { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}

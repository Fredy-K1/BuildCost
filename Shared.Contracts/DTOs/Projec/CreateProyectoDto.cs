namespace Shared.Contracts.DTOs.Projec
{
    public class CreateProyectoDto
    {
        public int UserOwnerId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
    }
}



namespace Shared.Contracts.DTOs.Propuestas
{
    public class CreatePropuestaDto
    {
        public int ProyectoId { get; set; }
        public Guid ContratistaId { get; set; }
        public decimal Costo { get; set; }
        public string Detalles { get; set; } = string.Empty;
        public string PdfUrl { get; set; } = string.Empty;
    }
}

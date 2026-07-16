

using Shared.Contracts.Enums;

namespace Shared.Contracts.DTOs.Propuestas
{
    public class PropuestaDto
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public int ContratistaId { get; set; }
        public decimal Costo { get; set; }
        public string Detalles { get; set; } = string.Empty;
        public PropoalStatus Estado { get; set; }
        public string PdfUrl { get; set; } = string.Empty;
    }
}

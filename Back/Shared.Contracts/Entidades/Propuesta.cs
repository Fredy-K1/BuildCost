using Shared.Contracts.Enums;

namespace Shared.Contracts.Entidades
{
    public class Propuesta
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; } // A que proyecto pertenece la propuesta
        public Guid ContratistaId { get; set; } // ID del contratista que hace la propuesta
        public decimal Costo { get; set; } = decimal.Zero;
        public string Detalles { get; set; } = string.Empty;
        public PropoalStatus Estado { get; set; }
        public string PdfUrl { get; set; } = string.Empty;

    }
}

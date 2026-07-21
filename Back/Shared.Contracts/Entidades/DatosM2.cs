namespace Shared.Contracts.Entidades
{
    public class DatosM2
    {
        public int Id { get; set; }

        public string DataType { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public string Description { get; set; } = string.Empty;

        public Guid ContratistaId { get; set; }
    }
}
namespace Shared.Contracts.Entidades
{
    public class Material
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid ContratistaId { get; set; }
    }
}
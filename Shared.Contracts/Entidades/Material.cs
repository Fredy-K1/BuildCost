using System.ComponentModel.DataAnnotations;

namespace Shared.Contracts.Entidades
{
    public class Material
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public string Unit { get; set; } = "";
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

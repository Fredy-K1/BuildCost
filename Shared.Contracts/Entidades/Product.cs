using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Contracts.Entidades
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }= Guid.NewGuid();
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string ImagenUrl { get; set; } = "";
    }
}

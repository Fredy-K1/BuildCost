using System.ComponentModel.DataAnnotations;

namespace Shared.Contracts.Entidades
{
    public class DatosM2
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string DataType { get; set; } = "";
        public decimal Value { get; set; }
        public string Description { get; set; } = "";
    }
}

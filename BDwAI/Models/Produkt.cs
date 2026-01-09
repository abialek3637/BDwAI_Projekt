using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDwAI.Models
{
    public class Produkt
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "nchar(50)")]
        public string Name { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "int")]
        public int Quantity { get; set; }
    }
}

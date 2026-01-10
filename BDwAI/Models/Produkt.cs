using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDwAI.Models
{
    public class Produkt
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa produktu")] // To zmieni nagłówek w tabeli i etykietę w formularzach
        public string Name { get; set; }

        [Display(Name = "Opis")]
        public string? Description { get; set; }

        [Display(Name = "Cena")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Display(Name = "Ilość")]
        public int Quantity { get; set; }
    }
}

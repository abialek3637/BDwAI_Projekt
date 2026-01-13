using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDwAI.Models
{
    public class Produkt
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa produktu jest wymagana")]
        [StringLength(100, ErrorMessage = "Nazwa nie może mieć więcej niż 100 znaków")]
        [Display(Name = "Nazwa produktu")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Opis nie może mieć więcej niż 500 znaków")]
        [Display(Name = "Opis")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Cena jest wymagana")]
        [Range(0.01, 1000000, ErrorMessage = "Cena musi być większa od 0")]
        [Display(Name = "Cena")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Ilość jest wymagana")]
        [Range(0, int.MaxValue, ErrorMessage = "Ilość nie może być ujemna")]
        [Display(Name = "Ilość")]
        public int Quantity { get; set; }

        [Display(Name = "Zdjęcie")]
        public string? ImagePath { get; set; }

        [NotMapped]
        [Display(Name = "Plik zdjęcia")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Kategoria jest wymagana")]
        [Display(Name = "Kategoria")]
        public int? CategoryId { get; set; }
    }
}

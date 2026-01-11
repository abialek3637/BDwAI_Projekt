using System.ComponentModel.DataAnnotations;

namespace BDwAI.Models
{
    public class Zamowienie
    {
        public int Id { get; set; }

        public DateTime DataZamowienia { get; set; } = DateTime.Now;

        
        public string AppUserId { get; set; }
        public AppUser? User { get; set; }

        [Display(Name = "Łączna kwota")]
        public decimal TotalAmount { get; set; }

        
        [Required]
        public string Imie { get; set; }
        [Required]
        public string Nazwisko { get; set; }
        [Required]
        public string Adres { get; set; }
        [Required]
        public string Miasto { get; set; }
        [Required]
        public string KodPocztowy { get; set; }

        
        public List<ElementZamowienia> Elementy { get; set; } = new List<ElementZamowienia>();
    }
}
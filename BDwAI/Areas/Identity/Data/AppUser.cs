using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BDwAI.Models
{
    // Dziedziczymy po IdentityUser, żeby mieć Login, Hasło, Email itp.
    public class AppUser : IdentityUser
    {
        [Display(Name = "Imię")]
        public string? FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string? LastName { get; set; }

        [Display(Name = "Ulica i numer")]
        public string? Street { get; set; }

        [Display(Name = "Miasto")]
        public string? City { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string? ZipCode { get; set; }

        // Relacja: Jeden użytkownik może mieć wiele zamówień
        public ICollection<Zamowienie>? Zamowienia { get; set; }
    }
}
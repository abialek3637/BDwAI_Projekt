using System.ComponentModel.DataAnnotations;

namespace BDwAI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa kategorii")]
        public string Name { get; set; }

        public ICollection<Produkt> Produkts { get; set; }
    }
}

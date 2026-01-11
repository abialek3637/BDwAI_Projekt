using System.ComponentModel.DataAnnotations;

namespace BDwAI.Models
{
    public class Category
    {
        [Display(Name = "IdKategorii")]
        public int Id { get; set; }
        [Display(Name = "NazwaKategorii")]
        public string Name { get; set; }
    }
}

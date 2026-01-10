using Microsoft.EntityFrameworkCore;

namespace BDwAI.Models
{
    public class ProduktDbContext : DbContext
    {
        public ProduktDbContext(DbContextOptions<ProduktDbContext> options) : base(options)
        {
        }
        public DbSet<Produkt> Produkts { get; set; }
    }
}

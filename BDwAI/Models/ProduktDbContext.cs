using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BDwAI.Models; 

namespace BDwAI.Models
{

    public class ProduktDbContext : IdentityDbContext<AppUser>
    {
        public ProduktDbContext(DbContextOptions<ProduktDbContext> options)
            : base(options)
        {
        }     
        public DbSet<Produkt> Produkty { get; set; }

        
        public DbSet<Zamowienie> Zamowienia { get; set; }
        public DbSet<ElementZamowienia> ElementyZamowienia { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
        }

        public DbSet<Category> Categories { get; set; }

    }
}
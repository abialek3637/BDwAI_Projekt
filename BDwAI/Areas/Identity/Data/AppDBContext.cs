using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BDwAI.Models; 
namespace BDwAI.Data
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        
        public DbSet<Produkt> Produkty { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }
        public DbSet<ElementZamowienia> ElementyZamowienia { get; set; }
    }
}
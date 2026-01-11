using BDwAI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore; 
using BDwAI.Data; 

namespace BDwAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _context; 

      
        public HomeController(ILogger<HomeController> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Category(int? id)
        {
            if (id == null) return NotFound();
            var category = await _context.Categories.FindAsync(id);
            ViewBag.CategoryName = category.Name;
            var products = await _context.Produkty.Where(p => p.CategoryId == id).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Index()
        {
           
           
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                
                return RedirectToAction("Index", "Produkty");
              
            }
            var produkty = await _context.Produkty.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(produkty);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
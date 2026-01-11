using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BDwAI.Models;
using Microsoft.EntityFrameworkCore;

namespace BDwAI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ProduktDbContext _context;
    public HomeController(ILogger<HomeController> logger, ProduktDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Buy(int id)
    {
        var produkt = _context.Produkts.FirstOrDefault(p => p.Id == id);
        if (produkt == null)
        {
            return NotFound();
        }
        return View(produkt);
    }

    public async Task<IActionResult> Category(int? id)
    {
        if (id == null) return NotFound();
        var category = await _context.Categories.FindAsync(id);
        ViewBag.CategoryName = category.Name;
        var products = await _context.Produkts.Where(p => p.CategoryId == id).ToListAsync();
        return View(products);
    }
    public async Task<IActionResult> Index()
    {
        var produkty = await _context.Produkts.ToListAsync();
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

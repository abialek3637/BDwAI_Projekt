using BDwAI.Data;
using BDwAI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BDwAI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ZamowieniaController : Controller
    {
        private readonly AppDBContext _context;

        public ZamowieniaController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var zamowienia = await _context.Zamowienia
                .Include(z => z.User)
                .OrderByDescending(z => z.DataZamowienia)
                .ToListAsync();

            return View(zamowienia);
        }

        public async Task<IActionResult> Szczegoly(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _context.Zamowienia
                .Include(z => z.User)
                .Include(z => z.Elementy)
                .ThenInclude(e => e.Produkt)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            return View(zamowienie);
        }
    }
}
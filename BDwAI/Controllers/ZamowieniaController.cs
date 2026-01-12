using BDwAI.Data;
using BDwAI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BDwAI.Controllers
{
    [Authorize]
    public class ZamowieniaController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ZamowieniaController(AppDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create(int produktId)
        {
            var produkt = await _context.Produkty.FindAsync(produktId);
            if (produkt == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            var model = new Zamowienie
            {
                Imie = user.FirstName ?? "",
                Nazwisko = user.LastName ?? "",
                Adres = user.Street ?? "",
                Miasto = user.City ?? "",
                KodPocztowy = user.ZipCode ?? "",
                TotalAmount = produkt.Price,
            };

            ViewBag.Produkt = produkt;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Zamowienie zamowienie, int produktId, int ilosc)
        {
            var user = await _userManager.GetUserAsync(User);
            var produkt = await _context.Produkty.FindAsync(produktId);

            if (produkt == null) return NotFound();

            if (ilosc > produkt.Quantity)
            {
                ModelState.AddModelError("", $"Przykro nam, dostępnych jest tylko {produkt.Quantity} sztuk tego produktu.");
                ViewBag.Produkt = produkt;
                return View(zamowienie);
            }

            if (ilosc <= 0)
            {
                ModelState.AddModelError("", "Ilość musi być większa niż 0.");
                ViewBag.Produkt = produkt;
                return View(zamowienie);
            }

            zamowienie.DataZamowienia = DateTime.Now;
            zamowienie.AppUserId = user.Id;
            zamowienie.TotalAmount = produkt.Price * ilosc;

            bool daneZmienione = false;
            if (user.FirstName != zamowienie.Imie) { user.FirstName = zamowienie.Imie; daneZmienione = true; }
            if (user.LastName != zamowienie.Nazwisko) { user.LastName = zamowienie.Nazwisko; daneZmienione = true; }
            if (user.Street != zamowienie.Adres) { user.Street = zamowienie.Adres; daneZmienione = true; }
            if (user.City != zamowienie.Miasto) { user.City = zamowienie.Miasto; daneZmienione = true; }
            if (user.ZipCode != zamowienie.KodPocztowy) { user.ZipCode = zamowienie.KodPocztowy; daneZmienione = true; }
            if (daneZmienione) await _userManager.UpdateAsync(user);

            _context.Zamowienia.Add(zamowienie);
            await _context.SaveChangesAsync();

            var element = new ElementZamowienia
            {
                ZamowienieId = zamowienie.Id,
                ProduktId = produkt.Id,
                Ilosc = ilosc,
                CenaJednostkowa = produkt.Price
            };
            _context.ElementyZamowienia.Add(element);

            produkt.Quantity -= ilosc;
            _context.Update(produkt);

            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { id = zamowienie.Id });
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var zamowienie = await _context.Zamowienia.FindAsync(id);
            if (zamowienie == null) return NotFound();
            return View(zamowienie);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var zamowienia = await _context.Zamowienia
                .Include(z => z.User)
                .OrderByDescending(z => z.DataZamowienia)
                .ToListAsync();

            return View(zamowienia);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var zamowienie = await _context.Zamowienia
                .Include(z => z.User)
                .Include(z => z.ElementyZamowienia)
                    .ThenInclude(e => e.Produkt)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (zamowienie == null) return NotFound();

            return View(zamowienie);
        }
    }
}
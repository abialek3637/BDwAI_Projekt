using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BDwAI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using BDwAI.Data;

namespace BDwAI.Controllers
{
    public class ProduktyController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public ProduktyController(AppDBContext context, UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // GET: Produkty
        public async Task<IActionResult> Index(string searchString)
        {
            var produkty = from p in _context.Produkty
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                produkty = produkty.Where(s => s.Name.Contains(searchString));
            }

            return View(await produkty.ToListAsync());
        }

        // GET: Produkty/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // GET: Produkty/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Produkty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Quantity,ImageFile,CategoryId")] Produkt produkt)
        {
            if (ModelState.IsValid)
            {
                if (produkt.ImageFile != null)
                {
                    var folder = Path.Combine(_env.WebRootPath, "images");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var fileName = Guid.NewGuid() + Path.GetExtension(produkt.ImageFile.FileName);
                    var path = Path.Combine(folder, fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await produkt.ImageFile.CopyToAsync(stream);

                    produkt.ImagePath = "/images/" + fileName;
                }
                _context.Add(produkt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", produkt.CategoryId);
            return View(produkt);
        }

        // GET: Produkty/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkty.FindAsync(id);
            if (produkt == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", produkt.CategoryId);
            return View(produkt);
        }

        // POST: Produkty/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Quantity,ImageFile,CategoryId")] Produkt produkt)
        {
            if (id != produkt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (produkt.ImageFile != null)
                {
                    var folder = Path.Combine(_env.WebRootPath, "images");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var fileName = Guid.NewGuid() + Path.GetExtension(produkt.ImageFile.FileName);
                    var path = Path.Combine(folder, fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await produkt.ImageFile.CopyToAsync(stream);

                    produkt.ImagePath = "/images/" + fileName;
                }

                try
                {
                    _context.Update(produkt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduktExists(produkt.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", produkt.CategoryId);
            return View(produkt);
        }

        // GET: Produkty/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // POST: Produkty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produkt = await _context.Produkty.FindAsync(id);
            if (produkt != null)
            {
                _context.Produkty.Remove(produkt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduktExists(int id)
        {
            return _context.Produkty.Any(e => e.Id == id);
        }

        [Authorize]
        public async Task<IActionResult> Kup(int? id)
        {
            if (id == null) return NotFound();

            var produkt = await _context.Produkty.FindAsync(id);
            if (produkt == null) return NotFound();

            var zamowienie = new Zamowienie();

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                zamowienie.Imie = user.FirstName ?? "";
                zamowienie.Nazwisko = user.LastName ?? "";
                zamowienie.Adres = user.Street ?? "";
                zamowienie.Miasto = user.City ?? "";
                zamowienie.KodPocztowy = user.ZipCode ?? "";
                zamowienie.AppUserId = user.Id;
            }

            ViewBag.Produkt = produkt;

            return View(zamowienie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kup(Zamowienie zamowienie, int produktId, int ilosc)
        {
            var produkt = await _context.Produkty.FindAsync(produktId);

            if (produkt != null)
            {
                zamowienie.DataZamowienia = DateTime.Now;
                zamowienie.TotalAmount = produkt.Price * ilosc;

                var user = await _userManager.GetUserAsync(User);
                if (user != null) zamowienie.AppUserId = user.Id;

                var element = new ElementZamowienia
                {
                    ProduktId = produktId,
                    CenaJednostkowa = produkt.Price,
                    Ilosc = ilosc,
                    Zamowienie = zamowienie
                };

                _context.Zamowienia.Add(zamowienie);
                _context.ElementyZamowienia.Add(element);

                produkt.Quantity -= ilosc;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(zamowienie);
        }
    }
}
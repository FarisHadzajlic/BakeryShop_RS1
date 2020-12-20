using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PekaraKonjic;
using PekaraKonjic.Helper;
using PekaraKonjic.Models;
using PekaraKonjic.ModelView;

namespace PekaraKonjic.Controllers
{
    public class ProizvodController : Controller
    {

        private readonly IProizvodRepozitorij _ProizvodRepozitorij;
        private readonly IKategorijaRepozitorij _KategorijaRepozitorij;
        private readonly MojContext _context;

        public ProizvodController(IProizvodRepozitorij proizvod, IKategorijaRepozitorij kategorijaRepozitorij, MojContext context)
        {
            _ProizvodRepozitorij = proizvod;
            _KategorijaRepozitorij = kategorijaRepozitorij;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mojContext = _context.Proizvodi.Include(p => p.Kategorija);
            return View(await mojContext.ToListAsync());
        }

        public ViewResult Lista(string kategorija)
        {
            IEnumerable<Proizvod> proizvodi;
            string trenutnaKategorija;


            if (string.IsNullOrEmpty(kategorija))
            {
                proizvodi = _ProizvodRepozitorij.Proizvodi.OrderBy(p => p.ProizvodID);
                trenutnaKategorija = "Svi proizvodi";
            }
            else
            {
                var kateg = _context.Kategorije.Where(i => i.Naziv == kategorija).Select(i => i.KategorijaId).FirstOrDefault();
                proizvodi = _ProizvodRepozitorij.Proizvodi.Where(p=>p.KategorijaId == kateg)
                    .OrderBy(p => p.ProizvodID);
                trenutnaKategorija = _KategorijaRepozitorij.SveKategorije.FirstOrDefault(c => c.Naziv == kategorija)?.Naziv;
            }

            return View(new ListaProizvodaVM
            {
                Proizvodi = proizvodi,
                TrenutnaKategorija = trenutnaKategorija
            });
        }

        // GET: Proizvod/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proizvod = await _context.Proizvodi
                .Include(p => p.Kategorija)
                .Include(k => k.Komentari)
                .FirstOrDefaultAsync(m => m.ProizvodID == id);
            if (proizvod == null)
            {
                return NotFound();
            }

            return View(proizvod);
        }

        [HttpPost]
        public IActionResult IzbrisiKomentar(int komentarId, int proizvodId)
        {
            var komentar = _context.Komentari.Find(komentarId);

            if (komentar == null)
            {
                ViewBag.ErrorMessage = $"Komentar sa ID = {komentarId} ne postoji";
                return View("NotFound");
            }
            else
            {
                _context.Remove(_context.Komentari.Single(k => k.KomentarID == komentarId));
                _context.SaveChanges();
                return Redirect($"/Proizvod/Details/{proizvodId}");
            }
        }
        public IActionResult LikeP(int proizvodId)
        {
            var proizvod = _context.Proizvodi.Find(proizvodId);

            proizvod.LikeBrojac++;

            _context.SaveChanges();

            return Redirect($"/Proizvod/Details/{proizvodId}");
        }

        public IActionResult Like(int komentarId, int proizvodId)
        {
            var komentar = _context.Komentari.Find(komentarId);

            komentar.LikeBrojac++;

            _context.SaveChanges();

            return Redirect($"/Proizvod/Details/{proizvodId}");
        }

        [HttpPost]
        public IActionResult Komentarisi(string sadrzaj, int proizvodId, int kupacId)
        {

            Komentar komentar = new Komentar();

            komentar.KupacId = kupacId;
            komentar.ProizvodId = proizvodId;
            komentar.Vrijeme = DateTime.Now;
            komentar.TekstKomentara = sadrzaj;

            _context.Komentari.Add(komentar);
            _context.SaveChanges();


            return Redirect($"/Proizvod/Details/{proizvodId}");
        }

        // GET: Proizvod/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["KategorijaId"] = new SelectList(_context.Kategorije, "KategorijaId", "Naziv");
            return View();
        }

        // POST: Proizvod/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ProizvodID,Naziv,Cijena,SlikaUrl,Opis,NaStanju,KategorijaId")] Proizvod proizvod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proizvod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KategorijaId"] = new SelectList(_context.Kategorije, "KategorijaId", "KategorijaId", proizvod.KategorijaId);
            return View(proizvod);
        }

        // GET: Proizvod/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proizvod = await _context.Proizvodi.FindAsync(id);
            if (proizvod == null)
            {
                return NotFound();
            }
            ViewData["KategorijaId"] = new SelectList(_context.Kategorije, "KategorijaId", "KategorijaId", proizvod.Kategorija);
            return View(proizvod);
        }

        // POST: Proizvod/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ProizvodID,Naziv,Cijena,SlikaUrl,Opis,NaStanju,KategorijaId")] Proizvod proizvod)
        {
            if (id != proizvod.ProizvodID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proizvod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProizvodExists(proizvod.ProizvodID))
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
            ViewData["KategorijaId"] = new SelectList(_context.Kategorije, "KategorijaId", "KategorijaId", proizvod.KategorijaId);
            return View(proizvod);
        }

        // GET: Proizvod/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proizvod = await _context.Proizvodi
                .Include(p => p.Kategorija)
                .FirstOrDefaultAsync(m => m.ProizvodID == id);
            if (proizvod == null)
            {
                return NotFound();
            }

            return View(proizvod);
        }
        // POST: Proizvod/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proizvod = await _context.Proizvodi.FindAsync(id);
            _context.Proizvodi.Remove(proizvod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProizvodExists(int id)
        {
            return _context.Proizvodi.Any(e => e.ProizvodID == id);
        }
    }
}

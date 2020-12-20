using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PekaraKonjic.Models;

namespace PekaraKonjic.Controllers
{
    [Authorize]
    public class NarudzbaController : Controller
    {      
        private readonly INarudzbaRepozitorij _NarudzbaRepozitorij;
        private readonly Korpa _Korpa;
        public NarudzbaController(INarudzbaRepozitorij narudzbaRepozitorij, Korpa korpa)
        {
            _NarudzbaRepozitorij = narudzbaRepozitorij;
            _Korpa = korpa;
        }
        public IActionResult Checkout()
        {
            return View();
        }        
        [HttpPost]
        public IActionResult Checkout(Narudzba narudzba)
        {
            var items = _Korpa.GetShoppingCartItems();
            _Korpa.KorpaItemi = items;

            if (_Korpa.KorpaItemi.Count == 0)
            {
                ModelState.AddModelError("", "Korpa je prazna, dodajte proizvode!");
            }

            if (ModelState.IsValid)
            {
                _NarudzbaRepozitorij.KreirajNarudzbu(narudzba);
                _Korpa.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }
            return View(narudzba);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Hvala vam na narudžbi, uskoro će te uživati u ukusnim proizvodima!";
            return View();
        }
    }
}
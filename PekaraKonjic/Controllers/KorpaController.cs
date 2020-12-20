using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PekaraKonjic.Models;
using PekaraKonjic.ModelView;

namespace PekaraKonjic.Controllers
{
    public class KorpaController : Controller
    {
        private readonly IProizvodRepozitorij _ProizvodRepozitorij;
        private readonly Korpa _Korpa;
        public KorpaController(IProizvodRepozitorij proizvod, Korpa korpa)
        {
            _ProizvodRepozitorij = proizvod;
            _Korpa = korpa;
        }
        public ViewResult Index()
        {
            var items = _Korpa.GetShoppingCartItems();
            _Korpa.KorpaItemi = items;

            var korpaVM = new KorpaVM
            {
                Korpa = _Korpa,
                Ukupno = _Korpa.Ukupno()
            };

            return View(korpaVM);
        }
        public RedirectToActionResult AddToShoppingCart(int proizvodId)
        {
            var izabraniProizvod = _ProizvodRepozitorij.Proizvodi.FirstOrDefault(p => p.ProizvodID == proizvodId);

            if (izabraniProizvod != null)
            {
                _Korpa.AddToCart(izabraniProizvod, 1);
            }
            return RedirectToAction("Index");
        }
        public RedirectToActionResult RemoveFromShoppingCart(int proizvodId)
        {
            var izabraniProizvod = _ProizvodRepozitorij.Proizvodi.FirstOrDefault(p => p.ProizvodID == proizvodId);

            if (izabraniProizvod != null)
            {
                _Korpa.RemoveFromCart(izabraniProizvod);
            }
            return RedirectToAction("Index");
        }
    }
}
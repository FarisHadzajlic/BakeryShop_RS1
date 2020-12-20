using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PekaraKonjic.Models;
using Microsoft.AspNetCore.Mvc;

namespace PekaraKonjic.Components
{
    public class KategorijaMenu : ViewComponent
    {
        private readonly IKategorijaRepozitorij _kategorijaRepozitorij;
        public KategorijaMenu(IKategorijaRepozitorij kategorijaRepozitorij)
        {
            _kategorijaRepozitorij = kategorijaRepozitorij;
        }

        public IViewComponentResult Invoke()
        {
            var kategorija = _kategorijaRepozitorij.SveKategorije.OrderBy(c => c.Naziv);
            return View(kategorija);
        }
    }
}

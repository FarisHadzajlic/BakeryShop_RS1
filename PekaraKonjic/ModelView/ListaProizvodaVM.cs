using PekaraKonjic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.ModelView
{
    public class ListaProizvodaVM
    {
        public IEnumerable<Proizvod> Proizvodi { get; set; }
        public string TrenutnaKategorija { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.Models
{
    public class Kategorija
    {
        public int KategorijaId { get; set; }
        public string Naziv { get; set; }
        public List<Proizvod> Proizvodi { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.Models
{
    public class DetaljiNarudzbe
    {
        public int DetaljiNarudzbeID { get; set; }
        public int NarudzbaId { get; set; }
        public Narudzba Narudzba { get; set; } 
        public int ProizvodId { get; set; }
        public Proizvod Proizvod { get; set; }
        public int Kolicina { get; set; }
        public float Cijena { get; set; }
    }
}

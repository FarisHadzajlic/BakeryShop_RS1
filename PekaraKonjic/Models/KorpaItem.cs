using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.Models
{
    public class KorpaItem
    {
        public int KorpaitemId { get; set; }
        public Proizvod Proizvod { get; set; }
        public int Kolicina { get; set; }
        public string KorpaId { get; set; }
    }
}

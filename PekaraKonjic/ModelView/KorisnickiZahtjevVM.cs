using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PekaraKonjic.ModelView
{
    public class KorisnickiZahtjevVM
    {
        public KorisnickiZahtjevVM()
        {
            Cliams = new List<KorisnikZahtjev>();
        }
        public string KorisnikId { get; set; }
        public List<KorisnikZahtjev> Cliams { get; set; }
    }
}

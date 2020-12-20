using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.Models
{
    public class Komentar
    {
        public Komentar(){
            LikeBrojac = 0;
        }
        public int KomentarID { get; set; }
        public string TekstKomentara { get; set; }
        public int KupacId { get; set; }
        public Kupac Kupac { get; set; }
        public int ProizvodId { get; set; }
        public Proizvod Proizvod { get; set; }
        public int LikeBrojac { get; set; }
        public DateTime Vrijeme { get; set; }
    }
}

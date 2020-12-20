using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.Models
{
    public class Proizvod
    {
        public Proizvod()
        {
            Komentari = new List<Komentar>();
            LikeBrojac = 0;
        }        
        public int ProizvodID { get; set; }
        public string Naziv { get; set; }
        public float Cijena { get; set; }
        [DisplayName("Slika (Url)")]
        public string SlikaUrl { get; set; }
        public int LikeBrojac { get; set; }
        public string Opis { get; set; }
        [DisplayName("Na stanju")]
        public bool NaStanju { get; set; }
        public List<Komentar> Komentari { get; set; }
        public int KategorijaId { get; set; }
        public Kategorija Kategorija { get; set; }
    }
}

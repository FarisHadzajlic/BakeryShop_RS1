using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.Models
{
    //Kupac
    public class Kupac : IdentityUser
    {
        public int KupacID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Adresa { get; set; }
        public string BrojTelefona { get; set; }
        public int? KomentarId { get; set; }
        public Komentar Komentar { get; set; }
        public int? NarudzbaId { get; set; }
        public Narudzba Narudzba { get; set; }
    }
}
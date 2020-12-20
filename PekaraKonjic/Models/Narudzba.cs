using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.Models
{
    public class Narudzba
    {
        [BindNever]
        public int NarudzbaID { get; set; }
        public List<DetaljiNarudzbe> DetaljiNarudzbe { get; set; }
        public Kupac Kupac { get; set; }

        [Required(ErrorMessage = "Unesite ime")]
        [Display(Name = "Ime")]
        [StringLength(50)]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Unesite prezime")]
        [Display(Name = "Prezime")]
        [StringLength(50)]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Unesite adresu")]
        [StringLength(100)]
        [Display(Name = "Adresa")]
        public string Adresa { get; set; }

        [Required(ErrorMessage = "Unesite broj telefona")]
        [StringLength(25)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Broj Telefona")]
        public string BrojTelefona { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
          ErrorMessage = "Uneseni E-mail nije unesen u vazecem formatu")]
        public string Email { get; set; }
        [BindNever]
        [ScaffoldColumn(false)]

        public float Ukupno { get; set; }
        [BindNever]
        [ScaffoldColumn(false)]
        public DateTime DatumKupovine { get; set; }
    }
}

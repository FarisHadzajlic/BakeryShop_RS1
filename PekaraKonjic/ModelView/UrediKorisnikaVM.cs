using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.ModelView
{
    public class UrediKorisnikaVM
    {
        public UrediKorisnikaVM()
        {
            Zahtjevi = new List<string>();
            Uloge = new List<string>();
        }

        public string Id { get; set; }
        [Required]
        public string KorisnickoIme { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public List<string> Zahtjevi { get; set; }
        public IList<string> Uloge { get; set; }
    }
}

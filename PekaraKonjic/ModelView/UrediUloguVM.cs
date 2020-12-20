using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.ModelView
{
    public class UrediUloguVM
    {
        public UrediUloguVM()
        {
            Korisnici = new List<string>();
        }
        public string Id { get; set; }
        [Required(ErrorMessage = "Naziv uloge je obavezan")]
        public string NazivUloge { get; set; }
        public List<string> Korisnici { get; set; }
    }
}

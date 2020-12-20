using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.ModelView
{
    public class KreirajUloguVM
    {
        [Required]
        public string NazivUloge { get; set; }
    }
}

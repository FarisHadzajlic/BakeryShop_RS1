using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PekaraKonjic.Models;

namespace PekaraKonjic.Models
{
    public interface IKategorijaRepozitorij
    {
        IEnumerable<Kategorija> SveKategorije { get; }
    }
}

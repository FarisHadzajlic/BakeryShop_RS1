using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.Models
{
    public interface IProizvodRepozitorij
    {
        IEnumerable<Proizvod> Proizvodi { get; }
        Proizvod GetProizvodById(int proizvodId);
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.Models
{
    public class ProizvodRepozitorij : IProizvodRepozitorij
    {
        private readonly MojContext db;

        public ProizvodRepozitorij(MojContext _db)
        {
            db = _db;
        }
        public IEnumerable<Proizvod> Proizvodi
        {
            get
            {
                return db.Proizvodi;
            }
        }
        public Proizvod GetProizvodById(int proizvodId)
        {
            return db.Proizvodi.FirstOrDefault(p => p.ProizvodID == proizvodId);
        }
    }
}
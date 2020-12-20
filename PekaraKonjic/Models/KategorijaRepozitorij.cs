using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.Models
{
    public class KategorijaRepozitorij : IKategorijaRepozitorij
    {
        private readonly MojContext db;
        public KategorijaRepozitorij(MojContext _db)
        {
            db = _db;
        }
        public IEnumerable<Kategorija> SveKategorije => db.Kategorije;
    }
}

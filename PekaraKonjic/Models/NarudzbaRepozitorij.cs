using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.Models
{
    public class NarudzbaRepozitorij : INarudzbaRepozitorij
    {
        private readonly MojContext db;
        private readonly Korpa _Korpa;

        public NarudzbaRepozitorij(MojContext _db, Korpa korpa)
        {
            db = _db;
            _Korpa = korpa;
        }

        public void KreirajNarudzbu(Narudzba narudzba)
        {
            narudzba.DatumKupovine = DateTime.Now;

            var itemi = _Korpa.KorpaItemi;
            narudzba.Ukupno = _Korpa.Ukupno();

            narudzba.DetaljiNarudzbe = new List<DetaljiNarudzbe>();

            foreach (var item in itemi)
            {
                var detaljiNarudzbe = new DetaljiNarudzbe
                {
                    Kolicina = item.Kolicina,
                    ProizvodId = item.Proizvod.ProizvodID,
                    Cijena = item.Proizvod.Cijena
                };

                narudzba.DetaljiNarudzbe.Add(detaljiNarudzbe);
            }

            db.Narudzbe.Add(narudzba);
            db.SaveChanges();
        }
    }
}

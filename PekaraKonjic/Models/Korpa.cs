using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PekaraKonjic.Models
{
    public class Korpa
    {
        private readonly MojContext db;
        public string KorpaId { get; set; }
        public List<KorpaItem> KorpaItemi { get; set; }
        private Korpa(MojContext _db)
        {
            db = _db;
        }
        public static Korpa GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<MojContext>();

            string korpaId = session.GetString("KorpaID") ?? Guid.NewGuid().ToString();

            session.SetString("KorpaID", korpaId);

            return new Korpa(context) { KorpaId = korpaId };
        }
        public void AddToCart(Proizvod p, int kolicina)
        {
            var itemi = db.KorpaItemi.SingleOrDefault(s => s.Proizvod.ProizvodID == p.ProizvodID && s.KorpaId == KorpaId);

            if (itemi == null)
            {
                itemi = new KorpaItem
                {
                    KorpaId = KorpaId,
                    Proizvod = p,
                    Kolicina = 1
                };
                db.KorpaItemi.Add(itemi);
            }
            else
            {
                itemi.Kolicina++;
            }
            db.SaveChanges();
        }
        public int RemoveFromCart(Proizvod p)
        {
            var itemi =
                    db.KorpaItemi.SingleOrDefault(
                        s => s.Proizvod.ProizvodID == p.ProizvodID && s.KorpaId == KorpaId);

            var localKol = 0;

            if (itemi != null)
            {
                if (itemi.Kolicina > 1)
                {
                    itemi.Kolicina--;
                    localKol = itemi.Kolicina;
                }
                else
                {
                    db.KorpaItemi.Remove(itemi);
                }
            }

            db.SaveChanges();

            return localKol;
        }
        public List<KorpaItem> GetShoppingCartItems()
        {
            return KorpaItemi ??
                   (KorpaItemi =
                       db.KorpaItemi.Where(c => c.KorpaId == KorpaId)
                           .Include(s => s.Proizvod)
                           .ToList());
        }
        public void ClearCart()
        {
            var item = db
                .KorpaItemi
                .Where(c => c.KorpaId == KorpaId);

            db.KorpaItemi.RemoveRange(item);

            db.SaveChanges();
        }
        public float Ukupno()
        {
            var total = db.KorpaItemi.Where(c => c.KorpaId == KorpaId)
                .Select(c => c.Proizvod.Cijena * c.Kolicina).Sum();
            return total;
        }
    }
}

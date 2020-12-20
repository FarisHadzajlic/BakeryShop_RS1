using PekaraKonjic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PekaraKonjic
{
    public class MojContext : IdentityDbContext<Kupac>
    {
        public MojContext(DbContextOptions<MojContext> options) : base(options)
        {
        }

       
        public DbSet<Proizvod> Proizvodi { get; set; }
        public DbSet<Narudzba> Narudzbe { get; set; }
        public DbSet<DetaljiNarudzbe> DetaljiNarudzbi { get; set; }
        public DbSet<Kupac> Kupci { get ; set; }
        public DbSet<Komentar> Komentari { get ; set; }
        public DbSet<KorpaItem> KorpaItemi { get; set; }
        public DbSet<Kategorija> Kategorije { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Kategorija>().HasData(new Kategorija { KategorijaId = 1, Naziv = "Pite" });
            modelBuilder.Entity<Kategorija>().HasData(new Kategorija { KategorijaId = 2, Naziv = "Kolači" });
            modelBuilder.Entity<Kategorija>().HasData(new Kategorija { KategorijaId = 3, Naziv = "Pogače" });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PekaraKonjic.Models;
using PekaraKonjic.ModelView;

namespace PekaraKonjic.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdministratorController : Controller
    {
        private readonly RoleManager<IdentityRole> ulogaManager;
        private readonly UserManager<Kupac> korisnikManager;
        private readonly ILogger<AdministratorController> logger;

        public AdministratorController(RoleManager<IdentityRole> roleManager,
            UserManager<Kupac> korisnikManager, ILogger<AdministratorController> logger)
        {
            this.ulogaManager = roleManager;
            this.korisnikManager = korisnikManager;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> IzbrisiKorisnika(string id)
        {
            var korisnik = await korisnikManager.FindByIdAsync(id);

            if (korisnik == null)
            {
                ViewBag.ErrorMessage = $"Korisnik sa ID = {id} ne postoji";
                return View("NotFound");
            }
            else
            {
                var rezultat = await korisnikManager.DeleteAsync(korisnik);

                if (rezultat.Succeeded)
                {
                    return RedirectToAction("ListaKorisnika");
                }

                foreach (var error in rezultat.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListaKorisnika");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageKorisnickeUloge(string id)
        {
            ViewBag.korisnikId = id;

            var korisnik = await korisnikManager.FindByIdAsync(id);

            if (korisnik == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new List<KorisnikUlogeVM>();

            foreach (var uloga in ulogaManager.Roles)
            {
                var korisnikUlogaVM = new KorisnikUlogeVM
                {
                    UlogaId = uloga.Id,
                    Naziv = uloga.Name
                };

                if (await korisnikManager.IsInRoleAsync(korisnik, uloga.Name))
                {
                    korisnikUlogaVM.Oznacen = true;
                }
                else
                {
                    korisnikUlogaVM.Oznacen = false;
                }

                model.Add(korisnikUlogaVM);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageKorisnickeUloge(List<KorisnikUlogeVM> model, string id)
        {
            var korisnik = await korisnikManager.FindByIdAsync(id);

            if (korisnik == null)
            {
                ViewBag.ErrorMessage = $"Korisnik sa ID = {id} ne postoji";
                return View("NotFound");
            }

            var uloge = await korisnikManager.GetRolesAsync(korisnik);
            var rezultat = await korisnikManager.RemoveFromRolesAsync(korisnik, uloge);

            if (!rezultat.Succeeded)
            {
                ModelState.AddModelError("", "Ne možete izbrisati trenutnu korisnicku ulogu");
                return View(model);
            }

            rezultat = await korisnikManager.AddToRolesAsync(korisnik,
                model.Where(x => x.Oznacen).Select(y => y.Naziv));

            if (!rezultat.Succeeded)
            {
                ModelState.AddModelError("", "Ne možete dodati izabranu ulogu korisniku");
                return View(model);
            }

            return RedirectToAction("UrediKorisnika", new { Id = id });
        }      

        [HttpPost]
        public async Task<IActionResult> IzbrisiUlogu(string id)
        {
            var uloga = await ulogaManager.FindByIdAsync(id);

            if (uloga == null)
            {
                ViewBag.ErrorMessage = $"Uloga sa ID = {id} ne postoji";
                return View("NotFound");
            }
            else
            {
                var rezultat = await ulogaManager.DeleteAsync(uloga);

                if (rezultat.Succeeded)
                {
                    return RedirectToAction("ListaUloga");
                }

                foreach (var error in rezultat.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListaUloga");
            }
        }

        [HttpGet]
        public IActionResult ListaKorisnika()
        {
            var korisnici = korisnikManager.Users;
            return View(korisnici);
        }
        [HttpGet]
        public async Task<IActionResult> UrediKorisnika(string id)
        {
            var korisnik = await korisnikManager.FindByIdAsync(id);

            if (korisnik == null)
            {
                ViewBag.ErrorMessage = $"Korisnik sa ID = {id} ne postoji";
                return View("NotFound");
            }

            // GetClaimsAsync retunrs the list of user Claims
            var korisnickiZahtjevi = await korisnikManager.GetClaimsAsync(korisnik);
            // GetRolesAsync returns the list of user Roles
            var korisnickeUloge = await korisnikManager.GetRolesAsync(korisnik);

            var model = new UrediKorisnikaVM
            {
                Id = korisnik.Id,
                Email = korisnik.Email,
                KorisnickoIme = korisnik.UserName,
                Zahtjevi = korisnickiZahtjevi.Select(c => c.Value).ToList(),
                Uloge = korisnickeUloge
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UrediKorisnika(UrediKorisnikaVM model)
        {
            var korisnik = await korisnikManager.FindByIdAsync(model.Id);

            if (korisnik == null)
            {
                ViewBag.ErrorMessage = $"Korisnik sa ID = {model.Id} ne postoji";
                return View("NotFound");
            }
            else
            {
                korisnik.Email = model.Email;
                korisnik.UserName = model.KorisnickoIme;

                var rezultat = await korisnikManager.UpdateAsync(korisnik);

                if (rezultat.Succeeded)
                {
                    return RedirectToAction("ListaKorisnika");
                }

                foreach (var error in rezultat.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }


        [HttpGet]
        public IActionResult KreirajUlogu()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> KreirajUlogu(KreirajUloguVM model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.NazivUloge
                };
                IdentityResult result = await ulogaManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListaUloga", "Administrator");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ListaUloga()
        {
            var uloge = ulogaManager.Roles;
            return View(uloge);
        }
        [HttpGet]
        public async Task<IActionResult> UrediUlogu(string id)
        {
            var uloga = await ulogaManager.FindByIdAsync(id);

            if(uloga == null)
            {
                ViewBag.ErrorMessage = $" Uloga sa ID = {id} ne moze biti pronadjena";
                return View("NotFound");
            }
            var model = new UrediUloguVM
            {
                Id = uloga.Id,
                NazivUloge = uloga.Name
            };

            foreach (var korisnik in korisnikManager.Users) 
            {
                if (await korisnikManager.IsInRoleAsync(korisnik, uloga.Name))
                {
                    model.Korisnici.Add(korisnik.UserName);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UrediUlogu(UrediUloguVM model)
        {
            var uloga = await ulogaManager.FindByIdAsync(model.Id);

            if (uloga == null)
            {
                ViewBag.ErrorMessage = $" Uloga sa ID = {model.Id} ne moze biti pronadjena";
                return View("NotFound");
            }
            else
            {
                uloga.Name = model.NazivUloge;
                var rezultat = await ulogaManager.UpdateAsync(uloga);

                if (rezultat.Succeeded)
                {
                    return RedirectToAction("ListaUloga");
                }
                foreach (var error in rezultat.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UrediKorisnikaUUlozi(string id)
        {
            ViewBag.id = id;
             
            var uloga = await ulogaManager.FindByIdAsync(id);

            if (uloga == null)
            {
                ViewBag.ErrorMessage = $" Uloga sa ID = {id} ne moze biti pronadjena";
                return View("NotFound");
            }
            var model = new List<KorisnikUlogaVM>();

                foreach (var korisnik in korisnikManager.Users)
                {
                var korisnikUlogaVM = new KorisnikUlogaVM
                {
                    KorisnikId = korisnik.Id,
                    KorisnickoIme = korisnik.UserName
                };
                if (await korisnikManager.IsInRoleAsync(korisnik, uloga.Name))
                {
                    korisnikUlogaVM.Oznacen = true;
                }
                else
                {
                    korisnikUlogaVM.Oznacen = false;
                }
                model.Add(korisnikUlogaVM);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UrediKorisnikaUUlozi(List<KorisnikUlogaVM> model, string id)
        {
            var uloga = await ulogaManager.FindByIdAsync(id);

            if (uloga == null)
            {
                ViewBag.ErrorMessage = $" Uloga sa ID = {id} ne moze biti pronadjena";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++) 
            {
                var korisnik = await korisnikManager.FindByIdAsync(model[i].KorisnikId);

                IdentityResult rezultat = null;

                if (model[i].Oznacen &&  !(await korisnikManager.IsInRoleAsync(korisnik, uloga.Name)))
                {
                    rezultat = await korisnikManager.AddToRoleAsync(korisnik, uloga.Name);
                }
                else if (!model[i].Oznacen && await korisnikManager.IsInRoleAsync(korisnik, uloga.Name))
                {
                    rezultat = await korisnikManager.RemoveFromRoleAsync(korisnik, uloga.Name);
                }
                else
                {
                    continue;
                }

                if (rezultat.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("UrediUlogu", new { Id = id });
                }
            }
            return RedirectToAction("UrediUlogu", new { Id = id });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PekaraKonjic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PekaraKonjic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace PekaraKonjic
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<MojContext>(o => o.UseSqlServer(Configuration.GetConnectionString("Konekcija")));

            //services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<MojContext>();

            services.AddIdentity<Kupac, IdentityRole>().AddEntityFrameworkStores<MojContext>().AddDefaultTokenProviders().AddDefaultUI();

            services.AddScoped<INarudzbaRepozitorij, NarudzbaRepozitorij>();
            services.AddScoped<IKategorijaRepozitorij, KategorijaRepozitorij>();
            services.AddScoped<IProizvodRepozitorij, ProizvodRepozitorij>();
            services.AddScoped<Korpa>(sp => Korpa.GetCart(sp));


            services.AddHttpContextAccessor();
            services.AddSession();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

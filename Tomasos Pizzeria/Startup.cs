using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tomasos_Pizzeria.Contexts;
using Tomasos_Pizzeria.Identity.Identitycontext;
using Tomasos_Pizzeria.Identity.IdentityModels;

namespace Tomasos_Pizzeria
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
            services.AddMvc(options => options.EnableEndpointRouting = false);
            
            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("Identity")));

            services.AddDbContext<TomasosContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("Tomasos")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
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
            }
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routes => 
            { routes.MapRoute(
                name: "default", 
                template: "{controller=Home}/{action=Index}/{id?}"); 
            });

        }
    }
}

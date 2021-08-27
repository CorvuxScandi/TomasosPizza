using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tomasos_Pizzeria.Identity.IdentityModels;

namespace Tomasos_Pizzeria.Identity
{
    public static class IdentityInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager)
        {
            SeedUser(userManager);
        }

        public static void SeedUser(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("administrator").Result == null)
            {
                ApplicationUser admin = new()
                {
                    UserName = "administrator",
                    Email = "admin@example.com",
                };

                IdentityResult result = userManager.CreateAsync(admin, "Admin123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, ApplicationRoles.Admin).Wait();
                }
            }
        }
    }
}
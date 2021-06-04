using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tomasos_Pizzeria.Identity.IdentityModels
{
    public static class ApplicationRoles
    {
        public static string Admin { get; } = "admin";
        public static string Customer { get; } = "customer";

    }
}

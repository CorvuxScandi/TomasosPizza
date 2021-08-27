using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tomasos_Pizzeria.Identity.IdentityModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "A username is required to log in"), Display(Name = "Användarnamn")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "A password is required to log in")]
        [DataType(DataType.Password), Display(Name = "Lösenord")]
        public string Password { get; set; }
    }
}
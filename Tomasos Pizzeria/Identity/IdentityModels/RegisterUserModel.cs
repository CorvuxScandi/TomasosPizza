using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tomasos_Pizzeria.Identity.IdentityModels
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "You need a user name")]
        public string UserName { get; set; }
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required(ErrorMessage = "You need a password")]
        public string Password { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
        public string Phone { get; set; }



    }
}

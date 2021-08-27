using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tomasos_Pizzeria.Models;

namespace Tomasos_Pizzeria.Identity.IdentityModels
{
    public class RegisterUserModel : LoginModel
    {
        public RegisterUserModel()
        {
            Bestallnings = new HashSet<Bestallning>();
        }

        public int KundId { get; set; }

        [Required, Display(Name = "E-Mail"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Required, Display(Name = "Gatuadress")]
        public string Adress { get; set; }

        [Required, Display(Name = "Postkod"), DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

        [Required, Display(Name = "Postort")]
        public string City { get; set; }

        [Required, Display(Name = "Telefonnummer"), DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public virtual ICollection<Bestallning> Bestallnings { get; set; }
    }
}
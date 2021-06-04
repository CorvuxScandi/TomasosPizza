using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Tomasos_Pizzeria.Contexts;
using Tomasos_Pizzeria.Identity.IdentityModels;
using Tomasos_Pizzeria.Models;

namespace Tomasos_Pizzeria.Controllers
{
    public class IdentityController : Controller
    {
        private TomasosContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public IdentityController(UserManager<ApplicationUser> userManager, TomasosContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        // Go to Loginin view
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<ActionResult> Register(RegisterUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                Kund newKund = new()
                {
                    AnvandarNamn = userModel.UserName,
                    Email = userModel.Email,
                    Gatuadress = userModel.Adress,
                    Losenord = userModel.Password,
                    Namn = userModel.FirstName + " " + userModel.LastName,
                    Postnr = userModel.ZipCode,
                    Postort = userModel.City,
                    Telefon = userModel.Phone
                };
                ApplicationUser newUser = new()
                {
                    Email = userModel.Email,
                    UserName = userModel.UserName,
                };

                _context.Add(newKund);

                var result = await _userManager.CreateAsync(newUser, userModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, ApplicationRoles.Customer);
                    return RedirectToAction("Index", "Home");
                }

            }
            return View(userModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(login.UserName);
                if(user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
                    if(result.Succeeded) return RedirectToAction("Shop", "Home");
                }
            }
            ModelState.AddModelError("", "Ogiltligt användarnamn eller lösenord");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}

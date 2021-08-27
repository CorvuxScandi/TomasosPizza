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
            RegisterUserModel model = new();
            return View(model);
        }

        // Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterUserModel userModel)
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
                    _context.SaveChanges();
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
                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Store", "Home");
                    }
                }
            }
            ModelState.AddModelError("password", "Ogiltligt användarnamn eller lösenord");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            foreach (var kund in _context.Kunds)
            {
                if (kund.AnvandarNamn == User.Identity.Name)
                {
                    string fname = kund.Namn.Substring(0, kund.Namn.IndexOf(" "));
                    string lname = kund.Namn[kund.Namn.LastIndexOf(' ')..];
                    RegisterUserModel userModel = new()
                    {
                        Bestallnings = kund.Bestallnings,
                        KundId = kund.KundId,
                        Adress = kund.Gatuadress,
                        City = kund.Postort,
                        Email = kund.Email,
                        FirstName = fname,
                        LastName = lname,
                        Password = kund.Losenord,
                        Phone = kund.Telefon,
                        UserName = kund.AnvandarNamn,
                        ZipCode = kund.Postnr
                    };

                    return View("Register", userModel);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditProfile(RegisterUserModel kund)
        {
            Kund edited = new()
            {
                AnvandarNamn = kund.UserName,
                Namn = kund.FirstName + " " + kund.LastName,
                Bestallnings = kund.Bestallnings,
                Email = kund.Email,
                Gatuadress = kund.Adress,
                KundId = kund.KundId,
                Losenord = kund.Password,
                Postnr = kund.ZipCode,
                Postort = kund.City,
                Telefon = kund.Phone
            };

            _context.Kunds.Update(edited);
            var result = _context.SaveChanges();

            if (result < 1)
            {
                ViewBag.Message = "Serverfel försök igen";
                return View("Register", kund);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
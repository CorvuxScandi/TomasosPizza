using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tomasos_Pizzeria.Contexts;
using Tomasos_Pizzeria.Identity.IdentityModels;
using Tomasos_Pizzeria.Models;
using Tomasos_Pizzeria.Models.ViewModels;

namespace Tomasos_Pizzeria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private TomasosContext _tomasosContext;
        private UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
            TomasosContext tomasosContext,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _tomasosContext = tomasosContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public IActionResult Store()
        {
            return View("MenuPage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(MenuItemModel model)
        {
            var bm = model.BestallningMatratt;
            List<BestallningMatratt> cart;
            string jsonObject = HttpContext.Session.GetString("cart");

            if (jsonObject is null)
            {
                cart = new();
            }
            else
            {
                cart = JsonConvert.DeserializeObject<List<BestallningMatratt>>(jsonObject);
            }

            var existingFoodItem = cart.FirstOrDefault(i => i.MatrattId == bm.MatrattId);
            if (existingFoodItem != null)
            {
                cart.Remove(existingFoodItem);
                bm.Antal += existingFoodItem.Antal;
            }
            cart.Add(bm);

            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));

            return RedirectToAction("Store");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCart(BestallningMatratt bm)
        {
            var cart = JsonConvert.DeserializeObject<List<BestallningMatratt>>(HttpContext.Session.GetString("cart"));

            foreach (var item in cart)
            {
                if (item.MatrattId == bm.MatrattId) item.Antal = bm.Antal;
            }
            HttpContext.Session.Remove("cart");
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));

            return RedirectToAction("Store");
        }

        public async Task<IActionResult> Checkout()
        {
            List<BestallningMatratt> cart;
            string jsonObject = HttpContext.Session.GetString("cart");

            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            string userMail = applicationUser.Email;

            DateTime today = DateTime.Today;
            int kundId = _tomasosContext.Kunds
                    .FirstOrDefault(k => k.Email == userMail).KundId;

            Bestallning bestallning = new()
            {
                BestallningDatum = today,
                KundId = kundId,
                Levererad = false,
                Totalbelopp = 0
            };
            _tomasosContext.Bestallnings.Add(bestallning);

            if (jsonObject is null)
            {
                return RedirectToAction("Store");
            }
            cart = JsonConvert.DeserializeObject<List<BestallningMatratt>>(jsonObject);

            foreach (var item in cart)
            {
                Matratt matratt = _tomasosContext.Matratts.FirstOrDefault(m => m.MatrattId == item.MatrattId);
                bestallning.Totalbelopp += (matratt.Pris * item.Antal);
                item.BestallningId = bestallning.BestallningId;
                item.Matratt = matratt;
                item.Bestallning = bestallning;
                _tomasosContext.BestallningMatratts.Add(item);
            }
            _tomasosContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
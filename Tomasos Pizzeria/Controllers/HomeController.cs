using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, 
            TomasosContext tomasosContext,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _tomasosContext = tomasosContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[Authorize(Roles = "admin, user")]
        public IActionResult Store()
        {

            MenuItemModel model = new()
            {
                Matratts = _tomasosContext.Matratts.ToList(),
                Typs = _tomasosContext.MatrattTyps.ToList(),
                Produkts = _tomasosContext.Produkts.ToList(),
                MatrattsProdukts = _tomasosContext.MatrattProdukts.ToList(),
                ShowAdminControlls = false
            };
            
            if (User.IsInRole(ApplicationRoles.Admin)) model.ShowAdminControlls = true;

            return View("MenuPage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void AddToCart(MenuItemModel model)
        {

            var bm = model.BestallningMatratt;            
            List<BestallningMatratt> cart;
            string jsonObject = HttpContext.Session.GetString("cart");

            if(jsonObject is null)
            {
                cart = new();
            }
            else
            {
                cart = JsonConvert.DeserializeObject<List<BestallningMatratt>>(jsonObject);
            }

            var existingFoodItem = cart.FirstOrDefault(i => i.MatrattId == bm.MatrattId);
            if(existingFoodItem != null)
            {
                cart.Remove(existingFoodItem);
                bm.Antal += existingFoodItem.Antal;
            }
            cart.Add(bm);
            
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            

        }
        public PartialViewResult CartPartial()
        {
            string jsonObject = HttpContext.Session.GetString("cart");

            ShoppingCartViewModel cartModel = new()
            {
                Matratts = _tomasosContext.Matratts.ToList(),
                Cart = JsonConvert.DeserializeObject<List<BestallningMatratt>>(jsonObject)
            };
            return PartialView("_ShoppingCartPartial", cartModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult EditCart(ShoppingCartViewModel model)
        {
            foreach (var item in model.Cart)
            {
                if (item.Antal == 0) model.Cart.Remove(item);
            }
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(model.Cart));

            return PartialView("_ShoppingCartPartial", model );
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout()
        {
            List<BestallningMatratt> cart;
            string jsonObject = HttpContext.Session.GetString("cart");
            Bestallning bestallning = new()
            {
                BestallningDatum = DateTime.Today,
                KundId = _tomasosContext.Kunds
                    .FirstOrDefault(k => k.Email ==
                    _userManager.FindByIdAsync(_userManager.GetUserId(User)).Result.Email).KundId,
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
                _tomasosContext.BestallningMatratts.Add(item);
            }
            _tomasosContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

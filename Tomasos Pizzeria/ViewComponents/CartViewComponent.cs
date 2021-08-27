using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tomasos_Pizzeria.Contexts;
using Tomasos_Pizzeria.Models;
using Newtonsoft.Json;
using Tomasos_Pizzeria.Models.ViewModels;

namespace Tomasos_Pizzeria.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private TomasosContext _context;

        public CartViewComponent(TomasosContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var jsonObject = HttpContext.Session.GetString("cart");
            List<BestallningMatratt> cart = new();

            if (jsonObject != null)
            {
                cart = JsonConvert.DeserializeObject<List<BestallningMatratt>>(jsonObject);
            }

            ShoppingCartViewModel model = new()
            {
                Cart = cart,
                Matratts = _context.Matratts.ToList(),
                BM = new()
            };

            return await Task.FromResult((IViewComponentResult)View("Cart", model));
        }
    }
}
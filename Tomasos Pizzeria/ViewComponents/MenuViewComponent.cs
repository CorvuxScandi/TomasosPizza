using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tomasos_Pizzeria.Contexts;
using Tomasos_Pizzeria.Models;

namespace Tomasos_Pizzeria.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private TomasosContext _tomasosContext;

        public MenuViewComponent(TomasosContext tomasosContext)
        {
            _tomasosContext = tomasosContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            MenuItemModel model = new()
            {
                Matratts = _tomasosContext.Matratts.ToList(),
                Typs = _tomasosContext.MatrattTyps.ToList(),
                Produkts = _tomasosContext.Produkts.ToList(),
                MatrattsProdukts = _tomasosContext.MatrattProdukts.ToList(),
            };

            return await Task.FromResult((IViewComponentResult)View("Menu", model));
        }
    }
}
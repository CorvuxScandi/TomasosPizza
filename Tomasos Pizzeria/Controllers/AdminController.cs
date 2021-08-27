using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private TomasosContext _context;

        public AdminController(TomasosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AdminPage()
        {
            AdminViewModel model = new()
            {
                Produkter = _context.Produkts.ToList(),
                Typer = _context.MatrattTyps.ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewMenuItem(NewMatrattModel matratt)
        {
            Matratt newMat = new()
            {
                Beskrivning = matratt.Beskrivning,
                MatrattNamn = matratt.MatrattNamn,
                MatrattTyp = matratt.MatrattTyp,
                Pris = matratt.Pris
            };
            _context.Matratts.Add(newMat);
            foreach (var id in matratt.ProduktList)
            {
                _context.MatrattProdukts.Add(new() { MatrattId = newMat.MatrattId, ProduktId = id });
            };
            _context.SaveChanges();
            return View("AdminPage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult c(int id)
        {
            _context.Matratts.Remove(_context.Matratts.First(m => m.MatrattId == id));
            _context.SaveChanges();
            return View("AdminPage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewProdukt(NewProduct mp)
        {
            _context.Produkts.Add(new() { ProduktNamn = mp.ProduktNamn });
            _context.SaveChanges();
            return View("AdminPage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewMatrattTyp(NewMatrattTyp mt)
        {
            _context.MatrattTyps.Add(new() { Beskrivning = mt.Beskrivning });
            _context.SaveChanges();
            return View("AdminPage");
        }
    }
}
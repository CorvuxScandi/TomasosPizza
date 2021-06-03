using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tomasos_Pizzeria.Identity.Identitycontext;

namespace Tomasos_Pizzeria.Controllers
{
    public class IdentityController : Controller
    {
        private AuthDbContext _context;

        public IdentityController(AuthDbContext context)
        {
            _context = context;
        }


        // Go to Loginin view
        public ActionResult Login()
        {
            return View();
        }

        // Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Authorize()
        {
            return View();
        }

        // POST: Identity/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Identity/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Identity/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Identity/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Identity/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

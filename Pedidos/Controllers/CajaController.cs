using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Controllers
{
    public class CajaController : Controller
    {
        // GET: CajaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CajaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CajaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CajaController/Create
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

        // GET: CajaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CajaController/Edit/5
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

        // GET: CajaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CajaController/Delete/5
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

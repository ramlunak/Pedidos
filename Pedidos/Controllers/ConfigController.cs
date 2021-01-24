using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Controllers
{
    public class ConfigController : BaseController
    {
        private readonly AppDbContext _context;

        public ConfigController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var model = await _context.P_Config.Where(x => x.idCuenta == Cuenta.id).FirstOrDefaultAsync();

            if (model == null)
            {
                var newconfig = new P_Config();
                newconfig.idCuenta = Cuenta.id;
                newconfig.printSize = 200;
                newconfig.fontSize = 18;

                _context.Add(newconfig);
                await _context.SaveChangesAsync();
                return View(newconfig);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Config config)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            if (id != config.id)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(config);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    PrompErro(ex.ToString());
                }

            }
            PrompSuccess("Dados atualizados corretamente");
            return RedirectToAction(nameof(Index));
        }

    }
}

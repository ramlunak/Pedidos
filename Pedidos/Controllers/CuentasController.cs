using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;

namespace Pedidos.Controllers
{
    public class CuentasController : BaseController
    {
        private readonly AppDbContext _context;

        public CuentasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Cuentas
        public async Task<IActionResult> Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var Cuentas = await _context.P_Cuentas.ToListAsync();
            return View(Cuentas);
        }

        // GET: Cuentas/Create
        public IActionResult Create()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View();
        }

        // POST: Cuentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Cuenta p_Cuenta)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (ModelState.IsValid)
            {
                if (await GetIdByName(p_Cuenta.id, p_Cuenta.usuario) != null)
                {
                    PrompInfo("A conta já existe");
                    return View(p_Cuenta);
                }

                p_Cuenta.activo = true;
                _context.Add(p_Cuenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Cuenta);
        }

        //// GET: Cuentas/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (!ValidarCuenta())
        //    {
        //        return RedirectToAction("Salir", "Login");
        //    }
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var P_Cuentas = await _context.P_Cuentas.FindAsync(id);
        //    if (P_Cuentas == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(P_Cuentas);
        //}

        //// POST: Cuentas/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("id,nombre,idCuenta,activo")] P_Cuentas P_Cuentas)
        //{
        //    if (!ValidarCuenta())
        //    {
        //        return RedirectToAction("Salir", "Login");
        //    }
        //    if (id != P_Cuentas.id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var idIngrediente = await GetIdByName(P_Cuentas.id, P_Cuentas.nombre);
        //        if (idIngrediente != null && idIngrediente != P_Cuentas.id)
        //        {
        //            PrompInfo("O ingrediente já existe");
        //            return View(P_Cuentas);
        //        }
        //        try
        //        {
        //            _context.Update(P_Cuentas);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!P_CuentasExists(P_Cuentas.id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(P_Cuentas);
        //}

        // GET: Cuentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var P_Cuentas = await _context.P_Cuentas.FirstOrDefaultAsync(m => m.id == id);
            if (P_Cuentas == null)
            {
                return NotFound();
            }

            return View(P_Cuentas);
        }

        // POST: Cuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var P_Cuentas = await _context.P_Cuentas.FindAsync(id);
            _context.P_Cuentas.Remove(P_Cuentas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<int?> GetIdByName(int id, string nombre)
        {
            try
            {
                var ingrediente = await _context.P_Cuentas.FirstOrDefaultAsync(e => e.usuario.ToLower() == nombre.ToLower());
                if (ingrediente == null)
                {
                    return null;
                }
                return ingrediente.id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool P_CuentasExists(int id)
        {
            return _context.P_Cuentas.Any(e => e.id == id);
        }
    }
}

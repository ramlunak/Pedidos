using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Extensions;
using Pedidos.Models;

namespace Pedidos.Controllers
{
    public class SalidaCajaController : BaseController
    {
        private readonly AppDbContext _context;

        public SalidaCajaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SalidaCaja
        public async Task<IActionResult> Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var salidas = await _context.P_SalidaCaja.Where(x => x.idCuenta == Cuenta.id && !x.idCaja.HasValue).ToListAsync();
            foreach (var item in salidas)
            {
                item.motivo = _context.P_MotivoSalidaCaja.FindAsync(item.id).Result.motivo;
            }

            return View(salidas);
        }

        // GET: SalidaCaja/Create
        public async Task<IActionResult> Create()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var motivos = await _context.P_MotivoSalidaCaja.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            ViewBag.Motivos = motivos;

            return View();
        }

        // POST: SalidaCaja/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_SalidaCaja p_SalidaCaja)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (ModelState.IsValid)
            {

                p_SalidaCaja.idCuenta = Cuenta.id;
                p_SalidaCaja.activo = true;
                p_SalidaCaja.fecha = DateTime.Now.ToSouthAmericaStandard();
                _context.Add(p_SalidaCaja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var motivos = await _context.P_MotivoSalidaCaja.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            ViewBag.Motivos = motivos;
            return View(p_SalidaCaja);
        }

        // GET: SalidaCaja/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var p_SalidaCaja = await _context.P_SalidaCaja.FindAsync(id);
            if (p_SalidaCaja == null)
            {
                return NotFound();
            }
            return View(p_SalidaCaja);
        }

        // POST: SalidaCaja/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_SalidaCaja p_SalidaCaja)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id != p_SalidaCaja.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_SalidaCaja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_SalidaCajaExists(p_SalidaCaja.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(p_SalidaCaja);
        }

        // GET: SalidaCaja/Delete/5
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

            var p_SalidaCaja = await _context.P_SalidaCaja
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_SalidaCaja == null)
            {
                return NotFound();
            }

            return View(p_SalidaCaja);
        }

        // POST: SalidaCaja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var p_SalidaCaja = await _context.P_SalidaCaja.FindAsync(id);
            _context.P_SalidaCaja.Remove(p_SalidaCaja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_SalidaCajaExists(int id)
        {
            return _context.P_SalidaCaja.Any(e => e.id == id);
        }
    }
}

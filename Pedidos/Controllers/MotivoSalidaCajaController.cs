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
    public class MotivoSalidaCajaController : BaseController
    {
        private readonly AppDbContext _context;

        public MotivoSalidaCajaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: MotivoSalidaCaja
        public async Task<IActionResult> Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View(await _context.P_MotivoSalidaCaja.Where(x => x.idCuenta == Cuenta.id).ToListAsync());
        }

        // GET: MotivoSalidaCaja/Create
        public IActionResult Create()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View();
        }

        // POST: MotivoSalidaCaja/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_MotivoSalidaCaja p_MotivoSalidaCaja)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (ModelState.IsValid)
            {

                p_MotivoSalidaCaja.idCuenta = Cuenta.id;
                p_MotivoSalidaCaja.activo = true;
                _context.Add(p_MotivoSalidaCaja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_MotivoSalidaCaja);
        }

        // GET: MotivoSalidaCaja/Edit/5
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

            var p_MotivoSalidaCaja = await _context.P_MotivoSalidaCaja.FindAsync(id);
            if (p_MotivoSalidaCaja == null)
            {
                return NotFound();
            }
            return View(p_MotivoSalidaCaja);
        }

        // POST: MotivoSalidaCaja/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_MotivoSalidaCaja p_MotivoSalidaCaja)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id != p_MotivoSalidaCaja.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_MotivoSalidaCaja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_MotivoSalidaCajaExists(p_MotivoSalidaCaja.id))
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
            return View(p_MotivoSalidaCaja);
        }

        // GET: MotivoSalidaCaja/Delete/5
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

            var p_MotivoSalidaCaja = await _context.P_MotivoSalidaCaja
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_MotivoSalidaCaja == null)
            {
                return NotFound();
            }

            return View(p_MotivoSalidaCaja);
        }

        // POST: MotivoSalidaCaja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var p_MotivoSalidaCaja = await _context.P_MotivoSalidaCaja.FindAsync(id);
            _context.P_MotivoSalidaCaja.Remove(p_MotivoSalidaCaja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_MotivoSalidaCajaExists(int id)
        {
            return _context.P_MotivoSalidaCaja.Any(e => e.id == id);
        }
    }
}

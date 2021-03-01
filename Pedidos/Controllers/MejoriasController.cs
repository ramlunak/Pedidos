using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;

namespace Pedidos.Controllers
{
    [Authorize(Roles = "Administrador,Establecimiento,Funcionario,Integracion")]
    public class MejoriasController : BaseController
    {
        private readonly AppDbContext _context;

        public MejoriasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Mejorias
        public async Task<IActionResult> Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View(await _context.P_Mejorias.Where(x => x.idCuenta == Cuenta.id).ToListAsync());
        }

        // GET: Mejorias/Create
        public IActionResult Create()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View();
        }

        // POST: Mejorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Mejorias p_Mejorias)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (ModelState.IsValid)
            {
                var count = await _context.P_Mejorias.Where(x => x.idCuenta == Cuenta.id).CountAsync();
                var codigo = $"M{Cuenta.id}-{++count}";
                p_Mejorias.codigo = codigo;

                p_Mejorias.idCuenta = Cuenta.id;
                p_Mejorias.status = "activo";
                _context.Add(p_Mejorias);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Mejorias);
        }

        // GET: Mejorias/Edit/5
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

            var p_Mejorias = await _context.P_Mejorias.FindAsync(id);
            if (p_Mejorias == null)
            {
                return NotFound();
            }
            return View(p_Mejorias);
        }

        // POST: Mejorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Mejorias p_Mejorias)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id != p_Mejorias.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Mejorias);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_MejoriasExists(p_Mejorias.id))
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
            return View(p_Mejorias);
        }

        // GET: Mejorias/Delete/5
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

            var p_Mejorias = await _context.P_Mejorias
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Mejorias == null)
            {
                return NotFound();
            }

            return View(p_Mejorias);
        }

        // POST: Mejorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var p_Mejorias = await _context.P_Mejorias.FindAsync(id);
            _context.P_Mejorias.Remove(p_Mejorias);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_MejoriasExists(int id)
        {
            return _context.P_Mejorias.Any(e => e.id == id);
        }
    }
}

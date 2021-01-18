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
    public class IngredientesController : BaseController
    {
        private readonly AppDbContext _context;

        public IngredientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Ingredientes
        public async Task<IActionResult> Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var ingredientes = await _context.P_Ingredientes.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            var model = ingredientes.OrderBy(x => x.nombre);
            return View(model);
        }

        // GET: Ingredientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var p_Ingredientes = await _context.P_Ingredientes
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Ingredientes == null)
            {
                return NotFound();
            }

            return View(p_Ingredientes);
        }

        // GET: Ingredientes/Create
        public IActionResult Create()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View();
        }

        // POST: Ingredientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombre,idCuenta,activo")] P_Ingredientes p_Ingredientes)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (ModelState.IsValid)
            {
                if (await GetIdByName(p_Ingredientes.id, p_Ingredientes.nombre) != null)
                {
                    PrompInfo("O ingrediente já existe");
                    return View(p_Ingredientes);
                }

                p_Ingredientes.idCuenta = Cuenta.id;
                p_Ingredientes.activo = true;
                _context.Add(p_Ingredientes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Ingredientes);
        }

        // GET: Ingredientes/Edit/5
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

            var p_Ingredientes = await _context.P_Ingredientes.FindAsync(id);
            if (p_Ingredientes == null)
            {
                return NotFound();
            }
            return View(p_Ingredientes);
        }

        // POST: Ingredientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nombre,idCuenta,activo")] P_Ingredientes p_Ingredientes)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id != p_Ingredientes.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var idIngrediente = await GetIdByName(p_Ingredientes.id, p_Ingredientes.nombre);
                if (idIngrediente != null && idIngrediente != p_Ingredientes.id)
                {
                    PrompInfo("O ingrediente já existe");
                    return View(p_Ingredientes);
                }
                try
                {
                    _context.Update(p_Ingredientes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_IngredientesExists(p_Ingredientes.id))
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
            return View(p_Ingredientes);
        }

        // GET: Ingredientes/Delete/5
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

            var p_Ingredientes = await _context.P_Ingredientes
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Ingredientes == null)
            {
                return NotFound();
            }

            return View(p_Ingredientes);
        }

        // POST: Ingredientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var p_Ingredientes = await _context.P_Ingredientes.FindAsync(id);
            _context.P_Ingredientes.Remove(p_Ingredientes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<int?> GetIdByName(int id, string nombre)
        {
            try
            {
                var ingrediente = await _context.P_Ingredientes.FirstOrDefaultAsync(e => e.nombre.ToLower() == nombre.ToLower() && e.idCuenta == Cuenta.id);
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

        private bool P_IngredientesExists(int id)
        {
            return _context.P_Ingredientes.Any(e => e.id == id);
        }
    }
}

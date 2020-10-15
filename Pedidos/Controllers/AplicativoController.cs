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
    public class AplicativoController : Controller
    {
        private readonly AppDbContext _context;

        public AplicativoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Aplicativo
        public async Task<IActionResult> Index()
        {
            return View(await _context.P_Aplicativo.ToListAsync());
        }

        // GET: Aplicativo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Aplicativo = await _context.P_Aplicativo
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Aplicativo == null)
            {
                return NotFound();
            }

            return View(p_Aplicativo);
        }

        // GET: Aplicativo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aplicativo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombre,idCuenta,activo")] P_Aplicativo p_Aplicativo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(p_Aplicativo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Aplicativo);
        }

        // GET: Aplicativo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Aplicativo = await _context.P_Aplicativo.FindAsync(id);
            if (p_Aplicativo == null)
            {
                return NotFound();
            }
            return View(p_Aplicativo);
        }

        // POST: Aplicativo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nombre,idCuenta,activo")] P_Aplicativo p_Aplicativo)
        {
            if (id != p_Aplicativo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Aplicativo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_AplicativoExists(p_Aplicativo.id))
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
            return View(p_Aplicativo);
        }

        // GET: Aplicativo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Aplicativo = await _context.P_Aplicativo
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Aplicativo == null)
            {
                return NotFound();
            }

            return View(p_Aplicativo);
        }

        // POST: Aplicativo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p_Aplicativo = await _context.P_Aplicativo.FindAsync(id);
            _context.P_Aplicativo.Remove(p_Aplicativo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_AplicativoExists(int id)
        {
            return _context.P_Aplicativo.Any(e => e.id == id);
        }
    }
}

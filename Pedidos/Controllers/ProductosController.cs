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
    public class ProductosController : BaseController
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            ValidarCuenta();
            return View(await _context.P_Productos.Where(x => x.idCuenta == Cuenta.id).ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Productos = await _context.P_Productos
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Productos == null)
            {
                return NotFound();
            }

            return View(p_Productos);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ValidarCuenta();
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,codigo,nombre,idCategoria,idCuenta,activo")] P_Productos p_Productos)
        {
            ValidarCuenta();
            if (ModelState.IsValid)
            {
                _context.Add(p_Productos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Productos);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Productos = await _context.P_Productos.FindAsync(id);
            if (p_Productos == null)
            {
                return NotFound();
            }
            return View(p_Productos);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,codigo,nombre,idCategoria,idCuenta,activo")] P_Productos p_Productos)
        {
            ValidarCuenta();
            if (id != p_Productos.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Productos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_ProductosExists(p_Productos.id))
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
            return View(p_Productos);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Productos = await _context.P_Productos
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Productos == null)
            {
                return NotFound();
            }

            return View(p_Productos);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ValidarCuenta();
            var p_Productos = await _context.P_Productos.FindAsync(id);
            _context.P_Productos.Remove(p_Productos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_ProductosExists(int id)
        {
            ValidarCuenta();
            return _context.P_Productos.Any(e => e.id == id);
        }
    }
}

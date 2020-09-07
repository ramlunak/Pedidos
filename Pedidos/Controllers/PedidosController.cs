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
    public class PedidosController : BaseController
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            ValidarCuenta();
            return View(await _context.P_Pedidos.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ValidarCuenta();

            if (id == null)
            {
                return NotFound();
            }

            var p_Pedido = await _context.P_Pedidos
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Pedido == null)
            {
                return NotFound();
            }

            return View(p_Pedido);
        }

        // GET: Pedidos/Create
        public async Task<IActionResult> Create()
        {
            ValidarCuenta();
            ViewBag.Productos = await _context.P_Productos.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Pedido p_Pedido)
        {
            ValidarCuenta();

            if (ModelState.IsValid)
            {
                _context.Add(p_Pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProducto(int idProducto)
        {
            ValidarCuenta();
            return RedirectToAction(nameof(Create));
            ;
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ValidarCuenta();

            if (id == null)
            {
                return NotFound();
            }

            var p_Pedido = await _context.P_Pedidos.FindAsync(id);
            if (p_Pedido == null)
            {
                return NotFound();
            }
            return View(p_Pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Pedido p_Pedido)
        {
            ValidarCuenta();

            if (id != p_Pedido.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_PedidoExists(p_Pedido.id))
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
            return View(p_Pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ValidarCuenta();

            if (id == null)
            {
                return NotFound();
            }

            var p_Pedido = await _context.P_Pedidos
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Pedido == null)
            {
                return NotFound();
            }

            return View(p_Pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ValidarCuenta();

            var p_Pedido = await _context.P_Pedidos.FindAsync(id);
            _context.P_Pedidos.Remove(p_Pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_PedidoExists(int id)
        {
            ValidarCuenta();

            return _context.P_Pedidos.Any(e => e.id == id);
        }
    }
}

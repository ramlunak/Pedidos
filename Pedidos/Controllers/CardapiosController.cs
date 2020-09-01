using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class CardapiosController : BaseController
    {
        private readonly AppDbContext _context;

        public CardapiosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Cardapios
        public async Task<IActionResult> Index()
        {
            return View(await _context.P_Cardapios.ToListAsync());
        }

        // GET: Cardapios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Cardapio = await _context.P_Cardapios
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Cardapio == null)
            {
                return NotFound();
            }

            return View(p_Cardapio);
        }

        // GET: Cardapios/Create
        public async Task<IActionResult> Create()
        {
            ValidarCuenta();
            ViewBag.Productos = await _context.P_Productos.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            return View(new P_Cardapio());
        }

        // POST: Cardapios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Cardapio p_Cardapio)
        {
            if (ModelState.IsValid)
            {
                p_Cardapio.valor = p_Cardapio.strValor.ToDecimal();
                p_Cardapio.idCuenta = Cuenta.id;
                _context.Add(p_Cardapio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Productos = await _context.P_Productos.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            return View(p_Cardapio);
        }

        // GET: Cardapios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Cardapio = await _context.P_Cardapios.FindAsync(id);
            if (p_Cardapio == null)
            {
                return NotFound();
            }
            return View(p_Cardapio);
        }

        // POST: Cardapios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Cardapio p_Cardapio)
        {
            if (id != p_Cardapio.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    p_Cardapio.valor = p_Cardapio.strValor.ToDecimal();
                    _context.Update(p_Cardapio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_CardapioExists(p_Cardapio.id))
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
            return View(p_Cardapio);
        }

        // GET: Cardapios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Cardapio = await _context.P_Cardapios
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Cardapio == null)
            {
                return NotFound();
            }

            return View(p_Cardapio);
        }

        // POST: Cardapios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p_Cardapio = await _context.P_Cardapios.FindAsync(id);
            _context.P_Cardapios.Remove(p_Cardapio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_CardapioExists(int id)
        {
            return _context.P_Cardapios.Any(e => e.id == id);
        }
    }
}

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
    public class FormaPagamentoController : Controller
    {
        private readonly AppDbContext _context;

        public FormaPagamentoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FormaPagamento
        public async Task<IActionResult> Index()
        {
            return View(await _context.P_FormaPagamento.ToListAsync());
        }

        // GET: FormaPagamento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_FormaPagamento = await _context.P_FormaPagamento
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_FormaPagamento == null)
            {
                return NotFound();
            }

            return View(p_FormaPagamento);
        }

        // GET: FormaPagamento/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FormaPagamento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombre,idCuenta,activo")] P_FormaPagamento p_FormaPagamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(p_FormaPagamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_FormaPagamento);
        }

        // GET: FormaPagamento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_FormaPagamento = await _context.P_FormaPagamento.FindAsync(id);
            if (p_FormaPagamento == null)
            {
                return NotFound();
            }
            return View(p_FormaPagamento);
        }

        // POST: FormaPagamento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nombre,idCuenta,activo")] P_FormaPagamento p_FormaPagamento)
        {
            if (id != p_FormaPagamento.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_FormaPagamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_FormaPagamentoExists(p_FormaPagamento.id))
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
            return View(p_FormaPagamento);
        }

        // GET: FormaPagamento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_FormaPagamento = await _context.P_FormaPagamento
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_FormaPagamento == null)
            {
                return NotFound();
            }

            return View(p_FormaPagamento);
        }

        // POST: FormaPagamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p_FormaPagamento = await _context.P_FormaPagamento.FindAsync(id);
            _context.P_FormaPagamento.Remove(p_FormaPagamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_FormaPagamentoExists(int id)
        {
            return _context.P_FormaPagamento.Any(e => e.id == id);
        }
    }
}

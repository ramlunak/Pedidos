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
    public class FormaPagamentoController : BaseController
    {
        private readonly AppDbContext _context;

        public FormaPagamentoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FormaPagamento
        public async Task<IActionResult> Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var formasPagamento = await _context.P_FormaPagamento.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            var model = formasPagamento.OrderBy(x=>x.nombre); 
            return View(model);
        }

        // GET: FormaPagamento/Details/5
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
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View();
        }

        // POST: FormaPagamento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_FormaPagamento p_FormaPagamento)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (ModelState.IsValid)
            {
                p_FormaPagamento.idCuenta = Cuenta.id;
                p_FormaPagamento.activo = true;

                _context.Add(p_FormaPagamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_FormaPagamento);
        }

        // GET: FormaPagamento/Edit/5
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
        public async Task<IActionResult> Edit(int id,P_FormaPagamento p_FormaPagamento)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
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
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
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
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
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

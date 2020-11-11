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
    public class AdicionaisController : BaseController
    {
        private readonly AppDbContext _context;

        public AdicionaisController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Adicionais
        public async Task<IActionResult> Index()
        {
            return View(await _context.P_Adicionais.ToListAsync());
        }

        // GET: Adicionais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Adicionais = await _context.P_Adicionais
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Adicionais == null)
            {
                return NotFound();
            }

            return View(p_Adicionais);
        }

        // GET: Adicionais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Adicionais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Adicionais p_Adicionais)
        {
            ValidarCuenta();
            if (ModelState.IsValid)
            {
                p_Adicionais.idCuenta = Cuenta.id;
                p_Adicionais.activo = true;

                _context.Add(p_Adicionais);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Adicionais);
        }

        // GET: Adicionais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Adicionais = await _context.P_Adicionais.FindAsync(id);
            if (p_Adicionais == null)
            {
                return NotFound();
            }
            return View(p_Adicionais);
        }

        // POST: Adicionais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,P_Adicionais p_Adicionais)
        {
            if (id != p_Adicionais.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Adicionais);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_AdicionaisExists(p_Adicionais.id))
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
            return View(p_Adicionais);
        }

        // GET: Adicionais/Delete/5
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Adicionais = await _context.P_Adicionais.FirstOrDefaultAsync(m => m.id == id);
            if (p_Adicionais == null)
            {
                return NotFound();
            }

            try
            {
                p_Adicionais.activo = !p_Adicionais.activo;
                _context.Update(p_Adicionais);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Ok(true);
        }

        // GET: Adicionais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Adicionais = await _context.P_Adicionais
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Adicionais == null)
            {
                return NotFound();
            }

            return Ok(true);
        }

        // POST: Adicionais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p_Adicionais = await _context.P_Adicionais.FindAsync(id);
            _context.P_Adicionais.Remove(p_Adicionais);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_AdicionaisExists(int id)
        {
            return _context.P_Adicionais.Any(e => e.id == id);
        }
    }
}

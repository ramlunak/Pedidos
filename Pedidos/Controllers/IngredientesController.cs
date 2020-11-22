﻿using System;
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
            return View(await _context.P_Ingredientes.ToListAsync());
        }

        // GET: Ingredientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
            return View();
        }

        // POST: Ingredientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombre,idCuenta,activo")] P_Ingredientes p_Ingredientes)
        {
            if (ModelState.IsValid)
            {
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
            if (id != p_Ingredientes.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            var p_Ingredientes = await _context.P_Ingredientes.FindAsync(id);
            _context.P_Ingredientes.Remove(p_Ingredientes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_IngredientesExists(int id)
        {
            return _context.P_Ingredientes.Any(e => e.id == id);
        }
    }
}
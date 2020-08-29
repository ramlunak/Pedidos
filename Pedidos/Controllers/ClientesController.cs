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
    public class ClientesController : BaseController
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            ValidarCuenta();
            return View(await _context.P_Clientes.Where(x => x.idCuenta == Cuenta.id).ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Cliente = await _context.P_Clientes
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Cliente == null)
            {
                return NotFound();
            }

            return View(p_Cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ValidarCuenta();
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( P_Cliente p_Cliente)
        {
            ValidarCuenta();
            if (ModelState.IsValid)
            {
                p_Cliente.idCuenta = Cuenta.id;
                _context.Add(p_Cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Cliente = await _context.P_Clientes.FindAsync(id);
            if (p_Cliente == null)
            {
                return NotFound();
            }
            return View(p_Cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,telefono,nombre,idCuenta,activo")] P_Cliente p_Cliente)
        {
            ValidarCuenta();
            if (id != p_Cliente.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_ClienteExists(p_Cliente.id))
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
            return View(p_Cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Cliente = await _context.P_Clientes
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Cliente == null)
            {
                return NotFound();
            }

            return View(p_Cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ValidarCuenta();
            var p_Cliente = await _context.P_Clientes.FindAsync(id);
            _context.P_Clientes.Remove(p_Cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_ClienteExists(int id)
        {
            ValidarCuenta();
            return _context.P_Clientes.Any(e => e.id == id);
        }
    }
}
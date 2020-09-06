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
    public class DireccionesController : Controller
    {
        private readonly AppDbContext _context;

        public DireccionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Direcciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.P_Direcciones.ToListAsync());
        }

        // GET: Direcciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Direcciones = await _context.P_Direcciones
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Direcciones == null)
            {
                return NotFound();
            }

            return View(p_Direcciones);
        }

        // GET: Direcciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Direcciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,code,state,city,district,address,numero,complemento,idCliente,idPedido,idCuenta,activo")] P_Direcciones p_Direcciones)
        {
            if (ModelState.IsValid)
            {
                _context.Add(p_Direcciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Direcciones);
        }

        // GET: Direcciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Direcciones = await _context.P_Direcciones.FindAsync(id);
            if (p_Direcciones == null)
            {
                return NotFound();
            }
            return View(p_Direcciones);
        }

        // POST: Direcciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,code,state,city,district,address,numero,complemento,idCliente,idPedido,idCuenta,activo")] P_Direcciones p_Direcciones)
        {
            if (id != p_Direcciones.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Direcciones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_DireccionesExists(p_Direcciones.id))
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
            return View(p_Direcciones);
        }

        // GET: Direcciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Direcciones = await _context.P_Direcciones
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Direcciones == null)
            {
                return NotFound();
            }

            return View(p_Direcciones);
        }

        // POST: Direcciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p_Direcciones = await _context.P_Direcciones.FindAsync(id);
            _context.P_Direcciones.Remove(p_Direcciones);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_DireccionesExists(int id)
        {
            return _context.P_Direcciones.Any(e => e.id == id);
        }
    }
}

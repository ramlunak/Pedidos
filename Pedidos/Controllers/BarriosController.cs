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
    public class BarriosController : BaseController
    {
        private readonly AppDbContext _context;

        public BarriosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Barrios
        public async Task<IActionResult> Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var Barrios = await _context.P_Barrios.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            var model = Barrios.OrderBy(x => x.nombre);
            return View(model);
        }

        // GET: Barrios/Create
        public IActionResult Create()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View();
        }

        // POST: Barrios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Barrio p_Barrios)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (ModelState.IsValid)
            {
                if (await GetIdByName(p_Barrios.id, p_Barrios.nombre, p_Barrios.municipio, p_Barrios.estado) != null)
                {
                    PrompInfo("O Barrio já existe");
                    return View(p_Barrios);
                }

                p_Barrios.idCuenta = Cuenta.id;
                p_Barrios.activo = true;
                _context.Add(p_Barrios);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Barrios);
        }

        // GET: Barrios/Edit/5
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

            var p_Barrios = await _context.P_Barrios.FindAsync(id);
            if (p_Barrios == null)
            {
                return NotFound();
            }
            return View(p_Barrios);
        }

        // POST: Barrios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Barrio p_Barrios)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id != p_Barrios.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var idBarrio = await GetIdByName(p_Barrios.id, p_Barrios.nombre, p_Barrios.municipio, p_Barrios.estado);
                if (idBarrio != null && idBarrio != p_Barrios.id)
                {
                    PrompInfo("O Barrio já existe");
                    return View(p_Barrios);
                }
                try
                {
                    var entidad = await _context.P_Barrios.FindAsync(id);
                    entidad.estado = p_Barrios.estado;
                    entidad.municipio = p_Barrios.municipio;
                    entidad.nombre = p_Barrios.nombre;
                    entidad.tasa = p_Barrios.tasa;
                    entidad.activo = p_Barrios.activo;

                    _context.Update(entidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_BarriosExists(p_Barrios.id))
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
            return View(p_Barrios);
        }

        // GET: Barrios/Delete/5
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

            var p_Barrios = await _context.P_Barrios
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Barrios == null)
            {
                return NotFound();
            }

            return View(p_Barrios);
        }

        // POST: Barrios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var p_Barrios = await _context.P_Barrios.FindAsync(id);
            _context.P_Barrios.Remove(p_Barrios);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<int?> GetIdByName(int id, string nombre, string municipio, string estado)
        {
            try
            {
                var barrio = await _context.P_Barrios.FirstOrDefaultAsync(e => e.nombre.ToLower() == nombre.ToLower().Trim() && e.municipio.ToLower() == municipio.ToLower().Trim() && e.estado.ToLower() == estado.ToLower().Trim() && e.idCuenta == Cuenta.id);
                if (barrio == null)
                {
                    return null;
                }
                return barrio.id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool P_BarriosExists(int id)
        {
            return _context.P_Barrios.Any(e => e.id == id);
        }
    }
}

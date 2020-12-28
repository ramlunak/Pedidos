using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;

namespace Pedidos.Controllers
{
    public class DireccionesController : BaseController
    {
        private readonly AppDbContext _context;

        public DireccionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Direcciones
        public async Task<IActionResult> Index(int? idCliente, string nombre, int pagina = 1)
        {

            ValidarCuenta();
            var cantidadRegistrosPorPagina = 3; // parámetro

            var Skip = ((pagina - 1) * cantidadRegistrosPorPagina);
            var sql = SqlConsultas.GetSqlAllDirecciones(Cuenta.id, idCliente is null ? 0 : idCliente.Value, Skip, cantidadRegistrosPorPagina, nombre);

            var lista = await _context.P_Direcciones.FromSqlRaw(sql).ToListAsync();

            var totalDeRegistros = 0;
            if (nombre is null)
            {
                totalDeRegistros = await _context.P_Direcciones.Where(x => x.idCuenta == Cuenta.id && x.idCliente == idCliente.Value).CountAsync();
            }
            else
            {
                totalDeRegistros = await _context.P_Direcciones.Where(x => x.idCuenta == Cuenta.id && x.idCliente == idCliente.Value && x.address.Contains(nombre)).CountAsync();
            }

            ViewBag.idCliente = idCliente;
            ViewBag.FlrNombre = nombre;

            var modelo = new ViewModels.VMDirecciones();
            modelo.Direcciones = lista;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new RouteValueDictionary();
            modelo.ValoresQueryString["pagina"] = pagina;
            modelo.ValoresQueryString["nombre"] = nombre;
            modelo.ValoresQueryString["idCliente"] = idCliente;

            return View(modelo);
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
        public IActionResult Create(int? idCliente)
        {
            ValidarCuenta();

            //Cargar configuracion de la cuenta
            var newDireccion = new P_Direcciones();
            newDireccion.state = Cuenta.estado;
            newDireccion.city = Cuenta.municipio;
            newDireccion.idCliente = idCliente;

            return View(newDireccion);
        }

        // POST: Direcciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Direcciones p_Direcciones)
        {
            ValidarCuenta();
            if (ModelState.IsValid)
            {
                p_Direcciones.idCuenta = Cuenta.id;
                _context.Add(p_Direcciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { p_Direcciones.idCliente });
            }
            return View(p_Direcciones);
        }

        // GET: Direcciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ValidarCuenta();
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
        public async Task<IActionResult> Edit(int id, P_Direcciones p_Direcciones, int? pagina)
        {
            ValidarCuenta();
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
                return RedirectToAction(nameof(Index), new { p_Direcciones.idCliente, pagina });
            }
            return View(p_Direcciones);
        }

        // GET: Direcciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ValidarCuenta();
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
            ValidarCuenta();
            var p_Direcciones = await _context.P_Direcciones.FindAsync(id);
            _context.P_Direcciones.Remove(p_Direcciones);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { p_Direcciones.idCliente });
        }

        private bool P_DireccionesExists(int id)
        {
            return _context.P_Direcciones.Any(e => e.id == id);
        }
    }
}

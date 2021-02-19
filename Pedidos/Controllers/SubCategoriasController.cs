using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;

namespace Pedidos.Controllers
{
    [Authorize(Roles = "Administrador,Establecimiento")]
    public class SubCategoriasController : BaseController
    {
        private readonly AppDbContext _context;

        public SubCategoriasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SubCategorias
        public async Task<IActionResult> Index(int? idCategoria, string nombre, int pagina = 1)
        {

            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var cantidadRegistrosPorPagina = 3; // parámetro

            var Skip = ((pagina - 1) * cantidadRegistrosPorPagina);
            var sql = SqlConsultas.GetSqlAllSubCategorias(Cuenta.id, idCategoria is null ? 0 : idCategoria.Value, Skip, cantidadRegistrosPorPagina, nombre);

            var lista = await _context.P_SubCategorias.FromSqlRaw(sql).ToListAsync();

            var totalDeRegistros = 0;
            if (nombre is null)
            {
                totalDeRegistros = await _context.P_SubCategorias.Where(x => x.idCuenta == Cuenta.id && x.idCategoria == idCategoria.Value).CountAsync();
            }
            else
            {
                totalDeRegistros = await _context.P_SubCategorias.Where(x => x.idCuenta == Cuenta.id && x.idCategoria == idCategoria.Value && x.nombre.Contains(nombre)).CountAsync();
            }

            ViewBag.idCategoria = idCategoria;
            ViewBag.FlrNombre = nombre;

            var modelo = new ViewModels.VMSubCategorias();
            modelo.SubCategorias = lista;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new RouteValueDictionary();
            modelo.ValoresQueryString["pagina"] = pagina;
            modelo.ValoresQueryString["nombre"] = nombre;
            modelo.ValoresQueryString["idCategoria"] = idCategoria;

            return View(modelo);
        }

        // GET: SubCategorias/Details/5
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

            var p_SubCategoria = await _context.P_SubCategorias
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_SubCategoria == null)
            {
                return NotFound();
            }

            return View(p_SubCategoria);
        }

        // GET: SubCategorias/Create
        public IActionResult Create(int? idCategoria)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            ViewBag.idCategoria = idCategoria;
            return View(new P_SubCategoria());
        }

        // POST: SubCategorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_SubCategoria p_SubCategoria)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (ModelState.IsValid)
            {
                p_SubCategoria.idCuenta = Cuenta.id;
                _context.Add(p_SubCategoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { p_SubCategoria.idCategoria});
            }
            return View(p_SubCategoria);
        }

        // GET: SubCategorias/Edit/5
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

            var p_SubCategoria = await _context.P_SubCategorias.FindAsync(id);
            if (p_SubCategoria == null)
            {
                return NotFound();
            }
            return View(p_SubCategoria);
        }

        // POST: SubCategorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_SubCategoria p_SubCategoria,int? pagina)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id != p_SubCategoria.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_SubCategoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_SubCategoriaExists(p_SubCategoria.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),new {p_SubCategoria.idCategoria,pagina});
            }
            return View(p_SubCategoria);
        }

        // GET: SubCategorias/Delete/5
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

            var p_SubCategoria = await _context.P_SubCategorias
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_SubCategoria == null)
            {
                return NotFound();
            }

            return View(p_SubCategoria);
        }

        // POST: SubCategorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var p_SubCategoria = await _context.P_SubCategorias.FindAsync(id);
            _context.P_SubCategorias.Remove(p_SubCategoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { p_SubCategoria.idCategoria });
        }

        private bool P_SubCategoriaExists(int id)
        {
            return _context.P_SubCategorias.Any(e => e.id == id);
        }
    }
}

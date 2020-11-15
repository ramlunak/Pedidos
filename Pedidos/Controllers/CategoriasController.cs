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
    public class CategoriasController : BaseController
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string nombre, int pagina = 1)
        {
            ValidarCuenta();
            var cantidadRegistrosPorPagina = 10; // parámetro

            var Skip = ((pagina - 1) * cantidadRegistrosPorPagina);
            var sql = SqlConsultas.GetSqlAllCategorias(Cuenta.id, Skip, cantidadRegistrosPorPagina, nombre);

            var lista = await _context.P_Categorias.FromSqlRaw(sql).ToListAsync();

            var totalDeRegistros = 0;
            if (nombre is null)
            {
                totalDeRegistros = await _context.P_Categorias.Where(x => x.idCuenta == Cuenta.id).CountAsync();
            }
            else
            {
                totalDeRegistros = await _context.P_Categorias.Where(x => x.idCuenta == Cuenta.id && x.nombre.Contains(nombre)).CountAsync();
            }

            ViewBag.FlrNombre = nombre;

            var modelo = new ViewModels.VMCategorias();
            modelo.Categorias = lista;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new RouteValueDictionary();
            modelo.ValoresQueryString["pagina"] = pagina;
            modelo.ValoresQueryString["nombre"] = nombre;

            return View(modelo);
        }

        public IActionResult Create()
        {
            ValidarCuenta();
            return View();
        }

        public async Task<IEnumerable<SelectListItem>> GetSubCategarias(int idCategoria)
        {
            ValidarCuenta();

            var subCategorias = await _context.P_SubCategorias.Where(x => x.idCategoria == idCategoria).Select(x => new SelectListItem
            {
                Text = x.nombre,
                Value = x.id.ToString()
            }).ToListAsync();

            return subCategorias;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Categoria p_Categoria)
        {
            ValidarCuenta();
            if (ModelState.IsValid)
            {
                p_Categoria.idCuenta = Cuenta.id;

                _context.Add(p_Categoria);
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync($"EXEC InsertIfNotExistCategoriaAdicional  @idCategoria = {p_Categoria.id},@idCuenta = {Cuenta.id}");

                return RedirectToAction(nameof(Index));
            }
            return View(p_Categoria);
        }

        public async Task<IActionResult> Edit(int? id, int? pagina)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Categoria = await _context.P_Categorias.FindAsync(id);
            if (p_Categoria == null)
            {
                return NotFound();
            }

            ViewBag.Pagina = pagina;
            return View(p_Categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Categoria p_Categoria, int? pagina)
        {
            ValidarCuenta();

            if (id != p_Categoria.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Categoria);                  
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_CategoriaExists(p_Categoria.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { pagina });
            }
            return View(p_Categoria);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            ValidarCuenta();

            if (id == null)
            {
                return NotFound();
            }

            var p_Categoria = await _context.P_Categorias
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Categoria == null)
            {
                return NotFound();
            }

            return View(p_Categoria);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ValidarCuenta();
            var p_Categoria = await _context.P_Categorias.FindAsync(id);
            _context.P_Categorias.Remove(p_Categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_CategoriaExists(int id)
        {
            ValidarCuenta();
            return _context.P_Categorias.Any(e => e.id == id);
        }
    }
}

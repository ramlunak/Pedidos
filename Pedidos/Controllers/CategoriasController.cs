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
    public class CategoriasController : BaseController
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string nombre, int pagina = 1)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
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
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View();
        }

        public async Task<IEnumerable<SelectListItem>> GetSubCategarias(int idCategoria)
        {
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
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            if (ModelState.IsValid)
            {
                if (await GetIdByName(p_Categoria.nombre) != null)
                {
                    PrompInfo("A categoria já existe");
                    return View(p_Categoria);
                }

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
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

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
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            if (id != p_Categoria.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var entityId = await GetIdByName(p_Categoria.nombre);
                if (entityId != null && entityId != p_Categoria.id)
                {
                    PrompInfo("A categoria já existe");
                    return View(p_Categoria);
                }

                try
                {
                    var categoria = await _context.P_Categorias.FindAsync(id);
                    categoria.nombre = p_Categoria.nombre;

                    _context.Update(categoria);
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
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

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
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var p_Categoria = await _context.P_Categorias.FindAsync(id);
            _context.P_Categorias.Remove(p_Categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private async Task<int?> GetIdByName(string nombre)
        {
            try
            {
                var entity = await _context.P_Categorias.FirstOrDefaultAsync(e => e.nombre.ToLower() == nombre.ToLower() && e.idCuenta == Cuenta.id);
                if (entity == null)
                {
                    return null;
                }
                return entity.id;
            }
            catch (Exception)
            {
                return null;
            }
        }


        private bool P_CategoriaExists(int id)
        {
            ValidarCuenta();
            return _context.P_Categorias.Any(e => e.id == id);
        }
    }
}

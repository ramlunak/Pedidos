using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Extensions;
using Pedidos.Models;

namespace Pedidos.Controllers
{
    public class ProductosController : BaseController
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index(string nombre, int pagina = 1)
        {
            ValidarCuenta();

            var cantidadRegistrosPorPagina = 10; // parámetro

            var Skip = ((pagina - 1) * cantidadRegistrosPorPagina);
            var sql = SqlConsultas.GetSqlAllProductos(Cuenta.id, Skip, cantidadRegistrosPorPagina, nombre);

            var lista = await new DBHelper(_context).ProductosFromCmd(sql);

            var totalDeRegistros = 0;
            if (nombre is null)
            {
                totalDeRegistros = await _context.P_Productos.Where(x => x.idCuenta == Cuenta.id).CountAsync();
            }
            else
            {
                totalDeRegistros = await _context.P_Productos.Where(x => x.idCuenta == Cuenta.id && x.nombre.Contains(nombre)).CountAsync();
            }

            ViewBag.FlrNombre = nombre;

            var modelo = new ViewModels.VMProductos();
            modelo.Productos = lista;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new RouteValueDictionary();
            modelo.ValoresQueryString["pagina"] = pagina;
            modelo.ValoresQueryString["nombre"] = nombre;
            
            return View(modelo);

        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Productos = await _context.P_Productos
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Productos == null)
            {
                return NotFound();
            }

            return View(p_Productos);
        }

        // GET: Productos/Create
        public async Task<IActionResult> Create()
        {
            ValidarCuenta();
            ViewBag.Categorias = await _context.P_Categorias.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            return View(new P_Productos());
        }
        
        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Productos p_Productos)
        {            
            ValidarCuenta();
            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files;
                foreach (var imagen in files)
                {
                    if (imagen != null && imagen.Length > 0)
                    {
                        if (!imagen.IsImage())
                        {
                            @ViewBag.Erro = "O arquivo selecionado não tem formato de imagem";
                            ViewBag.Categorias = await _context.P_Categorias.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
                            return View(p_Productos);
                        }

                        var imgBytes = await imagen.ToByteArray();
                        var newImg = imgBytes.Resize(50, 50);
                        p_Productos.imagen = await imagen.ToByteArray();
                    }
                }
                p_Productos.valor = p_Productos.strValor.ToDecimal();
                p_Productos.idCuenta = Cuenta.id;              
                _context.Add(p_Productos);
                await _context.SaveChangesAsync();
                Cuenta.Productos.Add(p_Productos);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = await _context.P_Categorias.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            return View(p_Productos);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id, int? pagina)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Productos = await _context.P_Productos.FindAsync(id);
            if (p_Productos == null)
            {
                return NotFound();
            }

            p_Productos.strValor = p_Productos.valor.ToString(CultureInfo.InvariantCulture);
            var ListaCaterorias = await _context.P_Categorias.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            ViewBag.Categorias = ListaCaterorias;
            ViewBag.Pagina = pagina;

            try
            {
                p_Productos.Categoria = ListaCaterorias.First(x => x.id == p_Productos.idCategoria).nombre;
            }
            catch
            {
            }

            return View(p_Productos);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Productos p_Productos)
        {
            ValidarCuenta();
            if (id != p_Productos.id)
            {
                return NotFound();
            }

            if (ModelState.ErrorCount == 1)
                ModelState.Clear();

            if (p_Productos.ImageBase64 != null)
                p_Productos.imagen = System.Convert.FromBase64String(p_Productos.ImageBase64);

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    foreach (var imagen in files)
                    {
                        if (!imagen.IsImage())
                        {
                            @ViewBag.Erro = "O arquivo selecionado não tem formato de imagem";
                            ViewBag.Categorias = await _context.P_Categorias.ToListAsync();
                            return View(p_Productos);
                        }

                        if (imagen != null && imagen.Length > 0)
                        {
                            var imgBytes = await imagen.ToByteArray();
                            var newImg = imgBytes.Resize(50, 50);
                            p_Productos.imagen = await imagen.ToByteArray();
                        }
                    }

                    p_Productos.valor = p_Productos.strValor.ToDecimal();
                    _context.Update(p_Productos);

                    var pold_roducto = await _context.P_Productos.FindAsync(p_Productos.id);
                    Cuenta.Productos.Remove(pold_roducto);
                    Cuenta.Productos.Add(p_Productos);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_ProductosExists(p_Productos.id))
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
            return View(p_Productos);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Productos = await _context.P_Productos
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Productos == null)
            {
                return NotFound();
            }

            return View(p_Productos);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ValidarCuenta();
            var p_Productos = await _context.P_Productos.FindAsync(id);
            _context.P_Productos.Remove(p_Productos);
            await _context.SaveChangesAsync();
            Cuenta.Productos.Remove(p_Productos);
            return RedirectToAction(nameof(Index));
        }

        private bool P_ProductosExists(int id)
        {
            ValidarCuenta();
            return _context.P_Productos.Any(e => e.id == id);
        }
    }
}

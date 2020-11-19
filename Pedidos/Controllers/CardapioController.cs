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
    public class CardapioController : BaseController
    {
        private readonly AppDbContext _context;

        public CardapioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Cardapio
        public async Task<IActionResult> Index()
        {
            ValidarCuenta();
            //var data = await _context.P_Aux.FromSqlRaw(SqlConsultas.GetSqlCardapio(53,Cuenta.id)).ToListAsync();
            ////var id = 1;
            //var query =
            //    from P in _context.P_Productos
            //    join C in _context.P_Categorias on P.idCategoria equals C.id
            //    where P.idCuenta == Cuenta.id
            //    group C by C.nombre into G
            //    select new
            //    {
            //        Categoria = G.Key,
            //        Productos = G.ToList()
            //    };


            //var item = from d in data
            //           group d by d.Categoria into g select g.ToList();

            //var result = await query.ToListAsync();
            var model = await _context.P_Categorias.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();

            return View(model);
        }

       
        public async Task<IActionResult> GetProductos(int id)
        {
            ValidarCuenta();
            //var data = await _context.P_Aux.FromSqlRaw(SqlConsultas.GetSqlCardapio(id, Cuenta.id)).ToListAsync();
            ////var id = 1;
            //var query =
            //    from P in _context.P_Productos
            //    join C in _context.P_Categorias on P.idCategoria equals C.id
            //    where P.idCuenta == Cuenta.id
            //    group C by C.nombre into G
            //    select new
            //    {
            //        Categoria = G.Key,
            //        Productos = G.ToList()
            //    };

            //var item = from d in data
            //           group d by d.Categoria into g
            //           select g.ToList();
            var items = await _context.P_Productos.Where(x => x.idCategoria == id && x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            return Ok(items);
        }


        // GET: Cardapio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Cardapio/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cardapio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombre,idCuenta,activo")] P_Categoria p_Categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(p_Categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Categoria);
        }

        // GET: Cardapio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Categoria = await _context.P_Categorias.FindAsync(id);
            if (p_Categoria == null)
            {
                return NotFound();
            }
            return View(p_Categoria);
        }

        // POST: Cardapio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nombre,idCuenta,activo")] P_Categoria p_Categoria)
        {
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
                return RedirectToAction(nameof(Index));
            }
            return View(p_Categoria);
        }

        // GET: Cardapio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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

        // POST: Cardapio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p_Categoria = await _context.P_Categorias.FindAsync(id);
            _context.P_Categorias.Remove(p_Categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_CategoriaExists(int id)
        {
            return _context.P_Categorias.Any(e => e.id == id);
        }
    }
}

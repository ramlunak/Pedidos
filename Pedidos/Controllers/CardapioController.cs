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

        [Route("Cardapio/{id}")]
        public async Task<IActionResult> Index(string id)
        {

            if (id == null) return View();
            //if (!ValidarCuenta())
            //{
            //    return RedirectToAction("Salir", "Login");
            //}
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
            try
            {
                TempData["IsQRCode"] = true;

                var cuenta = id.Split("_")[0];
                var idCuenta = Convert.ToInt32(cuenta.Split("acc")[1]);

                var model = await _context.P_Categorias.Where(x => x.idCuenta == idCuenta && x.activo).ToListAsync();

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View();
            }
        }


        public async Task<IActionResult> GetProductos(int id)
        {
            //if (!ValidarCuenta())
            //{
            //    return RedirectToAction("Salir", "Login");
            //}
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
            var items = await _context.P_Productos.Where(x => x.idCategoria == id && x.idCuenta == 5 && x.activo).ToListAsync();
            return Ok(items);
        }


        // GET: Cardapio/Details/5
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

            var p_Categoria = await _context.P_Categorias
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Categoria == null)
            {
                return NotFound();
            }

            return View(p_Categoria);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Extensions;
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
            TempData["IsQRCode"] = true;

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

            //ViewBag.LocalIpAddress = Request.HttpContext.Connection.LocalIpAddress;
            //ViewBag.RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            //ViewBag.HttpContextConnectionId = HttpContext.Connection.Id;
            ////var result = await query.ToListAsync();
            //try
            //{
            //  

            var cuenta = id.Split("_")[0];
            var table = id.Split("_")[1];
            var idCuenta = Convert.ToInt32(cuenta.Split("acc")[1]);
            var mesa = Convert.ToInt32(table.Split("table")[1]);
            ViewBag.IdCuenta = idCuenta;
            ViewBag.Mesa = mesa;
            //    var model = await _context.P_Categorias.Where(x => x.idCuenta == idCuenta && x.activo).ToListAsync();

            //    return View(model);
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.Erro = ex.Message;
            //    return View();
            //}

            return View();
        }

        public async Task<IActionResult> CargarCategorias(int id)
        {
            //ViewBag.LocalIpAddress = Request.HttpContext.Connection.LocalIpAddress;
            //ViewBag.RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            //ViewBag.HttpContextConnectionId = HttpContext.Connection.Id;
            //var result = await query.ToListAsync();

            var productos = await _context.P_Productos.Where(x => x.idCuenta == id && x.activo).ToListAsync();
            SetSession("CardapioProductos", productos);

            try
            {
                TempData["IsQRCode"] = true;

                var idCuenta = id;
                var model = await _context.P_Categorias.Where(x => x.idCuenta == idCuenta && x.activo).ToListAsync();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetProductos([FromBody] P_Categoria categoria)
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

            var productos = GetSession<List<P_Productos>>("CardapioProductos");

            var items = productos.Where(x => x.idCategoria == categoria.id && x.idCuenta == categoria.idCuenta && x.activo).ToList();
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

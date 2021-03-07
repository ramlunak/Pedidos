using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Controllers
{
    public class DeliveryController : BaseController
    {
        private readonly AppDbContext _context;

        public DeliveryController(AppDbContext context)
        {
            _context = context;
        }

        [Route("delivery/{id?}")]
        public IActionResult Index(string id)
        {
            ViewBag.Link = id;
            return View();
        }

        public async Task<IActionResult> CargarDatos()
        {
            try
            {
                var productos = await _context.P_Productos.FromSqlRaw(SqlConsultas.GetSqlProductosAll(1)).ToListAsync();
                SetSession("CardapioProductos", productos);

                var categorias = await _context.P_Categorias.Where(x => x.idCuenta == 1 && x.activo).ToListAsync();
                SetSession("CardapioCategorias", productos);

                //var model = new ViewModels.VMCardapioOnline();
                //model.idCuenta = 5;
                //model._Categorias = categorias;
                //model._Productos = productos;

                var grupoProductos = from C in categorias
                                     group C by C.id into G
                                     select new
                                     {
                                         idCategoria = G.Key,
                                         categoria = categorias.Find(x => x.id == G.Key).nombre,
                                         productos = productos.Where(x => x.idCategoria == G.Key)
                                     };


                return Ok(new { grupoProductos });
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
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
                var productos = await _context.P_Productos.FromSqlRaw(SqlConsultas.GetSqlProductosAll(5)).ToListAsync();
                SetSession("CardapioProductos", productos);

                var categorias = await _context.P_Categorias.Where(x => x.idCuenta == 5 && x.activo).ToListAsync();
                SetSession("CardapioCategorias", productos);

                var model = new ViewModels.VMCardapioOnline();
                model.idCuenta = 5;
                model._Categorias = categorias;
                model._Productos = productos;

                return Ok(new { categorias, productos });
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

    }
}

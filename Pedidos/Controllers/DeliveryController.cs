using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pedidos.Data;
using Pedidos.Extensions;
using Pedidos.Models;
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

                var grupoProductos = from C in categorias
                                     group C by C.id into G
                                     select new
                                     {
                                         idCategoria = G.Key,
                                         idCuenta = 1,
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

        public async Task<IActionResult> GetDetalleProducto(int idCuenta, int id)
        {
            try
            {
                var productos = GetSession<List<P_Productos>>("CardapioProductos");

                if (productos == null)
                {
                    productos = await _context.P_Productos.FromSqlRaw(SqlConsultas.GetSqlProductosAll(idCuenta)).ToListAsync();
                    SetSession("CardapioProductos", productos);
                }

                var filter = productos.Where(x => x.id == id).FirstOrDefault();

                var adicionales = new List<P_Adicionais>().ToArray();
                var ingredientes = new List<P_Ingredientes>().ToArray();

                if (!string.IsNullOrEmpty(filter.JsonAdicionales))
                {
                    adicionales = JsonConvert.DeserializeObject<P_Adicionais[]>(filter.JsonAdicionales);
                }
                if (!string.IsNullOrEmpty(filter.JsonIngredientes))
                {
                    ingredientes = JsonConvert.DeserializeObject<P_Ingredientes[]>(filter.JsonIngredientes);
                }

                var listaAdicionales = adicionales.GroupBy(x => x.id).Select(y => y.FirstOrDefault()).OrderBy(x => x.orden).ToList();
                var listaIngredientes = ingredientes.GroupBy(x => x.id).Select(y => y.FirstOrDefault()).ToList();
                return Ok(new { producto = filter, adicionales = listaAdicionales, ingredientes = listaIngredientes });
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

    }
}

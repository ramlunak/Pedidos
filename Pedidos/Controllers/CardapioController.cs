﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

            var cuenta = id.Split("_")[0];
            var table = id.Split("_")[1];
            var idCuenta = Convert.ToInt32(cuenta.Split("acc")[1]);
            var mesa = Convert.ToInt32(table.Split("table")[1]);
            ViewBag.IdCuenta = idCuenta;
            ViewBag.Mesa = mesa;

            return View();
        }

        public async Task<IActionResult> CargarCategorias(int id)
        {
            var idCuenta = id;

            try
            {
                var productos = await _context.P_Productos.FromSqlRaw(SqlConsultas.GetSqlProductosAll(idCuenta)).ToListAsync();
                SetSession("CardapioProductos", productos);

                var model = await _context.P_Categorias.Where(x => x.idCuenta == idCuenta && x.activo).ToListAsync();

                TempData["IsQRCode"] = true;

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
            var productos = GetSession<List<P_Productos>>("CardapioProductos");

            if (productos == null)
            {
                productos = await _context.P_Productos.FromSqlRaw(SqlConsultas.GetSqlProductosAll(categoria.idCuenta)).ToListAsync();
                SetSession("CardapioProductos", productos);
            }

            var items = productos.Where(x => x.idCategoria == categoria.id && x.idCuenta == categoria.idCuenta && x.activo).ToList();
            return Ok(items);
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


        public async Task<IActionResult> CadastrarCliente(int idCuenta, string nombre)
        {
            var cliente = new P_Cliente();
            cliente.idCuenta = idCuenta;
            cliente.nombre = nombre;
            cliente.registroPorCardapio = true;

            try
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return NotFound(new { ok = false });
            }
        }

    }
}

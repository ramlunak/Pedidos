using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;
using Newtonsoft;
using Newtonsoft.Json;
using Pedidos.Models.Enums;

namespace Pedidos.Controllers
{
    public class PedidosController : BaseController
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            ValidarCuenta();

            var formaPagamento = await _context.P_FormaPagamento.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            ViewBag.FormaPagamento = formaPagamento;
            SetSession("FormaPagamento", formaPagamento);

            if (GetSession<P_Pedido>("currentPedido") is null)
            {
                var currentPedido = new P_Pedido(Cuenta.id);
                SetSession("currentPedido", currentPedido);

                return View(currentPedido);
            }
            else
            {
                var currentPedido = GetSession<P_Pedido>("currentPedido");
                return View(currentPedido);
            }

        }

        [HttpPost]
        public IActionResult AddProducto([FromBody] P_Productos producto)
        {
            ValidarCuenta();
            var currentPedido = GetSession<P_Pedido>("currentPedido");
            if (currentPedido is null)
            {
                currentPedido = new P_Pedido(Cuenta.id);
            }

            producto.Adicionales.RemoveAll(a => a.cantidad == 0);
            producto.Ingredientes.RemoveAll(i => i.selected);

            //Para no cargar la base con muchos datos eliminar la foto
            producto.imagen = null;

            currentPedido.productos.Add(producto);
            currentPedido.cliente = producto.cliente;
            currentPedido.direccion = producto.direccion;
            currentPedido.telefono = producto.telefono;

            SetSession("currentPedido", currentPedido);
            return Ok(new { currentPedido });
        }

        public async Task<IActionResult> GetCurrentPedido()
        {
            var currentPedido = GetSession<P_Pedido>("currentPedido");
            return Ok(new { currentPedido });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarCurrentPedido([FromBody] PedidoDatosAux pedidoaux)
        {

            var currentPedido = GetSession<P_Pedido>("currentPedido");
            if (currentPedido.productos.Count > 0)
            {
                try
                {
                    currentPedido.fecha = DateTime.Now.ToUniversalTime();
                    currentPedido.status = StatusPedido.Pendiente.ToString();
                    currentPedido.jsonListProductos = JsonConvert.SerializeObject(currentPedido.productos);
                    currentPedido.total = currentPedido.valorProductos;

                    currentPedido.cliente = pedidoaux.cliente;
                    currentPedido.direccion = pedidoaux.direccion;
                    currentPedido.telefono = pedidoaux.telefono;

                    var formaPagamento = GetSession<List<P_FormaPagamento>>("FormaPagamento");
                    currentPedido.formaPagamento = formaPagamento.FirstOrDefault(x => x.id == Convert.ToInt32(pedidoaux.idFormaPagamento)).nombre;

                    _context.Add(currentPedido);
                    await _context.SaveChangesAsync();

                    currentPedido = new P_Pedido(Cuenta.id);
                    SetSession("currentPedido", currentPedido);

                    var pedidosPendientes = await _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.status == StatusPedido.Pendiente.ToString()).ToArrayAsync();

                    return Ok(new { ok = true, currentPedido, pedidosPendientes });
                }
                catch (Exception ex)
                {
                    return Ok(new { erro = ex.Message });
                }
            }
            else
            {
                return Ok(new { erro = "Adicione pelo menos um produto" });
            }
        }

        public async Task<IActionResult> CargarPedidosPendientes()
        {
            var pedidosPendientes = await _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.status == StatusPedido.Pendiente.ToString()).ToArrayAsync();
            return Ok(new { pedidosPendientes = pedidosPendientes });
        }

        private bool P_PedidoExists(int id)
        {
            ValidarCuenta();

            return _context.P_Pedidos.Any(e => e.id == id);
        }

        public async Task<IActionResult> Cancelar(int id)
        {
            try
            {
                var pedido = await _context.P_Pedidos.FindAsync(id);
                pedido.status = StatusPedido.Cancelado.ToString();
                _context.Update(pedido);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Finalizar(int id)
        {
            try
            {
                var pedido = await _context.P_Pedidos.FindAsync(id);
                pedido.status = StatusPedido.Finalizado.ToString();
                _context.Update(pedido);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Print(int id)
        {
            var pedido = await _context.P_Pedidos.FindAsync(id);
            return View(pedido);
        }
    }
}

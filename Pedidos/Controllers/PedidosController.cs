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
using Pedidos.Extensions;

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

            var clientes = await _context.P_Clientes.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            ViewBag.Clientes = clientes;

            var aplicativos = await _context.P_Aplicativos.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            ViewBag.Aplicativos = aplicativos;

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
            currentPedido.idCliente = producto.idCliente;
            currentPedido.aplicativo = producto.aplicativo;
            currentPedido.idAplicativo = producto.idAplicativo;
            currentPedido.idMesa = producto.idMesa;
            currentPedido.direccion = producto.direccion;
            currentPedido.telefono = producto.telefono;

            if (!string.IsNullOrEmpty(producto.tamanhoSeleccionado))
            {
                producto.valor = producto.valorTamanhoSeleccionado;
            }

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
                    currentPedido.fecha = DateTime.Now.ToSouthAmericaStandard();
                    currentPedido.status = StatusPedido.Pendiente.ToString();
                    currentPedido.jsonListProductos = JsonConvert.SerializeObject(currentPedido.productos);
                    currentPedido.total = currentPedido.valorProductos;

                    currentPedido.idCliente = pedidoaux.idCliente;
                    currentPedido.cliente = pedidoaux.cliente;
                    currentPedido.idAplicativo = pedidoaux.idAplicativo;
                    currentPedido.aplicativo = pedidoaux.aplicativo;
                    currentPedido.idMesa = pedidoaux.idMesa;
                    currentPedido.direccion = pedidoaux.direccion;
                    currentPedido.telefono = pedidoaux.telefono;
                    currentPedido.pago = pedidoaux.pago;

                    if (currentPedido.idFormaPagamento.HasValue)
                    {
                        var formaPagamento = GetSession<List<P_FormaPagamento>>("FormaPagamento");
                        currentPedido.formaPagamento = formaPagamento.FirstOrDefault(x => x.id == Convert.ToInt32(pedidoaux.idFormaPagamento)).nombre;
                    }

                    var actualizarPagina = false;

                    if (currentPedido.idCliente is null && !string.IsNullOrEmpty(currentPedido.cliente))
                    {
                        var cliente = new P_Cliente();
                        cliente.idCuenta = Cuenta.id;
                        cliente.activo = true;
                        cliente.telefono = currentPedido.telefono;
                        cliente.nombre = currentPedido.cliente;
                        _context.P_Clientes.Add(cliente);
                        await _context.SaveChangesAsync();
                        currentPedido.idCliente = cliente.id;
                        actualizarPagina = true;
                    }

                    if (currentPedido.idAplicativo is null && !string.IsNullOrEmpty(currentPedido.aplicativo))
                    {
                        var aplicativo = new P_Aplicativo();
                        aplicativo.idCuenta = Cuenta.id;
                        aplicativo.activo = true;
                        aplicativo.nombre = currentPedido.aplicativo;
                        _context.P_Aplicativos.Add(aplicativo);
                        await _context.SaveChangesAsync();
                        currentPedido.idAplicativo = aplicativo.id;
                        actualizarPagina = true;
                    }

                    _context.Add(currentPedido);
                    await _context.SaveChangesAsync();

                    currentPedido = new P_Pedido(Cuenta.id);
                    SetSession("currentPedido", currentPedido);

                    var pedidosPendientes = await _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.status == StatusPedido.Pendiente.ToString()).ToArrayAsync();

                    return Ok(new { ok = true, reload = actualizarPagina, currentPedido, pedidosPendientes });
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
            return Ok(new { pedidosPendientes = pedidosPendientes.OrderByDescending(x=>x.fecha).ToList() });
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

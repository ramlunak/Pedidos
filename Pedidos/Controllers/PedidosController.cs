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

            var direcciones = await _context.P_Direcciones.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            SetSession("Direcciones", direcciones);

            if (GetSession<P_Pedido>("currentPedido") is null)
            {
                var currentPedido = new P_Pedido(Cuenta.id);
                currentPedido.listaFormaPagamento = GetSession<List<P_FormaPagamento>>("FormaPagamento");
                SetSession("currentPedido", currentPedido);

                return View(currentPedido);
            }
            else
            {
                var currentPedido = GetSession<P_Pedido>("currentPedido");
                currentPedido.listaFormaPagamento = GetSession<List<P_FormaPagamento>>("FormaPagamento");
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
            currentPedido.idDireccion = producto.idDireccion;
            currentPedido.direccion = producto.direccion;
            currentPedido.telefono = producto.telefono;

            currentPedido.listaFormaPagamento = GetSession<List<P_FormaPagamento>>("FormaPagamento");

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

                    currentPedido.idCliente = pedidoaux.idCliente;
                    currentPedido.cliente = pedidoaux.cliente;
                    currentPedido.idAplicativo = pedidoaux.idAplicativo;
                    currentPedido.aplicativo = pedidoaux.aplicativo;
                    currentPedido.idMesa = pedidoaux.idMesa;
                    currentPedido.direccion = pedidoaux.direccion;
                    currentPedido.telefono = pedidoaux.telefono;
                    currentPedido.descuento = pedidoaux.descuento;
                    currentPedido.pago = pedidoaux.pago;

                    currentPedido.total = currentPedido.valorProductos - (currentPedido.descuento.HasValue ? currentPedido.descuento.Value : 0);

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

                        if (currentPedido.idDireccion is null && !string.IsNullOrEmpty(currentPedido.direccion))
                        {
                            var direccion = new P_Direcciones();
                            direccion.idCuenta = Cuenta.id;
                            direccion.idCliente = cliente.id;
                            direccion.activo = true;
                            direccion.code = "N/A";
                            direccion.address = "N/A";
                            direccion.numero = "N/A";
                            direccion.complemento = "N/A";
                            direccion.state = "N/A";
                            direccion.city = "N/A";
                            direccion.district = "N/A";
                            direccion.auxiliar = currentPedido.direccion;
                            _context.P_Direcciones.Add(direccion);
                            await _context.SaveChangesAsync();
                            currentPedido.idDireccion = direccion.id;
                            actualizarPagina = true;
                        }

                    }
                    else
                    {
                        if (currentPedido.idDireccion is null && !string.IsNullOrEmpty(currentPedido.direccion))
                        {
                            var direccion = new P_Direcciones();
                            direccion.idCuenta = Cuenta.id;
                            direccion.idCliente = currentPedido.idCliente;
                            direccion.activo = true;
                            direccion.code = "N/A";
                            direccion.address = "N/A";
                            direccion.numero = "N/A";
                            direccion.complemento = "N/A";
                            direccion.state = "N/A";
                            direccion.city = "N/A";
                            direccion.district = "N/A";
                            direccion.auxiliar = currentPedido.direccion;
                            _context.P_Direcciones.Add(direccion);
                            await _context.SaveChangesAsync();
                            currentPedido.idDireccion = direccion.id;
                            actualizarPagina = true;
                        }
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
                    currentPedido.listaFormaPagamento = GetSession<List<P_FormaPagamento>>("FormaPagamento");
                    SetSession("currentPedido", currentPedido);

                    var pedidosPendientes = await _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.status == StatusPedido.Pendiente.ToString()).OrderByDescending(x => x.fecha).ToArrayAsync();

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
            return Ok(new { pedidosPendientes = pedidosPendientes.OrderByDescending(x => x.fecha).ToList() });
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

        public async Task<IActionResult> GetDireccion(int id)
        {
            if (GetSession<List<P_Direcciones>>("Direcciones") != null)
            {
                var direciones = GetSession<List<P_Direcciones>>("Direcciones");
                var clienteDirecciones = direciones.Where(x => x.idCliente == id);
                return Ok(clienteDirecciones);
            }
            else
            {
                var direciones = await _context.P_Direcciones.Where(x => x.idCliente == id).ToArrayAsync();
                return Ok(direciones);
            }
        }
    }
}

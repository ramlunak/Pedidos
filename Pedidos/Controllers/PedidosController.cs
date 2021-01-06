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
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            if (GetSession<List<P_Pedido>>("PedidosFinalizados") != null)
            {
                var countPedidosFinalizados = GetSession<List<P_Pedido>>("PedidosFinalizados").Count;
                ViewBag.CountPedidosFinalizados = countPedidosFinalizados;
            }

            var clientes = await _context.P_Clientes.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            ViewBag.Clientes = clientes;

            var aplicativos = await _context.P_Aplicativos.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            ViewBag.Aplicativos = aplicativos;

            var formaPagamento = await _context.P_FormaPagamento.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            ViewBag.FormaPagamento = formaPagamento.OrderBy(x => x.nombre);
            SetSession("FormaPagamento", formaPagamento.OrderBy(x => x.nombre));

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
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
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

        public async Task<IActionResult> GetCurrentPedido(int? id)
        {
            if (id.HasValue)
            {
                var currentPedido = await _context.P_Pedidos.FindAsync(id);
                foreach (var item in JsonConvert.DeserializeObject<P_Productos[]>(currentPedido.jsonListProductos))
                {
                    currentPedido.productos.Add(item);
                }
                currentPedido.isNew = false;
                SetSession("currentPedido", currentPedido);
                return Ok(new { currentPedido });
            }
            else
            {
                var currentPedido = GetSession<P_Pedido>("currentPedido");
                return Ok(new { currentPedido });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarCurrentPedido([FromBody] PedidoDatosAux pedidoaux)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var currentPedido = GetSession<P_Pedido>("currentPedido");
            if (currentPedido.productos.Count > 0)
            {
                try
                {
                    currentPedido.fecha = DateTime.Now.ToSouthAmericaStandard();
                    currentPedido.status = StatusPedido.Pendiente.ToString();
                    currentPedido.jsonListProductos = JsonConvert.SerializeObject(currentPedido.productos);
                    currentPedido.valorProductos = currentPedido.productos.Sum(x => x.ValorMasAdicionales);
                    currentPedido.idCliente = pedidoaux.idCliente;
                    currentPedido.cliente = pedidoaux.cliente;
                    currentPedido.idAplicativo = pedidoaux.idAplicativo;
                    currentPedido.aplicativo = pedidoaux.aplicativo;
                    currentPedido.idMesa = pedidoaux.idMesa;
                    currentPedido.direccion = pedidoaux.direccion;
                    currentPedido.telefono = pedidoaux.telefono;
                    currentPedido.descuento = pedidoaux.descuento ?? 0;
                    currentPedido.pago = pedidoaux.pago;
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
                  
                    var formasPagamento = new List<P_FormaPagamento>();
                    if (currentPedido.idAplicativo.HasValue)
                    {
                        var fp = GetSession<List<P_FormaPagamento>>("FormaPagamento");
                        formasPagamento.AddRange(fp.Where(x => x.idAplicativo.HasValue && x.idAplicativo.Value == currentPedido.idAplicativo));
                        formasPagamento.AddRange(fp.Where(x => !x.idAplicativo.HasValue && x.app));
                    }
                    else
                    {
                        var fp = GetSession<List<P_FormaPagamento>>("FormaPagamento");
                        formasPagamento.AddRange(fp.Where(x => !x.idAplicativo.HasValue));
                    }

                    currentPedido.listaFormaPagamento = formasPagamento.OrderBy(x => x.nombre).ToList();
                    currentPedido.jsonFormaPagamento = JsonConvert.SerializeObject(currentPedido.listaFormaPagamento);

                    if (currentPedido.isNew)
                    {
                        _context.Add(currentPedido);
                        await _context.SaveChangesAsync();

                        currentPedido = new P_Pedido(Cuenta.id);
                        SetSession("currentPedido", currentPedido);

                        var pedidos = await _context.P_Pedidos.Where(x =>
                                         x.idCuenta == Cuenta.id &&
                                        (x.status == StatusPedido.Pendiente.ToString() || x.status == StatusPedido.Preparado.ToString())).ToArrayAsync();
                        var pedidosPendientes = pedidos.OrderByDescending(x => x.fecha).ThenBy(x => x.status).ToList();

                        return Ok(new { ok = true, reload = actualizarPagina, currentPedido, pedidosPendientes });
                    }
                    else
                    {
                        currentPedido.jsonListProductos = JsonConvert.SerializeObject(currentPedido.productos);

                        _context.Update(currentPedido);
                        await _context.SaveChangesAsync();

                        currentPedido = new P_Pedido(Cuenta.id);
                        SetSession("currentPedido", currentPedido);

                        var pedidos = await _context.P_Pedidos.Where(x =>
                                         x.idCuenta == Cuenta.id &&
                                        (x.status == StatusPedido.Pendiente.ToString() || x.status == StatusPedido.Preparado.ToString())).ToArrayAsync();
                        var pedidosPendientes = pedidos.OrderByDescending(x => x.fecha).ThenBy(x => x.status).ToList();

                        return Ok(new { ok = true, reload = false, currentPedido, pedidosPendientes });
                    }

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
            var pedidosPendientes = await _context.P_Pedidos.Where(x =>
                                     x.idCuenta == Cuenta.id &&
                                    (x.status == StatusPedido.Pendiente.ToString() || x.status == StatusPedido.Preparado.ToString())).ToArrayAsync();

            return Ok(new { pedidosPendientes = pedidosPendientes.OrderByDescending(x => x.fecha).ThenBy(x => x.status).ToList() });
        }

        private bool P_PedidoExists(int id)
        {
            ValidarCuenta();

            return _context.P_Pedidos.Any(e => e.id == id);
        }

        public async Task<IActionResult> Cancelar(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
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

        public async Task<IActionResult> Preparado(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            try
            {
                var pedido = await _context.P_Pedidos.FindAsync(id);
                pedido.status = StatusPedido.Preparado.ToString();
                _context.Update(pedido);
                await _context.SaveChangesAsync();
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Finalizar([FromBody] PedidoDatosAux pedidoaux)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            try
            {
                var pedido = await _context.P_Pedidos.FindAsync(pedidoaux.idPedido.Value);
                pedido.status = StatusPedido.Finalizado.ToString();
                pedido.descuento = pedidoaux.descuento ?? 0;
                pedido.jsonFormaPagamento = pedidoaux.listaFormaPagamento;
                pedido.pago = pedidoaux.pago;
                pedido.productos = JsonConvert.DeserializeObject<List<P_Productos>>(pedido.jsonListProductos);
                pedido.valorProductos = pedido.productos.Sum(x => x.ValorMasAdicionales);
                _context.Update(pedido);
                await _context.SaveChangesAsync();

                if (GetSession<List<P_Pedido>>("PedidosFinalizados") != null)
                {
                    var lista = GetSession<List<P_Pedido>>("PedidosFinalizados");
                    lista.Add(pedido);
                    SetSession("PedidosFinalizados", lista);
                }
                else
                {
                    var lista = new List<P_Pedido>();
                    lista.Add(pedido);
                    SetSession("PedidosFinalizados", lista);
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
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

        public async Task<IActionResult> CancelarCurrentPedido()
        {
            var currentPedido = new P_Pedido(Cuenta.id);
            currentPedido.listaFormaPagamento = GetSession<List<P_FormaPagamento>>("FormaPagamento");
            SetSession("currentPedido", currentPedido);
            return Ok(new { ok = true, reload = false, currentPedido });
        }

        public async Task<IActionResult> GetNumeroPedidosFinalizados()
        {
            if (GetSession<List<P_Direcciones>>("PedidosFinalizados") != null)
            {
                return Ok(GetSession<List<P_Direcciones>>("PedidosFinalizados").Count);
            }
            else
            {
                return Ok(0);
            }
        }

        public async Task<IActionResult> Print(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var pedido = await _context.P_Pedidos.FindAsync(id);
            pedido.productos = pedido.jsonListProductos.ConvertTo<List<P_Productos>>();
            return View(pedido);
        }

    }
}

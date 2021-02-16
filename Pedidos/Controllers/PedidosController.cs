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
using Pedidos.Hubs;

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

            var config = await _context.P_Config.Where(x => x.idCuenta == Cuenta.id).FirstOrDefaultAsync();

            if (config is null)
            {
                try
                {
                    config = new P_Config();
                    config.idCuenta = Cuenta.id;
                    config.printSize = 300;
                    config.fontSize = 13;
                    _context.Add(config);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    PrompErro(ex.ToString());
                }
            }


            ViewBag.NombreEstablecimiento = config.nombreEstablecimiento;
            ViewBag.TelefonoEstablecimiento = config.telefonoEstablecimiento;
            ViewBag.DireccionEstablecimiento = config.direccionEstablecimiento;
            ViewBag.CnpjEstablecimiento = config.cnpjEstablecimiento;

            ViewBag.PrintSize = config.printSize;
            ViewBag.FontSize = config.fontSize;

            if (GetSession<List<P_Pedido>>("PedidosFinalizados") != null)
            {
                var countPedidosFinalizados = GetSession<List<P_Pedido>>("PedidosFinalizados").Count;
                ViewBag.CountPedidosFinalizados = countPedidosFinalizados;
            }

            var clientes = await _context.P_Clientes.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            ViewBag.Clientes = clientes;

            SetSession("Clientes", clientes);

            var aplicativos = await _context.P_Aplicativos.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            ViewBag.Aplicativos = aplicativos;

            var formaPagamento = await _context.P_FormaPagamento.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            ViewBag.FormaPagamento = formaPagamento.OrderBy(x => x.nombre);
            SetSession("FormaPagamento", formaPagamento.OrderBy(x => x.nombre));

            var direcciones = await _context.P_Direcciones.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            SetSession("Direcciones", direcciones);

            var barrios = await _context.P_Barrios.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            SetSession("Barrios", barrios);

            var productos = await _context.P_Productos.FromSqlRaw(SqlConsultas.GetSqlProductosAll(Cuenta.id)).ToListAsync();
            SetSession("PedidoProductos", productos);

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
            try
            {
                if (currentPedido.productos.Any())
                {
                    var p = currentPedido.productos.OrderByDescending(x => x.posicion).FirstOrDefault();
                    producto.posicion = p.posicion + 1;
                }
            }
            catch
            {
                producto.posicion = 1;
            }
            //Para no cargar la base con muchos datos eliminar la foto
            producto.imagen = null;
            currentPedido.productos.Add(producto);
            currentPedido.valorProductos = currentPedido.productos.Sum(x => x.ValorMasAdicionales);
            currentPedido.cliente = producto.cliente;
            currentPedido.idCliente = producto.idCliente;
            currentPedido.aplicativo = producto.aplicativo;
            currentPedido.idAplicativo = producto.idAplicativo;
            currentPedido.idMesa = producto.idMesa;
            currentPedido.idDireccion = producto.idDireccion;
            currentPedido.direccion = producto.direccion;
            currentPedido.idBarrio = producto.idBarrio;
            currentPedido.barrio = producto.barrio;
            currentPedido.telefono = producto.telefono;

            currentPedido.deliveryDinheiroTotal = producto.deliveryDinheiroTotal;
            currentPedido.deliveryEmCartao = producto.deliveryEmCartao;
            currentPedido.deliveryPago = producto.deliveryPago;
            currentPedido.deliveryEmdinheiro = producto.deliveryEmdinheiro;

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
                    currentPedido.valorProductos = currentPedido.productos.Sum(x => x.ValorMasAdicionales);
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
                currentPedido.productos.Where(x => x.isNew).Select(p => { p.fecha_pedido = DateTime.Now.ToSouthAmericaStandard(); return p; }).ToList();
                currentPedido.productos.Where(x => x.isNew).Select(p => { p.isNew = false; return p; }).ToList();

                try
                {
                    if (currentPedido.isNew)
                    {
                        currentPedido.fecha = DateTime.Now.ToSouthAmericaStandard();
                    }

                    if (Cuenta.rol == RolesSistema.Funcionario.ToString())
                    {
                        currentPedido.idFuncionario = Cuenta.idFuncionario;
                    }

                    currentPedido.status = StatusPedido.Pendiente.ToString();
                    currentPedido.jsonListProductos = JsonConvert.SerializeObject(currentPedido.productos);
                    currentPedido.valorProductos = currentPedido.productos.Sum(x => x.ValorMasAdicionales);
                    currentPedido.idCliente = pedidoaux.idCliente;
                    currentPedido.cliente = pedidoaux.cliente;
                    currentPedido.idAplicativo = pedidoaux.idAplicativo;
                    currentPedido.aplicativo = pedidoaux.aplicativo;
                    currentPedido.idMesa = pedidoaux.idMesa;
                    currentPedido.idDireccion = pedidoaux.idDireccion;
                    currentPedido.direccion = pedidoaux.direccion;
                    currentPedido.idBarrio = pedidoaux.idBarrio;
                    currentPedido.barrio = pedidoaux.barrio;
                    currentPedido.telefono = pedidoaux.telefono;

                    currentPedido.deliveryDinheiroTotal = pedidoaux.deliveryDinheiroTotal;
                    currentPedido.deliveryEmCartao = pedidoaux.deliveryEmCartao;
                    currentPedido.deliveryPago = pedidoaux.deliveryPago;
                    currentPedido.deliveryEmdinheiro = pedidoaux.deliveryEmdinheiro;

                    if (currentPedido.idBarrio.HasValue)
                    {
                        var barrios = GetSession<List<P_Barrio>>("Barrios");
                        if (barrios != null && barrios.Any())
                        {
                            var tasa = barrios.First(x => x.id == currentPedido.idBarrio).tasa;
                            currentPedido.tasaEntrega = tasa.HasValue ? tasa.Value : 0;
                        }
                    }
                    else
                    {
                        currentPedido.tasaEntrega = 0;
                    }

                    var actualizarPagina = false;

                    if (pedidoaux.cadastrarCliente)
                    {
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

                            if (!string.IsNullOrEmpty(currentPedido.telefono))
                            {
                                var cliente = await _context.P_Clientes.FindAsync(currentPedido.idCliente);
                                if (string.IsNullOrEmpty(cliente.telefono) || cliente.telefono != currentPedido.telefono)
                                {
                                    cliente.telefono = currentPedido.telefono;
                                    cliente.activo = true;
                                    _context.P_Clientes.Update(cliente);
                                    await _context.SaveChangesAsync();
                                    actualizarPagina = true;
                                }
                            }

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
                    currentPedido.jsonFormaPagamentoAux = JsonConvert.SerializeObject(currentPedido.listaFormaPagamento);

                    if (currentPedido.isNew)
                    {
                        var count = await _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id).CountAsync();
                        var codigo = $"P{Cuenta.id}-{++count}";
                        currentPedido.codigo = codigo;
                        _context.Add(currentPedido);
                        await _context.SaveChangesAsync();

                        currentPedido = new P_Pedido(Cuenta.id);
                        SetSession("currentPedido", currentPedido);

                        var pedidos = await _context.P_Pedidos.Where(x =>
                                         x.idCuenta == Cuenta.id &&
                                        (x.status == StatusPedido.Pendiente.ToString() || x.status == StatusPedido.Preparado.ToString())).ToArrayAsync();

                        if (pedidos.Any())
                        {
                            pedidos.Select(c => { c.productos = c.jsonListProductos.ConvertTo<List<P_Productos>>(); return c; }).ToList();
                        }
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
                        if (pedidos.Any())
                        {
                            pedidos.Select(c => { c.productos = c.jsonListProductos.ConvertTo<List<P_Productos>>(); return c; }).ToList();
                        }
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
            if (pedidosPendientes.Any())
            {
                pedidosPendientes.Select(c => { c.productos = c.jsonListProductos.ConvertTo<List<P_Productos>>(); return c; }).ToList();

            }


            return Ok(new { pedidosPendientes = pedidosPendientes.OrderByDescending(x => x.fecha).ThenBy(x => x.status).ToList() });
        }

        public async Task<IActionResult> CargarPedidosFinalizados()
        {
            var idUltimoPedidoCaixa = 0;
            var ultimaCaixaFechada = await _context.P_Caja.Where(x => x.idCuenta == Cuenta.id && !x.isOpen).OrderBy(x => x.id).LastOrDefaultAsync();
            if (ultimaCaixaFechada != null)
            {
                idUltimoPedidoCaixa = ultimaCaixaFechada.idUltimoPedido.Value;
            }

            var pedidosFinalizados = await _context.P_Pedidos.Where(x =>
                                     x.idCuenta == Cuenta.id &&
                                     x.id > idUltimoPedidoCaixa &&
                                     x.status == StatusPedido.Finalizado.ToString()).ToArrayAsync();
            if (pedidosFinalizados.Any())
            {
                pedidosFinalizados.Select(c => { c.productos = c.jsonListProductos.ConvertTo<List<P_Productos>>(); return c; }).ToList();
            }

            var valorTotalPedidos = pedidosFinalizados.Sum(x => x.productos.Sum(p => p.ValorMasAdicionales));
            var totalDescuentos = pedidosFinalizados.Sum(x => x.descuento);
            var totalTasaEntraga = pedidosFinalizados.Sum(x => x.tasaEntrega);
            var diferencia = valorTotalPedidos - totalDescuentos + totalTasaEntraga;

            return Ok(new
            {
                pedidosFinalizados = pedidosFinalizados.OrderByDescending(x => x.fecha).ThenBy(x => x.status).ToList(),
                valorTotalPedidos = valorTotalPedidos,
                totalDescuentos = totalDescuentos,
                totalTasaEntraga = totalTasaEntraga,
                diferencia = diferencia
            });
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

        [HttpPost]
        public async Task<IActionResult> Preparado([FromBody] InfoAuxDelivery infoAuxDelivery)
        {

            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            try
            {
                var pedido = new P_Pedido(0);
                var currentPedido = GetSession<P_Pedido>("currentPedido");

                if (infoAuxDelivery.pedidoIsPreparado.Value)
                {
                    pedido = await _context.P_Pedidos.FindAsync(infoAuxDelivery.idPedido);
                }
                else
                {
                    pedido = currentPedido;
                }

                pedido.status = StatusPedido.Preparado.ToString();
                pedido.descuento = infoAuxDelivery.descuento ?? 0;
                pedido.tasaEntrega = infoAuxDelivery.tasaEntrega ?? 0;
                pedido.deliveryEmdinheiro = infoAuxDelivery.DeliveryEmdinheiro;
                pedido.deliveryDinheiroTotal = infoAuxDelivery.DeliveryDinheiroTotal;
                pedido.deliveryTroco = infoAuxDelivery.DeliveryTroco;
                pedido.deliveryEmCartao = infoAuxDelivery.DeliveryEmCartao;
                pedido.deliveryPago = infoAuxDelivery.DeliveryPago;

                if (infoAuxDelivery.pedidoIsPreparado.Value)
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                    return Ok(pedido);
                }
                else
                {
                    SetSession("currentPedido", pedido);
                    return Ok(pedido);
                }

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Finalizar(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            try
            {
                var pedido = await _context.P_Pedidos.FindAsync(id);
                pedido.productos = pedido.jsonListProductos.ConvertTo<List<P_Productos>>();
                pedido.productos.Where(x => !x.fecha_preparado.HasValue).Select(c => { c.fecha_preparado = DateTime.Now.ToSouthAmericaStandard(); return c; }).ToList();
                pedido.jsonListProductos = pedido.productos.ToJson();

                pedido.status = StatusPedido.Finalizado.ToString();
                pedido.fechaFinalizado = DateTime.Now.ToSouthAmericaStandard();
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
        public async Task<IActionResult> ActualizarFormaPagamento([FromBody] PedidoDatosAux pedidoaux)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            try
            {
                var pedido = await _context.P_Pedidos.FindAsync(pedidoaux.idPedido.Value);
                //if (pedidoaux.finalizar)
                //{
                //    pedido.status = StatusPedido.Finalizado.ToString();
                //}
                //else
                //{
                pedido.status = StatusPedido.Pendiente.ToString();
                // }
                pedido.descuento = pedidoaux.descuento ?? 0;
                pedido.jsonFormaPagamento = pedidoaux.listaFormaPagamento;
                pedido.deliveryPago = true;
                pedido.deliveryEmCartao = false;
                pedido.deliveryEmdinheiro = false;
                pedido.deliveryTroco = pedidoaux.troco;
                pedido.tasaEntrega = pedidoaux.tasaEntrega ?? 0;
                pedido.productos = JsonConvert.DeserializeObject<List<P_Productos>>(pedido.jsonListProductos);
                pedido.valorProductos = pedido.productos.Sum(x => x.ValorMasAdicionales);

                var formasPagamento = new List<P_FormaPagamento>();
                if (pedido.idAplicativo.HasValue)
                {
                    var fp = GetSession<List<P_FormaPagamento>>("FormaPagamento");
                    formasPagamento.AddRange(fp.Where(x => x.idAplicativo.HasValue && x.idAplicativo.Value == pedido.idAplicativo));
                    formasPagamento.AddRange(fp.Where(x => !x.idAplicativo.HasValue && x.app));
                }
                else
                {
                    var fp = GetSession<List<P_FormaPagamento>>("FormaPagamento");
                    formasPagamento.AddRange(fp.Where(x => !x.idAplicativo.HasValue));
                }

                pedido.jsonFormaPagamentoAux = formasPagamento.ToJson();

                _context.Update(pedido);
                await _context.SaveChangesAsync();

                //if (pedidoaux.finalizar)
                //    if (GetSession<List<P_Pedido>>("PedidosFinalizados") != null)
                //    {
                //        var lista = GetSession<List<P_Pedido>>("PedidosFinalizados");
                //        lista.Add(pedido);
                //        SetSession("PedidosFinalizados", lista);
                //    }
                //    else
                //    {
                //        var lista = new List<P_Pedido>();
                //        lista.Add(pedido);
                //        SetSession("PedidosFinalizados", lista);
                //    }

                return Ok(pedido);
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

        public async Task<IActionResult> GetListaTelefonos()
        {
            List<string> telefonos = new List<string>();

            if (GetSession<List<P_Cliente>>("Clientes") != null)
            {
                try
                {
                    var clientes = GetSession<List<P_Cliente>>("Clientes");
                    telefonos.AddRange(clientes.Select(x => x.telefono));
                }
                catch
                {

                }
            }

            return Ok(telefonos);

        }

        public async Task<IActionResult> CargarTelefono(int id)
        {
            if (GetSession<List<P_Cliente>>("Clientes") != null)
            {
                try
                {
                    var clientes = GetSession<List<P_Cliente>>("Clientes");
                    var cli = clientes.FirstOrDefault(x => x.id == id);
                    var telefono = cli != null ? cli.telefono : null;
                    return Ok(telefono);
                }
                catch (Exception)
                {
                    return Ok(null);
                }
            }
            return Ok(null);
        }


        public async Task<IActionResult> GetBarrios(int id)
        {
            if (GetSession<List<P_Barrio>>("Barrios") != null)
            {
                try
                {
                    var barrios = GetSession<List<P_Barrio>>("Barrios");
                    return Ok(barrios);
                }
                catch (Exception)
                {
                    return Ok(null);
                }
            }
            return Ok(null);
        }

        public async Task<IActionResult> CargarClientePorTelefono(string telefono)
        {

            string cliente = "";
            if (GetSession<List<P_Cliente>>("Clientes") != null)
            {
                try
                {
                    var clientes = GetSession<List<P_Cliente>>("Clientes");
                    var cli = clientes.FirstOrDefault(x => x.telefono == telefono);
                    if (cli != null)
                    {
                        cliente = cli.nombre;
                    }
                    return Ok(cliente);
                }
                catch
                {

                }
            }
            return Ok(cliente);
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

        public async Task<IActionResult> DeletePruducto(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            try
            {
                var currentPedido = GetSession<P_Pedido>("currentPedido");
                var producto = currentPedido.productos.First(x => x.id == id);
                currentPedido.productos.Remove(producto);
                SetSession("currentPedido", currentPedido);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarcarProductoPreparado([FromBody] MarcarProducto marcarProducto)
        {
            try
            {
                //Actualizar pedido base
                var pedido = await _context.P_Pedidos.FindAsync(marcarProducto.idPedido);
                pedido.productos = pedido.jsonListProductos.ConvertTo<List<P_Productos>>();
                pedido.productos.Where(x => x.id == marcarProducto.idProducto && x.posicion == marcarProducto.posicion).Select(p => { p.fecha_preparado = DateTime.Now.ToSouthAmericaStandard(); return p; }).ToList();
                pedido.jsonListProductos = pedido.productos.ToJson();
                _context.P_Pedidos.Update(pedido);
                await _context.SaveChangesAsync();
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> MarcarProductoEntregado([FromBody] MarcarProducto marcarProducto)
        //{
        //    try
        //    {
        //        //Actualizar pedido base
        //        var pedido = await _context.P_Pedidos.FindAsync(marcarProducto.idPedido);
        //        pedido.productos = pedido.jsonListProductos.ConvertTo<List<P_Productos>>();
        //        pedido.productos.Where(x => x.id == marcarProducto.idProducto && x.posicion == marcarProducto.posicion).Select(p => { p.fecha_entrega = DateTime.Now.ToSouthAmericaStandard(); return p; }).ToList();
        //        pedido.jsonListProductos = pedido.productos.ToJson();
        //        _context.P_Pedidos.Update(pedido);
        //        await _context.SaveChangesAsync();
        //        return Ok(pedido);
        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound();
        //    }
        //}

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

        //CARDAPIO

        public async Task<IActionResult> GuardarMensaje([FromBody] MessageHub message)
        {
            var mensajes = GetSession<List<MessageHub>>("CardapioMensajes");

            if (mensajes == null)
            {
                mensajes = new List<MessageHub>();
            }

            mensajes.Add(message);

            SetSession("CardapioMensajes", mensajes);
            return Ok(message);

        }

        public IActionResult CargarGruposMensajes()
        {
            var mensajes = GetSession<List<MessageHub>>("CardapioMensajes");

            if (mensajes == null)
            {
                mensajes = new List<MessageHub>();
            }

            mensajes = mensajes.Where(x => x != null).ToList();

            var grupos = from mensage in mensajes
                         group mensage by mensage.codigoConeccionCliente into grupo
                         select new GroupMessageHub
                         {
                             codigoConeccionCliente = grupo.Key,
                             nombreCliente = grupo.FirstOrDefault().nombreCliente,
                             mesa = grupo.FirstOrDefault().mesa,
                             sinLeer = 3,
                             mensajes = grupo.ToList()
                         };

            grupos.OrderBy(x => x.codigoConeccionCliente).ToList();

            return Ok(grupos);

        }

        public async Task<IActionResult> CardapioGuardarProductoPendiente([FromBody] P_Productos producto)
        {
            var productosPendientes = GetSession<List<P_Productos>>("CardapioProductosPendientes");

            if (productosPendientes == null)
            {
                productosPendientes = new List<P_Productos>();
            }

            producto.fecha_pedido = DateTime.Now.ToSouthAmericaStandard();
            producto.imagen = null;

            if (!string.IsNullOrEmpty(producto.tamanhoSeleccionado))
            {
                producto.valor = producto.valorTamanhoSeleccionado;
            }

            producto.Adicionales.RemoveAll(a => a.cantidad == 0);
            producto.Ingredientes.RemoveAll(i => i.selected);

            productosPendientes.Add(producto);
            productosPendientes.Where(x => x.isNew).Select((p, i) => { p.posicion = i; return p; }).ToList();

            SetSession("CardapioProductosPendientes", productosPendientes);

            var index = productosPendientes.IndexOf(producto);
            return Ok(new { producto, index });
        }

        public IActionResult CargarProductosPendientes()
        {
            var productosPendientes = GetSession<List<P_Productos>>("CardapioProductosPendientes");

            if (productosPendientes == null)
            {
                productosPendientes = new List<P_Productos>();
            }

            return Ok(productosPendientes.OrderBy(x => x.posicion).ToList());

        }

    }
}

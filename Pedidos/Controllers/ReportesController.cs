using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Extensions;
using Pedidos.Models;
using Pedidos.Models.Enums;
using Pedidos.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Controllers
{

    public class VentasProductoPorPeriodo
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int count { get; set; }
    }

    public class ReportesController : BaseController
    {
        private readonly AppDbContext _context;

        public ReportesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> VentasPorPeriodo()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            try
            {
                ViewBag.HayVentas = false;
                var fecha = DateTime.Now.ToSouthAmericaStandard();
                var pedidos = await _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.jsonFormaPagamento != null && x.status == StatusPedido.Finalizado.ToString() && x.fecha == fecha).ToListAsync();
                var caja = new P_Caja();

                if (pedidos.Count > 0)
                {
                    ViewBag.HayVentas = true;
                    var formasPagamento = await _context.P_FormaPagamento.Where(x => x.idCuenta == Cuenta.id).ToListAsync();

                    foreach (var pedido in pedidos)
                    {

                        pedido.listaFormaPagamento = pedido.jsonFormaPagamento.ConvertTo<List<P_FormaPagamento>>().Where(x => x.valor.HasValue).OrderBy(x => x.nombre).ToList();

                        foreach (var item in pedido.listaFormaPagamento)
                        {
                            formasPagamento.Where(x => x.id == item.id).ToList().ForEach(x => x.valor += item.valor.Value);
                            if (item.tasa.HasValue)
                            {
                                formasPagamento.Where(x => x.id == item.id).ToList().ForEach(x => x.valorTasa += item.valorTasa);
                            }
                        }
                    }


                    caja.idCuenta = Cuenta.id;
                    caja.idUltimoPedido = pedidos.OrderBy(x => x.id).LastOrDefault().id;
                    caja.fecha = DateTime.Now;
                    caja.totalVentas = pedidos.Sum(x => x.valorProductos);
                    caja.totalDescuentos = pedidos.Sum(x => x.descuento);
                    caja.totalTasasEntrega = pedidos.Sum(x => x.tasaEntrega);
                    caja.totalTasas = pedidos.Sum(x => x.listaFormaPagamento.Sum(f => f.valorTasa));
                    caja.formaPagamentos = formasPagamento.OrderBy(x => x.nombre).Where(x => x.valor > 0).ToList();
                    caja.jsonFormaPagamento = caja.formaPagamentos.ToJson();

                }

                return View(caja);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = true;
                await InsertLog(_context, Cuenta.id, ex.ToString());
                PrompErro(ex.Message);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VentasPorPeriodo(VentasPorPeriodoFilter filtro)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            try
            {
                ViewBag.HayVentas = false;

                var fechaActual = DateTime.Now.ToSouthAmericaStandard();

                DateTime? fechaInicio = null;
                DateTime? fechaFin = null;

                ViewBag.FechaInicio = null;
                ViewBag.FechaFin = null;

                if (filtro.FechaInicio.HasValue)
                {
                    fechaInicio = filtro.FechaInicio.Value.ToSouthAmericaStandard();
                    ViewBag.FechaInicio = filtro.FechaInicio.Value.ToSouthAmericaStandard();
                }
                if (filtro.FechaFin.HasValue)
                {
                    fechaFin = filtro.FechaFin.Value.ToSouthAmericaStandard();
                    ViewBag.FechaFin = filtro.FechaFin.Value.ToSouthAmericaStandard();
                }


                var query = _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.jsonFormaPagamento != null && x.status == StatusPedido.Finalizado.ToString() && x.fecha == fechaActual);
                if (filtro.FechaInicio.HasValue && filtro.FechaFin.HasValue)
                {
                    query = _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.jsonFormaPagamento != null && x.status == StatusPedido.Finalizado.ToString() && x.fecha >= fechaInicio && x.fecha <= fechaFin);

                }
                else if (filtro.FechaInicio.HasValue)
                {
                    query = _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.jsonFormaPagamento != null && x.status == StatusPedido.Finalizado.ToString() && x.fecha >= fechaInicio);

                }
                else if (filtro.FechaFin.HasValue)
                {
                    query = _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.jsonFormaPagamento != null && x.status == StatusPedido.Finalizado.ToString() && x.fecha <= fechaFin);

                }

                var pedidos = await query.ToListAsync();
                var caja = new P_Caja();

                if (pedidos.Count > 0)
                {
                    ViewBag.HayVentas = true;
                    var formasPagamento = await _context.P_FormaPagamento.Where(x => x.idCuenta == Cuenta.id).ToListAsync();

                    foreach (var pedido in pedidos)
                    {

                        pedido.listaFormaPagamento = pedido.jsonFormaPagamento.ConvertTo<List<P_FormaPagamento>>().Where(x => x.valor.HasValue).OrderBy(x => x.nombre).ToList();

                        foreach (var item in pedido.listaFormaPagamento)
                        {
                            formasPagamento.Where(x => x.id == item.id).ToList().ForEach(x => x.valor += item.valor.Value);
                            if (item.tasa.HasValue)
                            {
                                formasPagamento.Where(x => x.id == item.id).ToList().ForEach(x => x.valorTasa += item.valorTasa);
                            }
                        }
                    }


                    caja.idCuenta = Cuenta.id;
                    caja.idUltimoPedido = pedidos.OrderBy(x => x.id).LastOrDefault().id;
                    caja.fecha = DateTime.Now;
                    caja.totalVentas = pedidos.Sum(x => x.valorProductos);
                    caja.totalDescuentos = pedidos.Sum(x => x.descuento);
                    caja.totalTasasEntrega = pedidos.Sum(x => x.tasaEntrega);
                    caja.totalTasas = pedidos.Sum(x => x.listaFormaPagamento.Sum(f => f.valorTasa));
                    caja.formaPagamentos = formasPagamento.OrderBy(x => x.nombre).Where(x => x.valor > 0).ToList();
                    caja.jsonFormaPagamento = caja.formaPagamentos.ToJson();

                }

                return View(caja);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = true;
                await InsertLog(_context, Cuenta.id, ex.ToString());
                PrompErro(ex.Message);
                return View();
            }
        }

        public async Task<ActionResult> VentasProductoPorPeriodo()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            ViewBag.HayVentas = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VentasProductoPorPeriodo(VentasPorPeriodoFilter filtro)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            try
            {
                ViewBag.HayVentas = false;

                var fechaActual = DateTime.Now.ToSouthAmericaStandard();

                DateTime? fechaInicio = null;
                DateTime? fechaFin = null;

                ViewBag.FechaInicio = null;
                ViewBag.FechaFin = null;

                if (filtro.FechaInicio.HasValue)
                {
                    fechaInicio = filtro.FechaInicio.Value.ToSouthAmericaStandard();
                    ViewBag.FechaInicio = filtro.FechaInicio.Value.ToSouthAmericaStandard();
                }
                if (filtro.FechaFin.HasValue)
                {
                    fechaFin = filtro.FechaFin.Value.ToSouthAmericaStandard();
                    ViewBag.FechaFin = filtro.FechaFin.Value.ToSouthAmericaStandard();
                }


                var query = _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.jsonFormaPagamento != null && x.status == StatusPedido.Finalizado.ToString() && x.fecha == fechaActual);
                if (filtro.FechaInicio.HasValue && filtro.FechaFin.HasValue)
                {
                    query = _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.jsonFormaPagamento != null && x.status == StatusPedido.Finalizado.ToString() && x.fecha >= fechaInicio && x.fecha <= fechaFin);

                }
                else if (filtro.FechaInicio.HasValue)
                {
                    query = _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.jsonFormaPagamento != null && x.status == StatusPedido.Finalizado.ToString() && x.fecha >= fechaInicio);

                }
                else if (filtro.FechaFin.HasValue)
                {
                    query = _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.jsonFormaPagamento != null && x.status == StatusPedido.Finalizado.ToString() && x.fecha <= fechaFin);

                }

                var pedidos = await query.ToListAsync();
                var caja = new P_Caja();

                var listaProdustos = new List<P_Productos>();

                foreach (var item in pedidos)
                {
                    listaProdustos.AddRange(item.jsonListProductos.ConvertTo<List<P_Productos>>());
                }

                var grupo = listaProdustos.GroupBy(x => x.id).ToList();

                var ventasProductoPorPeriodo = new List<VentasProductoPorPeriodo>();

                foreach (var g in grupo)
                {
                    var productoPorPeriodo = new VentasProductoPorPeriodo();
                    productoPorPeriodo.id = g.Key;
                    productoPorPeriodo.nombre = g.ToList().FirstOrDefault().nombre;
                    productoPorPeriodo.count = g.ToList().Sum(x => x.cantidad);
                    ventasProductoPorPeriodo.Add(productoPorPeriodo);
                }

                ViewBag.VentasProductoPorPeriodo = ventasProductoPorPeriodo.OrderByDescending(x => x.count);

                if (ventasProductoPorPeriodo != null && ventasProductoPorPeriodo.Any())
                    ViewBag.HayVentas = true;
                //if (pedidos.Count > 0)
                //{
                //    ViewBag.HayVentas = true;
                //    var formasPagamento = await _context.P_FormaPagamento.Where(x => x.idCuenta == Cuenta.id).ToListAsync();

                //    foreach (var pedido in pedidos)
                //    {



                //        pedido.listaFormaPagamento = pedido.jsonFormaPagamento.ConvertTo<List<P_FormaPagamento>>().Where(x => x.valor.HasValue).OrderBy(x => x.nombre).ToList();

                //        foreach (var item in pedido.listaFormaPagamento)
                //        {
                //            formasPagamento.Where(x => x.id == item.id).ToList().ForEach(x => x.valor += item.valor.Value);
                //            if (item.tasa.HasValue)
                //            {
                //                formasPagamento.Where(x => x.id == item.id).ToList().ForEach(x => x.valorTasa += item.valorTasa);
                //            }
                //        }
                //    }


                //    caja.idCuenta = Cuenta.id;
                //    caja.idUltimoPedido = pedidos.OrderBy(x => x.id).LastOrDefault().id;
                //    caja.fecha = DateTime.Now;
                //    caja.totalVentas = pedidos.Sum(x => x.valorProductos);
                //    caja.totalDescuentos = pedidos.Sum(x => x.descuento);
                //    caja.totalTasasEntrega = pedidos.Sum(x => x.tasaEntrega);
                //    caja.totalTasas = pedidos.Sum(x => x.listaFormaPagamento.Sum(f => f.valorTasa));
                //    caja.formaPagamentos = formasPagamento.OrderBy(x => x.nombre).Where(x => x.valor > 0).ToList();
                //    caja.jsonFormaPagamento = caja.formaPagamentos.ToJson();

                //}

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Erro = true;
                await InsertLog(_context, Cuenta.id, ex.ToString());
                PrompErro(ex.Message);
                return View();
            }
        }

    }
}

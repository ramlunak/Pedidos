using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Extensions;
using Pedidos.Models;
using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Controllers
{
    [Authorize(Roles = "Administrador,Establecimiento,Funcionario")]
    public class CajaController : BaseController
    {
        private readonly AppDbContext _context;

        public CajaController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Lista()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var model = await _context.P_Caja.Where(x => x.idCuenta == Cuenta.id).Take(50).ToListAsync();
            ViewBag.CajaAbierta = model.Any(x => x.isOpen);

            if (model.Any(x => x.isOpen))
            {
                //ACTUALIZAR CAJA ABIERTA
                var caja = model.FirstOrDefault(x => x.isOpen);
                var pedidos = await GetPedidosPorCaja(caja);
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

                    caja.idPrimerPedido = pedidos.OrderBy(x => x.id).FirstOrDefault().id;
                    caja.codigoPrimerPedido = pedidos.OrderBy(x => x.id).FirstOrDefault().codigo;
                    caja.idUltimoPedido = pedidos.OrderBy(x => x.id).LastOrDefault().id;
                    caja.codigoUltimoPedido = pedidos.OrderBy(x => x.id).LastOrDefault().codigo;
                    caja.totalVentas = pedidos.Sum(x => x.valorProductos);
                    caja.totalDescuentos = pedidos.Sum(x => x.descuento);
                    caja.totalTasasEntrega = pedidos.Sum(x => x.tasaEntrega);
                    caja.totalTasas = pedidos.Sum(x => x.listaFormaPagamento.Sum(f => f.valorTasa));
                    caja.formaPagamentos = formasPagamento.OrderBy(x => x.nombre).Where(x => x.valor > 0).ToList();
                    caja.jsonFormaPagamento = caja.formaPagamentos.ToJson();

                }

                caja.fechaCierre = DateTime.Now.ToSouthAmericaStandard();

                _context.P_Caja.Update(caja);
                await _context.SaveChangesAsync();

                model.Where(x => x.isOpen).Select(c => { c = caja; return c; });
            }

            return View(model.OrderByDescending(x => x.id).ToList());
        }

        public async Task<IActionResult> AbrirCaja(decimal valor)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var cajaAbierta = await _context.P_Caja.Where(x => x.idCuenta == Cuenta.id && x.isOpen).CountAsync();
            if (cajaAbierta > 0)
            {
                return Ok(new { open = false, erro = "A caixa já foi aberta" });
            }

            try
            {
                var caja = new P_Caja();
                caja.idCuenta = Cuenta.id;
                caja.inicio = valor;
                caja.fecha = DateTime.Now.ToSouthAmericaStandard();
                caja.isOpen = true;

                _context.Add(caja);
                await _context.SaveChangesAsync();
                return Ok(new { open = true });
            }
            catch (Exception ex)
            {
                return Ok(new { open = false, erro = ex.ToString() });
            }
        }

        public async Task<ActionResult> Detalle(int? id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            try
            {
                ViewBag.HayVentas = false;

                var caja = await _context.P_Caja.FindAsync(id);
                var pedidos = await GetPedidosPorCaja(caja);
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

                    caja.idPrimerPedido = pedidos.OrderBy(x => x.id).FirstOrDefault().id;
                    caja.idUltimoPedido = pedidos.OrderBy(x => x.id).LastOrDefault().id;
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

        public async Task<ActionResult> CerrarCaja(int? id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var caja = await _context.P_Caja.FindAsync(id);
            caja.isOpen = false;
            caja.fechaCierre = DateTime.Now.ToSouthAmericaStandard();
            _context.P_Caja.Update(caja);
            await _context.SaveChangesAsync();

            try
            {
                var salidasCaja = await _context.P_SalidaCaja.Where(x => x.idCuenta == Cuenta.id && !x.idCaja.HasValue).ToListAsync();
                foreach (var salida in salidasCaja)
                {
                    salida.idCaja = caja.id;
                    _context.P_SalidaCaja.Update(salida);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                PrompErro(ex.ToString());
            }

            return RedirectToAction(nameof(Lista));
        }

        public async Task<List<P_Pedido>> GetPedidosPorCaja(P_Caja caja)
        {
            var pedidos = new List<P_Pedido>();

            if (caja.isOpen)
            {
                var ultimaCajaCerrada = await _context.P_Caja.Where(x => x.idCuenta == Cuenta.id && !x.isOpen).OrderByDescending(x => x.id).Take(1).ToListAsync();
                var idUltimoPedidoCerrado = 0;
                if (ultimaCajaCerrada.Any())
                {
                    idUltimoPedidoCerrado = ultimaCajaCerrada.FirstOrDefault().idUltimoPedido.Value;
                }
                pedidos = await _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.status == StatusPedido.Finalizado.ToString() && x.id > idUltimoPedidoCerrado).ToListAsync();
            }
            else
            {
                pedidos = await _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.status == StatusPedido.Finalizado.ToString() && x.id >= caja.idPrimerPedido & x.id <= caja.idUltimoPedido).ToListAsync();
            }

            return pedidos;
        }

    }
}

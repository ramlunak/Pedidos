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
    public class CajaControllerOld : BaseController
    {
        private readonly AppDbContext _context;

        public CajaControllerOld(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ActionResult> Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            try
            {
                var caja = await _context.P_Caja.Where(x => x.idCuenta == Cuenta.id && x.isOpen).FirstOrDefaultAsync();

                ViewBag.HayVentas = false;

                if (caja == null)
                {
                    PrompInfo("Deve abrir a caixa para poder continuar");
                    return RedirectToAction(nameof(Abrir));
                }

                var pedidos = await _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.jsonFormaPagamento != null && x.status == StatusPedido.Finalizado.ToString() && x.id > caja.idUltimoPedido).ToListAsync();

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
                    caja.idUltimoPedido = pedidos.OrderBy(x => x.id).FirstOrDefault().id;
                    caja.idUltimoPedido = pedidos.OrderBy(x => x.id).LastOrDefault().id;
                    caja.fecha = DateTime.Now;
                    caja.totalVentas = pedidos.Sum(x => x.valorProductos);
                    caja.totalDescuentos = pedidos.Sum(x => x.descuento);
                    caja.totalTasasEntrega = pedidos.Sum(x => x.tasaEntrega);
                    caja.totalTasas = pedidos.Sum(x => x.listaFormaPagamento.Sum(f => f.valorTasa));
                    caja.formaPagamentos = formasPagamento.OrderBy(x => x.nombre).Where(x => x.valor > 0).ToList();
                    caja.jsonFormaPagamento = caja.formaPagamentos.ToJson();
                    SetSession("Caja", caja);
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

        public async Task<ActionResult> Lista()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var model = await _context.P_Caja.Where(x => x.idCuenta == Cuenta.id && !x.isOpen).Take(50).ToListAsync();

            return View(model.OrderByDescending(x => x.id).ToList());
        }

        public async Task<ActionResult> Fechar()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var caja = GetSession<P_Caja>("Caja");
            caja.isOpen = false;
            _context.P_Caja.Update(caja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        public async Task<ActionResult> Abrir()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            //var caja = GetSession<P_Caja>("Caja");
            //_context.Add(caja);
            //await _context.SaveChangesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Abrir(P_Caja p_Caja)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var cajaAbierta = await _context.P_Caja.Where(x => x.idCuenta == Cuenta.id && x.isOpen).CountAsync();
            if (cajaAbierta > 0)
            {
                PrompInfo("A caixa já foi aberta");
                return View(p_Caja);
            }

            var countCierres = await _context.P_Caja.Where(x => x.idCuenta == Cuenta.id).CountAsync();
            var ultimoIdPedido = 0;
            if (countCierres > 0)
            {
                var ultimoCierre = await _context.P_Caja.OrderByDescending(x => x.id).Where(x => x.idCuenta == Cuenta.id && !x.isOpen).Take(1).ToListAsync();
                ultimoIdPedido = ultimoCierre.First().idUltimoPedido;
            }


            p_Caja.idCuenta = Cuenta.id;
            p_Caja.idUltimoPedido = ultimoIdPedido;
            p_Caja.fecha = DateTime.Now.ToSouthAmericaStandard();
            p_Caja.isOpen = true;

            _context.Add(p_Caja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}

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
    public class CajaController : BaseController
    {
        private readonly AppDbContext _context;

        public CajaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CajaController
        public async Task<ActionResult> Index()
        {
            //var countCierres = await _context.P_Caja.Where(x => x.idCuenta == Cuenta.id).CountAsync();

            //var pedidos = new List<P_Pedido>();
            //if (countCierres > 0)
            //{
            //    var ultimoCierre = await _context.P_Caja.OrderByDescending(x => x.id).Where(x => x.idCuenta == Cuenta.id).Take(1).ToListAsync();
            //    var ultimoIdPedido = ultimoCierre.First().idUltimoPedido;
            //    pedidos = await _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.jsonFormaPagamento != null && x.status == StatusPedido.Finalizado.ToString() && x.id > ultimoIdPedido).ToListAsync();
            //}
            //else
            //{
            //    pedidos = await _context.P_Pedidos.Where(x =>
            //   x.idCuenta == Cuenta.id &&
            //   x.jsonFormaPagamento != null &&
            //   x.status == StatusPedido.Finalizado.ToString()).ToListAsync();
            //}

            //  var caja = new P_Caja();

            var caja = await _context.P_Caja.Where(x => x.idCuenta == Cuenta.id && x.isOpen).FirstOrDefaultAsync();

            ViewBag.HayVentas = false;

            if (caja == null)
            {
                PrompInfo("Deve abrir a caixa para poder continuar");
                return View();
            }

           

            //if (pedidos.Count > 0)
            //{
            //    ViewBag.HayVentas = true;
            //    var formasPagamento = await _context.P_FormaPagamento.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            //    foreach (var pedido in pedidos)
            //    {
            //        pedido.listaFormaPagamento = pedido.jsonFormaPagamento.ConvertTo<List<P_FormaPagamento>>().OrderBy(x => x.nombre).ToList();
            //        foreach (var item in pedido.listaFormaPagamento)
            //        {
            //            formasPagamento.Where(x => x.id == item.id).ToList().ForEach(x => x.valor += item.valor);
            //            if (item.tasa.HasValue)
            //            {
            //                formasPagamento.Where(x => x.id == item.id).ToList().ForEach(x => x.valorTasa += item.valorTasa);
            //            }
            //        }
            //    }

            //    caja.idCuenta = Cuenta.id;
            //    caja.idUltimoPedido = pedidos.OrderBy(x => x.id).LastOrDefault().id;
            //    caja.fecha = DateTime.Now;
            //    //caja.totalVentas = pedidos.Sum(x => x.total);
            //    caja.totalTasas = pedidos.Sum(x => x.listaFormaPagamento.Sum(f => f.valorTasa));
            //    caja.formaPagamentos = formasPagamento.OrderBy(x => x.nombre).Where(x => x.valor > 0).ToList();
            //    caja.jsonFormaPagamento = caja.formaPagamentos.ToJson();
            //    SetSession("Caja", caja);
            //}

            return View(caja);
        }

        public async Task<ActionResult> Fechar()
        {
            var caja = GetSession<P_Caja>("Caja");
            _context.Add(caja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Abrir()
        {
            //var caja = GetSession<P_Caja>("Caja");
            //_context.Add(caja);
            //await _context.SaveChangesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Abrir(P_Caja p_Caja)
        {

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
                var ultimoCierre = await _context.P_Caja.OrderByDescending(x => x.id).Where(x => x.idCuenta == Cuenta.id).Take(1).ToListAsync();
                ultimoIdPedido = ultimoCierre.First().idUltimoPedido;
            }

            var caja = new P_Caja();
            caja.idCuenta = Cuenta.id;
            caja.idUltimoPedido = ultimoIdPedido;
            caja.fecha = DateTime.Now.ToSouthAmericaStandard();
            caja.isOpen = true;
            caja.isOpen = true;
            _context.Add(caja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}

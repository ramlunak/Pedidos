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
            var pedidos = await _context.P_Pedidos.Where(x => x.idCuenta == Cuenta.id && x.status == StatusPedido.Finalizado.ToString()).ToListAsync();
            var formasPagamento = await _context.P_FormaPagamento.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            foreach (var pedido in pedidos)
            {
                pedido.listaFormaPagamento = pedido.jsonFormaPagamento.ConvertTo<List<P_FormaPagamento>>().OrderBy(x => x.nombre).ToList();
                foreach (var item in pedido.listaFormaPagamento)
                {
                    formasPagamento.Where(x => x.id == item.id).ToList().ForEach(x => x.valor += item.valor);
                    if (item.tasa.HasValue)
                    {
                        formasPagamento.Where(x => x.id == item.id).ToList().ForEach(x => x.valorTasa += item.valorTasa);
                    }
                }
            }

            var caja = new P_Caja();
            caja.idCuenta = Cuenta.id;
            caja.fecha = DateTime.Now;
            caja.dataInicio = DateTime.Now;
            caja.dataFin = DateTime.Now;
            caja.totalVentas = pedidos.Sum(x => x.total);
            caja.totalTasas = pedidos.Sum(x => x.listaFormaPagamento.Sum(f => f.valorTasa));
            caja.formaPagamentos = formasPagamento;
            
            return View(caja);
        }

        // GET: CajaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CajaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CajaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CajaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CajaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CajaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CajaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

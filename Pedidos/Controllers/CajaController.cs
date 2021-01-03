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
            foreach (var pedido in pedidos)
            {
                pedido.productos = pedido.jsonListProductos.ConvertTo<List<P_Productos>>();
                pedido.listaFormaPagamento = pedido.jsonListProductos.ConvertTo<List<P_FormaPagamento>>();
            }

            return View();
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

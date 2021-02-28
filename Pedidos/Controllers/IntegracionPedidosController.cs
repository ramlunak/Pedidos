using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;
using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Controllers
{
    [Authorize(Roles = "Integracion")]
    public class IntegracionPedidosController : BaseController
    {
        private readonly AppDbContext _context;

        public IntegracionPedidosController(AppDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            return View();
        }

        public async Task<IActionResult> GetGruposPedidoPorBarrio()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            try
            {
                var integracionesPedido = await _context.P_IntegracionPedidos.Where(x => x.idCuentaIntegracion == Cuenta.id && x.statusIntegracion == StatusIntegracionPedido.Esperando.ToString()).ToListAsync();
                var grupoPedidosPorBarrio = from integracion in integracionesPedido
                                            group integracion by integracion.barrio.ToUpper() into g
                                            select new DTOGrupoPedidosPorBarrio
                                            {
                                                barrio = g.Key,
                                                idBarrio = g.First().idBarrio,
                                                count = g.Count(),
                                                listIntegracionPedidos = g.ToList()
                                            };

                return Ok(grupoPedidosPorBarrio);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> AddNuevaRuta()
        {
            var ruta = new P_IntegracionRuta();
            if (GetSession<P_IntegracionRuta>("IntegracionRuta") != null)
            {
                return NotFound();
            }
            else
            {
                ruta.idCuentaIntegracion = Cuenta.id;
                SetSession("IntegracionRuta", ruta);
            }
            return Ok(ruta);
        }

        public async Task<IActionResult> GetRutas()
        {
            var rutas = new List<P_IntegracionRuta>();
            var currentRuta = new P_IntegracionRuta();
            if (GetSession<P_IntegracionRuta>("IntegracionRuta") != null)
            {
                currentRuta = GetSession<P_IntegracionRuta>("IntegracionRuta");
                rutas.Add(currentRuta);
            }

            return Ok(rutas);
        }


    }
}

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
                                                listIntegracionPedidos = g.OrderBy(x => x.idCuentaIntegracion).ToList()
                                            };
                SetSession("integracionesGrupoPedidos", grupoPedidosPorBarrio.ToList());
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

        public async Task<IActionResult> AddBarrio([FromBody] DTOGrupoPedidosPorBarrio dTOGrupoPedidosPorBarrio)
        {

            //Adicionar pedido a la ruta
            var currentRuta = GetSession<P_IntegracionRuta>("IntegracionRuta");
            if (currentRuta != null)
            {

                var barios = new List<DTOIntegracionBarrio>();
                if (currentRuta.barrios != null)
                    barios = currentRuta.barrios.ToList();

                if (barios.Where(x => x.nombre.ToLower() == dTOGrupoPedidosPorBarrio.barrio.ToLower()).Any())
                {
                    barios.Where(x => x.nombre.ToLower() == dTOGrupoPedidosPorBarrio.barrio.ToLower()).Select(x => { x.count++; return x; }).ToList();
                }
                else
                {
                    barios.Add(new DTOIntegracionBarrio
                    {
                        nombre = dTOGrupoPedidosPorBarrio.barrio,
                        count = 1
                    });
                }

                currentRuta.barrios = barios.ToArray();
                SetSession("IntegracionRuta", currentRuta);
            }

            //REMOVER primer integracion pedido
            var grupoPedidosPorBarrio = GetSession<List<DTOGrupoPedidosPorBarrio>>("integracionesGrupoPedidos");
            grupoPedidosPorBarrio.Where(x => x.barrio.ToLower() == dTOGrupoPedidosPorBarrio.barrio.ToLower()).Select(x => { x.listIntegracionPedidos.RemoveAt(0); x.count--; return x; }).ToList();
            grupoPedidosPorBarrio = grupoPedidosPorBarrio.Where(x => x.count > 0).ToList();

            //ACTUALIZAR IntegracionPedido en BD
            var idIntegracionPedido = dTOGrupoPedidosPorBarrio.listIntegracionPedidos.FirstOrDefault().id;
            var result = await _context.Database.ExecuteSqlRawAsync($"UPDATE [dbo].[P_IntegracionPedidos] SET [statusIntegracion] = 'EnCurrentRuta' WHERE id = {idIntegracionPedido}");

            SetSession("integracionesGrupoPedidos", grupoPedidosPorBarrio.ToList());

            return Ok(new { currentRuta, currentRuta.barrios, grupoPedidosPorBarrio });

        }


    }
}

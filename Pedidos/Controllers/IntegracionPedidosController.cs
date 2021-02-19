using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Controllers
{
    [Authorize(Roles = "Integracion")]
    public class IntegracionPedidosController : BaseController
    {
        // GET: IntegracionPedidosController
        public ActionResult Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            return View();
        }

    }
}

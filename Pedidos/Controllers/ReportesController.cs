using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Controllers
{
    public class ReportesController : BaseController
    {

        public ActionResult VentasPorPeriodo()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }


            return View();
        }

    }
}

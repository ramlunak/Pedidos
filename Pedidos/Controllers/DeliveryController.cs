using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Controllers
{
    public class DeliveryController : Controller
    {
        public IActionResult Index(string link)
        {
            ViewBag.Link = link;
            return View();
        }
    }
}

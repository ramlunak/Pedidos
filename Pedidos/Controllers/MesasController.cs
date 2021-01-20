using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Controllers
{
    public class MesasController : BaseController
    {
        public ActionResult Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode("https://www.mastereat.com.br/cardapio", QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(qRCodeData);
                using (Bitmap oBitmat = qRCode.GetGraphic(20))
                {
                    oBitmat.Save(ms, ImageFormat.Png);
                    ViewBag.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }


            return View();
        }

        public async Task<ActionResult> Lista()
        {
            return View();
        }

    }
}

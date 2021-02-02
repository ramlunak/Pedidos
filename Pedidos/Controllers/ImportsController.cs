using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Controllers
{
    public class ImportsController : BaseController
    {
        // GET: ImportsController
        public ActionResult Producto()
        {

            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Producto(IFormFile file)
        {

            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var productos = new List<P_Productos>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        try
                        {
                            var sd = reader.GetValue(0);
                            ;
                        }
                        catch (Exception ex)
                        {
                            ;
                        }
                        
                    }
                }

                //using (var package = new ExcelPackage())
                //{
                //    ExcelWorksheets worksheets = package.Workbook.Worksheets;
                //    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                //    var rowcount = worksheet.Dimension.Rows;

                //    for (int row = 2; row <= rowcount; row++)
                //    {
                //        productos.Add(new P_Productos
                //        {
                //            nombre = worksheet.Cells[row, 2].ToString().Trim()
                //        });
                //    }

                //}
            }

            var sdafsd = productos;
            return View();
        }

        // GET: ImportsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ImportsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImportsController/Create
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

        // GET: ImportsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ImportsController/Edit/5
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

        // GET: ImportsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ImportsController/Delete/5
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

using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Pedidos.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Pedidos.Controllers
{
    [Authorize(Roles = "Administrador,Establecimiento")]
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

            var categorias = new List<P_Categoria>();
            var productos = new List<P_Productos>();
            var rowErros = 0;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var row = 1;
                   
                    while (reader.Read())
                    {

                        if (row >= 4)
                        {
                            try
                            {
                                //CARGAR CATEGORIAS
                                var nomeCategoria = reader.GetValue(0);
                                if (nomeCategoria != null && !nomeCategoria.ToString().IsNullOrEmtpy())
                                {
                                    categorias.Add(new P_Categoria
                                    {
                                        nombre = nomeCategoria.ToString().Trim()
                                    });
                                }

                                //CARGAR PRODUCTOS
                                var nomeProducto = reader.GetValue(2);
                                var categoriaProducto = reader.GetValue(3);
                                var valor = reader.GetValue(4);
                                var horas = reader.GetValue(5);
                                var minutos = reader.GetValue(6);
                                var descripcion = reader.GetValue(7);
                                var tamanho1 = reader.GetValue(8);
                                var valorTamanho1 = reader.GetValue(9);
                                var tamanho2 = reader.GetValue(10);
                                var valorTamanho2 = reader.GetValue(11);
                                var tamanho3 = reader.GetValue(12);
                                var valorTamanho3 = reader.GetValue(13);

                                if (nomeProducto == null || categoriaProducto == null)
                                {
                                    rowErros++;
                                }
                                else if (valor == null && valorTamanho1 == null && valorTamanho2 == null && valorTamanho3 == null)
                                {
                                    rowErros++;
                                }
                                else
                                {
                                    var producto = new P_Productos();
                                    producto.nombre = nomeProducto.ToString().Trim();
                                    producto.Categoria = categoriaProducto.ToString().Trim();
                                    producto.valor = valor is null ? 0 : Convert.ToDecimal(valor.ToString().Trim().Replace(",", "."));
                                    producto.horasPreparacion = horas is null ? 0 : Convert.ToInt32(horas.ToString().Trim());
                                    producto.minutosPreparacion = minutos is null ? 0 : Convert.ToInt32(minutos.ToString().Trim());
                                    producto.descripcion = descripcion.ToString();
                                    producto.tamanho1 = tamanho1 is null ? null : tamanho1.ToString().Trim();
                                    producto.valorTamanho1 = valorTamanho1 is null ? 0 : Convert.ToDecimal(valorTamanho1.ToString().Trim().Replace(",", "."));
                                    producto.tamanho2 = tamanho2 is null ? null : tamanho2.ToString().Trim();
                                    producto.valorTamanho2 = valorTamanho2 is null ? 0 : Convert.ToDecimal(valorTamanho2.ToString().Trim().Replace(",", "."));
                                    producto.tamanho3 = tamanho3 is null ? null : tamanho3.ToString().Trim();
                                    producto.valorTamanho3 = valorTamanho3 is null ? 0 : Convert.ToDecimal(valorTamanho3.ToString().Trim().Replace(",", "."));

                                    productos.Add(producto);
                                }

                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                        }
                        row++;
                    }
                }

            }

            ViewBag.Categorias = categorias;
            ViewBag.Productos = productos;
            ViewBag.RowErros = rowErros;

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

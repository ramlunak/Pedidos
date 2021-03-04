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
using IronXL;
using Pedidos.Data;
using Microsoft.EntityFrameworkCore;

namespace Pedidos.Controllers
{
    [Authorize(Roles = "Administrador,Establecimiento")]
    public class ImportsController : BaseController
    {
        private readonly AppDbContext _context;

        public ImportsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ImportsController
        public ActionResult Producto()
        {

            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            return View();
        }
                
        [HttpGet]
        public async Task<IActionResult> Producto(string data)
        {

            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var erros = new List<ErrorDetail>();

            if (GetSession<List<P_Categoria>>("Categorias") != null && data.Equals("aplicar"))
            {
                var categorias = GetSession<List<P_Categoria>>("Categorias");
                foreach (var item in categorias) {
                    var result = await _context.P_Categorias.Where(x => x.nombre.Equals(item.nombre) && x.idCuenta == Cuenta.id).ToListAsync();
                    if (result.Count > 0)
                    {
                        erros.Add(new ErrorDetail
                        {
                            Code = item.codigo,
                            Column = "Nome da Categoria",
                            Detail = "A categoria '"+item.nombre+"' já existe no banco de dados"
                        });
                    } else 
                    {
                        item.idCuenta = Cuenta.id;
                        _context.Add(item);
                        await _context.SaveChangesAsync();
                    }
                }

                if (GetSession<List<P_Productos>>("Productos") != null) {
                    var productos = GetSession<List<P_Productos>>("Productos");
                    foreach (var item in productos)
                    {
                        var result = await _context.P_Productos.Where(x => x.nombre.Equals(item.nombre) && x.idCuenta == Cuenta.id).ToListAsync();
                        if (result.Count > 0)
                        {
                            erros.Add(new ErrorDetail
                            {
                                Code = item.codigo,
                                Column = "Nome do Produto",
                                Detail = "O produto '" + item.nombre + "' já existe no banco de dados"
                            });
                        }
                        else
                        {
                            var cat = await _context.P_Categorias.Where(x => x.nombre.Equals(item.Categoria)).ToListAsync();
                            item.idCuenta = Cuenta.id;
                            item.idCategoria = cat.First().id;
                            _context.Add(item);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                ViewBag.ErrosAplicar = erros;
                ViewBag.rowErrosAplicar = erros.Count();
                //SetSession("Categorias", null);
                //SetSession("Productos", null);
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

            WorkBook workbook;

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                workbook = WorkBook.Load(ms);
            }

            WorkSheet sheet = workbook.WorkSheets.First();

            //Select cells easily in Excel notation and return the calculated value
            var cellValue = sheet["A2"].Value;

            var categorias = new List<P_Categoria>();
            var productos = new List<P_Productos>();
            var erros = new List<ErrorDetail>();

            //CARGAR CATEGORIAS
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
                                var errorCategoria = false;
                                var codigoCategoria = reader.GetValue(0);
                                var nomeCategoria = reader.GetValue(1);

                                if (nomeCategoria == null || nomeCategoria.ToString().IsNullOrEmtpy())
                                {
                                    break;
                                }

                                var exist = categorias.Find(
                                    delegate (P_Categoria cat)
                                    {
                                        return cat.nombre.Equals(nomeCategoria.ToString().Trim());
                                    }
                                );

                                if (exist != null)
                                {
                                    erros.Add(new ErrorDetail
                                    {
                                        Row = row,
                                        Code = codigoCategoria.ToString().Trim(),
                                        Column = "Nome da Categoria",
                                        Detail = "Categoria duplicada"
                                    });
                                    errorCategoria = true;
                                }

                                if (!errorCategoria)
                                {
                                    categorias.Add(new P_Categoria
                                    {
                                        codigo = codigoCategoria.ToString().Trim(),
                                        nombre = nomeCategoria.ToString().Trim()
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                //throw ex;
                            }
                        }
                        row++;
                    }
                }
            }

            //CARGAR PRODUCTOS
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
                                var errorProducto = false;
                                var codigoProducto = reader.GetValue(3);
                                var nomeProducto = reader.GetValue(4);
                                var categoriaProducto = reader.GetValue(5);
                                var valor = reader.GetValue(6);
                                var horas = reader.GetValue(7);
                                var minutos = reader.GetValue(8);
                                var descripcion = reader.GetValue(9);
                                var tamanho1 = reader.GetValue(10);
                                var valorTamanho1 = reader.GetValue(11);
                                var tamanho2 = reader.GetValue(12);
                                var valorTamanho2 = reader.GetValue(13);
                                var tamanho3 = reader.GetValue(14);
                                var valorTamanho3 = reader.GetValue(15);
                                var tamanho4 = reader.GetValue(16);
                                var valorTamanho4 = reader.GetValue(17);
                                var tamanho5 = reader.GetValue(18);
                                var valorTamanho5 = reader.GetValue(19);

                                if (nomeProducto == null)
                                {
                                    break;
                                }

                                if (categoriaProducto == null || categoriaProducto.ToString().IsNullOrEmtpy())
                                {
                                    erros.Add(new ErrorDetail
                                    {
                                        Row = row,
                                        Code = codigoProducto.ToString().Trim(),
                                        Column = "Categoria de produto",
                                        Detail = "A categoria do produto é obrigatória"
                                    });
                                    errorProducto = true;
                                }
                                else
                                {
                                    var existCat = categorias.Find(
                                        delegate (P_Categoria cat)
                                        {
                                            return cat.nombre.Equals(categoriaProducto.ToString().Trim());
                                        }
                                    );

                                    if (existCat is null)
                                    {
                                        erros.Add(new ErrorDetail
                                        {
                                            Row = row,
                                            Code = codigoProducto.ToString().Trim(),
                                            Column = "Categoria de produto",
                                            Detail = "A categoria não existe"
                                        });
                                        errorProducto = true;
                                    }
                                }

                                var existPro = productos.Find(
                                    delegate (P_Productos pro)
                                    {
                                        return pro.nombre.Equals(nomeProducto.ToString().Trim());
                                    }
                                );

                                if (existPro != null)
                                {
                                    erros.Add(new ErrorDetail
                                    {
                                        Row = row,
                                        Code = codigoProducto.ToString().Trim(),
                                        Column = "Nome do produto",
                                        Detail = "Produto duplicado"
                                    });
                                    errorProducto = true;
                                }

                                if (valor == null && valorTamanho1 == null && valorTamanho2 == null && valorTamanho3 == null && valorTamanho4 == null && valorTamanho5 == null)
                                {
                                    erros.Add(new ErrorDetail
                                    {
                                        Row = row,
                                        Code = codigoProducto.ToString().Trim(),
                                        Column = "Valor",
                                        Detail = "Deve ter um valor ou pelo menos um tamanho"
                                    });
                                    errorProducto = true;
                                }

                                if ((tamanho1 == null && valorTamanho1 != null) || (tamanho1 != null && valorTamanho1 == null))
                                {
                                    erros.Add(new ErrorDetail
                                    {
                                        Row = row,
                                        Code = codigoProducto.ToString().Trim(),
                                        Column = "Tamanho 1",
                                        Detail = "Se tiver um valor, deve ter um tamanho e vice-versa"
                                    });
                                    errorProducto = true;
                                }

                                if ((tamanho2 == null && valorTamanho2 != null) || (tamanho2 != null && valorTamanho2 == null))
                                {
                                    erros.Add(new ErrorDetail
                                    {
                                        Row = row,
                                        Code = codigoProducto.ToString().Trim(),
                                        Column = "Tamanho 2",
                                        Detail = "Se tiver um valor, deve ter um tamanho e vice-versa"
                                    });
                                    errorProducto = true;
                                }

                                if ((tamanho3 == null && valorTamanho3 != null) || (tamanho3 != null && valorTamanho3 == null))
                                {
                                    erros.Add(new ErrorDetail
                                    {
                                        Row = row,
                                        Code = codigoProducto.ToString().Trim(),
                                        Column = "Tamanho 3",
                                        Detail = "Se tiver um valor, deve ter um tamanho e vice-versa"
                                    });
                                    errorProducto = true;
                                }

                                if ((tamanho4 == null && valorTamanho4 != null) || (tamanho4 != null && valorTamanho4 == null))
                                {
                                    erros.Add(new ErrorDetail
                                    {
                                        Row = row,
                                        Code = codigoProducto.ToString().Trim(),
                                        Column = "Tamanho 4",
                                        Detail = "Se tiver um valor, deve ter um tamanho e vice-versa"
                                    });
                                    errorProducto = true;
                                }

                                if ((tamanho5 == null && valorTamanho5 != null) || (tamanho5 != null && valorTamanho5 == null))
                                {
                                    erros.Add(new ErrorDetail
                                    {
                                        Row = row,
                                        Code = codigoProducto.ToString().Trim(),
                                        Column = "Tamanho 5",
                                        Detail = "Se tiver um valor, deve ter um tamanho e vice-versa"
                                    });
                                    errorProducto = true;
                                }

                                if (!errorProducto)
                                {
                                    productos.Add(new P_Productos
                                    {
                                        codigo = codigoProducto.ToString().Trim(),
                                        nombre = nomeProducto.ToString().Trim(),
                                        Categoria = categoriaProducto.ToString().Trim(),
                                        valor = valor is null ? 0 : Convert.ToDecimal(valor.ToString().Trim().Replace(",", ".")),
                                        horasPreparacion = horas is null ? 0 : Convert.ToInt32(horas.ToString().Trim()),
                                        minutosPreparacion = minutos is null ? 0 : Convert.ToInt32(minutos.ToString().Trim()),
                                        descripcion = descripcion is null ? "" : descripcion.ToString().Trim(),
                                        tamanho1 = tamanho1?.ToString().Trim(),
                                        valorTamanho1 = valorTamanho1 is null ? 0 : Convert.ToDecimal(valorTamanho1.ToString().Trim().Replace(",", ".")),
                                        tamanho2 = tamanho2?.ToString().Trim(),
                                        valorTamanho2 = valorTamanho2 is null ? 0 : Convert.ToDecimal(valorTamanho2.ToString().Trim().Replace(",", ".")),
                                        tamanho3 = tamanho3?.ToString().Trim(),
                                        valorTamanho3 = valorTamanho3 is null ? 0 : Convert.ToDecimal(valorTamanho3.ToString().Trim().Replace(",", ".")),
                                        tamanho4 = tamanho4?.ToString().Trim(),
                                        valorTamanho4 = valorTamanho4 is null ? 0 : Convert.ToDecimal(valorTamanho4.ToString().Trim().Replace(",", ".")),
                                        tamanho5 = tamanho5?.ToString().Trim(),
                                        valorTamanho5 = valorTamanho5 is null ? 0 : Convert.ToDecimal(valorTamanho5.ToString().Trim().Replace(",", ".")),

                                    });
                                }

                            }
                            catch (Exception ex)
                            {
                                //throw ex;
                            }
                        }
                        row++;
                    }
                }
            }

            ViewBag.Categorias = categorias;
            ViewBag.Productos = productos;
            ViewBag.Erros = erros;
            ViewBag.rowErros = erros.Count();

            SetSession("Categorias", categorias);
            SetSession("Productos", productos);                       

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

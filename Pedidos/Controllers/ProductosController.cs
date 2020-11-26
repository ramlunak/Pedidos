using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pedidos.Data;
using Pedidos.Extensions;
using Pedidos.Models;

//CROPPER
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Pedidos.Controllers
{
    public class ProductosController : BaseController
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index(string nombre, int pagina = 1)
        {
            ValidarCuenta();

            var cantidadRegistrosPorPagina = 10; // parámetro

            var Skip = ((pagina - 1) * cantidadRegistrosPorPagina);
            var sql = SqlConsultas.GetSqlAllProductos(Cuenta.id, Skip, cantidadRegistrosPorPagina, nombre);

            var lista = await new DBHelper(_context).ProductosFromCmd(sql);

            var totalDeRegistros = 0;
            if (nombre is null)
            {
                totalDeRegistros = await _context.P_Productos.Where(x => x.idCuenta == Cuenta.id).CountAsync();
            }
            else
            {
                totalDeRegistros = await _context.P_Productos.Where(x => x.idCuenta == Cuenta.id && x.nombre.Contains(nombre)).CountAsync();
            }

            ViewBag.FlrNombre = nombre;

            var modelo = new ViewModels.VMProductos();
            modelo.Productos = lista;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new RouteValueDictionary();
            modelo.ValoresQueryString["pagina"] = pagina;
            modelo.ValoresQueryString["nombre"] = nombre;

            return View(modelo);

        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Productos = await _context.P_Productos
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Productos == null)
            {
                return NotFound();
            }

            return View(p_Productos);
        }

        // GET: Productos/Create
        public async Task<IActionResult> Create()
        {
            ValidarCuenta();
            SetSession("base64String", "");
            ViewBag.Categorias = await _context.P_Categorias.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            var model = new P_Productos() { ImageBase64 = ImageLoad };
            return View(model);
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Productos p_Productos)
        {
            ValidarCuenta();
            if (ModelState.IsValid)
            {
                //var files = HttpContext.Request.Form.Files;
                //foreach (var imagen in files)
                //{
                //    if (imagen != null && imagen.Length > 0)
                //    {
                //        if (!imagen.IsImage())
                //        {
                //            @ViewBag.Erro = "O arquivo selecionado não tem formato de imagem";
                //            ViewBag.Categorias = await _context.P_Categorias.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
                //            return View(p_Productos);
                //        }

                //        var imgBytes = await imagen.ToByteArray();
                //        var newImg = imgBytes.Resize(50, 50);
                //        p_Productos.imagen = await imagen.ToByteArray();
                //    }
                //}

                // p_Productos.valor = p_Productos.strValor.ToDecimal();
                p_Productos.idCuenta = Cuenta.id;
                p_Productos.ImageBase64 = GetSession("base64String");
                if (!string.IsNullOrEmpty(p_Productos.ImageBase64))
                {
                    p_Productos.imagen = Convert.FromBase64String(p_Productos.ImageBase64);
                }

                _context.Add(p_Productos);
                await _context.SaveChangesAsync();

                //Actualizar lista productos de la session
                var SSproductos = GetSession("Productos");
                if (SSproductos != null)
                {
                    var ListProductos = JsonConvert.DeserializeObject<List<P_Productos>>(SSproductos);
                    ListProductos.Add(p_Productos);
                    var json = JsonConvert.SerializeObject(ListProductos);
                    SetSession("Productos", json);
                }

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = await _context.P_Categorias.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            return View(p_Productos);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id, int? pagina)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Productos = await _context.P_Productos.FindAsync(id);
            if (p_Productos == null)
            {
                return NotFound();
            }

            // p_Productos.strValor = p_Productos.valor.ToString(CultureInfo.InvariantCulture);
            var ListaCaterorias = await _context.P_Categorias.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            ViewBag.Categorias = ListaCaterorias;
            ViewBag.Pagina = pagina;

            try
            {
                p_Productos.Categoria = ListaCaterorias.First(x => x.id == p_Productos.idCategoria).nombre;
            }
            catch
            {
            }

            return View(p_Productos);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Productos p_Productos)
        {
            ValidarCuenta();
            if (id != p_Productos.id)
            {
                return NotFound();
            }

            if (ModelState.ErrorCount == 1)
                ModelState.Clear();

            if (p_Productos.ImageBase64 != null)
                p_Productos.imagen = System.Convert.FromBase64String(p_Productos.ImageBase64);

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    foreach (var imagen in files)
                    {
                        if (!imagen.IsImage())
                        {
                            @ViewBag.Erro = "O arquivo selecionado não tem formato de imagem";
                            ViewBag.Categorias = await _context.P_Categorias.ToListAsync();
                            return View(p_Productos);
                        }

                        if (imagen != null && imagen.Length > 0)
                        {
                            var imgBytes = await imagen.ToByteArray();
                            var newImg = imgBytes.Resize(50, 50);
                            p_Productos.imagen = await imagen.ToByteArray();
                        }
                    }

                    // p_Productos.valor = p_Productos.strValor.ToDecimal();
                    _context.Update(p_Productos);
                    await _context.SaveChangesAsync();

                    //Actualizar lista productos de la session
                    var SSproductos = GetSession("Productos");
                    if (SSproductos != null)
                    {
                        var ListProductos = JsonConvert.DeserializeObject<List<P_Productos>>(SSproductos);
                        var oldProducto = ListProductos.Where(x => x.id == p_Productos.id).FirstOrDefault();
                        ListProductos.Remove(oldProducto);
                        ListProductos.Add(p_Productos);
                        var json = JsonConvert.SerializeObject(ListProductos);
                        SetSession("Productos", json);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_ProductosExists(p_Productos.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(p_Productos);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Productos = await _context.P_Productos
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Productos == null)
            {
                return NotFound();
            }

            return View(p_Productos);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ValidarCuenta();
            var p_Productos = await _context.P_Productos.FindAsync(id);
            _context.P_Productos.Remove(p_Productos);
            await _context.SaveChangesAsync();

            //Actualizar lista productos de la session
            var SSproductos = GetSession("Productos");
            if (SSproductos != null)
            {
                var ListProductos = JsonConvert.DeserializeObject<List<P_Productos>>(SSproductos);
                ListProductos.Remove(p_Productos);
                var json = JsonConvert.SerializeObject(ListProductos);
                SetSession("Productos", json);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool P_ProductosExists(int id)
        {
            ValidarCuenta();
            return _context.P_Productos.Any(e => e.id == id);
        }

        public async Task<IActionResult> GetProductos()
        {
            var data = await _context.P_Aux.FromSqlRaw(SqlConsultas.GetSqlCardapio(null, Cuenta.id)).ToListAsync();
            //var id = 1;
            //var query = from P in _context.P_Productos
            //            join C in _context.P_Categorias on P.idCategoria equals C.id
            //            where P.idCuenta == Cuenta.id
            //            group C by C.nombre into G
            //            select G;

       //     var item = await query.ToListAsync();


            ValidarCuenta();
            //  var items = await _context.P_Productos.Where(x=>x.idCuenta == Cuenta.id && x.activo).ToListAsync();
            var items = await _context.P_Productos.ToListAsync();
            return Ok(items);
        }

        [HttpPost]
        public IActionResult CustomCrop(string filename, IFormFile blob)
        {
            try
            {
                string base64String;
                using (var image = Image.Load(blob.OpenReadStream()))
                {
                    string systemFileExtenstion = filename.Substring(filename.LastIndexOf('.'));

                    //image.Mutate(x => x.Resize(180, 180));
                    //var newfileName180 = GenerateFileName("Photo_180_180_", systemFileExtenstion);
                    // var filepath160 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img")).Root + $@"\{newfileName180}";
                    //image.Save(newfileName180);

                    using (MemoryStream m = new MemoryStream())
                    {
                        image.SaveAsPng(m);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        base64String = Convert.ToBase64String(imageBytes);
                    }

                    //var newfileName200 = GenerateFileName("Photo_200_200_", systemFileExtenstion);
                    //var filepath200 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\{newfileName200}";
                    // image.Mutate(x => x.Resize(100, 100));
                    //image.Save(filepath200);

                    //var newfileName32 = GenerateFileName("Photo_32_32_", systemFileExtenstion);
                    //var filepath32 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\{newfileName32}";
                    //image.Mutate(x => x.Resize(32, 32));
                    //image.Save(filepath32);

                }

                SetSession("base64String", base64String);
                return Json(new { Message = base64String });

            }
            catch (Exception ex)
            {
                SetSession("base64String", "");
                return Json(new { Message = "ERROR" });
            }
        }

        public string GenerateFileName(string fileTypeName, string fileextenstion)
        {
            if (fileTypeName == null) throw new ArgumentNullException(nameof(fileTypeName));
            if (fileextenstion == null) throw new ArgumentNullException(nameof(fileextenstion));
            return $"{fileTypeName}_{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{fileextenstion}";
        }

    }
}

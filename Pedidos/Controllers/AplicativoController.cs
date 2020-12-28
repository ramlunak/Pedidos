using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pedidos.Data;
using Pedidos.Models;

namespace Pedidos.Controllers
{
    public class AplicativoController : BaseController
    {
        private readonly AppDbContext _context;

        public AplicativoController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string nombre, int pagina = 1)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var cantidadRegistrosPorPagina = 10; // parámetro

            var Skip = ((pagina - 1) * cantidadRegistrosPorPagina);
            var sql = SqlConsultas.GetSqlAllAplicativos(Cuenta.id, Skip, cantidadRegistrosPorPagina, nombre);

            var lista = await _context.P_Aplicativos.FromSqlRaw(sql).ToListAsync();

            var totalDeRegistros = 0;
            if (nombre is null)
            {
                totalDeRegistros = await _context.P_Aplicativos.Where(x => x.idCuenta == Cuenta.id).CountAsync();
            }
            else
            {
                totalDeRegistros = await _context.P_Aplicativos.Where(x => x.idCuenta == Cuenta.id && x.nombre.Contains(nombre)).CountAsync();
            }

            ViewBag.FlrNombre = nombre;

            var modelo = new ViewModels.VMAplicativos();
            modelo.Aplicativos = lista;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new RouteValueDictionary();
            modelo.ValoresQueryString["pagina"] = pagina;
            modelo.ValoresQueryString["nombre"] = nombre;

            return View(modelo);
        }

        public IActionResult Create()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Aplicativo p_Aplicativo)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (ModelState.IsValid)
            {
                p_Aplicativo.idCuenta = Cuenta.id;

                _context.Add(p_Aplicativo);
                await _context.SaveChangesAsync();

                //Actualizar lista aplicativos de la session
                var SSaplicativos = GetSession("Aplicativos");
                if (SSaplicativos != null)
                {
                    var ListAplicativos = JsonConvert.DeserializeObject<List<P_Aplicativo>>(SSaplicativos);
                    ListAplicativos.Add(p_Aplicativo);
                    var json = JsonConvert.SerializeObject(ListAplicativos);
                    SetSession("Aplicativos", json);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(p_Aplicativo);
        }

        public async Task<IActionResult> Edit(int? id, int? pagina)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var p_Aplicativo = await _context.P_Aplicativos.FindAsync(id);
            if (p_Aplicativo == null)
            {
                return NotFound();
            }

            ViewBag.Pagina = pagina;
            return View(p_Aplicativo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Aplicativo p_Aplicativo, int? pagina)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            if (id != p_Aplicativo.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Aplicativo);
                    await _context.SaveChangesAsync();

                    //Actualizar lista aplicativos de la session
                    var SSaplicativos = GetSession("Aplicativos");
                    if (SSaplicativos != null)
                    {
                        var ListAplicativos = JsonConvert.DeserializeObject<List<P_Aplicativo>>(SSaplicativos);
                        var oldAplicativo = ListAplicativos.Where(x => x.id == p_Aplicativo.id).FirstOrDefault();
                        ListAplicativos.Remove(oldAplicativo);
                        ListAplicativos.Add(p_Aplicativo);
                        var json = JsonConvert.SerializeObject(ListAplicativos);
                        SetSession("Aplicativos", json);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_AplicativoExists(p_Aplicativo.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { pagina });
            }
            return View(p_Aplicativo);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var p_Aplicativo = await _context.P_Aplicativos
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Aplicativo == null)
            {
                return NotFound();
            }

            return View(p_Aplicativo);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var p_Aplicativo = await _context.P_Aplicativos.FindAsync(id);
            _context.P_Aplicativos.Remove(p_Aplicativo);
            await _context.SaveChangesAsync();

            //Actualizar lista aplicativos de la session
            var SSaplicativos = GetSession("Aplicativos");
            if (SSaplicativos != null)
            {
                var ListAplicativos = JsonConvert.DeserializeObject<List<P_Aplicativo>>(SSaplicativos);
                var oldAplicativo = ListAplicativos.Where(x => x.id == p_Aplicativo.id).FirstOrDefault();
                ListAplicativos.Remove(oldAplicativo);
                ListAplicativos.Add(p_Aplicativo);
                var json = JsonConvert.SerializeObject(ListAplicativos);
                SetSession("Aplicativos", json);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool P_AplicativoExists(int id)
        {
            ValidarCuenta();
            return _context.P_Aplicativos.Any(e => e.id == id);
        }
    }
}

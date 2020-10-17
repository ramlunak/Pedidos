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
    public class ClientesController : BaseController
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(string nombre, int pagina = 1)
        {
            ValidarCuenta();
            var cantidadRegistrosPorPagina = 10; // parámetro

            var Skip = ((pagina - 1) * cantidadRegistrosPorPagina);
            var sql = SqlConsultas.GetSqlAllClientes(Cuenta.id, Skip, cantidadRegistrosPorPagina, nombre);

            var lista = await _context.P_Clientes.FromSqlRaw(sql).ToListAsync();

            var totalDeRegistros = 0;
            if (nombre is null)
            {
                totalDeRegistros = await _context.P_Clientes.Where(x => x.idCuenta == Cuenta.id).CountAsync();
            }
            else
            {
                totalDeRegistros = await _context.P_Clientes.Where(x => x.idCuenta == Cuenta.id && x.nombre.Contains(nombre)).CountAsync();
            }

            ViewBag.FlrNombre = nombre;

            var modelo = new ViewModels.VMClientes();
            modelo.Clientes = lista;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new RouteValueDictionary();
            modelo.ValoresQueryString["pagina"] = pagina;
            modelo.ValoresQueryString["nombre"] = nombre;

            return View(modelo);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Cliente = await _context.P_Clientes
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Cliente == null)
            {
                return NotFound();
            }

            return View(p_Cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ValidarCuenta();
            return View(new P_Cliente());
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Cliente p_Cliente)
        {
            ValidarCuenta();
            if (ModelState.IsValid)
            {
                p_Cliente.idCuenta = Cuenta.id;
                _context.Add(p_Cliente);
                await _context.SaveChangesAsync();

                //Actualizar lista cliente de la session
                var SSclientes = GetSession("Clientes");
                if (SSclientes != null)
                {
                    var ListClientes = JsonConvert.DeserializeObject<List<P_Cliente>>(SSclientes);
                    ListClientes.Add(p_Cliente);
                    var json = JsonConvert.SerializeObject(ListClientes);
                    SetSession("Clientes", json);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(p_Cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Cliente = await _context.P_Clientes.FindAsync(id);
            if (p_Cliente == null)
            {
                return NotFound();
            }
            return View(p_Cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,telefono,nombre,idCuenta,activo")] P_Cliente p_Cliente)
        {
            ValidarCuenta();
            if (id != p_Cliente.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Cliente);
                    await _context.SaveChangesAsync();

                    //Actualizar lista cliente de la session
                    var SSclientes = GetSession("Clientes");
                    if (SSclientes != null)
                    {
                        var ListClientes = JsonConvert.DeserializeObject<List<P_Cliente>>(SSclientes);
                        var oldCliente = ListClientes.Where(x => x.id == p_Cliente.id).FirstOrDefault();
                        ListClientes.Remove(oldCliente);
                        ListClientes.Add(p_Cliente);
                        var json = JsonConvert.SerializeObject(ListClientes);
                        SetSession("Clientes", json);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_ClienteExists(p_Cliente.id))
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
            return View(p_Cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ValidarCuenta();
            if (id == null)
            {
                return NotFound();
            }

            var p_Cliente = await _context.P_Clientes
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Cliente == null)
            {
                return NotFound();
            }

            return View(p_Cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ValidarCuenta();
            var p_Cliente = await _context.P_Clientes.FindAsync(id);
            _context.P_Clientes.Remove(p_Cliente);
            await _context.SaveChangesAsync();

            //Actualizar lista cliente de la session
            var SSclientes = GetSession("Clientes");
            if (SSclientes != null)
            {
                var ListClientes = JsonConvert.DeserializeObject<List<P_Cliente>>(SSclientes);
                ListClientes.Remove(p_Cliente);
                var json = JsonConvert.SerializeObject(ListClientes);
                SetSession("Clientes", json);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool P_ClienteExists(int id)
        {
            ValidarCuenta();
            return _context.P_Clientes.Any(e => e.id == id);
        }


        public async Task<IActionResult> GetTelefono(int idCliente)
        {
            string telefono = null;
            var SSclientes = GetSession("Clientes");
            if (SSclientes != null)
            {
                var ListClientes = JsonConvert.DeserializeObject<List<P_Cliente>>(SSclientes);
                var cliente = ListClientes.Where(x => x.id == idCliente).FirstOrDefault();
                if (cliente != null)
                {
                    telefono = cliente.telefono;
                }
            }

            return Json(telefono);
        }

        public async Task<IActionResult> GetDireccion(int idCliente)
        {
            var direciones = await _context.P_Direcciones.Where(x => x.idCliente == idCliente).ToArrayAsync();
            return Json(direciones.ToArray());
        }

    }
}

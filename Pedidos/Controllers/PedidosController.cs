using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;
using Newtonsoft;
using Newtonsoft.Json;

namespace Pedidos.Controllers
{
    public class PedidosController : BaseController
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            ValidarCuenta();
            return View(await _context.P_Pedidos.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ValidarCuenta();

            if (id == null)
            {
                return NotFound();
            }

            var p_Pedido = await _context.P_Pedidos
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Pedido == null)
            {
                return NotFound();
            }

            return View(p_Pedido);
        }

        // GET: Pedidos/Create
        public async Task<IActionResult> Create()
        {
            ValidarCuenta();


            if (GetSession("Productos") == null)
            {
                var productos = ViewBag.Productos = await _context.P_Productos.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
                var json = JsonConvert.SerializeObject(productos);
                SetSession("Productos", json);
            }
            else
            {
                var SSproductos = GetSession("Productos");
                ViewBag.Productos = JsonConvert.DeserializeObject(SSproductos);
            }

            if (GetSession("Clientes") == null)
            {
                var clientes = ViewBag.Clientes = await _context.P_Clientes.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
                var json = JsonConvert.SerializeObject(clientes);
                SetSession("Clientes", json);
            }
            else
            {
                var SSclientes = GetSession("Clientes");
                ViewBag.Clientes = JsonConvert.DeserializeObject<List<P_Cliente>>(SSclientes);
            }

            ViewBag.State = Cuenta.estado;
            ViewBag.City = Cuenta.municipio;

            var ProductosPedido = new List<P_Productos>();
            if (GetSession("ProductosPedido") != null)
            {
                ProductosPedido = GetSession<List<P_Productos>>("ProductosPedido");
            }
            var newPedido = new P_Pedido();
            newPedido.Productos = ProductosPedido;

            return View(newPedido);
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Pedido p_Pedido)
        {
            ValidarCuenta();

            //CARGAR PRODUCTOS SESSION
            var productosDB = new List<P_Productos>();
            if (GetSession("Productos") == null)
            {
                var productos = ViewBag.Productos = await _context.P_Productos.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
                productosDB = productos;
                var jsonProductos = JsonConvert.SerializeObject(productos);
                SetSession("Productos", jsonProductos);
            }
            else
            {
                var SSproductos = GetSession("Productos");
                productosDB = JsonConvert.DeserializeObject<List<P_Productos>>(SSproductos);
            }
            ViewBag.Productos = productosDB;
            //-------------------------------------------

            var ProductosPedido = new List<P_Productos>();

            if (GetSession("ProductosPedido") != null)
            {
                ProductosPedido = JsonConvert.DeserializeObject<List<P_Productos>>(GetSession("ProductosPedido"));
            }

            if (p_Pedido.IdProducto != null)
            {
                var p = productosDB.Where(x => x.id == p_Pedido.IdProducto).FirstOrDefault();
                p.Observacion = p_Pedido.Observacion;
                p.DataPedido = DateTime.Now;

                for (int i = 0; i < p_Pedido.Cantidad; i++)
                {
                    ProductosPedido.Add(p);
                }

                //ACTUALIZAR INDEX EN LA LISTA
                var index = 0;
                foreach (var item in ProductosPedido)
                {
                    item.Index = index;
                    index++;
                }

                var json = JsonConvert.SerializeObject(ProductosPedido);
                SetSession("ProductosPedido", json);

                p_Pedido.Producto = null;
                p_Pedido.IdProducto = null;
                p_Pedido.Cantidad = 1;
                p_Pedido.Observacion = null;               
            }
            else
            {
                ViewBag.Erro = "Seleccione um produto da lista";
            }

            p_Pedido.Productos = ProductosPedido;

            return View(p_Pedido);
        }


        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ValidarCuenta();

            if (id == null)
            {
                return NotFound();
            }

            var p_Pedido = await _context.P_Pedidos.FindAsync(id);
            if (p_Pedido == null)
            {
                return NotFound();
            }
            return View(p_Pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Pedido p_Pedido)
        {
            ValidarCuenta();

            if (id != p_Pedido.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_PedidoExists(p_Pedido.id))
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
            return View(p_Pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> DeleteProducto(int? index)
        {
            ValidarCuenta();

            var ProductosPedido = new List<P_Productos>();
            if (GetSession("ProductosPedido") != null)
            {
                ProductosPedido = GetSession<List<P_Productos>>("ProductosPedido");
            }

            var p = ProductosPedido.Where(x => x.Index == index).FirstOrDefault();
            ProductosPedido.Remove(p);
            SetSession("ProductosPedido", ProductosPedido);

            return RedirectToAction(nameof(Create));
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ValidarCuenta();

            var p_Pedido = await _context.P_Pedidos.FindAsync(id);
            _context.P_Pedidos.Remove(p_Pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_PedidoExists(int id)
        {
            ValidarCuenta();

            return _context.P_Pedidos.Any(e => e.id == id);
        }
    }
}

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

        public async Task<IActionResult> Index()
        {
            ValidarCuenta();
            return View(await _context.P_Pedidos.ToListAsync());
        }

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

            if (GetSession("Aplicativos") == null)
            {
                var aplicativos = ViewBag.Aplicativos = await _context.P_Aplicativos.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
                var json = JsonConvert.SerializeObject(aplicativos);
                SetSession("Aplicativos", json);
            }
            else
            {
                var SSaplicativos = GetSession("Aplicativos");
                ViewBag.Aplicativos = JsonConvert.DeserializeObject<List<P_Aplicativo>>(SSaplicativos);
            }

            var jsonPedidoDTO = GetSession("PedidoDTO");
            var pedidoDTO = new PedidoDTO();

            if (jsonPedidoDTO != null)
            {
                pedidoDTO = JsonConvert.DeserializeObject<PedidoDTO>(jsonPedidoDTO);
            }

            ViewBag.State = string.IsNullOrEmpty(pedidoDTO.state) ? Cuenta.estado : pedidoDTO.state;
            ViewBag.City = string.IsNullOrEmpty(pedidoDTO.city) ? Cuenta.municipio : pedidoDTO.city;

            ViewBag.Aplicativo = pedidoDTO.aplicativo;
            ViewBag.Mesa = pedidoDTO.idMesa;
            ViewBag.Cliente = pedidoDTO.cliente;
            ViewBag.Direccion = $"{pedidoDTO.address} {pedidoDTO.numero}, {pedidoDTO.district} {pedidoDTO.city}, {pedidoDTO.state}";
            ViewBag.ShowDireccion = string.IsNullOrEmpty(pedidoDTO.address) ? false : true;

            var ProductosPedido = new List<P_Productos>();
            if (GetSession("ProductosPedido") != null)
            {
                ProductosPedido = GetSession<List<P_Productos>>("ProductosPedido");
            }

            var newPedido = new P_Pedido();
            newPedido.Productos = ProductosPedido;
            newPedido.PedidoDTO = pedidoDTO;

            return View(newPedido);
        }

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

            var jsonPedidoDTO = GetSession("PedidoDTO");
            var pedidoDTO = new PedidoDTO();

            if (jsonPedidoDTO != null)
            {
                pedidoDTO = JsonConvert.DeserializeObject<PedidoDTO>(jsonPedidoDTO);
            }

            ViewBag.State = string.IsNullOrEmpty(pedidoDTO.state) ? Cuenta.estado : pedidoDTO.state;
            ViewBag.City = string.IsNullOrEmpty(pedidoDTO.city) ? Cuenta.municipio : pedidoDTO.city;

            ViewBag.Aplicativo = pedidoDTO.aplicativo;
            ViewBag.Mesa = pedidoDTO.idMesa;
            ViewBag.Cliente = pedidoDTO.cliente;
            ViewBag.Direccion = $"{pedidoDTO.address} {pedidoDTO.numero}, {pedidoDTO.district} {pedidoDTO.city}, {pedidoDTO.state}";
            ViewBag.ShowDireccion = string.IsNullOrEmpty(pedidoDTO.address) ? false : true;

            p_Pedido.Productos = ProductosPedido;
            p_Pedido.PedidoDTO = pedidoDTO;

            return View(p_Pedido);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormClienteDireccion(PedidoDTO pedidoDTO)
        {
            ValidarCuenta();

            var jsonPedidoDTO = JsonConvert.SerializeObject(pedidoDTO);
            SetSession("PedidoDTO", jsonPedidoDTO);

            return RedirectToAction(nameof(Create));

        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> GuardarPedido()
        {
            var p_Pedido = new P_Pedido();

            var jsonPedidoDTO = GetSession("PedidoDTO");
            var pedidoDTO = new PedidoDTO();
            if (jsonPedidoDTO != null)
            {
                pedidoDTO = JsonConvert.DeserializeObject<PedidoDTO>(jsonPedidoDTO);

                #region CLIENTE
                if (pedidoDTO.IdCliente.HasValue)
                {
                    p_Pedido.idCliente = pedidoDTO.IdCliente;

                    //ACTUALIZAR DATOS CLIENTE
                    var Ncliente = new P_Cliente();
                    Ncliente.id = pedidoDTO.IdCliente.Value;
                    Ncliente.nombre = pedidoDTO.cliente;
                    Ncliente.telefono = pedidoDTO.telefono;
                    _context.Update(Ncliente);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (!string.IsNullOrEmpty(pedidoDTO.cliente))
                    {
                        var Ncliente = new P_Cliente();
                        Ncliente.nombre = pedidoDTO.cliente;
                        Ncliente.telefono = pedidoDTO.telefono;
                        Ncliente.idCuenta = Cuenta.id;
                        Ncliente.activo = true;

                        _context.Add(Ncliente);
                        await _context.SaveChangesAsync();

                        p_Pedido.idCliente = Ncliente.id;
                    }

                }
                #endregion



            }


            return RedirectToAction(nameof(Index));
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

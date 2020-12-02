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
using Pedidos.Models.Enums;

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
            return View();
        }

        [HttpPost]
        public IActionResult AddProducto([FromBody] P_Productos producto)
        {
            ValidarCuenta();
            var currentPedido = GetSession<P_Pedido>("currentPedido");
            if (currentPedido is null)
            {
                currentPedido = new P_Pedido();
            }
            currentPedido.productos.Add(producto);
            SetSession("currentPedido", currentPedido);
            return Ok(new { currentPedido });
        }

      
        //public async Task<IActionResult> Create()
        //{
        //    ValidarCuenta();

        //    if (GetSession<List<P_Productos>>("Productos") == null || !GetSession<List<P_Productos>>("Productos").Any())
        //    {
        //        var productos = await _context.P_Productos.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
        //        ViewBag.Productos = productos;
        //        SetSession("Productos", productos);
        //    }
        //    else
        //    {
        //        var productos = GetSession<List<P_Productos>>("Productos");
        //        ViewBag.Productos = productos;
        //    }

        //    if (GetSession<List<P_Cliente>>("Clientes") == null || !GetSession<List<P_Cliente>>("Clientes").Any())
        //    {
        //        var clientes = ViewBag.Clientes = await _context.P_Clientes.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
        //        SetSession("Clientes", clientes);
        //    }
        //    else
        //    {
        //        ViewBag.Clientes = GetSession<List<P_Cliente>>("Clientes");
        //    }

        //    if (GetSession<List<P_Aplicativo>>("Aplicativos") == null || !GetSession<List<P_Aplicativo>>("Aplicativos").Any())
        //    {
        //        var aplicativos = ViewBag.Aplicativos = await _context.P_Aplicativos.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
        //        SetSession("Aplicativos", aplicativos);
        //    }
        //    else
        //    {
        //        ViewBag.Aplicativos = GetSession<List<P_Aplicativo>>("Aplicativos");
        //    }

        //    var jsonPedidoDTO = GetSession("PedidoDTO");
        //    var pedidoDTO = new PedidoDTO();

        //    if (jsonPedidoDTO != null)
        //    {
        //        pedidoDTO = JsonConvert.DeserializeObject<PedidoDTO>(jsonPedidoDTO);
        //    }

        //    ViewBag.State = string.IsNullOrEmpty(pedidoDTO.state) ? Cuenta.estado : pedidoDTO.state;
        //    ViewBag.City = string.IsNullOrEmpty(pedidoDTO.city) ? Cuenta.municipio : pedidoDTO.city;

        //    ViewBag.Aplicativo = pedidoDTO.aplicativo;
        //    ViewBag.Mesa = pedidoDTO.idMesa;
        //    ViewBag.Cliente = pedidoDTO.cliente;
        //    ViewBag.Direccion = $"{pedidoDTO.address} {pedidoDTO.numero}, {pedidoDTO.district} {pedidoDTO.city}, {pedidoDTO.state}";
        //    ViewBag.ShowDireccion = string.IsNullOrEmpty(pedidoDTO.address) ? false : true;

        //    var ProductosPedido = new List<P_Productos>();
        //    if (GetSession("ProductosPedido") != null)
        //    {
        //        ProductosPedido = GetSession<List<P_Productos>>("ProductosPedido");
        //    }

        //    var newPedido = new P_Pedido();
        //    newPedido.Productos = ProductosPedido;
        //    newPedido.PedidoDTO = pedidoDTO;

        //    return View(newPedido);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(P_Pedido p_Pedido)
        //{
        //    ValidarCuenta();

        //    //CARGAR SESSION
        //    var productosDB = new List<P_Productos>();

        //    if (GetSession<List<P_Productos>>("Productos") == null || !GetSession<List<P_Productos>>("Productos").Any())
        //    {
        //        var productos = await _context.P_Productos.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
        //        ViewBag.Productos = productos;
        //        SetSession("Productos", productos);
        //        productosDB = productos;
        //    }
        //    else
        //    {
        //        var productos = GetSession<List<P_Productos>>("Productos");
        //        ViewBag.Productos = productos;
        //        productosDB = productos;
        //    }

        //    if (GetSession<List<P_Cliente>>("Clientes") == null || !GetSession<List<P_Cliente>>("Clientes").Any())
        //    {
        //        var clientes = ViewBag.Clientes = await _context.P_Clientes.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
        //        SetSession("Clientes", clientes);
        //    }
        //    else
        //    {
        //        ViewBag.Clientes = GetSession<List<P_Cliente>>("Clientes");
        //    }

        //    if (GetSession<List<P_Aplicativo>>("Aplicativos") == null || !GetSession<List<P_Aplicativo>>("Aplicativos").Any())
        //    {
        //        var aplicativos = ViewBag.Aplicativos = await _context.P_Aplicativos.Where(x => x.idCuenta == Cuenta.id && x.activo).ToListAsync();
        //        SetSession("Aplicativos", aplicativos);
        //    }
        //    else
        //    {
        //        ViewBag.Aplicativos = GetSession<List<P_Aplicativo>>("Aplicativos");
        //    }

        //    //-------------------------------------------

        //    var ProductosPedido = new List<P_Productos>();

        //    if (GetSession("ProductosPedido") != null)
        //    {
        //        ProductosPedido = JsonConvert.DeserializeObject<List<P_Productos>>(GetSession("ProductosPedido"));
        //    }

        //    if (p_Pedido.IdProducto != null)
        //    {
        //        var p = productosDB.Where(x => x.id == p_Pedido.IdProducto).FirstOrDefault();
        //        p.Observacion = p_Pedido.Observacion;
        //        p.DataPedido = DateTime.Now;

        //        for (int i = 0; i < p_Pedido.Cantidad; i++)
        //        {
        //            ProductosPedido.Add(p);
        //        }

        //        //ACTUALIZAR INDEX EN LA LISTA
        //        var index = 0;
        //        foreach (var item in ProductosPedido)
        //        {
        //            item.Index = index;
        //            index++;
        //        }

        //        var json = JsonConvert.SerializeObject(ProductosPedido);
        //        SetSession("ProductosPedido", json);

        //        p_Pedido.Producto = null;
        //        p_Pedido.IdProducto = null;
        //        p_Pedido.Cantidad = 1;
        //        p_Pedido.Observacion = null;
        //    }
        //    else
        //    {
        //        ViewBag.Erro = "Seleccione um produto da lista";
        //    }

        //    var jsonPedidoDTO = GetSession("PedidoDTO");
        //    var pedidoDTO = new PedidoDTO();

        //    if (jsonPedidoDTO != null)
        //    {
        //        pedidoDTO = JsonConvert.DeserializeObject<PedidoDTO>(jsonPedidoDTO);
        //    }

        //    ViewBag.State = string.IsNullOrEmpty(pedidoDTO.state) ? Cuenta.estado : pedidoDTO.state;
        //    ViewBag.City = string.IsNullOrEmpty(pedidoDTO.city) ? Cuenta.municipio : pedidoDTO.city;

        //    ViewBag.Aplicativo = pedidoDTO.aplicativo;
        //    ViewBag.Mesa = pedidoDTO.idMesa;
        //    ViewBag.Cliente = pedidoDTO.cliente;
        //    ViewBag.Direccion = $"{pedidoDTO.address} {pedidoDTO.numero}, {pedidoDTO.district} {pedidoDTO.city}, {pedidoDTO.state}";
        //    ViewBag.ShowDireccion = string.IsNullOrEmpty(pedidoDTO.address) ? false : true;

        //    p_Pedido.Productos = ProductosPedido;
        //    p_Pedido.PedidoDTO = pedidoDTO;

        //    return View(p_Pedido);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> FormClienteDireccion(PedidoDTO pedidoDTO)
        //{
        //    ValidarCuenta();

        //    var jsonPedidoDTO = JsonConvert.SerializeObject(pedidoDTO);
        //    SetSession("PedidoDTO", jsonPedidoDTO);

        //    return RedirectToAction(nameof(Create));

        //}

        //// GET: Pedidos/Edit/5
        //public async Task<IActionResult> GuardarPedido()
        //{
        //    var valid = true;
        //    var p_Pedido = new P_Pedido();

        //    var jsonPedidoDTO = GetSession("PedidoDTO");
        //    var pedidoDTO = new PedidoDTO();
        //    if (jsonPedidoDTO != null)
        //    {
        //        pedidoDTO = JsonConvert.DeserializeObject<PedidoDTO>(jsonPedidoDTO);

        //        #region CLIENTE
        //        if (pedidoDTO.IdCliente.HasValue)
        //        {
        //            p_Pedido.idCliente = pedidoDTO.IdCliente;

        //            //ACTUALIZAR TELEFONO CLIENTE
        //            var ssClientes = GetSession<List<P_Cliente>>("Clientes");
        //            if (ssClientes.Any(x => x.id == pedidoDTO.IdCliente.Value && string.IsNullOrEmpty(x.telefono)))
        //            {
        //                var Ncliente = new P_Cliente();
        //                Ncliente.id = pedidoDTO.IdCliente.Value;
        //                Ncliente.nombre = pedidoDTO.cliente;
        //                Ncliente.telefono = pedidoDTO.telefono;
        //                _context.Update(Ncliente);
        //                await _context.SaveChangesAsync();

        //                UpdateSessionCliente(SessionTransaction.Edit, Ncliente);
        //            }
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(pedidoDTO.cliente))
        //            {
        //                var Ncliente = new P_Cliente();
        //                Ncliente.nombre = pedidoDTO.cliente;
        //                Ncliente.telefono = pedidoDTO.telefono;
        //                Ncliente.idCuenta = Cuenta.id;
        //                Ncliente.activo = true;

        //                _context.Add(Ncliente);
        //                await _context.SaveChangesAsync();

        //                UpdateSessionCliente(SessionTransaction.Add, Ncliente);

        //                p_Pedido.idCliente = Ncliente.id;
        //            }

        //        }
        //        #endregion

        //        #region APLICATIVO
        //        if (pedidoDTO.IdAplicativo.HasValue)
        //        {
        //            p_Pedido.idAplicativo = pedidoDTO.IdAplicativo;
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(pedidoDTO.aplicativo))
        //            {
        //                var Naplicativo = new P_Aplicativo();
        //                Naplicativo.nombre = pedidoDTO.aplicativo;
        //                Naplicativo.idCuenta = Cuenta.id;
        //                Naplicativo.activo = true;

        //                _context.Add(Naplicativo);
        //                await _context.SaveChangesAsync();

        //                UpdateSessionAplicativos(SessionTransaction.Add, Naplicativo);

        //                p_Pedido.idCliente = Naplicativo.id;
        //            }

        //        }
        //        #endregion

        //        #region DIRECCION
        //        if (pedidoDTO.idDireccion.HasValue)
        //        {
        //            p_Pedido.idDireccion = pedidoDTO.idDireccion;
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(pedidoDTO.address) && pedidoDTO.IdCliente.HasValue)
        //            {
        //                var Ndireccion = new P_Direcciones();
        //                Ndireccion.code = pedidoDTO.code;
        //                Ndireccion.address = pedidoDTO.address;
        //                Ndireccion.numero = pedidoDTO.numero;
        //                Ndireccion.complemento = pedidoDTO.complemento;
        //                Ndireccion.district = pedidoDTO.district;
        //                Ndireccion.city = pedidoDTO.city;
        //                Ndireccion.state = pedidoDTO.state;                      

        //                Ndireccion.idCliente = pedidoDTO.IdCliente;
        //                Ndireccion.idCuenta = Cuenta.id;
        //                Ndireccion.activo = true;

        //                _context.Add(Ndireccion);
        //                await _context.SaveChangesAsync();

        //                p_Pedido.idDireccion = Ndireccion.id;
        //            }

        //        }
        //        #endregion

        //    }

        //    if (valid)
        //    {
        //        //INGRESAR PEDIDO
        //        p_Pedido.status = StatusPedido.Pendiente.ToString();
        //        p_Pedido.fecha = DateTime.Now;
        //        p_Pedido.idCliente = Cuenta.id;

        //        _context.Add(p_Pedido);
        //        await _context.SaveChangesAsync();

        //        //INGRESAR LISTA PRODUCTOS PEDIDOS
        //        var ProductosPedido = new List<P_Productos>();
        //        if (GetSession("ProductosPedido") != null)
        //        {
        //            ProductosPedido = JsonConvert.DeserializeObject<List<P_Productos>>(GetSession("ProductosPedido"));
        //        }

        //        if (ProductosPedido.Any())
        //        {
        //            foreach (var producto in ProductosPedido)
        //            {
        //                var p_PedidoProductos = new P_PedidoProductos();
        //                p_PedidoProductos.idPedido = p_Pedido.id;
        //                p_PedidoProductos.idProducto = producto.id;
        //                p_PedidoProductos.valorProducto = producto.valor;

        //                _context.Add(p_PedidoProductos);
        //                await _context.SaveChangesAsync();
        //            }

        //        }
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        //// GET: Pedidos/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    ValidarCuenta();

        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var p_Pedido = await _context.P_Pedidos.FindAsync(id);
        //    if (p_Pedido == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(p_Pedido);
        //}

        //// POST: Pedidos/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, P_Pedido p_Pedido)
        //{
        //    ValidarCuenta();

        //    if (id != p_Pedido.id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(p_Pedido);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!P_PedidoExists(p_Pedido.id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(p_Pedido);
        //}

        //// GET: Pedidos/Delete/5
        //public async Task<IActionResult> DeleteProducto(int? index)
        //{
        //    ValidarCuenta();

        //    var ProductosPedido = new List<P_Productos>();
        //    if (GetSession("ProductosPedido") != null)
        //    {
        //        ProductosPedido = GetSession<List<P_Productos>>("ProductosPedido");
        //    }

        //    var p = ProductosPedido.Where(x => x.Index == index).FirstOrDefault();
        //    ProductosPedido.Remove(p);
        //    SetSession("ProductosPedido", ProductosPedido);

        //    return RedirectToAction(nameof(Create));
        //}

        //// POST: Pedidos/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    ValidarCuenta();

        //    var p_Pedido = await _context.P_Pedidos.FindAsync(id);
        //    _context.P_Pedidos.Remove(p_Pedido);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool P_PedidoExists(int id)
        {
            ValidarCuenta();

            return _context.P_Pedidos.Any(e => e.id == id);
        }
    }
}

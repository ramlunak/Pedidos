using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Components
{
    public class BuscarProductoComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public BuscarProductoComponent(AppDbContext context)
        {
            _context = context;
        }

        async Task<IViewComponentResult> Invoke()
        {
            //var result = await _context.P_Productos.ToListAsync();
            return View(new List<P_Productos>());
        }


    }
}

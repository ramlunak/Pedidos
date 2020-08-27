using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pedidos.Models;

namespace Pedidos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Pedidos.Models.P_Cliente> P_Clientes { get; set; }

        public DbSet<Pedidos.Models.P_Productos> P_Productos { get; set; }

        public DbSet<Pedidos.Models.P_Categoria> P_Categorias { get; set; }

        public DbSet<Pedidos.Models.P_Cuenta> P_Cuentas { get; set; }
    }
}

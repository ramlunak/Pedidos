using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pedidos.Models;

namespace Pedidos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Pedidos.Models.P_Cliente> P_Clientes { get; set; }

        public DbSet<Pedidos.Models.P_Productos> P_Productos { get; set; }

        public DbSet<Pedidos.Models.P_Categoria> P_Categorias { get; set; }

        public DbSet<Pedidos.Models.P_Cuenta> P_Cuentas { get; set; }

        public DbSet<Pedidos.Models.P_Cardapio> P_Cardapios { get; set; }

        public DbSet<Pedidos.Models.P_SubCategoria> P_SubCategorias { get; set; }

        public DbSet<Pedidos.Models.P_Direcciones> P_Direcciones { get; set; }

        public DbSet<Pedidos.Models.P_Pedido> P_Pedidos { get; set; }

        public DbSet<Pedidos.Models.P_Venta> P_Ventas { get; set; }

        public DbSet<Pedidos.Models.P_Aplicativo> P_Aplicativos { get; set; }

        public DbSet<Pedidos.Models.P_PedidoProductos> P_PedidoProductos { get; set; }

        public DbSet<Pedidos.Models.P_Ingredientes> P_Ingredientes { get; set; }

        public DbSet<Pedidos.Models.P_Adicionais> P_Adicionais { get; set; }

        public DbSet<Pedidos.Models.P_Log> P_Logs { get; set; }

        public DbSet<Pedidos.Models.P_AdicionalCategorias> P_AdicionalCategorias { get; set; }

        public DbSet<Pedidos.Models.P_CategoriaAdicional> P_CategoriaAdicional { get; set; }

        public DbSet<Pedidos.Models.P_Aux> P_Aux { get; set; }

        public DbSet<Pedidos.Models.P_FormaPagamento> P_FormaPagamento { get; set; }

        public DbSet<Pedidos.Models.P_Caja> P_Caja { get; set; }
    }
}

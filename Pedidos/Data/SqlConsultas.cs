
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedidos.Data
{
    public static class SqlConsultas
    {
        public static string GetSqlAllCategorias(int idCuenta, int Skip, int Take, string nombre)
        {
            StringBuilder filtro = new StringBuilder();
            if (nombre != null)
            {               
                filtro.Append($" AND nombre LIKE '%{nombre}%'");
            }
                       
            return $"DECLARE @Skip INT = {Skip}, @Take INT = {Take}" +
                   $" SELECT* FROM[dbo].[P_Categorias]" +
                   $" WHERE idCuenta = {idCuenta}" +                   
                   filtro +
                   $" ORDER BY id ASC" +
                   $" OFFSET(@Skip) ROWS FETCH NEXT(@Take) ROWS ONLY";
        }

        public static string GetSqlAllAplicativos(int idCuenta, int Skip, int Take, string nombre)
        {
            StringBuilder filtro = new StringBuilder();
            if (nombre != null)
            {
                filtro.Append($" AND nombre LIKE '%{nombre}%'");
            }

            return $"DECLARE @Skip INT = {Skip}, @Take INT = {Take}" +
                   $" SELECT* FROM[dbo].[P_Aplicativos]" +
                   $" WHERE idCuenta = {idCuenta}" +
                   filtro +
                   $" ORDER BY id ASC" +
                   $" OFFSET(@Skip) ROWS FETCH NEXT(@Take) ROWS ONLY";
        }

        public static string GetSqlAllSubCategorias(int idCuenta, int idCategoria, int Skip, int Take, string nombre)
        {
            StringBuilder filtro = new StringBuilder();
            if (nombre != null)
            {
                filtro.Append($" AND nombre LIKE '%{nombre}%'");
            }

            return $"DECLARE @Skip INT = {Skip}, @Take INT = {Take}" +
                   $" SELECT* FROM[dbo].[P_SubCategorias]" +
                   $" WHERE idCuenta = {idCuenta}" +
                   $" AND idCategoria = {idCategoria}" +
                   filtro +
                   $" ORDER BY id ASC" +
                   $" OFFSET(@Skip) ROWS FETCH NEXT(@Take) ROWS ONLY";
        }

        public static string GetSqlAllDirecciones(int idCuenta, int idcliente, int Skip, int Take, string nombre)
        {
            StringBuilder filtro = new StringBuilder();
            if (nombre != null)
            {
                filtro.Append($" AND nombre LIKE '%{nombre}%'");
            }

            return $"DECLARE @Skip INT = {Skip}, @Take INT = {Take}" +
                   $" SELECT* FROM[dbo].[P_Direcciones]" +
                   $" WHERE idCuenta = {idCuenta}" +
                   $" AND idcliente = {idcliente}" +
                   filtro +
                   $" ORDER BY id ASC" +
                   $" OFFSET(@Skip) ROWS FETCH NEXT(@Take) ROWS ONLY";
        }
        
        public static string GetSqlAllProductos(int idCuenta, int Skip, int Take, string nombre)
        {
            StringBuilder filtro = new StringBuilder();
            if (nombre != null)
            {
                filtro.Append($" AND nombre LIKE '%{nombre}%'");
            }

            return $"DECLARE @Skip INT = {Skip}, @Take INT = {Take}" +
                   $" SELECT [id]" +
                   $" ,[nombre]" +
                   $" ,[codigo]" +
                   $" ,[descripcion]" +
                   $" ,[idCategoria]" +
                   $" ,(SELECT TOP(1) nombre FROM P_Categorias where P_Categorias.id = PRO.idCategoria) AS Categoria" +
                   $" ,[activo]" +
                   $" ,[idCuenta]" +
                   $" ,[valor]" +
                   $" ,[horasPreparacion]" +
                   $" ,[minutosPreparacion]" +
                   $" ,[imagen]" +
                   $" FROM[dbo].[P_Productos] AS PRO" +
                   $" WHERE idCuenta = {idCuenta}" +
                   filtro +
                   $" ORDER BY id ASC" +
                   $" OFFSET(@Skip) ROWS FETCH NEXT(@Take) ROWS ONLY";
        }

        public static string GetSqlAllClientes(int idCuenta, int Skip, int Take, string nombre)
        {
            StringBuilder filtro = new StringBuilder();
            if (nombre != null)
            {
                filtro.Append($" AND nombre LIKE '%{nombre}%'");
            }

            return $"DECLARE @Skip INT = {Skip}, @Take INT = {Take}" +
                   $" SELECT* FROM[dbo].[P_Clientes]" +
                   $" WHERE idCuenta = {idCuenta}" +
                   filtro +
                   $" ORDER BY id ASC" +
                   $" OFFSET(@Skip) ROWS FETCH NEXT(@Take) ROWS ONLY";
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedidos.Data
{
    public static class SqlConsultas
    {
        public static string GetSqlCardapio(int? idCategoria, int idCuenta)
        {
            StringBuilder query = new StringBuilder();

            query.Append($" SELECT P.id as id");
            query.Append($" ,(SELECT * FROM (SELECT value  FROM STRING_SPLIT(CAST ((select top(1) idsAdicionales from [dbo].[P_CategoriaAdicional] where idCategoria = {idCategoria} and idCuenta = {idCuenta}) AS varchar(MAX)), ',') WHERE RTRIM(value) <> '') AS V JOIN[dbo].[P_Adicionais] AS A on V.value = A.id FOR JSON AUTO) as JsonAdicionales ");
            query.Append($" ,'' as JsonIngredientes ");

            query.Append($" ,C.nombre as categoria ");
            query.Append($" ,(SELECT * FROM[dbo].[P_Productos] where id = P.id FOR JSON AUTO) as JsonProducto ");

            query.Append($" FROM[dbo].[P_Productos] AS P ");
            query.Append($" JOIN ");
            query.Append($" [dbo].[P_Categorias] AS C ");
            query.Append($" ON C.id = P.idCategoria ");
            query.Append($" WHERE P.idCuenta = {idCuenta} ");
            query.Append($" AND P.idCategoria = {idCategoria} ");

            var str = query.ToString();
            return str;
            //return $"DECLARE @Skip INT = {Skip}, @Take INT = {Take}" +
            //       $" SELECT* FROM[dbo].[P_Categorias]" +
            //       $" WHERE idCuenta = {idCuenta}" +
            //       filtro +
            //       $" ORDER BY id ASC" +
            //       $" OFFSET(@Skip) ROWS FETCH NEXT(@Take) ROWS ONLY";
        }

        public static string GetSqlProductosAll(int idCuenta)
        {
            StringBuilder query = new StringBuilder();

            query.Append($"  SELECT id,codigo,nombre,descripcion,idCategoria,idSubCategoria,idCuenta,valor,unidadeMedida,horasPreparacion,minutosPreparacion,cantidadSabores,activo,tamanho1,valorTamanho1,tamanho2,valorTamanho2,tamanho3,valorTamanho3,tamanho4,valorTamanho4,tamanho5,valorTamanho5,tamanho6,valorTamanho6,tamanho7,valorTamanho7,tamanho8,valorTamanho8,tamanho9,valorTamanho9,imagen ");
            query.Append($" ,JsonAdicionales = (SELECT * FROM (SELECT value  FROM STRING_SPLIT(CAST ((select top(1) idsAdicionales from [dbo].[P_CategoriaAdicional] where idCategoria = P.idCategoria and idCuenta = {idCuenta}) AS varchar(MAX)), ',') WHERE RTRIM(value) <> '') AS V JOIN[dbo].[P_Adicionais] AS A on V.value = A.id FOR JSON PATH) ");
            query.Append($" ,JsonIngredientes = (SELECT * FROM (SELECT value  FROM STRING_SPLIT(CAST ((select top(1) idsIngrediente from [dbo].[P_IngredientesProducto] where idProducto = P.id and idCuenta = {idCuenta}) AS varchar(MAX)), ',') WHERE RTRIM(value) <> '') AS V JOIN [dbo].[P_Ingredientes] AS A on V.value = A.id FOR JSON PATH)  ");
            query.Append($" FROM[dbo].[P_Productos] AS P ");
            query.Append($" WHERE P.idCuenta = {idCuenta} ");
            var str = query.ToString();
            return str;
        }

        public static string GetSqlProductosDetalle(int idCuenta, int? idProducto)
        {
            StringBuilder query = new StringBuilder();

            query.Append($" SELECT P.id as id");
            query.Append($" ,(SELECT * FROM (SELECT value  FROM STRING_SPLIT(CAST ((select top(1) idsAdicionales from [dbo].[P_CategoriaAdicional] where idCategoria = P.idCategoria and idCuenta = {idCuenta}) AS varchar(MAX)), ',') WHERE RTRIM(value) <> '') AS V JOIN[dbo].[P_Adicionais] AS A on V.value = A.id FOR JSON PATH) as JsonAdicionales ");
            query.Append($" ,(SELECT * FROM (SELECT value  FROM STRING_SPLIT(CAST ((select top(1) idsIngrediente from [dbo].[P_IngredientesProducto] where idProducto = P.id and idCuenta = {idCuenta}) AS varchar(MAX)), ',') WHERE RTRIM(value) <> '') AS V JOIN [dbo].[P_Ingredientes] AS A on V.value = A.id FOR JSON PATH) as JsonIngredientes  ");

            query.Append($" ,C.nombre as categoria ");
            query.Append($" ,(SELECT * FROM[dbo].[P_Productos] where id = P.id FOR JSON PATH) as JsonProducto ");

            query.Append($" FROM[dbo].[P_Productos] AS P ");
            query.Append($" JOIN ");
            query.Append($" [dbo].[P_Categorias] AS C ");
            query.Append($" ON C.id = P.idCategoria ");
            query.Append($" WHERE P.idCuenta = {idCuenta} ");

            if (idProducto.HasValue)
            {
                query.Append($" and P.id = {idProducto} ");
            }

            var str = query.ToString();
            return str;
            //return $"DECLARE @Skip INT = {Skip}, @Take INT = {Take}" +
            //       $" SELECT* FROM[dbo].[P_Categorias]" +
            //       $" WHERE idCuenta = {idCuenta}" +
            //       filtro +
            //       $" ORDER BY id ASC" +
            //       $" OFFSET(@Skip) ROWS FETCH NEXT(@Take) ROWS ONLY";
        }

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
                   $" ,[cantidadSabores]" +
                   $" ,[imagen]" +
                   $" ,[tamanho1]" +
                   $" ,[valorTamanho1]" +
                   $" ,[tamanho2]" +
                   $" ,[valorTamanho2]" +
                   $" ,[tamanho3]" +
                   $" ,[valorTamanho3]" + 
                   $" ,[tamanho4]" +
                   $" ,[valorTamanho4]" +
                   $" ,[tamanho5]" +
                   $" ,[valorTamanho5]" +
                   $" ,[tamanho6]" +
                   $" ,[valorTamanho6]" +
                   $" ,[tamanho7]" +
                   $" ,[valorTamanho7]" +
                   $" ,[tamanho8]" +
                   $" ,[valorTamanho8]" +
                   $" ,[tamanho9]" +
                   $" ,[valorTamanho9]" +
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

﻿
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
                Skip = 0;
                filtro.Append($" AND nombre LIKE '%{nombre}%'");
            }

            return $"DECLARE @Skip INT = {Skip}, @Take INT = {Take}" +
                   $" SELECT* FROM[dbo].[P_Categorias]" +
                   $" WHERE idCuenta = {idCuenta}" +                   
                   filtro +
                   $" ORDER BY id ASC" +
                   $" OFFSET(@Skip) ROWS FETCH NEXT(@Take) ROWS ONLY";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Utils
{
    public static class Util
    {
        public static string CreateCodigoPedido(int idCuenta)
        {
            var cuenta = idCuenta.ToString();
            var year = DateTime.Now.Year.ToString();
            var mes = DateTime.Now.Month.ToString();
            var dia = DateTime.Now.Day.ToString();
            var hora = DateTime.Now.Hour.ToString();
            var min = DateTime.Now.Minute.ToString();
            var sec = DateTime.Now.Second.ToString();

            return "P" + cuenta + year + mes + dia + hora + min + sec;
        }
    }
}

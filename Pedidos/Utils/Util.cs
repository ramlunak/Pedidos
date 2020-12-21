using Pedidos.Extensions;
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
            var year = DateTime.Now.ToSouthAmericaStandard().Year.ToString();
            var mes = DateTime.Now.ToSouthAmericaStandard().Month.ToString();
            var dia = DateTime.Now.ToSouthAmericaStandard().Day.ToString();
            var hora = DateTime.Now.ToSouthAmericaStandard().Hour.ToString();
            var min = DateTime.Now.ToSouthAmericaStandard().Minute.ToString();
            var sec = DateTime.Now.ToSouthAmericaStandard().Second.ToString();

            return "P" + cuenta + year + mes + dia + hora + min + sec;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Extensions
{
    public static class NumberExtension
    {
        public static decimal ToDecimal(this string value)
        {
            if (value is null) return 0;
            return Convert.ToDecimal(value.Replace('.', ','));
        }

    }
}

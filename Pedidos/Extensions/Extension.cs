using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Extensions
{
    public static class Extension
    {
        public static bool IsNullOrEmtpy(this string value)
        {   
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        public static T ConvertTo<T>(this string o)
        {         
            return JsonConvert.DeserializeObject<T>(o);
        }

    }
}

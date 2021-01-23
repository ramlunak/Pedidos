using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        public static string ToJson<T>(this T o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static StringContent ToStringContent<T>(this T o)
        {
            return new StringContent(JsonConvert.SerializeObject(o), UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
        }
    }
}

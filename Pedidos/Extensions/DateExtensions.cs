using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pedidos.Extensions
{
    public static class DateExtensions
    {
        public static DateTime ToTimeZone(this DateTime date,string id)
        {
            var kstZone = TimeZoneInfo.FindSystemTimeZoneById(id);
            var data = TimeZoneInfo.ConvertTimeFromUtc(date.ToUniversalTime(), kstZone);
            return data;
        }
    }
}

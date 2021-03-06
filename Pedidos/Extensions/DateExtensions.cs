﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pedidos.Extensions
{
    public static class DateExtensions
    {
        public static DateTime ToTimeZone(this DateTime date, string id)
        {
            var kstZone = TimeZoneInfo.FindSystemTimeZoneById(id);
            var data = TimeZoneInfo.ConvertTimeFromUtc(date.ToUniversalTime(), kstZone);
            return data;
        }

        public static DateTime ToSouthAmericaStandard(this DateTime date)
        {
            var kstZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            var data = TimeZoneInfo.ConvertTimeFromUtc(date.ToUniversalTime(), kstZone);
            return data;
        }

        public static int GetSemana(this DateTime date)
        {
            return CultureInfo.GetCultureInfo("pt-BR").Calendar.GetWeekOfYear(date.ToSouthAmericaStandard(), CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        }

    }
}

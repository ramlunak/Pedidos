using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pedidos.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RolesSistema
    {
        Administrador,
        Establecimiento,
        Funcionario,
        Cliente
    }
}

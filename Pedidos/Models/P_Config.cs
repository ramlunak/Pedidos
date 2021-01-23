using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Pedidos.Extensions;

namespace Pedidos.Models
{
    public class P_Config
    {
        public int id { get; set; }
        public int idCuenta { get; set; }

        [DisplayName("Tamanho")]
        public int printSize { get; set; }

        [DisplayName("Tamanho letra")]
        public int fontSize { get; set; }

    }
}

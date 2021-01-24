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

        [DisplayName("Tamanho folha:")]
        public int printSize { get; set; }

        [DisplayName("Tamanho letra:")]
        public int fontSize { get; set; }

        //ESTABLECIMIENTO
        [DisplayName("Nome:")]
        public string nombreEstablecimiento { get; set; }
        [DisplayName("Endereço:")]
        public string direccionEstablecimiento { get; set; }
        [DisplayName("Telefone:")]
        public string telefonoEstablecimiento { get; set; }
        [DisplayName("CNPJ:")]
        public string cnpjEstablecimiento { get; set; }

    }
}

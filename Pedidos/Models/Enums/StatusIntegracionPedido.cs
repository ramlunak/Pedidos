﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models.Enums
{
    public enum StatusIntegracionPedido
    {
        Esperando,
        EnCurrentRuta,
        Enviado,
        Entregado,
        Cancelado
    }
}

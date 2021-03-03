using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class ErrorDetail
    {
      
        public int Row { get; set; }

        public string Code { get; set; }

        public string Column { get; set; }
        
        public string Detail { get; set; }  
        
    }
}

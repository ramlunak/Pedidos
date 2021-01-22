using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public abstract class NegocioAbstract
    {
        private AppDbContext _contexto;

        internal AppDbContext Contexto
        {
            get
            {
                return _contexto;
            }
        }

        public NegocioAbstract()
        {
            _contexto = new AppDbContext();
        }

        internal NegocioAbstract(AppDbContext contexto)
        {
            _contexto = contexto == null ? new AppDbContext() : contexto;
        }

    }
}

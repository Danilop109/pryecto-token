using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interface;
using Persistencia;

namespace Aplicacion.Repository
{
    public class RolRepository : GenericRepository<Rol>, IRol
    {
        protected readonly ApiTokenContext _context;

        public RolRepository(ApiTokenContext context) : base(context)
        {
        _context = context;
            
        }
    }
}
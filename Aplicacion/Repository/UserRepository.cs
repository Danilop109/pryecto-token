using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interface;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository
{
    public class UserRepository : GenericRepository<User>, IUser
    {
         protected readonly ApiTokenContext _context;

        public UserRepository(ApiTokenContext context) : base(context)
        {
        _context = context;
            
        }
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.Include(u => u.Rols).FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
        }
    }
}
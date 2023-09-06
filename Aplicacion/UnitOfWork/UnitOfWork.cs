using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Repository;
using Dominio.Interface;
using Persistencia;

namespace Aplicacion.UnitOfWork;
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApiTokenContext context;
        RolRepository _rols;
        UserRepository _users;


    public UnitOfWork(ApiTokenContext _context)
    {
        context = _context;
    }

    public IUser Users
    {
        get{
            if(_users == null){
                _users = new UserRepository(context);
            }
            return _users;
        }
    } 

    public IRol Rols
    {
        get{
            if(_rols == null){
                _rols = new RolRepository(context);
            }
            return _rols;
        }
    } 

    public void Dispose()
    {
        context.Dispose();
    }
    public async Task<int> SaveAsync()
    {
        return await context.SaveChangesAsync();
    }
    }

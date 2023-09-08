using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Api.Helpers
{
    public class GlobalVerbRoleRequirement : IAuthorizationRequirement
    {
        public bool IsAllowed(string role, string verb)
        {
            //se necesita especificar los roles y las especificacioes para poder realiar el crud
            //Al no especificar get o algun verbo, este le permitira hacer de todo con la base de datos
            if(string.Equals("Administrador", role, StringComparison.OrdinalIgnoreCase)) return true;
            if(string.Equals("Gerente", role, StringComparison.OrdinalIgnoreCase)) return true;

            //Permite solo el "Get" 
            if(string.Equals("Empleado", role, StringComparison.OrdinalIgnoreCase) && string.Equals("GET", verb, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            };
            if(string.Equals("Camper", role, StringComparison.OrdinalIgnoreCase) && string.Equals("GET", verb, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            };
            // ... agrega otros roles si quieres...

            return false;

        }
    }
}
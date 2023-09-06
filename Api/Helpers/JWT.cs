using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }//quien lo emitio, el token
        public string Audience { get; set; }//quien resive la peticion del token
        public double DurationInMinutes { get; set; }

    }
}
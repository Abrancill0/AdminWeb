using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class EncriptaDesencriptaController : ApiController
    {
        public class Datos
        {
            public string valor { get; set; }
            public int variable { get; set; }
        }

        public string PostEncriptaDesencripta(Datos datos)
        {
            if (datos.variable == 0)
            {
                string Desencripta = Seguridad.DesEncriptar(datos.valor);
                return Desencripta;
            }
            else
            {
                string Encripta = Seguridad.Encriptar(datos.valor);
                return Encripta;
            }
         
        }

    }
}

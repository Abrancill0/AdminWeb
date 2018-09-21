using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;

namespace SCGESP.Controllers
{
    public class BajaAlternoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string RmUaaUsuario { get; set; }
            public string RmUaaUsuarioAlterno { get; set; }
        }

        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public XmlDocument Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.RmUaaUsuario,
                Origen = "AdminAPP",
                Transaccion = 120795,
                Operacion = 9,
            };

            entrada.agregaElemento("RmUaaUsuarioAlterno", Datos.RmUaaUsuarioAlterno);
           
            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                return respuesta.Documento;
            }
            else
            {
                var errores = respuesta.Errores;

                return null;
            }
        }

        public static DocumentoSalida PeticionCatalogo(XmlDocument doc)
        {
            Localhost.Elegrp ws = new Localhost.Elegrp();
            ws.Timeout = -1;
            string respuesta = ws.PeticionCatalogo(doc);
            return new DocumentoSalida(respuesta);
        }
    }
}

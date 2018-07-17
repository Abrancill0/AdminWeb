using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class AutorizaAdministrativoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public int RmOcoId { get; set; }
        }

        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public XmlDocument Post(ParametrosEntrada Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencripta,
                Origen = "AdminAPP",
                Transaccion = 120768,
                Operacion = 10,
            };

            entrada.agregaElemento("estatus", 2);
            entrada.agregaElemento("RmOcoId", Datos.RmOcoId);

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

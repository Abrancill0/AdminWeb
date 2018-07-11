using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class AutorizaRequisicionController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
            public string RmReqId { get; set; }
            public string RmReqEstatus { get; set; }
        }

        public class AutorizaRequisicionResult
        {
            public int Resultado { get; set; }
        }

        public XmlDocument Post(datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencripta,
                Origen = "Programa CGE",  //Datos.Origen; 
                Transaccion = 120760,
                Operacion = 10 //autorizar requisiciones
            };
            entrada.agregaElemento("RmReqId", Datos.RmReqId);
            entrada.agregaElemento("RmReqEstatus", Datos.RmReqEstatus);
            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                return respuesta.Documento;
            }
            else
            {
                return respuesta.Documento;
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


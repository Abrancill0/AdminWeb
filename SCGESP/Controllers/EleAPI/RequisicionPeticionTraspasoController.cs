using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;

namespace SCGESP.Controllers.EleAPI
{
    public class RequisicionPeticionTraspasoController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
            public string RmReqId { get; set; }
        }

        public class RequisicionEncabezadoResult
        {
            public int Resultado { get; set; }
        }

        public DocumentoSalida Post(datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada();
            entrada.Usuario = UsuarioDesencripta;  
            entrada.Origen = "Programa CGE";  //Datos.Origen; 
            entrada.Transaccion = 120760;
            entrada.Operacion = 21;

            entrada.agregaElemento("RmReqId", Datos.RmReqId);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                //var resultado = "Requisicion Generadacorrectamente";

                return respuesta;
            }
            else
            {
                var errores = respuesta;

                return errores;
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
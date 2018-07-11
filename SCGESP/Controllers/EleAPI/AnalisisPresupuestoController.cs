using Ele.Generales;
using SCGESP.Clases;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.EleAPI
{
    public class AnalisisPresupuestoV2Controller : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string RmReqId { get; set; }
        }

        public class AutorizaRequisicionResult
        {
            public int Resultado { get; set; }
        }

        public XmlDocument Post(datos Datos)
        {
            try
            {
                string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

                DocumentoEntrada entrada = new DocumentoEntrada
                {
                    Usuario = UsuarioDesencripta,
                    Origen = "Programa CGE",
                    Transaccion = 120760,
                    Operacion = 16
                };

                //entrada.agregaElemento("SgUsuMostrarGraficaAlAutorizar", 1);
                entrada.agregaElemento("RmReqId", Datos.RmReqId);

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                // return entrada;

                if (respuesta.Resultado == "1")
                {
                    return respuesta.Documento;
                }
                else
                {
                    return respuesta.Documento;
                }
            }
            catch (System.Exception ex)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml("<Error>" + ex.ToString() + "</Error>");

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

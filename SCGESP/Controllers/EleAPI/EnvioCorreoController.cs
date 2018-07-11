using Ele.Generales;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.EleAPI
{
    public class EnvioCorreoController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string correo { get; set; }
            public string correoCopia { get; set; }
            public string Asunto { get; set; }
            public string Mensaje { get; set; }
        }

        public class AutorizaRequisicionResult
        {
            public int Resultado { get; set; }
        }

        public XmlDocument Post(datos Datos)
        {
            try
            {
                string UsuarioDesencripta = Clases.Seguridad.DesEncriptar(Datos.Usuario);

                DocumentoEntrada entrada = new DocumentoEntrada
                {
                    Usuario = UsuarioDesencripta,
                    Origen = "Programa CGE",
                    Transaccion = 3
                };

                entrada.agregaElemento("Para", Datos.correo);
                entrada.agregaElemento("Copia", Datos.correoCopia);
                entrada.agregaElemento("Asunto", Datos.Asunto);
                entrada.agregaElemento("Mensaje", Datos.Mensaje);

                DocumentoSalida respuesta = PeticionGeneral(entrada.Documento);

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

                return xml;
            }

        }

        public static DocumentoSalida PeticionGeneral(XmlDocument doc)
        {
            Localhost.Elegrp ws = new Localhost.Elegrp();
            ws.Timeout = -1;
            string respuesta = ws.PeticionGeneral(doc);
            return new DocumentoSalida(respuesta);
        }


    }
}
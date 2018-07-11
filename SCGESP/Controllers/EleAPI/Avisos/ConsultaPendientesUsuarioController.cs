using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class ConsultaPendientesUsuarioController : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
        }

        public class PendientesUsuarioResult
        {
            public int Resultado { get; set; }
        }

        public XmlDocument Post(Datos Datos)
        {
            try
            {
                string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

                DocumentoEntrada entrada = new DocumentoEntrada
                {
                    Usuario = UsuarioDesencripta,
                    Origen = "Programa CGE",  //Datos.Origen; 
                    Transaccion = 100004, //usuario
                    Operacion = 18
                };
                entrada.agregaElemento("SgUsuId", UsuarioDesencripta);
                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                if (respuesta.Resultado == "1")
                {
                    return respuesta.Documento;
                }
                else
                {
                    //var errores = respuesta.Errores;

                    return respuesta.Documento;

                }
            }
            catch (System.Exception)
            {

                return null;
            }
            

        }


        public static DocumentoSalida PeticionCatalogo(XmlDocument doc)
        {
            Localhost.Elegrp ws = new Localhost.Elegrp
            {
                Timeout = -1
            };
            string respuesta = ws.PeticionCatalogo(doc);
            return new DocumentoSalida(respuesta);
        }
        
    }
}

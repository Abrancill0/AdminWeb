using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class ConsultaMaterial2Controller : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
            public string RmTrmTipoRequisicion { get; set; }
            public string RmRdeRequisicion { get; set; }
            public string RmTrmMaterial { get; set; }
            public string Requisicion { get; set; }
            public string Valida { get; set; } 
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
                    Transaccion = 120796,
                    Operacion = 16//regresa una tabla con todos los campos de la tabla ( La cantidad de registros depende del filtro enviado)
                };

                //entrada.agregaElemento("RmRdeRequisicion", Datos.RmRdeRequisicion);
                //entrada.agregaElemento("RmTrmTipoRequisicion", Datos.RmTrmTipoRequisicion);
                //entrada.agregaElemento("RmTrmMaterial", 1);//Datos.RmTrmMaterial
                entrada.agregaElemento("requisicion", Datos.Requisicion);
                entrada.agregaElemento("valida", Datos.Valida);

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                if (respuesta.Resultado == "1")
                {
                    return respuesta.Documento;
                }
                else
                {
                    var errores = respuesta.Documento;

                    return errores;
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

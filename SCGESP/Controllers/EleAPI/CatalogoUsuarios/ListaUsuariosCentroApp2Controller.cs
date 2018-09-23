using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;
using System.Data;
using System.Collections.Generic;
using System;

namespace SCGESP.Controllers
{
    public class ListaUsuariosCentroApp2Controller : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
            public string Empleado { get; set; }
        }

        public class ParametrosSalidaResult
        {
            public string Empleado { get; set; }
            public string Nombre { get; set; }
            
        }

        public DocumentoSalida Post(Datos Datos)
        {

            try
            {
                string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

                DocumentoEntrada entrada = new DocumentoEntrada
                {
                    Usuario = UsuarioDesencripta,
                    Origen = "AdminApp",
                    Transaccion = 120037,
                    Operacion = 17
                };
                entrada.agregaElemento("GrEmpId", Datos.Empleado);
                //

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                if (respuesta.Resultado == "1")
                {
                    return respuesta;
                }
                else
                {
                  //  var errores = respuesta;

                    return respuesta;
                }
            }
            catch (Exception)
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

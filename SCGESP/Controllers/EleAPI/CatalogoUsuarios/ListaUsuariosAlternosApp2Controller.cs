using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;
using System.Data;
using System.Collections.Generic;
using System;

namespace SCGESP.Controllers
{
    public class ListaUsuariosAlternosApp2Controller : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
        }

        public class ParametrosSalidaResult
        {
            public string Alterno { get; set; }
            public string Nombre { get; set; }
            public string FechaInicial { get; set; }
            public string FechaFinal { get; set; }

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
                    Transaccion = 120795,
                    Operacion = 16
                };

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                if (respuesta.Resultado == "1")
                {
                    return respuesta;
                }
                else
                {
                    //var errores = respuesta.Errores;

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

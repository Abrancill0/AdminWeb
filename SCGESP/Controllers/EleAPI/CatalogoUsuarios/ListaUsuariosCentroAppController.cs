using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;
using System.Data;
using System.Collections.Generic;
using System;

namespace SCGESP.Controllers
{
    public class ListaUsuariosCentroAppController : ApiController
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

        public List<ParametrosSalidaResult> Post(Datos Datos)
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

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                DataTable DTRequisiciones = new DataTable();

                if (respuesta.Resultado == "1")
                {
                    DTRequisiciones = respuesta.obtieneTabla("Catalogo");

                    List<ParametrosSalidaResult> lista = new List<ParametrosSalidaResult>();

                    foreach (DataRow row in DTRequisiciones.Rows)
                    {
                        ParametrosSalidaResult ent = new ParametrosSalidaResult
                        {
                            Empleado = Convert.ToString(row["Empleado"]),
                            Nombre = Convert.ToString(row["Nombre"]),
                        };
                        lista.Add(ent);
                    }
                    return lista;
                }
                else
                {
                    var errores = respuesta.Errores.InnerText;

                    List<ParametrosSalidaResult> lista = new List<ParametrosSalidaResult>();

                    ParametrosSalidaResult ent = new ParametrosSalidaResult
                    {
                        Empleado = "Error",
                        Nombre = Convert.ToString(errores),
                    };

                    lista.Add(ent);

                    return lista;
                }
            }
            catch (Exception ex)
            {

                List<ParametrosSalidaResult> lista = new List<ParametrosSalidaResult>();

                ParametrosSalidaResult ent = new ParametrosSalidaResult
                {
                    Empleado = "Error ex",
                    Nombre = ex.ToString(),
                };

                lista.Add(ent);

                return lista;
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

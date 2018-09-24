using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;
using System.Data;
using System.Collections.Generic;
using System;

namespace SCGESP.Controllers
{
    public class ListaUsuariosAlternosAppController : ApiController
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

        public List<ParametrosSalidaResult> Post(Datos Datos)
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

                DataTable DTRequisiciones = new DataTable();

                if (respuesta.Resultado == "1")
                {
                    DTRequisiciones = respuesta.obtieneTabla("Alternos");

                    List<ParametrosSalidaResult> lista = new List<ParametrosSalidaResult>();

                    foreach (DataRow row in DTRequisiciones.Rows)
                    {
                        ParametrosSalidaResult ent = new ParametrosSalidaResult
                        {
                            Alterno = Convert.ToString(row["Alterno"]),
                            Nombre = Convert.ToString(row["Nombre"]),
                            FechaInicial = Convert.ToString(row["FechaInicial"]),
                            FechaFinal = Convert.ToString(row["FechaFinal"]),
                        };
                        lista.Add(ent);
                    }
                    return lista;
                }
                else
                {
                    var errores = respuesta.Errores;

                    List<ParametrosSalidaResult> lista = new List<ParametrosSalidaResult>();

                    ParametrosSalidaResult ent = new ParametrosSalidaResult
                    {
                        Alterno = "Error",
                        Nombre = Convert.ToString(errores),
                        FechaInicial = "",
                        FechaFinal = ""
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
                    Alterno = "Error",
                    Nombre = ex.ToString(),
                    FechaInicial = "",
                    FechaFinal = ""
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

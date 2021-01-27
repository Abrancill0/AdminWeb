using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace SCGESP.Controllers.APP
{
    public class App_HistoricoController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string FechaInicio { get; set; }
            public string FechaFinal { get; set; }
            public string Origen { get; set; }
        }

        public class RequisicionesPorAutorizarResult
        {
            public string Requisicion { get; set; }
            public string Solicitante { get; set; }
            public string NombreSolicitante { get; set; }
            public string Departamento { get; set; }
            public string NombreDepartamento { get; set; }
            public string Oficina { get; set; }
            public string NombreOficina { get; set; }
            public string TipoGasto { get; set; }
            public string Importe { get; set; }
            public string Moneda { get; set; }
            public string NombreMoneda { get; set; }
            public string Usuario { get; set; }
            public string NombreUsuario { get; set; }
            public string NombreTipoGasto { get; set; }
            public string UsuarioObligado { get; set; }
            public string NombreUsuarioObligado { get; set; }
            public string FechaAutorizacion { get; set; }
            public string NombreProveedor { get; set; }
            public string Justificacion { get; set; }
            public string NombreEstatus { get; set; }
        }
        public JObject Post(datos Datos)
            {
                try
                {
                    string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

                    //string EmpleadoDesencripta = Seguridad.DesEncriptar(Datos.Empleado);

                    DocumentoEntrada entrada = new DocumentoEntrada();
                    entrada.Usuario = UsuarioDesencripta; //Datos.Usuario;  
                    entrada.Origen = "AdminApp";  //Datos.Origen; 
                    entrada.Transaccion = 120761;
                    entrada.Operacion = 16;//ConsultaAdicional1
                                           //entrada.agregaElemento("proceso", 9);
                    entrada.agregaElemento("FechaInicial", Datos.FechaInicio);
                    entrada.agregaElemento("FechaFinal", Datos.FechaFinal);

                    DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                    DataTable DTRequisiciones = new DataTable();

                    if (respuesta.Resultado == "1")
                    {
                        DTRequisiciones = respuesta.obtieneTabla("Autorizaciones");

                        List<RequisicionesPorAutorizarResult> lista = new List<RequisicionesPorAutorizarResult>();

                        foreach (DataRow row in DTRequisiciones.Rows)
                        {

                            RequisicionesPorAutorizarResult ent = new RequisicionesPorAutorizarResult
                            {
                                Requisicion = Convert.ToString(row["Requisicion"]), //OK
                                Solicitante = Convert.ToString(row["Solicitante"]),
                                NombreSolicitante = Convert.ToString(row["NombreSolicitante"]),
                                Departamento = Convert.ToString(row["Departamento"]),
                                NombreDepartamento = Convert.ToString(row["NombreDepartamento"]),
                                Oficina = Convert.ToString(row["Oficina"]),
                                NombreOficina = Convert.ToString(row["NombreOficina"]),
                                TipoGasto = Convert.ToString(row["TipoGasto"]),
                                NombreTipoGasto = Convert.ToString(row["NombreTipoGasto"]),
                                Importe = Convert.ToString(row["Importe"]),
                                Moneda = Convert.ToString(row["Moneda"]),
                                NombreMoneda = Convert.ToString(row["NombreMoneda"]),
                                Usuario = Convert.ToString(row["Usuario"]),
                                NombreUsuario = Convert.ToString(row["NombreUsuario"]),
                                UsuarioObligado = Convert.ToString(row["UsuarioObligado"]),
                                NombreUsuarioObligado = Convert.ToString(row["NombreUsuarioObligado"]),
                                FechaAutorizacion = Convert.ToString(row["FechaAutorizacion"]),
                                NombreProveedor = Convert.ToString(row["NombreProveedor"]),
                                Justificacion = Convert.ToString(row["Justificacion"]),
                               // NombreEstatus = Convert.ToString(row["NombreEstatus"]),
                            };
                            lista.Add(ent);
                        }

                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = "OK",
                        estatus = 1,
                        Result = lista

                    });


                    return Resultado;

                }
                else
                {


                    XDocument doc = XDocument.Parse(respuesta.Documento.InnerXml);
                    XElement Salida = doc.Element("Salida");
                    XElement Errores = Salida.Element("Errores");
                    XElement Error = Errores.Element("Error");
                    XElement Descripcion = Error.Element("Descripcion");
                    

                    string resultado2 = respuesta.Errores.InnerText;

                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = Descripcion.Value,
                        estatus = 0,
                    });

                    return Resultado;

                }
            }
            catch (Exception ex)
            {

                JObject Resultado = JObject.FromObject(new
                {
                    mensaje = ex.ToString(),
                    estatus = 0,
                    CambiaContrasena = false,
                });


                return Resultado;
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


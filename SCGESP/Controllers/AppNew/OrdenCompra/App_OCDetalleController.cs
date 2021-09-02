using Ele.Generales;
using Newtonsoft.Json.Linq;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

<<<<<<< HEAD:SCGESP/Controllers/AppNew/App_OCDetalleController.cs
namespace SCGESP.Controllers.AppNew
{
    public class App_OCDetalleController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string RmRdeOrdenCompra { get; set; }
        }

        public class RequisicionDetalleResult
        {
            public string RmRdeOrdenCompra { get; set; }
            public string RmRdeId { get; set; }
            public string RmRdeEstatus { get; set; }
            public string RmRdeEstatusNombre { get; set; }
            public string RmRdeMaterialCol { get; set; }
            public string RmRdeUnidadMedidaCol { get; set; }
            public string RmRdeUnidadMedidaColNombre { get; set; }
            public string RmRdeCantidadColocada { get; set; }
            public string RmRdeGranTotalCol { get; set; }
            public string RmRdePrecioUniCol { get; set; }
            public string RmRdeSubtotalCol { get; set; }
            public string RmRdePorcIvaCol { get; set; }
            public string RmRdeTotalCol { get; set; }
            public string RmRdeCuentaCol { get; set; }
            public string RmRdeCuentaColNombre { get; set; }
        }

        public JObject Post(datos Datos)
        {
            try
            {
                string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

                DocumentoEntrada entrada = new DocumentoEntrada();
                entrada.Usuario = UsuarioDesencripta;
                entrada.Origen = "AdminApp";  //Datos.Origen; 
                entrada.Transaccion = 120769;
                entrada.Operacion = 1;

                entrada.agregaElemento("RmRdeOrdenCompra", Datos.RmRdeOrdenCompra);

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                DataTable DTRequisiciones = new DataTable();

                if (respuesta.Resultado == "1")
                {
                    DTRequisiciones = respuesta.obtieneTabla("Catalogo");

                    List<RequisicionDetalleResult> lista = new List<RequisicionDetalleResult>();

                    foreach (DataRow row in DTRequisiciones.Rows)
                    {
                        RequisicionDetalleResult ent = new RequisicionDetalleResult
                        {
                            RmRdeOrdenCompra = Convert.ToString(row["RmRdeOrdenCompra"]),
                            RmRdeId = Convert.ToString(row["RmRdeId"]),
                            RmRdeEstatus = Convert.ToString(row["RmRdeEstatus"]),
                            RmRdeEstatusNombre = Convert.ToString(row["RmRdeEstatusNombre"]),
                            RmRdeMaterialCol = Convert.ToString(row["RmRdeMaterialCol"]),
                            RmRdeUnidadMedidaCol = Convert.ToString(row["RmRdeUnidadMedidaCol"]),
                            RmRdeUnidadMedidaColNombre = Convert.ToString(row["RmRdeUnidadMedidaColNombre"]),
                            RmRdeCantidadColocada = Convert.ToString(row["RmRdeCantidadColocada"]),
                            RmRdeGranTotalCol = Convert.ToString(row["RmRdeGranTotalCol"]),
                            RmRdePrecioUniCol = Convert.ToString(row["RmRdePrecioUniCol"]),
                            RmRdeSubtotalCol = Convert.ToString(row["RmRdeSubtotalCol"]),
                            RmRdePorcIvaCol = Convert.ToString(row["RmRdePorcIvaCol"]),
                            RmRdeTotalCol = Convert.ToString(row["RmRdeTotalCol"]),
                            RmRdeCuentaCol = Convert.ToString(row["RmRdeCuentaCol"]),
                            RmRdeCuentaColNombre = Convert.ToString(row["RmRdeCuentaColNombre"]),
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
            Localhost.Elegrp ws = new Localhost.Elegrp();
            ws.Timeout = -1;
            string respuesta = ws.PeticionCatalogo(doc);
            return new DocumentoSalida(respuesta);
        }


    }
}


=======
namespace SCGESP.Controllers.AppNew
{
    public class App_OCDetalleController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
            public string RmRdeOrdenCompra { get; set; }
        }

        public class RequisicionDetalleResult
        {
            public string RmRdeOrdenCompra { get; set; }
            public string RmRdeId { get; set; }
            public string RmRdeEstatus { get; set; }
            public string RmRdeEstatusNombre { get; set; }
            public string RmRdeMaterialCol { get; set; }
            public string RmRdeUnidadMedidaCol { get; set; }
            public string RmRdeUnidadMedidaColNombre { get; set; }
            public string RmRdeCantidadColocada { get; set; }
            public string RmRdeGranTotalCol { get; set; }
            public string RmRdePrecioUniCol { get; set; }
            public string RmRdeSubtotalCol { get; set; }
            public string RmRdePorcIvaCol { get; set; }
            public string RmRdeTotalCol { get; set; }
            public string RmRdeCuentaCol { get; set; }
            public string RmRdeCuentaColNombre { get; set; }
        }

        public JObject Post(datos Datos)
        {
            try
            {
                string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

                DocumentoEntrada entrada = new DocumentoEntrada();
                entrada.Usuario = UsuarioDesencripta;
                entrada.Origen = "AdminApp";  //Datos.Origen; 
                entrada.Transaccion = 120769;
                entrada.Operacion = 1;

                entrada.agregaElemento("RmRdeOrdenCompra", Datos.RmRdeOrdenCompra);

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                DataTable DTRequisiciones = new DataTable();

                if (respuesta.Resultado == "1")
                {
                    DTRequisiciones = respuesta.obtieneTabla("Catalogo");

                    List<RequisicionDetalleResult> lista = new List<RequisicionDetalleResult>();

                    foreach (DataRow row in DTRequisiciones.Rows)
                    {
                        RequisicionDetalleResult ent = new RequisicionDetalleResult
                        {
                            RmRdeOrdenCompra = Convert.ToString(row["RmRdeOrdenCompra"]),
                            RmRdeId = Convert.ToString(row["RmRdeId"]),
                            RmRdeEstatus = Convert.ToString(row["RmRdeEstatus"]),
                            RmRdeEstatusNombre = Convert.ToString(row["RmRdeEstatusNombre"]),
                            RmRdeMaterialCol = Convert.ToString(row["RmRdeMaterialCol"]),
                            RmRdeUnidadMedidaCol = Convert.ToString(row["RmRdeUnidadMedidaCol"]),
                            RmRdeUnidadMedidaColNombre = Convert.ToString(row["RmRdeUnidadMedidaColNombre"]),
                            RmRdeCantidadColocada = Convert.ToString(row["RmRdeCantidadColocada"]),
                            RmRdeGranTotalCol = Convert.ToString(row["RmRdeGranTotalCol"]),
                            RmRdePrecioUniCol = Convert.ToString(row["RmRdePrecioUniCol"]),
                            RmRdeSubtotalCol = Convert.ToString(row["RmRdeSubtotalCol"]),
                            RmRdePorcIvaCol = Convert.ToString(row["RmRdePorcIvaCol"]),
                            RmRdeTotalCol = Convert.ToString(row["RmRdeTotalCol"]),
                            RmRdeCuentaCol = Convert.ToString(row["RmRdeCuentaCol"]),
                            RmRdeCuentaColNombre = Convert.ToString(row["RmRdeCuentaColNombre"]),
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
            Localhost.Elegrp ws = new Localhost.Elegrp();
            ws.Timeout = -1;
            string respuesta = ws.PeticionCatalogo(doc);
            return new DocumentoSalida(respuesta);
        }


    }
}


>>>>>>> f0800bfa784d8b3a190d97c9212b0c9c91ba8b94:SCGESP/Controllers/AppNew/OrdenCompra/App_OCDetalleController.cs

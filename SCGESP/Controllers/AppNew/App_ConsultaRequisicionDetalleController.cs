using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.APP
{
    public class ConsultaRequisicionDetalleAppController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
            public string RmRdeRequisicion { get; set; }
        }

        public class RequisicionDetalleResult
        {
            public int RmReqId { get; set; }
            public int RmRdeRequisicion { get; set; }
            public int RmRdeId { get; set; }
            public string RmRdeEstatus { get; set; }
            public string RmRdeEstatusNombre { get; set; }
            public string RmReqIdRmRdeEstatusNombre { get; set; }
            public string RmReqTipoRequisicion { get; set; }
            public string RmReqTipoRequisicionNombre { get; set; }
            public string RmRdeCantidadSolicitada { get; set; }
            public string RmRdeMaterial { get; set; }
            public string RmRdeMaterialNombre { get; set; }
            public string RmRdePrecioUnitario { get; set; }
            public string RmRdePorcIva { get; set; }
            public double RmRdeSubtotal { get; set; }
            public string RmRdeIva { get; set; }
            public string RmRdeCuenta { get; set; }
            public string RmRdeCuentaNombre { get; set; }
            public string RmRdeGrupoMaterial { get; set; }

        }

        public JObject Post(datos Datos)
        {
            try
            {
                string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

                DocumentoEntrada entrada = new DocumentoEntrada();
                entrada.Usuario = UsuarioDesencripta;
                entrada.Origen = "Programa CGE";  //Datos.Origen; 
                entrada.Transaccion = 120762;
                entrada.Operacion = 1;

                entrada.agregaElemento("RmRdeRequisicion", Datos.RmRdeRequisicion);

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
                            RmRdeRequisicion = Convert.ToInt32(row["RmRdeRequisicion"]),
                            RmRdeId = Convert.ToInt32(row["RmRdeId"]),
                            RmRdeEstatus = Convert.ToString(row["RmRdeEstatus"]),
                            RmRdeEstatusNombre = Convert.ToString(row["RmRdeEstatusNombre"]),
                            RmReqTipoRequisicion = Convert.ToString(row["RmReqTipoRequisicion"]),
                            RmReqTipoRequisicionNombre = Convert.ToString(row["RmReqTipoRequisicionNombre"]),
                            RmRdeCantidadSolicitada = Convert.ToString(row["RmRdeCantidadSolicitada"]),
                            RmRdeMaterial = Convert.ToString(row["RmRdeMaterial"]),
                            RmRdeMaterialNombre = Convert.ToString(row["RmRdeMaterialNombre"]),
                            RmRdePrecioUnitario = Convert.ToString(row["RmRdePrecioUnitario"]),
                            RmRdePorcIva = Convert.ToString(row["RmRdePorcIva"]),
                            RmRdeSubtotal = Convert.ToDouble(row["RmRdeSubtotal"]),
                            RmRdeIva = Convert.ToString(row["RmRdeIva"]),
                            RmRdeCuenta = Convert.ToString(row["RmRdeCuenta"]),
                            RmRdeCuentaNombre = Convert.ToString(row["RmRdeCuentaNombre"]),
                            RmRdeGrupoMaterial = Convert.ToString(row["RmRdeGrupoMaterial"]),


                        };
                        lista.Add(ent);
                    }


                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = Mensaje,
                        estatus = Estatus,
                        Result = lista

                    });


                    return Resultado;

                }
                else
                {

                    //  < Error >< Concepto > SgUsuClaveAcceso </ Concepto >< Descripcion > CONTRASEÑA INVÁLIDA </ Descripcion ></ Error >
                    Mensaje = respuesta.Errores.InnerText;
                    XDocument doc = XDocument.Parse(respuesta.Documento.InnerXml);
                    XElement Salida = doc.Element("Salida");
                    XElement Errores = Salida.Element("Errores");
                    XElement Error = Errores.Element("Error");
                    XElement Descripcion = Error.Element("Descripcion");
                    Estatus = 0;

                    string resultado2 = respuesta.Errores.InnerText;

                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = Descripcion.Value,
                        estatus = Estatus,
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



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

        public List<RequisicionDetalleResult> Post(datos Datos)
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

                    return lista;

                }
                else
                {
                    var errores = respuesta.Errores;

                    List<RequisicionDetalleResult> lista = new List<RequisicionDetalleResult>();

                    RequisicionDetalleResult ent = new RequisicionDetalleResult
                    {
                        RmReqId = Convert.ToInt32(0),
                        RmRdeRequisicion = Convert.ToInt32(0),
                        RmRdeId = Convert.ToInt32(0),
                        RmRdeEstatus = Convert.ToString(""),
                        RmRdeEstatusNombre = Convert.ToString(""),
                        RmReqTipoRequisicion = Convert.ToString("0"),
                        RmReqTipoRequisicionNombre = Convert.ToString("0"),
                        RmRdeCantidadSolicitada = Convert.ToString("0"),
                        RmRdeMaterial = Convert.ToString("0"),
                        RmRdeMaterialNombre = Convert.ToString(errores),
                        RmRdePrecioUnitario = Convert.ToString("0"),
                        RmRdePorcIva = Convert.ToString("0"),
                        RmRdeSubtotal = Convert.ToDouble("0"),
                        RmRdeIva = Convert.ToString("0"),
                        RmRdeCuenta = Convert.ToString("0"),
                        RmRdeCuentaNombre = Convert.ToString("0"),
                        RmRdeGrupoMaterial = Convert.ToString("0"),
                       
                    };

                    lista.Add(ent);

                    return lista;
                }

            }
            catch (Exception ex)
            {  
                List<RequisicionDetalleResult> lista = new List<RequisicionDetalleResult>();

                RequisicionDetalleResult ent = new RequisicionDetalleResult
                {
                    RmReqId = Convert.ToInt32(0),
                    RmRdeRequisicion = Convert.ToInt32(0),
                    RmRdeId = Convert.ToInt32(0),
                    RmRdeEstatus = Convert.ToString(""),
                    RmRdeEstatusNombre = Convert.ToString(""),
                    RmReqTipoRequisicion = Convert.ToString("0"),
                    RmReqTipoRequisicionNombre = Convert.ToString("0"),
                    RmRdeCantidadSolicitada = Convert.ToString("0"),
                    RmRdeMaterial = Convert.ToString("0"),
                    RmRdeMaterialNombre = Convert.ToString(ex.ToString()),
                    RmRdePrecioUnitario = Convert.ToString("0"),
                    RmRdePorcIva = Convert.ToString("0"),
                    RmRdeSubtotal = Convert.ToDouble("0"),
                    RmRdeIva = Convert.ToString("0"),
                    RmRdeCuenta = Convert.ToString("0"),
                    RmRdeCuentaNombre = Convert.ToString("0"),
                    RmRdeGrupoMaterial = Convert.ToString("0"),

                };

                lista.Add(ent);

                return lista;
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



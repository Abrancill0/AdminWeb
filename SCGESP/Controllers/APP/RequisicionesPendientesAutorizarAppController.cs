using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.APP
{
    public class RequisicionesPendientesAutorizarAppController : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
        }

        public class RequisicionesPorAutorizarResult
        {
            public string RmReqId { get; set; }
            public string RmReqEstatusNombre { get; set; }
            public string RmReqTipoRequisicion { get; set; }
            public double RmReqTotal { get; set; }
            public string RmReqSolicitanteNombre { get; set; }
            public string RmReqSubramo { get; set; }
            public string RmReqJustificacion { get; set; }
            public string RmReqOficinaNombre { get; set; }
            public string RmReqEstatus { get; set; }
            public string RmReqMonedaNombre { get; set; }
            public string RmReqTipoGastoNombre { get; set; }
            public string RmReqProveedorNombre { get; set; }
            public string RmReqCentroNombre { get; set; }
            public string RmReqEmpleadoObligado  {get; set; }
            public string RmReqEmpleadoObligadoNombre { get; set; }
            public string RmReqTipoRequisicionNombre { get; set; }
            public string RMCuenta { get; set; }
            public string RmTirRutaProceso { get; set; }
            public string RmReqPolizaReferencia { get; set; }   /*(Póliza Referencia)*/
            public string RmReqCliente { get; set; } /*(Cliente póliza referencia)*/
            public string RmReqFechaInicialPoliza { get; set; }
            public string RmReqFechaFinalPoliza { get; set; } //(Vigencia póliza)
            public string RmReqClasificacionNombre { get; set; } //(Clasificación)
        }

        public List<RequisicionesPorAutorizarResult> Post(Datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencripta,
                Origen = "Programa CGE",  //Datos.Origen; 
                Transaccion = 120760,
                Operacion = 1//regresa una tabla con todos los campos de la tabla ( La cantidad de registros depende del filtro enviado)
            };

            entrada.agregaElemento("proceso", "2");

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            DataTable DTRequisiciones = new DataTable();

            if (respuesta.Resultado == "1")
            {
                DTRequisiciones = respuesta.obtieneTabla("Catalogo");

                List<RequisicionesPorAutorizarResult> lista = new List<RequisicionesPorAutorizarResult>();

                foreach (DataRow row in DTRequisiciones.Rows)
                {

                    string Cuenta = ObtieneCuenta(UsuarioDesencripta, Convert.ToString(row["RmReqId"]));


                    string RmTirRutaProceso = Convert.ToString(row["RmTirRutaProceso"]);
                    string RmReqPolizaReferencia = "";
                    string RmReqCliente = "";
                    string RmReqFechaInicialPoliza = "";
                    string RmReqFechaFinalPoliza = "";
                    string RmReqClasificacionNombre = "";

                    if (RmTirRutaProceso == "9")
                    {
                        RmReqPolizaReferencia = Convert.ToString(row["RmReqPolizaReferencia"]);
                        RmReqCliente = Convert.ToString(row["RmReqCliente"]);
                        RmReqFechaInicialPoliza = Convert.ToString(row["RmReqFechaInicialPoliza"]);
                        RmReqFechaFinalPoliza = Convert.ToString(row["RmReqFechaFinalPoliza"]);
                        RmReqClasificacionNombre = Convert.ToString(row["RmReqClasificacionNombre"]);
                    }

                    RequisicionesPorAutorizarResult ent = new RequisicionesPorAutorizarResult
                    {
                        RmReqId = Convert.ToString(row["RmReqId"]), //OK
                        RmReqEstatusNombre = Convert.ToString(row["RmReqEstatusNombre"]),
                        RmReqJustificacion = Convert.ToString(row["RmReqJustificacion"]),
                        RmReqOficinaNombre = Convert.ToString(row["RmReqOficinaNombre"]),
                        RmReqSolicitanteNombre = Convert.ToString(row["RmReqSolicitanteNombre"]),
                        RmReqTotal = Convert.ToDouble(row["RmReqTotal"]),
                        RmReqEstatus = Convert.ToString(row["RmReqEstatus"]),
                        RmReqMonedaNombre = Convert.ToString(row["RmReqMonedaNombre"]),
                        RmReqTipoGastoNombre = Convert.ToString(row["RmReqTipoGastoNombre"]),
                        RmReqProveedorNombre = Convert.ToString(row["RmReqProveedorNombre"]),
                        RmReqCentroNombre = Convert.ToString(row["RmReqCentroNombre"]),
                        RmReqTipoRequisicion = Convert.ToString(row["RmReqTipoRequisicion"]),
                        RmReqSubramo = Convert.ToString(row["RmReqSubramo"]),
                        RmReqEmpleadoObligado = Convert.ToString(row["RmReqEmpleadoObligado"]),
                        RmReqEmpleadoObligadoNombre = Convert.ToString(row["RmReqEmpleadoObligadoNombre"]),
                        RmReqTipoRequisicionNombre = Convert.ToString(row["RmReqTipoRequisicionNombre"]),
                        RMCuenta = Cuenta,
                        RmTirRutaProceso = RmTirRutaProceso,
                        RmReqPolizaReferencia = RmReqPolizaReferencia,
                        RmReqCliente = RmReqCliente,
                        RmReqFechaInicialPoliza = RmReqFechaInicialPoliza,
                        RmReqFechaFinalPoliza = RmReqFechaFinalPoliza,
                        RmReqClasificacionNombre = RmReqClasificacionNombre,
                    };
                    lista.Add(ent);
                }


                return lista;
            }
            else
            {
                var errores = respuesta.Errores;

                return null;
            }

        }


        public string ObtieneCuenta(string Usuario, string RmRdeRequisicion)
        {
            try
            {
                DocumentoEntrada entrada = new DocumentoEntrada();
                entrada.Usuario = Usuario;
                entrada.Origen = "Programa CGE";  //Datos.Origen; 
                entrada.Transaccion = 120762;
                entrada.Operacion = 1;

                entrada.agregaElemento("RmRdeRequisicion", RmRdeRequisicion);

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                if (respuesta.Resultado == "1")
                {
                    DataTable DTRequisiciones = new DataTable();

                    DTRequisiciones = respuesta.obtieneTabla("Catalogo");

                    double MontoActual = 0;
                    string Cuenta = "";

                    foreach (DataRow row in DTRequisiciones.Rows)
                    {
                        if ((Convert.ToDouble(row["RmRdeSubtotal"]) + Convert.ToDouble(row["RmRdeIva"])) > MontoActual)
                        {

                            Cuenta = Convert.ToString(row["RmRdeCuenta"]) + " - " + Convert.ToString(row["RmRdeCuentaNombre"]);

                            MontoActual = Convert.ToDouble(row["RmRdeSubtotal"]) + Convert.ToDouble(row["RmRdeIva"]);
                        }

                    }

                    return Cuenta;


                }
                else
                {
                    var errores = respuesta.Errores;

                    return "";
                }
            }
            catch (Exception ex)
            {

                return "";
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

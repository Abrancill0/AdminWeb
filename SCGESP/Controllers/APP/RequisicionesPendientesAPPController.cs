using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;

namespace SCGESP.Controllers.APP
{
    public class RequisicionesPendientesAPPController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string Empleado { get; set; }
            public string Origen { get; set; }
        }

        public class RequisicionesPorAutorizarResult
        {
            public string RmReqId { get; set; }
            public string RmReqEstatusNombre { get; set; }
            public string RmReqTipoRequisicion { get; set; }
            public string RmReqTotal { get; set; }
            public string RmReqSolicitanteNombre { get; set; }
            public string RmReqSubramo { get; set; }
            public string RmReqJustificacion { get; set; }
            public string RmReqOficinaNombre { get; set; }
            public string RmReqEstatus { get; set; }
            public string RmReqTipoGastoNombre { get; set; }
            public string RmReqProveedorNombre { get; set; }
            public string RmReqCentroNombre { get; set; }
            public string RmReqMonedaNombre { get; set; }
            public string RmCuenta { get; set; }
        }

        public List<RequisicionesPorAutorizarResult> Post(datos Datos)
        {
            try
            {
                string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

                string EmpleadoDesencripta = Seguridad.DesEncriptar(Datos.Empleado);

                DocumentoEntrada entrada = new DocumentoEntrada();
                entrada.Usuario = UsuarioDesencripta; //Datos.Usuario;  
                entrada.Origen = "Programa CGE";  //Datos.Origen; 
                entrada.Transaccion = 120760;
                entrada.Operacion = 1;
                //entrada.agregaElemento("proceso", 9);
                entrada.agregaElemento("RmReqSolicitante", Convert.ToInt32(EmpleadoDesencripta));

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                DataTable DTRequisiciones = new DataTable();

                if (respuesta.Resultado == "1")
                {
                    DTRequisiciones = respuesta.obtieneTabla("Catalogo");

                    List<RequisicionesPorAutorizarResult> lista = new List<RequisicionesPorAutorizarResult>();

                    foreach (DataRow row in DTRequisiciones.Rows)
                    {

                       //string Cuenta= ObtieneCuenta(UsuarioDesencripta, Convert.ToString(row["RmReqId"]));
                        
                        RequisicionesPorAutorizarResult ent = new RequisicionesPorAutorizarResult
                        {
                            RmReqId = Convert.ToString(row["RmReqId"]), //OK
                            RmReqEstatusNombre = Convert.ToString(row["RmReqEstatusNombre"]),
                            RmReqJustificacion = Convert.ToString(row["RmReqJustificacion"]),
                            RmReqOficinaNombre = Convert.ToString(row["RmReqOficinaNombre"]),
                            RmReqSolicitanteNombre = Convert.ToString(row["RmReqSolicitanteNombre"]),
                            RmReqTotal = Convert.ToString(row["RmReqTotal"]),
                            RmReqEstatus = Convert.ToString(row["RmReqEstatus"]),
                            RmReqTipoGastoNombre = Convert.ToString(row["RmReqTipoGastoNombre"]),
                            RmReqProveedorNombre = Convert.ToString(row["RmReqProveedorNombre"]),
                            RmReqCentroNombre = Convert.ToString(row["RmReqCentroNombre"]),
                            RmReqTipoRequisicion = Convert.ToString(row["RmReqTipoRequisicion"]),
                            RmReqSubramo = Convert.ToString(row["RmReqSubramo"]),
                            RmReqMonedaNombre = Convert.ToString(row["RmReqMonedaNombre"])
                            
                        };
                        lista.Add(ent);
                    }

                    return lista;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                List<RequisicionesPorAutorizarResult> lista = new List<RequisicionesPorAutorizarResult>();

                RequisicionesPorAutorizarResult ent = new RequisicionesPorAutorizarResult
                {
                    RmReqId = Convert.ToString("0"),
                    RmReqEstatusNombre = Convert.ToString("0"),
                    RmReqJustificacion = Convert.ToString("0"),
                    RmReqOficinaNombre = Convert.ToString(ex.ToString()),
                    RmReqSolicitanteNombre = Convert.ToString("Error Ex"),
                    RmReqTotal = Convert.ToString("0"),
                    RmReqEstatus = Convert.ToString("0"),
                    RmReqTipoGastoNombre = Convert.ToString("0"),
                    RmReqProveedorNombre = Convert.ToString("0"),
                    RmReqCentroNombre = Convert.ToString("0"),
                    RmReqTipoRequisicion = Convert.ToString("0"),
                    RmReqSubramo = Convert.ToString("Error Ex"),
                    RmCuenta =""
                };
                lista.Add(ent);

                return lista;

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
                        if ((Convert.ToDouble(row["RmRdeSubtotal"]) + Convert.ToDouble(row["RmRdeIva"])) > MontoActual){

                            Cuenta = Convert.ToString(row["RmRdeCuentaNombre"]);

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
            catch (Exception)
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

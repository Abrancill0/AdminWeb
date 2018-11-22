using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;

namespace SCGESP.Controllers
{
    public class FlujoProcesoSolicitudController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
        }

        public class ObtieneParametrosSalida
        {
            public string Proceso { get; set; }
            public string IdResponsable { get; set; }
            public string Responsable { get; set; }
            public string Terminado { get; set; }
            public string Usuario { get; set; }
            public string Importe { get; set; } 
            public string Alterno { get; set; }
            public string RmReqId { get; set; }
            public string RmReqEstatusNombre { get; set; }
            public string RmReqTipoRequisicion { get; set; }
            public string RmReqTotal { get; set; }
            public string RmReqSolicitanteNombre { get; set; }
            public string RmReqSubramo { get; set; }
            public string RmReqJustificacion { get; set; }
            public string RmReqOficinaNombre { get; set; }
            public string RmReqEstatus { get; set; }
            public string RmReqMonedaNombre { get; set; }
            public string RmReqTipoGastoNombre { get; set; }
            public string RmReqProveedorNombre { get; set; }
            public string RmReqCentroNombre { get; set; }
            public string RmReqEmpleadoObligado { get; set; }
            public string RmReqEmpleadoObligadoNombre { get; set; }

        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120402,
                Operacion = 17,
            };
            
            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            DataTable DTLista = new DataTable();


            try
            {

                if (respuesta.Resultado == "1")
            {
                DTLista = respuesta.obtieneTabla("Catalogo");

                int NumOCVobo = DTLista.Rows.Count;

                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                foreach (DataRow row in DTLista.Rows)
                {
                    ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                    {
                        Proceso =Convert.ToString(row["Proceso"]),
                        IdResponsable=Convert.ToString(row["IdResponsable"]),
                        Responsable = Convert.ToString(row["Responsable"]),
                        Terminado =Convert.ToString(row["Terminado"]),
                        Usuario = Convert.ToString(row["Usuario"]),
                        Importe = Convert.ToString(row["Importe"]),
                        Alterno = Convert.ToString(row["Alterno"]),
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
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                {
                    RmReqJustificacion = Convert.ToString("no encontro ningun registro"),
                    
                };
                lista.Add(ent);
                
                return lista;
            }

            }
            catch (Exception ex)
            {
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                {
                    RmReqJustificacion = ex.ToString()

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

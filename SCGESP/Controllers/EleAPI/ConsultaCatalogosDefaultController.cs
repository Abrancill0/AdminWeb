using SCGESP.Clases;
using Ele.Generales;
using System.Xml;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System;

namespace SCGESP.Controllers.EleAPI
{
    public class ConsultaCatalogosDefaultController : ApiController
    {
        public class datos
        {
            public string GrEmpID { get; set; }
        }

        public class ConsultaCatalogosResult
        {
            public int GrEmpCentro { get; set; }
            public int GrEmpOficina { get; set; }
            public int GrEmpTipoGasto { get; set; }
        }

        public List<ConsultaCatalogosResult> Post(datos Datos)
        {
            DataTable DT = new DataTable();
            SqlDataAdapter DA; // = new SqlDataAdapter();

            string Consulta = "select GrEmpCentro,GrEmpOficina,GrEmpTipoGasto from GrEmpleado  where GrEmpID = " + Datos.GrEmpID;

            SqlConnection Conexion = new SqlConnection();

            Conexion.ConnectionString = "";// VariablesGlobales.CadenaConexionEle;

            DA = new SqlDataAdapter(Consulta, Conexion);

            DA.Fill(DT);


            List<ConsultaCatalogosResult> lista = new List<ConsultaCatalogosResult>();

            if (DT.Rows.Count > 0)
            {
                // DataRow row = DT.Rows[0];
                foreach (DataRow row in DT.Rows)
                {
                    ConsultaCatalogosResult ent = new ConsultaCatalogosResult
                    {
                        GrEmpCentro = Convert.ToInt32(row["GrEmpCentro"]),
                        GrEmpOficina = Convert.ToInt32(row["GrEmpOficina"]),
                        GrEmpTipoGasto = Convert.ToInt32(row["GrEmpTipoGasto"])
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

        public static DocumentoSalida PeticionCatalogo(XmlDocument doc)
        {
            Localhost.Elegrp ws = new Localhost.Elegrp();
            ws.Timeout = -1;
            string respuesta = ws.PeticionCatalogo(doc);
            return new DocumentoSalida(respuesta);
        }


    }
}


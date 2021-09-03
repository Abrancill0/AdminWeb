using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class ConfrontarInformeController : ApiController
    {
        public class ListResult
        {
            public bool ConfrontacionOk { get; set; }
            public string Descripcion { get; set; }
        }
        public class Parametros
        {
            public int IdInforme { get; set; }
            public double ImporteRequisicion { get; set; }
            public double ImporteMovBanco { get; set; }
            public double ImporteGastado { get; set; }
            public double ImporteFondeo { get; set; }
        }
        public ListResult Post(Parametros Datos)
        {
            SqlDataAdapter DA;
            DataTable DT = new DataTable();

            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };
            string consulta = "UPDATE informe " +
                              "SET i_conciliacionbancos = 1 " +
                              "WHERE i_id = " + Datos.IdInforme + "; " +
                              "UPDATE gastos " +
                              "SET g_conciliacionbancos = IIF(ISNULL(g_idmovbanco, 0) > 0, 1, 0) " +
                              "WHERE g_idinforme = " + Datos.IdInforme + "; " +
                              "SELECT TOP(1) *, " + 
                              " (SElECT TOP(1) i_uresponsable FROM informe WHERE i_id=" + Datos.IdInforme + ") AS usuario, " +
                              " (SElECT TOP(1) i_ninforme FROM informe WHERE i_id=" + Datos.IdInforme + ") AS ninforme, " +
                              " (SElECT TOP(1) r_idrequisicion FROM informe WHERE i_id=" + Datos.IdInforme + ") AS idrequisicion " +
                              "FROM AutorizaOpcional WHERE administrador = 1;";
            try
            {
                DA = new SqlDataAdapter(consulta, Conexion);
                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow row in DT.Rows)
                    {
                        string UsuarioSolicita = Convert.ToString(row["usuario"]);
                        string UsuarioId = Convert.ToString(row["uautoriza"]);
                        string EmpleadoId = Convert.ToString(row["idempleado"]);
                        int ninforme = Convert.ToInt32(row["ninforme"]);
                        int idrequisicion = Convert.ToInt32(row["idrequisicion"]);

                        string mensaje = "Confrontación Generada de la Requisición de Viaje (Informe) #" + ninforme + " Requisición " + idrequisicion + ". \n" +
                            " Importe confrontado: $ " + Datos.ImporteMovBanco + "\n" +
                            " Importe requisición: $ " + Datos.ImporteRequisicion + "\n" +
                            " Importe gastado: $ " + Datos.ImporteGastado + "\n " +
                            " Importe a retirar: $ " + Datos.ImporteFondeo + " (solo en caso necesario). \n";

                        EnvioCorreosELE.Envio(UsuarioSolicita, "", EmpleadoId, UsuarioId, "",
                            "Confrontación Generada de Requisición de Viaje (Informe) #" + ninforme + " Requisición " + idrequisicion + ".",
                            mensaje, 0);
                        
                    }
                }

                ListResult resultado = new ListResult
                {
                    ConfrontacionOk = true,
                    Descripcion = "Informe confrontado."
                };
                return resultado;
            }
            catch (Exception err)
            {
                var error = Convert.ToString(err);
                ListResult resultado = new ListResult
                {
                    ConfrontacionOk = false,
                    Descripcion = "Error al confrontar informe. " + error
                };
                return resultado;
            }
        }
    }
}

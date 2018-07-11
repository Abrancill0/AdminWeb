using SCGESP.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class ResetInformeConfrontadoController : ApiController
    {
        public class ListResult
        {
            public bool ResetOk { get; set; }
            public string Descripcion { get; set; }
        }

        public class Parametros
        {
            public int IdInforme { get; set; }
        }

        public ListResult Post(Parametros Datos)
        {
            bool exito = false;
            string mensaje = "";

            try
            {
                SqlDataAdapter DA;
                DataTable DT = new DataTable();

                SqlConnection Conexion = new SqlConnection
                {
                    ConnectionString = VariablesGlobales.CadenaConexion
                };
                string consulta = "UPDATE informe SET i_conciliacionbancos = 0 WHERE i_id = " + Datos.IdInforme + "; " +
                                  "UPDATE gastos SET g_conciliacionbancos = 0, g_idmovbanco = 0 WHERE g_idinforme = " + Datos.IdInforme + "; " +
                                  "DELETE FROM movbancarios WHERE m_idinfcarga = " + Datos.IdInforme + " OR m_idinforme = " + Datos.IdInforme + ";";

                DA = new SqlDataAdapter(consulta, Conexion);
                DA.Fill(DT);

                exito = true;
                mensaje = "Informe listo para confrontar";
            }
            catch (Exception err)
            {
                exito = false;
                mensaje = "Error al reiniciar el informe para confrontar. " + err.ToString();
            }

            ListResult lista = new ListResult
            {
                ResetOk = exito,
                Descripcion = mensaje
            };

            return lista;
        }
    }
}

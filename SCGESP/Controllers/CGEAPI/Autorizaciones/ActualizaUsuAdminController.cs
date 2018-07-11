using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class ActualizaUsuAdminController : ApiController
    {
        public class ParametrosUsuarioAdmin
        {
            public string SgUsuId { get; set; }
            public string SgUsuEmpleado { get; set; }
            public string SgUsuEmpleadoNombre { get; set; }
            public int StAdministrador { get; set; }
            public int StSuplente { get; set; }
            public int ExistiaUnAdm { get; set; }
            public string SgUsuIdAdm { get; set; }
            public string SgUsuEmpleadoAdm { get; set; }
            public string SgUsuEmpleadoNombreAdm { get; set; }
            public string Origen { get; set; }
        }

        public string Post(ParametrosUsuarioAdmin Datos)
        {
            DataTable DT;
            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };
            
            string query = "";

            if (Datos.ExistiaUnAdm == 1 && Datos.StSuplente == 0 && Datos.Origen == "Administrador")
            {
                query = "DELETE FROM AutorizaOpcional WHERE idempleado = '" + Datos.SgUsuEmpleadoAdm + "'";
                EjecutarQuery(query, Conexion);

                query = "UPDATE autorizarinforme SET a_uautoriza = '" + Datos.SgUsuEmpleado + "' WHERE a_uautoriza = '" + Datos.SgUsuEmpleadoAdm + "' AND a_autorizado=0";
                EjecutarQuery(query, Conexion);
            }

            query = "SELECT ISNULL(COUNT(uautoriza), 0) existe FROM AutorizaOpcional WHERE idempleado = '" + Datos.SgUsuEmpleado + "'";
            DT = EjecutarQuery(query, Conexion);

            if (DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    int RowExiste = Convert.ToInt32(row["existe"]);

                    if (RowExiste > 0)
                    {
                        if (Datos.StAdministrador > 0 || Datos.StAdministrador > 0)
                        {
                            query = "UPDATE AutorizaOpcional SET administrador = " + Datos.StAdministrador + " WHERE idempleado = '" + Datos.SgUsuEmpleado + "'";
                            EjecutarQuery(query, Conexion);
                        }
                        else {
                            query = "DELETE FROM AutorizaOpcional WHERE idempleado = '" + Datos.SgUsuEmpleado + "'";
                            EjecutarQuery(query, Conexion);
                        }
                    }
                    else
                    {
                        if (Datos.StAdministrador == 1)
                        {
                            query = "INSERT INTO AutorizaOpcional (uautoriza,idempleado,Nombre,administrador) " +
                                    "VALUES ('" + Datos.SgUsuId + "','" + Datos.SgUsuEmpleado + "','" + Datos.SgUsuEmpleadoNombre + "','" + Datos.StAdministrador + "')";
                            EjecutarQuery(query, Conexion);
                        }
                        else if (Datos.StSuplente == 1) {
                            query = "INSERT INTO AutorizaOpcional (uautoriza,idempleado,Nombre,administrador) " +
                                    "VALUES ('" + Datos.SgUsuId + "','" + Datos.SgUsuEmpleado + "','" + Datos.SgUsuEmpleadoNombre + "','" + Datos.StAdministrador + "')";
                            EjecutarQuery(query, Conexion);
                        }
                    }
                }

            }
            return "";
        }
        public DataTable EjecutarQuery(string query, SqlConnection Conexion)
        {
            SqlDataAdapter DA;
            DataTable DT = new DataTable();
            DA = new SqlDataAdapter(query, Conexion);
            DA.Fill(DT);
            return DT;
        }
    }
}
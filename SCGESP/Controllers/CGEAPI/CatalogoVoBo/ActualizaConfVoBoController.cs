using SCGESP.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class ActualizaConfVoBoController : ApiController
    {
        public class ListResult
        {
            public bool ActualizadoOk { get; set; }
            public string Descripcion { get; set; }
        }

        public class Parametros
        {
            public int Id { get; set; }
            public string Usuario { get; set; }
            public string NombreUsuario { get; set; }
            public int ValorDefault { get; set; }
            public int ChkBloqueado { get; set; }
            public int ValidarImporte { get; set; }
            public decimal ImporteMayorQue { get; set; }
        }

        public ListResult Post(Parametros Datos)
        {
            SqlDataAdapter DA;
            DataTable DT = new DataTable();

            SqlConnection Conexion = new SqlConnection
            {
                ConnectionString = VariablesGlobales.CadenaConexion
            };
            string consulta = "UPDATE configuracion SET " +
                                "c_usuario = '" + Datos.Usuario + "', " +
                                "c_usuario_nombre = '" + Datos.NombreUsuario + "', " + 
                                "c_valor_default = " + Datos.ValorDefault + ", " +
                                "c_chk_bloqueado = " + Datos.ChkBloqueado + ", " +
                                "c_validar_importe = " + Datos.ValidarImporte + ", " +
                                "c_importe_mayor_que = " + Datos.ImporteMayorQue + " " +
                              "WHERE LOWER(c_accion) = 'vobo'; ";
            try
            {
                DA = new SqlDataAdapter(consulta, Conexion);
                DA.Fill(DT);
                ListResult lista = new ListResult
                {
                    ActualizadoOk = true,
                    Descripcion = "Configuración VoBo Actualizada."
                };
                return lista;
            }
            catch (Exception err) {
                ListResult lista = new ListResult
                {
                    ActualizadoOk = false,
                    Descripcion = "Error al actualizar. " + err.ToString()
                };
                return lista;
            }
            
            
        }

    }
}

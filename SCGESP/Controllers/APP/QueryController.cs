using SCGESP.Clases;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SCGESP.Controllers.APP
{
    public class QueryController : ApiController
    {
        public class datos
        {
            public string cadena { get; set; }
        }

        
        public string Post(datos Datos)
        {
            DataTable DT = new DataTable();
            SqlDataAdapter DA; // = new SqlDataAdapter();

            string Consulta = Datos.cadena;

            SqlConnection Conexion = new SqlConnection();

            Conexion.ConnectionString = VariablesGlobales.CadenaConexion;

            DA = new SqlDataAdapter(Consulta, Conexion);

            DA.Fill(DT);

            //Dim conexion As New SqlClient.SqlConnection()
            //conexion.ConnectionString = My.Settings.FarmacosSanLuisConnectionString

            //DA1 = New SqlClient.SqlDataAdapter(llenatablapivote, conexion)
            //DA1.Fill(DT1, "Tabla1")

            return "OK";
        }
        
    }
}

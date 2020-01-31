using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Xml;

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
			public string SgUsuEmpleado { get; set; }
		}

        public ListResult Post(Parametros Datos)
        {			
            try
            {
				string UAlterno = GetUsuarioAlterno.UsuarioAlterno(Datos.Usuario).Resultado;
				string correo = ObtieneCorreo(Datos.SgUsuEmpleado, Datos.Usuario);

				SqlCommand comando = new SqlCommand("ActualizaUsuarioVoBo1")
				{
					CommandType = CommandType.StoredProcedure
				};

				//Declaracion de parametros
				comando.Parameters.Add("@Usuario", SqlDbType.VarChar);
				comando.Parameters.Add("@NombreUsuario", SqlDbType.VarChar);
				comando.Parameters.Add("@Correo", SqlDbType.VarChar);
				comando.Parameters.Add("@ValorDefault", SqlDbType.Int);
				comando.Parameters.Add("@ChkBloqueado", SqlDbType.Int);
				comando.Parameters.Add("@ValidarImporte", SqlDbType.Int);
				comando.Parameters.Add("@ImporteMayorQue", SqlDbType.Decimal);
				comando.Parameters.Add("@UAlterno", SqlDbType.VarChar);

				comando.Parameters["@Usuario"].Value = Datos.Usuario;
				comando.Parameters["@NombreUsuario"].Value = Datos.NombreUsuario;
				comando.Parameters["@Correo"].Value = correo;
				comando.Parameters["@ValorDefault"].Value = Datos.ValorDefault;
				comando.Parameters["@ChkBloqueado"].Value = Datos.ChkBloqueado;
				comando.Parameters["@ValidarImporte"].Value = Datos.ValidarImporte;
				comando.Parameters["@ImporteMayorQue"].Value = Datos.ImporteMayorQue;
				comando.Parameters["@UAlterno"].Value = UAlterno;

				comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
				comando.CommandTimeout = 0;
				comando.Connection.Open();
				//DA.SelectCommand = comando;
				comando.ExecuteNonQuery();

				DataTable DT = new DataTable();
				SqlDataAdapter DA = new SqlDataAdapter(comando);
				comando.Connection.Close();
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

		public static string ObtieneCorreo(string Empelado, string Usuario)
		{
			string UsuarioDesencripta = Seguridad.DesEncriptar(Usuario);

			DocumentoEntrada entrada = new DocumentoEntrada
			{
				Usuario = UsuarioDesencripta,
				Origen = "AdminWEB",  //Datos.Origen; 
				Transaccion = 120037,
				Operacion = 6//verifica si existe una llave y regresa una tabla de un renglon con todos los campos de la tabla
			};
			entrada.agregaElemento("GrEmpId", Empelado);

			DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

			DataTable DTCorreo = new DataTable();
			string CorreoResult = "";

			if (respuesta.Resultado == "1")
			{
				DTCorreo = respuesta.obtieneTabla("Llave");

				for (int i = 0; i < DTCorreo.Rows.Count; i++)
				{
					CorreoResult = Convert.ToString(DTCorreo.Rows[i]["GrEmpCorreoElectronico"]);
				}

				return CorreoResult;
			}
			else
			{
				var errores = respuesta.Errores;

				return null;
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

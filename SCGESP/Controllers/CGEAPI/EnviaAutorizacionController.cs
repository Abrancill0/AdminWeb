using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ele.Generales;
using System.Xml;

namespace SCGESP.Controllers.CGEAPI
{
    public class EnviaAutorizacionController : ApiController
    {
        public class ParametrosGastos
        {
            public int idinforme { get; set; }

        }

        public string PostInsertGasto(ParametrosGastos Datos)
        {
			
			SqlCommand comando = new SqlCommand("EnviaAutorizacion");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros

            comando.Parameters.Add("@idinforme", SqlDbType.Int);
			comando.Parameters.Add("@ualterno", SqlDbType.VarChar);

			//Asignacion de valores a parametros
			comando.Parameters["@idinforme"].Value = Datos.idinforme;
			comando.Parameters["@ualterno"].Value = "";

			comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            //DA.SelectCommand = comando;
            // comando.ExecuteNonQuery();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);


            if (DT.Rows.Count > 0)
            {

                foreach (DataRow row in DT.Rows)
                {
					string usuarioResponsable = Convert.ToString(row["usuarioResponsable"]);
                    string usuarioAutoriza = Convert.ToString(row["usuarioAutoriza"]);
                    string mensaje = Convert.ToString(row["msn"]);
                    string titulo = Convert.ToString(row["titulo"]);
                    int idgasto = Convert.ToInt32(row["idgasto"]);

					//agregar altenerno del autorizador
					try
					{
						var ResultadoAlterno = GetUsuarioAlterno.UsuarioAlterno(usuarioAutoriza);
						string consulta = "";
						if (ResultadoAlterno.Estatus == 1)
						{
							consulta = "UPDATE autorizarinforme SET a_ualterno = '" + ResultadoAlterno.Resultado + "' " +
							"WHERE a_idinforme = " + Datos.idinforme + " AND a_nivel = 1 AND a_uautoriza = '" + usuarioAutoriza + "'";
						}
						else
						{
							consulta = "UPDATE autorizarinforme SET a_ualterno = '" + usuarioAutoriza + "' " +
							"WHERE a_idinforme = " + Datos.idinforme + " AND a_nivel = 1 AND a_uautoriza = '" + usuarioAutoriza + "'";
						}
						DA = new SqlDataAdapter(consulta, VariablesGlobales.CadenaConexion);
						DA.Fill(DT);
					}
					catch (Exception ex)
					{
						//ex
					}
					


					EnvioCorreosELE.Envio(usuarioResponsable, "", "", usuarioAutoriza, "", titulo, mensaje, 0);

                    try
                    {
                        DocumentoEntrada entrada = new DocumentoEntrada
                        {
                            Usuario = usuarioResponsable,
                            Origen = "AdminWeb", 
                            Transaccion = 120090,
                            Operacion = 13
                        };
                        entrada.agregaElemento("FiGasId", idgasto);
                        entrada.agregaElemento("Accion", 5);//5 = envio (validacion/aprobación)

                        DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                    }
                    catch (Exception)
                    {
                        //throw;
                    }

                }

                return "";
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

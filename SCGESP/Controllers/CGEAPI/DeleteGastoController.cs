using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.CGEAPI
{
    public class DeleteGastoController : ApiController
    {
        public class datos
        {
            public int id { get; set; }
            public int idinforme { get; set; }
        }

        public class ObtieneInformeResult
        {
            public string mensaje { get; set; }
        }

        public List<ObtieneInformeResult> PostObtieneInformes(datos Datos)
        {
            try
            {
                SqlCommand comando = new SqlCommand("DeleteGasto");
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@id", SqlDbType.Int);
                comando.Parameters.Add("@idinforme", SqlDbType.Int);

                //Asignacion de valores a parametros
                comando.Parameters["@id"].Value = Datos.id;
                comando.Parameters["@idinforme"].Value = Datos.idinforme;

                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();
                //DA.SelectCommand = comando;
                //comando.ExecuteNonQuery();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();

                DA.Fill(DT);

                //ObtieneInformeResult items;

                List<ObtieneInformeResult> lista = new List<ObtieneInformeResult>();
                
                if (DT.Rows.Count > 0)
                {
                    // DataRow row = DT.Rows[0];
                    foreach (DataRow row in DT.Rows)
                    {//
                        string UUID = Convert.ToString(row["xuuid"]);
                        string idGasto = Convert.ToString(row["idgasto"]);
                        string uResponsable = Convert.ToString(row["uresponsable"]);

						string DirXML = Convert.ToString(row["DirXML"]);
						string DirPDF = Convert.ToString(row["DirPDF"]);
						string DirIMG = Convert.ToString(row["DirIMG"]);

						if (UUID.Trim() != "")
                        {
                            try
                            {
                                DocumentoEntrada entradadoc = new DocumentoEntrada
                                {
                                    Usuario = uResponsable,//Variables.usuario;
                                    Origen = "AdminWEB",
                                    Transaccion = 120092,
                                    Operacion = 22
                                };//21:Agregar XML, 22:Eliminar XML
                                entradadoc.agregaElemento("FiGfaGasto", idGasto);
                                entradadoc.agregaElemento("FiGfaUuid", UUID);

                                DocumentoSalida respuesta = PeticionCatalogo(entradadoc.Documento);
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                        }

                        ObtieneInformeResult ent = new ObtieneInformeResult
                        {
                            mensaje = Convert.ToString(row["ELIMINADO"]),
                        };

						if (DirXML != "" || DirPDF != "" || DirIMG != "")
						{
							string path = HttpContext.Current.Server.MapPath("/");
							string consulta = "";
							if (File.Exists(path + DirXML))
							{
								File.Delete(path + DirXML);
								consulta = "UPDATE gastos SET " +
										"g_dirxml = ''" +
										"WHERE g_idinforme = " + Datos.idinforme + " AND " +
											"g_dirxml = '" + DirXML + "';";
								DA = new SqlDataAdapter(consulta, VariablesGlobales.CadenaConexion);
								DA.Fill(DT);
							}
							if (File.Exists(path + DirPDF))
							{
								File.Delete(path + DirPDF);
								consulta = "UPDATE gastos SET " +
										"g_dirpdf = ''" +
										"WHERE g_idinforme = " + Datos.idinforme + " AND " +
											"g_dirpdf = '" + DirPDF + "';";
								DA = new SqlDataAdapter(consulta, VariablesGlobales.CadenaConexion);
								DA.Fill(DT);
							}
							if (File.Exists(path + DirIMG))
							{
								File.Delete(path + DirIMG);
								consulta = "UPDATE gastos SET " +
										"g_dirotros = ''" +
										"WHERE g_idinforme = " + Datos.idinforme + " AND " +
											"g_dirotros = '" + DirIMG + "';";
								DA = new SqlDataAdapter(consulta, VariablesGlobales.CadenaConexion);
								DA.Fill(DT);
							}
						}
                        lista.Add(ent);
                    }

                    return lista;
                }
                else
                {
                   
                    ObtieneInformeResult ent = new ObtieneInformeResult
                    {
                        mensaje = "Error al eliminar el registro",
                    };

                    lista.Add(ent);

                    return lista;
                }
            }
            catch (Exception ex)
            {

                List<ObtieneInformeResult> lista = new List<ObtieneInformeResult>();

                ObtieneInformeResult ent = new ObtieneInformeResult
                {
                    mensaje = ex.ToString(),
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

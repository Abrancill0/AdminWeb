using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers
{
    public class BrowseInformeController : ApiController
    {
        public class Parametros1Informes
        {
            public int estatus { get; set; }
            public string uresponsable { get; set; }
            public int idempresa { get; set; }
            public string uconsulta { get; set; }
            public string empleadoactivo { get; set; }
        }

        public class ObtieneInformeResult
        {
            public int i_id { get; set; }
            public int i_idproyecto { get; set; }
            public int i_ninforme { get; set; }
            public string i_nmb { get; set; }
            public int i_estatus { get; set; }
            public string e_estatus { get; set; }
            public string i_fcrea { get; set; }
            public string i_uresponsable { get; set; }
            public string responsable { get; set; }
            public string i_finicio { get; set; }
            public string i_ffin { get; set; }
            public double i_total { get; set; }
            public double i_totalg { get; set; }
            public int i_idempresa { get; set; }
            public string usuconsulta { get; set; }
            public int r_idrequisicion { get; set; }

            public string anticipo { get; set; }
            public string disponible { get; set; }
            
            public string p_nmb { get; set; }
            public string i_motivo { get; set; }
            public string i_notas { get; set; }
            public string proycontable { get; set; }
            public string i_tipo { get; set; }
            public string i_mescontable { get; set; }
            public string i_tarjetatoka { get; set; }
            public double MontoRequisicion { get; set; }
            public int rechazado { get; set; }
        }

        public List<ObtieneInformeResult> PostObtieneInformes(Parametros1Informes Datos)
        {

            //traer todas las requisiciones del responsable que se encuentren en status 51
            //traerme todos los informes del responsable
            //recorrer los informes en base a las requisisciones
            //De las requisiciones sobrantes generar un informe por cada una de ellas
            //Ejecutar el BrowseInforme
            
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.uresponsable);
            string EmpleadoDesencripta = Seguridad.DesEncriptar(Datos.empleadoactivo);

            DocumentoEntrada entrada = new DocumentoEntrada();
            entrada.Usuario = UsuarioDesencripta; //Datos.Usuario;  
            entrada.Origen = "AdminWEB";  //Datos.Origen; 
            entrada.Transaccion = 120760;
            entrada.Operacion = 1;

            entrada.agregaElemento("RmReqSolicitante", Convert.ToInt32(EmpleadoDesencripta));
            entrada.agregaElemento("proceso", 9);

            //entrada.agregaElemento("RmReqTipoRequisicion", Convert.ToInt32(99));
            //entrada.agregaElemento("RmReqEstatus", Convert.ToInt32(51));


            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);
            
            try
            {

                DataTable DTRequisiciones = new DataTable();
                DataTable DTUsuarios = new DataTable();


                if (respuesta.Resultado == "1")
                {
                    
                    DTRequisiciones = respuesta.obtieneTabla("Catalogo");
                    
                }


                DocumentoEntrada entradaUsuarios = new DocumentoEntrada
                {
                    Usuario = UsuarioDesencripta,
                    Origen = "Programa CGE",  //Datos.Origen; 
                    Transaccion = 100004,
                    Operacion = 1//regresa una tabla con todos los campos de la tabla ( La cantidad de registros depende del filtro enviado)
                };

                string usuSolicitante = "";
                DocumentoSalida respuestaUsuarios = PeticionCatalogo(entradaUsuarios.Documento);
                if (respuestaUsuarios.Resultado == "1")
                {
                    DTUsuarios = respuestaUsuarios.obtieneTabla("Catalogo");

                }

                

                    SqlCommand comando = new SqlCommand("BrowseInforme");
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@estatus", SqlDbType.Int);
                comando.Parameters.Add("@uresponsable", SqlDbType.VarChar);
                comando.Parameters.Add("@uconsulta", SqlDbType.VarChar);
                //comando.Parameters.Add("@idempresa", SqlDbType.Int);

                //Asignacion de valores a parametros
                comando.Parameters["@estatus"].Value = Datos.estatus;
                comando.Parameters["@uresponsable"].Value = UsuarioDesencripta;
                comando.Parameters["@uconsulta"].Value = UsuarioDesencripta;

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
                int CreaInforme = 0;
                int Req = 0;
                string usu = "";
                string RmReqSolicitante = "";
                string motivo = "";
                string TarjetaToka = "";
                string NombreEmpleado = "";
                DateTime FechaRequrida;
                DateTime FechaFinal;
                double MontoRequisicion = 0;
                int Gastorequisicion = 0;

                //if (DT.Rows.Count > 0)
                //{


                for (int i = 0; i < DTRequisiciones.Rows.Count; i++)
                {
					CreaInforme = 0;

					foreach (DataRow row in DT.Rows)
                    {
						int requisicionInf = Convert.ToInt32(row["r_idrequisicion"]);
						int idRequisicion = Convert.ToInt32(DTRequisiciones.Rows[i]["RmReqId"]);

						if (requisicionInf == idRequisicion)
						{
							CreaInforme = 1;
						}


                    }

                    if (CreaInforme == 0)
                    {
                        Req = Convert.ToInt32(DTRequisiciones.Rows[i]["RmReqId"]);
                        NombreEmpleado = Convert.ToString(DTRequisiciones.Rows[i]["RmReqSolicitanteNombre"]);
                        motivo = Convert.ToString(DTRequisiciones.Rows[i]["RmReqJustificacion"]);
                        usu = Convert.ToString(DTRequisiciones.Rows[i]["RmReqUsuarioAlta"]);
                        RmReqSolicitante = Convert.ToString(DTRequisiciones.Rows[i]["RmReqSolicitante"]);//UsuarioDesencripta;//Convert.ToString(DTRequisiciones.Rows[i]["RmReqSolicitante"]);

                        //if (EmpleadoDesencripta != RmReqSolicitante) {

                            for (int ii = 0; ii < DTUsuarios.Rows.Count; ii++)
                            {
                            string SgUsuEmpleado = Convert.ToString(DTUsuarios.Rows[ii]["SgUsuEmpleado"]);
                                if (SgUsuEmpleado == RmReqSolicitante) {
                                    usu = Convert.ToString(DTUsuarios.Rows[ii]["SgUsuId"]);
                                }
                            }
                        //}
                        

                        MontoRequisicion = Convert.ToDouble(DTRequisiciones.Rows[i]["RmReqTotal"]);
                        Gastorequisicion = Convert.ToInt32(DTRequisiciones.Rows[i]["RmReqGasto"]);
                        TarjetaToka = Convert.ToString(DTRequisiciones.Rows[i]["RmReqTarjetaToka"]);
						try{
							FechaRequrida = Convert.ToDateTime(DTRequisiciones.Rows[i]["RmReqFechaRequerida"]);
						}catch (Exception err) {
							var error = err.ToString();
							try{
								string fini = DTRequisiciones.Rows[i]["RmReqFechaRequerida"].ToString();
								fini = fini.Substring(0, 10);
								FechaRequrida = Convert.ToDateTime(fini);
							}catch (Exception err2){
								error = err2.ToString();
								FechaRequrida = DateTime.Today;
							}
						}
						try{
							FechaFinal = Convert.ToDateTime(DTRequisiciones.Rows[i]["RmReqFechaFinal"]);
						}catch (Exception err){
							var error = err.ToString();
							try{
								string ffin = DTRequisiciones.Rows[i]["RmReqFechaFinal"].ToString();
								ffin = ffin.Substring(0, 10);
								FechaFinal = Convert.ToDateTime(ffin);
							}catch (Exception err2){
								error = err2.ToString();
								FechaFinal = DateTime.Today;
							}
						}
						if (Convert.ToInt32(DTRequisiciones.Rows[i]["RmReqEstatus"]) == 51 || Convert.ToInt32(DTRequisiciones.Rows[i]["RmReqEstatus"]) == 52)
                        {
                            InsertaInforme(usu, "", motivo, 1, Req, NombreEmpleado, MontoRequisicion, Gastorequisicion, TarjetaToka, FechaRequrida, FechaFinal);
                        }

                    }

                }
                return ObtieneInformesActuales(Datos.estatus, UsuarioDesencripta);

                //}
                //else
                //{
                //    return null;
                //}
            }
            catch (Exception err)
            {
                List<ObtieneInformeResult> lista = new List<ObtieneInformeResult>();
                ObtieneInformeResult ent = new ObtieneInformeResult
                {
                    i_motivo = err.ToString()
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

        public List<ObtieneInformeResult> ObtieneInformesActuales(int status,string usuario)
        {
            SqlCommand comando = new SqlCommand("BrowseInforme");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@estatus", SqlDbType.Int);
            comando.Parameters.Add("@uresponsable", SqlDbType.VarChar);
            comando.Parameters.Add("@uconsulta", SqlDbType.VarChar);
            //comando.Parameters.Add("@idempresa", SqlDbType.Int);

            //Asignacion de valores a parametros
            comando.Parameters["@estatus"].Value = status;
            comando.Parameters["@uresponsable"].Value = usuario;
            comando.Parameters["@uconsulta"].Value = usuario;

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
                string FechaInicio = "";
                string FechaFin = "";

                // DataRow row = DT.Rows[0];
                foreach (DataRow row in DT.Rows)
                {
                    if (row["i_finicio"] != null && Convert.ToString(row["i_finicio"]) != "")
                    {
                        FechaInicio = Convert.ToDateTime(row["i_finicio"]).ToString("dd/MM/yyyy");//.ToShortDateString();
					}
                    else
                    {
                        FechaInicio = "";
                    }

                    if (row["i_ffin"] != null && Convert.ToString(row["i_ffin"]) != "")
                    {
                        FechaFin = Convert.ToDateTime(row["i_ffin"]).ToString("dd/MM/yyyy");//.ToShortDateString();
					}
                    else
                    {
                        FechaFin = "";
                    }


                    ObtieneInformeResult ent = new ObtieneInformeResult
                    {
                        i_id = Convert.ToInt32(row["i_id"]),
                        i_ninforme = Convert.ToInt32(row["i_ninforme"]),
                        i_nmb = Convert.ToString(row["i_nmb"]),
                        i_estatus = Convert.ToInt32(row["i_estatus"]),
                        e_estatus = Convert.ToString(row["e_estatus"]),
                        i_fcrea = Convert.ToString(row["i_fcrea"]),
                        i_uresponsable = Convert.ToString(row["i_uresponsable"]),
                        responsable = Convert.ToString(row["responsable"]),
                        i_finicio = Convert.ToString(FechaInicio),
                        i_ffin = Convert.ToString(FechaFin),
                        i_total = Convert.ToDouble(row["i_total"]),
                        i_totalg = Convert.ToDouble(row["i_totalg"]),
                        r_idrequisicion = Convert.ToInt32(row["r_idrequisicion"]),
                        usuconsulta = Convert.ToString(row["usuconsulta"]),
                        i_motivo = Convert.ToString(row["i_motivo"]),
                        i_notas = Convert.ToString(row["i_notas"]),
                        i_tipo = Convert.ToString(row["i_tipo"]),
                        i_tarjetatoka = Convert.ToString(row["i_tarjetatoka"]),
                        MontoRequisicion = Convert.ToDouble(row["r_montorequisicion"]),
                        rechazado = Convert.ToInt32(row["i_rechazado"] ?? 0)
                    };

                    lista.Add(ent);
                }

                return lista;

            }

            return null;
            }

        public static string InsertaInforme(string usuario,string Motivo,string viaje,int tipo,int idreq,string nombre,double monto,int RmReqGasto,string Tarjetatoka, DateTime FechaRequrida, DateTime FechaFinal)
        {

            //SqlConnection Conexion = new SqlConnection
            //{
            //    ConnectionString = VariablesGlobales.CadenaConexion
            //};

            SqlCommand comando = new SqlCommand("InsertInforme")
            {
                CommandType = CommandType.StoredProcedure
            };

            //Declaracion de parametros
            comando.Parameters.Add("@uresponsable", SqlDbType.VarChar);
            comando.Parameters.Add("@motivo", SqlDbType.VarChar, 500);
            comando.Parameters.Add("@viaje", SqlDbType.VarChar, 500);
            comando.Parameters.Add("@tipo", SqlDbType.Int);
            comando.Parameters.Add("@idempresa", SqlDbType.Int);
            comando.Parameters.Add("@IDRequisiciones", SqlDbType.Int);
            comando.Parameters.Add("@NombreResponsable", SqlDbType.VarChar);
            comando.Parameters.Add("@MontoRequisicion", SqlDbType.Float);
            comando.Parameters.Add("@RmReqGasto", SqlDbType.Int);
            comando.Parameters.Add("@TarjetaToka", SqlDbType.VarChar);

            comando.Parameters.Add("@fini", SqlDbType.Date);
            comando.Parameters.Add("@ffin", SqlDbType.Date);

            //comando.Parameters.Add("@idempresa", SqlDbType.Int);

            //string dayIni = FechaRequrida.Substring(0, 2);
            //string monthIni = FechaRequrida.Substring(3, 2);
            //string yearIni = FechaRequrida.Substring(6, 4);

            //string dayFin = FechaFinal.Substring(0, 2);
            //string monthFin = FechaFinal.Substring(3, 2);
            //string yearFin = FechaFinal.Substring(6, 4);

            //DateTime FechaI, FechaF;
            
            //try
            //{
            //    FechaI = Convert.ToDateTime(yearIni + "-" + monthIni + "-" + dayIni);
            //}
            //catch (Exception)
            //{
            //    FechaI = Convert.ToDateTime(dayIni + "-" + monthIni + "-" + yearIni);
            //}
            //try
            //{
            //    FechaF = Convert.ToDateTime(yearFin + "-" + monthFin + "-" + dayFin);
            //}
            //catch (Exception)
            //{
            //    FechaF = Convert.ToDateTime(dayFin + "-" + monthFin + "-" + yearFin);
            //}

            //Asignacion de valores a parametros
            comando.Parameters["@uresponsable"].Value = usuario;
            comando.Parameters["@motivo"].Value = viaje;
            comando.Parameters["@viaje"].Value = viaje;
            comando.Parameters["@tipo"].Value = tipo;
            comando.Parameters["@idempresa"].Value = 0;
            comando.Parameters["@IDRequisiciones"].Value = idreq;
            comando.Parameters["@NombreResponsable"].Value = nombre;
            comando.Parameters["@MontoRequisicion"].Value = monto;
            comando.Parameters["@RmReqGasto"].Value = RmReqGasto;
            comando.Parameters["@TarjetaToka"].Value = Tarjetatoka ?? "";

			int Anioi = FechaRequrida.Year;
			int diai = FechaRequrida.Day;
			int mesi = FechaRequrida.Month;

			int Aniof = FechaFinal.Year;
			int diaf = FechaFinal.Day;
			int mesf = FechaFinal.Month;

			comando.Parameters["@fini"].Value = FechaRequrida;

            if (FechaFinal != null)
            {
                comando.Parameters["@ffin"].Value = FechaFinal;
            }
            else
            {
                comando.Parameters["@ffin"].Value = FechaRequrida;
			}


            try
            {
                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();
                //DA.SelectCommand = comando;
                //comando.ExecuteNonQuery();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);
            }
            catch (Exception ex) {
				var error = ex.ToString();
                return error;
            }

                    return "";
        }
    }
}

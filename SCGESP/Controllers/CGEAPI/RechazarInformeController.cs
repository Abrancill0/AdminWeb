using SCGESP.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using Ele.Generales;
using System.Xml;

namespace SCGESP.Controllers.CGEAPI
{
    public class RechazarInformeController : ApiController
    {

        public class datos
        {
            public int idinforme { get; set; }
            public string comentarioaut { get; set; }
            public string usuario { get; set; }
        }


        public string Post(datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.usuario);

            SqlCommand comando = new SqlCommand("RegresarInforme");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@idinforme", SqlDbType.Int);
            comando.Parameters.Add("@comentarioaut", SqlDbType.VarChar);
            comando.Parameters.Add("@usuario", SqlDbType.VarChar);
            

            //Asignacion de valores a parametros
            comando.Parameters["@idinforme"].Value = Datos.idinforme;
            comando.Parameters["@comentarioaut"].Value = Datos.comentarioaut;
            comando.Parameters["@usuario"].Value = UsuarioDesencripta;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            //DA.SelectCommand = comando;

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);

            //ObtieneInformeResult items;
            
            if (DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    string mensaje = Convert.ToString(row["msn"]);
                    string titulo = Convert.ToString(row["titulo"]);
                    string usuarioResponsable = Convert.ToString(row["usuarioResponsable"]);
                    string autorizador = Convert.ToString(row["autorizador"]);
                    int idgasto = Convert.ToInt32(row["idgasto"]);
                    int estatus = Convert.ToInt32(row["estatus"]);

                    try
                    {
                        if (estatus == 2) {
                            DocumentoEntrada entrada = new DocumentoEntrada
                            {
                                Usuario = usuarioResponsable,
                                Origen = "AdminWeb",
                                Transaccion = 120090,
                                Operacion = 13
                            };
                            entrada.agregaElemento("FiGasId", idgasto);
                            entrada.agregaElemento("Accion", 6);//6 = rechazo

                            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);
                        }                        
                    }
                    catch (Exception)
                    {

                        //throw;
                    }

                    if (autorizador != "")
                    {
                        EnvioCorreosELE.Envio(UsuarioDesencripta, "", "", autorizador, "", titulo, mensaje, 0);
                    }
                    else {
                        EnvioCorreosELE.Envio(UsuarioDesencripta, "", "", usuarioResponsable, "", titulo, mensaje, 0);
                    }
                    

                }
            }

            return "";
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

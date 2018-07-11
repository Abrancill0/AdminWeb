using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class UpdateGastoPDFOtrosController : ApiController
    {
        public class ParametrosGastos
        {
            public int id { get; set; }
            public int idinforme { get; set; }
            public string dir { get; set; }
            public string Valida { get; set; }
        }

        public class Resultado
        {
            public string UrlWeb { get; set; }
            public string UrlDisco { get; set; }
        }

        public Resultado Post(ParametrosGastos Datos)
        {
            try
            {
                Resultado Ruta;
            string ruta2 = HttpContext.Current.Server.MapPath("/");
                string ruta3 = System.Web.Hosting.HostingEnvironment.MapPath("/");
                if (Datos.Valida == "1")
                {
                    Ruta = PostSave(Datos.dir);
                }
                else
                {
                    Ruta = PostSaveImage(Datos.dir);
                }


                SqlCommand comando = new SqlCommand("UpdateGastoPDFOtros");
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@id", SqlDbType.Int);
                comando.Parameters.Add("@idinforme", SqlDbType.Int);
                comando.Parameters.Add("@dir", SqlDbType.VarChar);
                comando.Parameters.Add("@Valida", SqlDbType.VarChar);

                //Asignacion de valores a parametros
                comando.Parameters["@id"].Value = Datos.id;
                comando.Parameters["@idinforme"].Value = Datos.idinforme;
                comando.Parameters["@dir"].Value = Ruta.UrlWeb;
                comando.Parameters["@Valida"].Value = Datos.Valida;

                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();
                //DA.SelectCommand = comando;
                comando.ExecuteNonQuery();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);


                if (DT.Rows.Count > 0)
                {
                    DataRow row = DT.Rows[0];

                    return Ruta;
                }
                else
                {
                    return Ruta;
                }
                
            }
            catch (Exception)
            {

                return null;
            }

        }

        public Resultado PostSave(string Based64BinaryString)
        {
            string result = "";
            try
            {
                string format = "";
                string path = HttpContext.Current.Server.MapPath("/PDF/");
                string name = DateTime.Now.ToString("yyyyMMddhhmmss");


                if (Based64BinaryString.Contains("data:application/pdf;base64,"))
                {
                    format = "pdf";
                }
                
                string str = Based64BinaryString.Replace("data:application/pdf;base64,", " ");//jpg check

                byte[] data = Convert.FromBase64String(str);

                MemoryStream ms = new MemoryStream(data, 0, data.Length);
                ms.Write(data, 0, data.Length);
                //System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                string rutacompleta = path +  name + "." + format;
                File.WriteAllBytes(rutacompleta, data);
                //image.Save(rutacompleta);
                result = "PDF/" + name + "." + format;

                Resultado rutas = new Resultado
                {
                    UrlDisco = rutacompleta,
                    UrlWeb = result
                };
                return rutas;
            }
            catch (Exception ex)
            {
                result = "Error : " + ex;
                Resultado rutas = new Resultado
                {
                    UrlDisco = result,
                    UrlWeb = result
                };
                return rutas;
            }
            
        }

        public Resultado PostSaveImage(string Based64BinaryString)
        {
            string result = "";
            try
            {
                string format = "";
                string path = HttpContext.Current.Server.MapPath("/Comprobantes/");
                string name = DateTime.Now.ToString("yyyyMMddhhmmss");


                if (Based64BinaryString.Contains("data:image/jpeg;base64,"))
                {
                    format = "jpg";
                }
                if (Based64BinaryString.Contains("data:image/png;base64,"))
                {
                    format = "png";
                }


                string str = Based64BinaryString.Replace("data:image/jpeg;base64,", " ");//jpg check
                str = str.Replace("data:image/png;base64,", " ");//png check
                str = str.Replace("data:text/plain;base64,", " ");//text file check
                str = str.Replace("data:;base64,", " ");//zip file check
                str = str.Replace("data:application/zip;base64,", " ");//zip file check

                byte[] data = Convert.FromBase64String(str);

                MemoryStream ms = new MemoryStream(data, 0, data.Length);
                ms.Write(data, 0, data.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                string rutacompleta = path + "Image" + name + "." + format;
                image.Save(rutacompleta);

                

                result = "Comprobantes/" + "Image" + name + "." + format;
                Resultado rutas = new Resultado
                {
                    UrlDisco = rutacompleta,
                    UrlWeb = result
                };
                return rutas;

            }
            catch (Exception ex)
            {
                result = "Error : " + ex;
                Resultado rutas = new Resultado
                {
                    UrlDisco = result,
                    UrlWeb = result
                };
                return rutas;
            }
        }


    }
}

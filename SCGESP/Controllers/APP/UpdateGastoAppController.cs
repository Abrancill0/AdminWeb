using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class UpdateGastoAppController : ApiController
    {
        public class ParametrosGastos
        {
            public int id { get; set; }
            public int idinforme { get; set; }
            public string fgasto { get; set; }
            public string hgasto { get; set; }
            public string formapago { get; set; }
            public int categoria { get; set; }
            public double total { get; set; }
            public string dirotros { get; set; }
            public string rfc { get; set; }
            public string contacto { get; set; }
            public string telefono { get; set; }
            public string correo { get; set; }
            public string nombreCategoria { get; set; }
            public double ivaCategoria { get; set; }
            public string observaciones { get; set; }
            public int Convierte { get; set; }
            public int ncomensales { get; set; }
            public string nmbcomensales { get; set; }
            public double importecomprobar { get; set; }
            public double importenodeducible { get; set; }
            public double importereembolsable { get; set; }
            public double importenoreembolsable { get; set; }
            public double importenoaceptable { get; set; }

        }

        public class ObtieneGastoResult
        {
            public string ACTUALIZADO { get; set; }
            public int id { get; set; }
            public int idinforme { get; set; }
            public string Ruta { get; set; }
        }

        public List<ObtieneGastoResult> PostInsertGasto(ParametrosGastos Datos)
        {
            try
            {
                //string dir = "";

                //if  (Datos.Convierte == 1)
                //    {
                //    try
                //    {
                //        if (Datos.dirotros != "" && Datos.dirotros != null)
                //        {
                //            dir = PostSaveImage(Datos.dirotros);
                //        }
                //        else
                //        {
                //            dir = "";
                //        }

                //    }
                //    catch (Exception ex)
                //    {

                //        dir = ex.ToString();
                //    }

                //}
                //else
                //{
                //    dir = Datos.dirotros;
                //}
                
         
                SqlCommand comando = new SqlCommand("UpdateGastoApp");
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@id", SqlDbType.Int);
                comando.Parameters.Add("@idinforme", SqlDbType.Int);
                comando.Parameters.Add("@fgasto", SqlDbType.Date);
                comando.Parameters.Add("@hgasto", SqlDbType.VarChar);
                comando.Parameters.Add("@formapago", SqlDbType.VarChar);
                comando.Parameters.Add("@categoria", SqlDbType.Int);
                comando.Parameters.Add("@total", SqlDbType.Float);
                comando.Parameters.Add("@observaciones", SqlDbType.VarChar);
                comando.Parameters.Add("@nombreCategoria", SqlDbType.VarChar);
                comando.Parameters.Add("@ivaCategoria", SqlDbType.Float);
                comando.Parameters.Add("@dirotros", SqlDbType.VarChar);
                comando.Parameters.Add("@rfc", SqlDbType.VarChar);
                comando.Parameters.Add("@contacto", SqlDbType.VarChar);
                comando.Parameters.Add("@telefono", SqlDbType.VarChar);
                comando.Parameters.Add("@correo", SqlDbType.VarChar);
                comando.Parameters.Add("@ncomensales", SqlDbType.Int);
                comando.Parameters.Add("@nmbcomensales", SqlDbType.VarChar);
                comando.Parameters.Add("@deducible", SqlDbType.Int);
                comando.Parameters.Add("@importenodeducible", SqlDbType.Float);
                comando.Parameters.Add("@importereembolsable", SqlDbType.Float);
                comando.Parameters.Add("@importenoreembolsable", SqlDbType.Float);
                comando.Parameters.Add("@importenoaceptable", SqlDbType.Float);

                //Asignacion de valores a parametros
                comando.Parameters["@id"].Value = Datos.id;
                comando.Parameters["@idinforme"].Value = Datos.idinforme;
                comando.Parameters["@fgasto"].Value = Convert.ToDateTime(Datos.fgasto);

                string hora = "";
                if (Datos.hgasto != null)
                {
                    hora = Datos.hgasto;
                }
                comando.Parameters["@hgasto"].Value = hora;
               
                comando.Parameters["@formapago"].Value = Datos.formapago;
                comando.Parameters["@categoria"].Value = Datos.categoria;
                comando.Parameters["@total"].Value = Datos.total;
                string Obs = "";
                if (Datos.observaciones != null)
                {
                    Obs = Datos.observaciones;
                }
                comando.Parameters["@observaciones"].Value = Obs;
                comando.Parameters["@nombreCategoria"].Value = Datos.nombreCategoria;
                comando.Parameters["@ivaCategoria"].Value = Datos.ivaCategoria;
                comando.Parameters["@dirotros"].Value = Datos.dirotros != null ? Datos.dirotros : "";
                comando.Parameters["@rfc"].Value = Datos.rfc != null ? Datos.rfc : ""; 
                comando.Parameters["@contacto"].Value = Datos.contacto != null ? Datos.contacto : ""; 
                comando.Parameters["@telefono"].Value = Datos.telefono != null ? Datos.telefono : ""; 
                comando.Parameters["@correo"].Value = Datos.correo != null ? Datos.correo : "";
               
              
                comando.Parameters["@ncomensales"].Value = Datos.ncomensales;
                comando.Parameters["@nmbcomensales"].Value = Datos.nmbcomensales != null ? Datos.nmbcomensales : "";
                comando.Parameters["@deducible"].Value = Datos.importenodeducible == 0 ? 1 : 0;
                comando.Parameters["@importenodeducible"].Value = Datos.importenodeducible;
                comando.Parameters["@importereembolsable"].Value = Datos.importereembolsable;
                comando.Parameters["@importenoreembolsable"].Value = Datos.importenoreembolsable;
                comando.Parameters["@importenoaceptable"].Value = Datos.importenoaceptable;

                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();
                //DA.SelectCommand = comando;
                //comando.ExecuteNonQuery();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);


                List<ObtieneGastoResult> lista = new List<ObtieneGastoResult>();

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow row in DT.Rows)
                    {
                        ObtieneGastoResult ent = new ObtieneGastoResult
                        {
                            ACTUALIZADO = Datos.dirotros, //Convert.ToString(row["ACTUALIZADO"]),
                            id = Convert.ToInt32(row["id"]),
                            idinforme = Convert.ToInt32(row["idinforme"]),
                            Ruta = Convert.ToString(row["Ruta"])

                        };

                        lista.Add(ent);
                    }

                    return lista;
                    
                }
                else
                {
                    ObtieneGastoResult ent = new ObtieneGastoResult
                    {
                        ACTUALIZADO = "Error al actualizar",
                        id = 0,
                        idinforme = 0,
                    };

                    lista.Add(ent);


                    return lista;
                }
            }
            catch (System.Exception ex)
            {
                List<ObtieneGastoResult> lista = new List<ObtieneGastoResult>();

                ObtieneGastoResult ent = new ObtieneGastoResult
                {
                    ACTUALIZADO = ex.ToString(),
                    id = 0,
                    idinforme = 0,
                };

                lista.Add(ent);


                return lista;

            }

            
        }


        public string PostSaveImage(string Based64BinaryString)
        {
            try
            {
                string format = "";
                string path = HttpContext.Current.Server.MapPath("/Comprobantes/");
                string name = DateTime.Now.ToString("hhmmss");


                if (Based64BinaryString.Contains("data:image/jpeg;base64,"))
                {
                    format = "jpg";
                }
                else if (Based64BinaryString.Contains("data:image/png;base64,"))
                {
                    format = "png";
                }
                else
                {
                    format = "jpg";
                }


                string str = Based64BinaryString.Replace("data:image/jpeg;base64,", " ");//jpg check
                str = str.Replace("data:image/png;base64,", " ");//png check
                str = str.Replace("data:text/plain;base64,", " ");//text file check
                str = str.Replace("data:;base64,", " ");//zip file check
                str = str.Replace("data:application/zip;base64,", " ");//zip file check

                byte[] data = Convert.FromBase64String(str);

                MemoryStream ms = new MemoryStream(data, 0, data.Length);
                ms.Write(data, 0, data.Length);
                //File.WriteAllBytes(@"C:\testpdf.pdf", data);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                string rutacompleta = path + "Image" + name + "." + format;
                image.Save(rutacompleta);
                return "Comprobantes/" + "Image" + name + "." + format;
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
            //return result;
        }



    }
}

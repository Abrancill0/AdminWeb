using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Http;

namespace SCGESP.Controllers.APP
{
    public class InsertGastoAppController : ApiController
    {
        public class ParametrosGasto
        {
            public int idinforme { get; set; }
            public string fgasto { get; set; }
            public string ugasto { get; set; }
            public string concepto { get; set; }
            public string negocio { get; set; }
            public string formapago { get; set; }
            public int categoria { get; set; }
            public float subtotal { get; set; }
            public float iva { get; set; }
            public float total { get; set; }
            public string ucreo { get; set; }
            public int comprobante { get; set; }
            public string idapp { get; set; }
            public string dirxml { get; set; }
            public string observaciones { get; set; }
            public int estatus { get; set; }
            public string rfc { get; set; }
            public string contacto { get; set; }
            public string telefono { get; set; }
            public string correo { get; set; }
            public int idempresa { get; set; }
            public string hgasto { get; set; }
            public string fileotros { get; set; }
            public string nombreCategoria { get; set; }
            public double ivaCategoria { get; set; }
            
            public double importecomprobar { get; set; }
            public double importenodeducible { get; set; }
            public double importereembolsable { get; set; }
            public double importenoreembolsable { get; set; }
            public double importenoaceptable { get; set; }

           public int ncomensales { get; set; }
           public string nmbcomensales { get; set; }
     }

        public class ObtieneGastoResult
        {
            public string IdGasto { get; set; }
            public string NEWID { get; set; }
            public string Ruta { get; set; }
        }


        public List<ObtieneGastoResult> PostObtieneInformes(ParametrosGasto Datos)
        {
            string dirotros = "";
            try
            {
                if (Datos.fileotros != "" && Datos.fileotros != null)
                {
                    dirotros = PostSaveImage(Datos.fileotros);
                }
                else
                {
                    dirotros = "";
                }

            }
            catch (Exception ex)
            {

                dirotros = ex.ToString();
            }


            string obs = "";
            string rfc = "";
            string contacto = "";
            string telefono = "";
            string correo = "";

            if (Datos.observaciones != "" && Datos.observaciones != null)
            {
                obs = Datos.observaciones;
            }
            if (Datos.rfc != "" && Datos.rfc != null)
            {
                rfc = Datos.rfc;
            }

            if (Datos.contacto != "" && Datos.contacto != null)
            {
                contacto = Datos.contacto;
            }

            if (Datos.telefono != "" && Datos.telefono != null)
            {
                telefono = Datos.telefono;
            }

            if (Datos.correo != "" && Datos.correo != null)
            {
                correo = Datos.correo;
            }

            try
            {
                string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.ugasto);

                SqlCommand comando = new SqlCommand("InsertGasto");
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@idinforme", SqlDbType.Int);
                comando.Parameters.Add("@fgasto", SqlDbType.Date);
                comando.Parameters.Add("@ugasto", SqlDbType.VarChar);
                comando.Parameters.Add("@concepto", SqlDbType.VarChar);
                comando.Parameters.Add("@negocio", SqlDbType.VarChar);
                comando.Parameters.Add("@formapago", SqlDbType.VarChar);
                comando.Parameters.Add("@categoria", SqlDbType.Int);
                comando.Parameters.Add("@subtotal", SqlDbType.Float);
                comando.Parameters.Add("@iva", SqlDbType.Float);
                comando.Parameters.Add("@total", SqlDbType.Float);
                comando.Parameters.Add("@ucreo", SqlDbType.VarChar);
                comando.Parameters.Add("@comprobante", SqlDbType.Int);
                comando.Parameters.Add("@estatus", SqlDbType.Int);
                comando.Parameters.Add("@idapp", SqlDbType.VarChar);
                comando.Parameters.Add("@dirxml", SqlDbType.VarChar);
                comando.Parameters.Add("@dirpdf", SqlDbType.VarChar);
                comando.Parameters.Add("@dirotros", SqlDbType.VarChar);
                comando.Parameters.Add("@observaciones", SqlDbType.VarChar);
                comando.Parameters.Add("@rfc", SqlDbType.VarChar);
                comando.Parameters.Add("@contacto", SqlDbType.VarChar);
                comando.Parameters.Add("@telefono", SqlDbType.VarChar);
                comando.Parameters.Add("@correo", SqlDbType.VarChar);
                comando.Parameters.Add("@hgasto", SqlDbType.VarChar);
                comando.Parameters.Add("@nombreCategoria", SqlDbType.VarChar);
                comando.Parameters.Add("@ivaCategoria", SqlDbType.Float);

                comando.Parameters.Add("@deducible", SqlDbType.Int);
                comando.Parameters.Add("@importenodeducible", SqlDbType.Float);
                comando.Parameters.Add("@importereembolsable", SqlDbType.Float);
                comando.Parameters.Add("@importenoreembolsable", SqlDbType.Float);
                comando.Parameters.Add("@importenoaceptable", SqlDbType.Float);

                comando.Parameters.Add("@ncomensales", SqlDbType.Int);
                comando.Parameters.Add("@nmbcomensales", SqlDbType.VarChar);


                //Asignacion de valores a parametros
                comando.Parameters["@idinforme"].Value = Datos.idinforme;
                comando.Parameters["@fgasto"].Value = Convert.ToDateTime(Datos.fgasto);
                comando.Parameters["@ugasto"].Value = UsuarioDesencripta;
                comando.Parameters["@concepto"].Value = Datos.concepto;
                comando.Parameters["@negocio"].Value = Datos.negocio;
                comando.Parameters["@formapago"].Value = Datos.formapago;
                comando.Parameters["@categoria"].Value = Datos.categoria;
                comando.Parameters["@subtotal"].Value = Datos.subtotal;
                comando.Parameters["@iva"].Value = Datos.iva;
                comando.Parameters["@total"].Value = Datos.total;
                comando.Parameters["@ucreo"].Value = UsuarioDesencripta;
                comando.Parameters["@comprobante"].Value = Datos.comprobante;
                comando.Parameters["@idapp"].Value = "Web";
                comando.Parameters["@dirxml"].Value = "";//Datos.dirxml;
                comando.Parameters["@dirpdf"].Value = "";//Datos.dirpdf;
                comando.Parameters["@dirotros"].Value = dirotros; //Datos.dirotros;
                comando.Parameters["@observaciones"].Value = obs;
                comando.Parameters["@estatus"].Value = Datos.estatus;
                comando.Parameters["@rfc"].Value = rfc;
                comando.Parameters["@contacto"].Value = contacto;
                comando.Parameters["@telefono"].Value = telefono;
                comando.Parameters["@correo"].Value = correo;
                comando.Parameters["@hgasto"].Value = Datos.hgasto;
                comando.Parameters["@nombreCategoria"].Value = Datos.nombreCategoria;
                comando.Parameters["@ivaCategoria"].Value = Datos.ivaCategoria;
                comando.Parameters["@deducible"].Value = Datos.importenodeducible == 0 ? 1 : 0;
                comando.Parameters["@importenodeducible"].Value = Datos.importenodeducible;
                comando.Parameters["@importereembolsable"].Value = Datos.importereembolsable;
                comando.Parameters["@importenoreembolsable"].Value = Datos.importenoreembolsable;
                comando.Parameters["@importenoaceptable"].Value = Datos.importenoaceptable;
                comando.Parameters["@ncomensales"].Value = Datos.ncomensales;
                comando.Parameters["@nmbcomensales"].Value = Datos.nmbcomensales != null ? Datos.nmbcomensales : "";


                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();

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
                            IdGasto = Convert.ToString(row["idgasto"]),
                            NEWID = Convert.ToString(row["NEWID"]),
                            Ruta = Convert.ToString(row["Ruta"])
                        };

                          lista.Add(ent);
                    }
                 
                    return lista;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                List<ObtieneGastoResult> lista = new List<ObtieneGastoResult>();

                ObtieneGastoResult ent = new ObtieneGastoResult
                {
                    IdGasto = ex.ToString()
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
                string name = DateTime.Now.ToString("yyyyMMddhhmmss");


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
                return  "Comprobantes/" + "Image" + name + "." + format;
            }
            catch (Exception ex)
            {
                return  "Error : " + ex;
            }
            //return result;
        }

    }
}
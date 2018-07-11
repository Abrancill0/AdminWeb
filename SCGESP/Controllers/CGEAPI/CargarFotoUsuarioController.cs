using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class CargarFotoUsuarioController : ApiController
    {
        public class ListResult
        {
            public bool FotoOK { get; set; }
            public string Img { get; set; }
            public string Descripcion { get; set; }
        }

        public class ParametrosFotoUsuario
        {
            public string Usuario { get; set; }
            public string Foto { get; set; }
            public string FotoOld { get; set; }
        }

        public IEnumerable<ListResult> PostObtieneDatos(ParametrosFotoUsuario Datos)
        {
            if (Datos.FotoOld != "" || Datos.FotoOld != null)
                DeleteImage(Datos.FotoOld);

            return PostSaveImage(Datos.Foto, Datos.Usuario.ToLower());
            
        }

        public IEnumerable<ListResult> PostSaveImage(string Based64BinaryString, string Usuario)
        {
            List<ListResult> resultado = new List<ListResult>();
            try
            {
                string format = "";
                string path = HttpContext.Current.Server.MapPath("/");
                path = path + "img\\usuarios";
                string name = Usuario;


                if (Based64BinaryString.Contains("data:image/jpeg;base64,"))
                {
                    format = "jpg";
                }else if (Based64BinaryString.Contains("data:image/png;base64,"))
                {
                    format = "png";
                }
                else if (Based64BinaryString.Contains("data:image/gif;base64,"))
                {
                    format = "gif";
                }


                string str = Based64BinaryString.Replace("data:image/jpeg;base64,", " ");//jpg check
                str = str.Replace("data:image/png;base64,", " ");//png check
                str = str.Replace("data:image/gif;base64,", " ");//png check

                byte[] data = Convert.FromBase64String(str);

                MemoryStream ms = new MemoryStream(data, 0, data.Length);
                ms.Write(data, 0, data.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                string rutacompleta = path + "\\" + name + "." + format;
                image.Save(rutacompleta);

                string NuevaImagen = "img/usuarios/" + name + "." + format;
                ListResult imgOK = new ListResult
                {
                    FotoOK = true,
                    Img = NuevaImagen,
                    Descripcion = "Imagen de perfil actualizada."
                };
                resultado.Add(imgOK);

            }
            catch (Exception ex)
            {
                ListResult imgOK = new ListResult
                {
                    FotoOK = false,
                    Img = "img/usuarios/default.png",
                    Descripcion = "Error al cargar Imagen. " + Convert.ToString(ex)
                };
                resultado.Add(imgOK);
            }
            return resultado;
        }

       public bool DeleteImage(string imagen)
        {
            try
            {
                string ruta = imagen;
                string raiz = HttpContext.Current.Server.MapPath("/");
                string curFile = raiz + ruta;
                curFile = curFile.Replace("/", "\\");
                File.Delete(curFile);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
}

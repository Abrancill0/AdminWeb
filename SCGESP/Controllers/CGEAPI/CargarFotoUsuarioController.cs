using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;

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

		//public IEnumerable<ListResult> PostObtieneDatos(ParametrosFotoUsuario Datos)
		[HttpPost]
		public async Task<ListResult> Post()
		{
			HttpRequest httpRequest = HttpContext.Current.Request;
			ParametrosFotoUsuario Datos = new ParametrosFotoUsuario
			{
				Usuario = Convert.ToString(httpRequest.Params["Usuario"]),
				Foto = Convert.ToString(httpRequest.Params["Foto"]),
				FotoOld = Convert.ToString(httpRequest.Params["FotoOld"])
			};
			if (Datos.FotoOld != "" || Datos.FotoOld != null)
                DeleteImage(Datos.FotoOld);

            return PostSaveImage(Datos.Foto, Datos.Usuario.ToLower(), httpRequest);
            
        }

        public ListResult PostSaveImage(string Based64BinaryString, string Usuario, HttpRequest FileFoto)
        {
            ListResult resultado = new ListResult();
            try
            {
                string format = "";
                string path = HttpContext.Current.Server.MapPath("/");
                path = path + "img\\usuarios";
                string name = Usuario;
				string rutacompleta = path + "\\" + name; // + "." + format;


				try
				{
					if (FileFoto.Files.Count > 0)
					{

						foreach (string file in FileFoto.Files)
						{

							var postedFile = FileFoto.Files[file];

							switch (postedFile.ContentType)
							{// image/gif, image/png, image/jpeg
								case "image/jpeg":
									format = "jpg";
									break;
								case "image/png":
									format = "png";
									break;
								case "image/gif":
									format = "gif";
									break;
								default:
									string[] nmbext = (postedFile.FileName).Split('.');
									format = nmbext[nmbext.Length - 1];
									break;
							}

							rutacompleta = rutacompleta + "." + format;
							postedFile.SaveAs(rutacompleta);
						}
					}

					resultado.FotoOK = true;
					resultado.Img = "img/usuarios/" + name + "." + format;
					resultado.Descripcion = "Imagen de perfil actualizada.";
				}
				catch (Exception)
				{
					if (Based64BinaryString.Contains("data:image/jpeg;base64,"))
					{
						format = "jpg";
					}
					else if (Based64BinaryString.Contains("data:image/png;base64,"))
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
					rutacompleta = path + "\\" + name + "." + format;
					image.Save(rutacompleta);

					string NuevaImagen = "img/usuarios/" + name + "." + format;

					resultado.FotoOK = true;
					resultado.Img = NuevaImagen;
					resultado.Descripcion = "Imagen de perfil actualizada.";
				}
            }
            catch (Exception ex)
            {
				resultado.FotoOK = false;
				resultado.Img = "img/usuarios/default.png";
				resultado.Descripcion = "Error al cargar Imagen. " + Convert.ToString(ex);
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
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}

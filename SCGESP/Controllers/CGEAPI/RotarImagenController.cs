using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
	public class RotarImagenController : ApiController
	{
		public class ParametrosImagen
		{
			public string Imagen { get; set; }
			public int Angulo { get; set; }
		}
		public class ListResult
		{
			public bool RotarOk { get; set; }
			public string Imagen { get; set; }
		}
		public ListResult PostRotar(ParametrosImagen Datos)
		{
			string path = HttpContext.Current.Server.MapPath("/");
			path += Datos.Imagen;
			try
			{
				Bitmap bitmap1 = (Bitmap)Bitmap.FromFile(path);
				//bitmap1.RotateFlip(RotateFlipType.Rotate180FlipY);
				if (Datos.Angulo == 90)
				{
					bitmap1.RotateFlip(RotateFlipType.Rotate90FlipXY);
				}
				else {
					bitmap1.RotateFlip(RotateFlipType.Rotate270FlipXY);
				}
				
				ImageConverter converter = new ImageConverter();
				var data = (byte[])converter.ConvertTo(bitmap1, typeof(byte[]));
				
				MemoryStream ms = new MemoryStream(data, 0, data.Length);
				ms.Write(data, 0, data.Length);
				string rutacompleta = path;
				File.WriteAllBytes(rutacompleta, data);
			}
			catch (Exception)
			{
				//
			}
			return null;
		}
	}
}

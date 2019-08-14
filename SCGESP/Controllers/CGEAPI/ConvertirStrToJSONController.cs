using SCGESP.Clases;
using System;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
	public class ConvertirStrToJSONController : ApiController
	{
		public class Datos
		{
			public string Cadena { get; set; }
		}

		public dynamic PostStrToJSON(Datos datos)
		{
			try
			{
				if (datos.Cadena != "")
				{
					return StrToJson.Convertir(datos.Cadena);
				}
				else
				{
					return null;
				}
			}
			catch (Exception)
			{
				return null;
			}

		}
	}
}
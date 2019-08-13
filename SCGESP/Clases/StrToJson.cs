using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SCGESP.Clases
{
	public static class StrToJson
	{
		public static dynamic Convertir(this string _cadena)
		{
			try
			{
				return JsonConvert.DeserializeObject(_cadena);
			}
			catch (Exception)
			{
				try
				{
					return JObject.Parse(_cadena);
				}
				catch (Exception error)
				{
					var er = "{'error':'" + error.ToString() + "'}";
					return JObject.Parse(er);
				}
			}
		}
	}
}
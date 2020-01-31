using Ele.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Data;

namespace SCGESP.Clases
{
	public class GetUsuarioAlterno
	{
		public class Respuesta
		{
			public int Estatus { get; set; }
			public string Mensaje { get; set; }
			public string Resultado { get; set; }
		}
		public static Respuesta UsuarioAlterno(string Usuario)
		{
			Respuesta Respuesta = new Respuesta
			{
				Estatus = 0,
				Mensaje = "Error al consultar.",
				Resultado = Usuario
			};
			try
			{
				DocumentoEntrada entrada = new DocumentoEntrada
				{
					Usuario = Usuario,
					Origen = "AdminWEB",
					Transaccion = 120795,
					Operacion = 16
				};

				DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);
				
				DataTable DTAlternos = new DataTable();

				if (respuesta.Resultado == "1")
				{
					DTAlternos = respuesta.obtieneTabla("Alternos");
					string alternos = "";
					for (int i = 0; i < DTAlternos.Rows.Count; i++)
					{
						string alterno = Convert.ToString(DTAlternos.Rows[i]["Alterno"]).Trim();
						alternos += alternos == "" ? alterno : "," + alterno; 
					}
					alternos = alternos == "" ? Usuario : alternos;

					Respuesta.Estatus = 1;
					Respuesta.Mensaje = "Alterno seleccionado.";
					Respuesta.Resultado = alternos;
					return Respuesta;
				}
				else {
					return Respuesta;
				}
				
			}
			catch (Exception)
			{
				return Respuesta;
			}
		}
		private static DocumentoSalida PeticionCatalogo(XmlDocument doc)
		{
			Localhost.Elegrp ws = new Localhost.Elegrp
			{
				Timeout = -1
			};
			string respuesta = ws.PeticionCatalogo(doc);
			return new DocumentoSalida(respuesta);
		}
	}
}
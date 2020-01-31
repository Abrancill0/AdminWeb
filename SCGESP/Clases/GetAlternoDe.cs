using Ele.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Data;

namespace SCGESP.Clases
{
	public class GetAlternoDe
	{
		public class Respuesta
		{
			public int Estatus { get; set; }
			public string Mensaje { get; set; }
			public string Resultado { get; set; }
		}
		public static Respuesta Usuarios(string Usuario)
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
					Operacion = 17
				};

				DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

				DataTable DTUsuarios = new DataTable();

				if (respuesta.Resultado == "1")
				{
					DTUsuarios = respuesta.obtieneTabla("Alternos");
					string usuarios = "";
					for (int i = 0; i < DTUsuarios.Rows.Count; i++)
					{
						string usuario = Convert.ToString(DTUsuarios.Rows[i]["Usuario"]).Trim();
						usuarios += usuarios == "" ? usuario : "," + usuario;
					}

					Respuesta.Estatus = 1;
					Respuesta.Mensaje = "Usuarios selecionados.";
					Respuesta.Resultado = usuarios;
					return Respuesta;
				}
				else
				{
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
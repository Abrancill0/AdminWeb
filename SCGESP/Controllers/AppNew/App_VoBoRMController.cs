﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Ele.Generales;
using Newtonsoft.Json.Linq;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
	public class App_VoBoRMController : ApiController
	{
		//Parametros Entrada
		public class ParametrosEntrada
		{
			public string Usuario { get; set; }
		}

		//Parametros Salida
		public class ObtieneParametrosSalida
		{
			public string RmOcoId { get; set; }
			public string RmOcoRequisicion { get; set; }
			public string RmOcoCentroNombre { get; set; }
			public string RmReqOficinaNombre { get; set; }
			public string RmReqSubramoNombre { get; set; }
			public string RmOcoSolicitoNombre { get; set; }
			public string RmReqJustificacion { get; set; }
			public string RmOcoProveedorNombre { get; set; }
			public string RmOcoSubtotal { get; set; }
			public string RmOcoIva { get; set; }
			public string RmOcoTotal { get; set; }
			public string RmOcoComprador { get; set; }
		}

		public JObject Post(ParametrosEntrada Datos)
		{
			string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

			DocumentoEntrada entrada = new DocumentoEntrada
			{
				Usuario = UsuarioDesencripta,
				Origen = "AdminAPP",
				Transaccion = 120768,
				Operacion = 1,
			};

			//entrada.agregaElemento("estatus", 1);
			entrada.agregaElemento("estatus", 1);

			DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

			DataTable DTListaVobo = new DataTable();

			try
			{
				if (respuesta.Resultado == "1")
				{
					DTListaVobo = respuesta.obtieneTabla("Catalogo");

					List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

					foreach (DataRow row in DTListaVobo.Rows)
					{
						ObtieneParametrosSalida ent = new ObtieneParametrosSalida
						{
							RmOcoId = Convert.ToString(row["RmOcoId"]),
							RmOcoRequisicion = Convert.ToString(row["RmOcoRequisicion"]),
							RmOcoCentroNombre = Convert.ToString(row["RmOcoCentroNombre"]),
							RmOcoSolicitoNombre = Convert.ToString(row["RmOcoSolicitoNombre"]),
							RmReqSubramoNombre = Convert.ToString(row["RmReqSubramoNombre"]),
							RmReqOficinaNombre = Convert.ToString(row["RmReqOficinaNombre"]),
							RmReqJustificacion = Convert.ToString(row["RmReqJustificacion"]),
							RmOcoProveedorNombre = Convert.ToString(row["RmOcoProveedorNombre"]),
							RmOcoSubtotal = Convert.ToString(row["RmOcoSubtotal"]),
							RmOcoIva = Convert.ToString(row["RmOcoIva"]),
							RmOcoTotal = Convert.ToString(row["RmOcoTotal"]),
							RmOcoComprador = Convert.ToString(row["RmOcoComprador"]),
						};
						lista.Add(ent);
					}

					JObject Resultado = JObject.FromObject(new
					{
						mensaje = "OK",
						estatus = 1,
						Result = lista

					});


					return Resultado;

				}
				else
				{

					XDocument doc = XDocument.Parse(respuesta.Documento.InnerXml);
					XElement Salida = doc.Element("Salida");
					XElement Errores = Salida.Element("Errores");
					XElement Error = Errores.Element("Error");
					XElement Descripcion = Error.Element("Descripcion");
					

					string resultado2 = respuesta.Errores.InnerText;

					JObject Resultado = JObject.FromObject(new
					{
						mensaje = Descripcion.Value,
						estatus = 0,
					});

					return Resultado;

				}
			}
			catch (Exception ex)
			{

				JObject Resultado = JObject.FromObject(new
				{
					mensaje = ex.ToString(),
					estatus = 0,
					CambiaContrasena = false,
				});


				return Resultado;
			}


		}

		public static DocumentoSalida PeticionCatalogo(XmlDocument doc)
		{
			Localhost.Elegrp ws = new Localhost.Elegrp();
			ws.Timeout = -1;
			string respuesta = ws.PeticionCatalogo(doc);
			return new DocumentoSalida(respuesta);
		}

	}

}

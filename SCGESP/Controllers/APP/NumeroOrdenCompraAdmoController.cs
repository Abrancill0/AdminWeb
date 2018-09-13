using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.APP
{
	public class NumeroOrdenCompraAdmoController : ApiController
	{
		public class Datos
		{
			public string Usuario { get; set; }
			public string Origen { get; set; }
		}

		public class NumeroOrdenesAdmoResult
		{
			public string Tipo { get; set; }
			public int NumeroOrdenesVobo { get; set; }

		}

		public List<NumeroOrdenesAdmoResult> Post(Datos Datos)
		{
			string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

			DocumentoEntrada entrada = new DocumentoEntrada
			{
				Usuario = UsuarioDesencripta,
				Origen = "AdminAPP",
				Transaccion = 120768,
				Operacion = 1,
			};

			//entrada.agregaElemento("estatus", 2);
			entrada.agregaElemento("estatus", 2);

			DataTable DTListaAdministrativos = new DataTable();

			DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

			DataTable DTListaVobo = new DataTable();

			if (respuesta.Resultado == "1")
			{
				DTListaVobo = respuesta.obtieneTabla("Catalogo");

				int NumOCVobo = DTListaVobo.Rows.Count;

				List<NumeroOrdenesAdmoResult> lista = new List<NumeroOrdenesAdmoResult>();

				NumeroOrdenesAdmoResult ent = new NumeroOrdenesAdmoResult
				{
					Tipo = "Ordenes Admo Pendientes",
					NumeroOrdenesVobo = NumOCVobo

				};
				lista.Add(ent);

				return lista;
			}
			else
			{
				List<NumeroOrdenesAdmoResult> lista = new List<NumeroOrdenesAdmoResult>();

				NumeroOrdenesAdmoResult ent = new NumeroOrdenesAdmoResult
				{
					Tipo = "Ordenes Admo Pendientes",
					NumeroOrdenesVobo = 0

				};
				lista.Add(ent);

				return lista;
			}

		}


		public static DocumentoSalida PeticionCatalogo(XmlDocument doc)
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

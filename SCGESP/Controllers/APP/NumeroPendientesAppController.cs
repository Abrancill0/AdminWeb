using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.APP
{
	public class NumeroPendientesAppController : ApiController
	{
		public class Datos
		{
			public string Usuario { get; set; }
			public string Origen { get; set; }
		}

		public class NumeroPendientesResult
		{
			public string Tipo { get; set; }
			public int NumeroPendiente { get; set; }

		}

		public List<NumeroPendientesResult> Post(Datos Datos)
		{
			string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

			List<NumeroPendientesResult> lista = new List<NumeroPendientesResult>();
			
			//Ordenes de compra Admo
			DocumentoEntrada entrada = new DocumentoEntrada
			{
				Usuario = UsuarioDesencripta,
				Origen = "AdminAPP",
				Transaccion = 120768,
				Operacion = 1,
			};
			
			entrada.agregaElemento("estatus", 2);

			DataTable DTListaAdministrativos = new DataTable();

			DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

			if (respuesta.Resultado == "1")
			{
				DTListaAdministrativos = respuesta.obtieneTabla("Catalogo");

				int NumOCVobo = DTListaAdministrativos.Rows.Count;

				NumeroPendientesResult ent = new NumeroPendientesResult
				{
					Tipo = "Ordenes Admo Pendientes",
					NumeroPendiente = NumOCVobo

				};
				lista.Add(ent);
				
			}
			else
			{
				NumeroPendientesResult ent = new NumeroPendientesResult
				{
					Tipo = "Ordenes Admo Pendientes",
					NumeroPendiente = 0

				};
				lista.Add(ent);
				
			}

			//Ordenes de compra Vobo
			DocumentoEntrada entrada1 = new DocumentoEntrada
			{
				Usuario = UsuarioDesencripta,
				Origen = "AdminAPP",
				Transaccion = 120768,
				Operacion = 1,
			};

			entrada1.agregaElemento("estatus", 1);

			DocumentoSalida respuesta1 = PeticionCatalogo(entrada1.Documento);

			DataTable DTListaVobo = new DataTable();

			if (respuesta1.Resultado == "1")
			{
				DTListaVobo = respuesta1.obtieneTabla("Catalogo");

				int NumOCVobo = DTListaVobo.Rows.Count;
				
				NumeroPendientesResult ent = new NumeroPendientesResult
				{
					Tipo = "Ordenes Vobo Pendientes",
					NumeroPendiente = NumOCVobo

				};
				lista.Add(ent);
				
			}
			else
			{
			
				NumeroPendientesResult ent = new NumeroPendientesResult
				{
					Tipo = "Ordenes Vobo Pendientes",
					NumeroPendiente = 0

				};
				lista.Add(ent);
				
			}

			//Requisiciones pendientes
			DocumentoEntrada entrada2 = new DocumentoEntrada
			{
				Usuario = UsuarioDesencripta,
				Origen = "AdminApp",
				Transaccion = 120760,
				Operacion = 1
			};

			entrada2.agregaElemento("proceso", "2");

			DocumentoSalida respuesta2 = PeticionCatalogo(entrada2.Documento);

			DataTable DTRequisiciones = new DataTable();

			if (respuesta2.Resultado == "1")
			{
				DTRequisiciones = respuesta2.obtieneTabla("Catalogo");

				int NumReq = DTRequisiciones.Rows.Count;
				
				NumeroPendientesResult ent = new NumeroPendientesResult
				{
					Tipo = "Requisiciones Pendientes",
					NumeroPendiente = NumReq

				};
				lista.Add(ent);
				
			}
			else
			{
				NumeroPendientesResult ent = new NumeroPendientesResult
				{
					Tipo = "Requisiciones Pendientes",
					NumeroPendiente = 0

				};
				lista.Add(ent);
				
			}
			
			return lista;

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

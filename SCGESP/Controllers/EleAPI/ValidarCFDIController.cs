using Ele.Generales;
using System.Xml;
using System.Web.Http;
using System.Collections.Generic;
using SCGESP.Clases;

namespace SCGESP.Controllers.EleAPI
{
	public class ValidarCFDIController : ApiController
	{
		public class Datos
		{
			public string Usuario { get; set; }
			public string Origen { get; set; }
			public string FiCfdUuid { get; set; }
		}

		public XmlDocument Post(Datos Datos)
		{
			string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

			DocumentoEntrada entradadoc = new DocumentoEntrada
			{
				Usuario = UsuarioDesencripta,//Variables.usuario;
				Origen = "AdminWEB",
				Transaccion = 120870,
				Operacion = 6
			};
			entradadoc.agregaElemento("FiCfdUuid", Datos.FiCfdUuid);

			DocumentoSalida respuesta = PeticionCatalogo(entradadoc.Documento);

			return respuesta.Documento;
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

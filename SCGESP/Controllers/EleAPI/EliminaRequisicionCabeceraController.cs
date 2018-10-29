
using Ele.Generales;
using System.Xml;
using System.Web.Http;
using System.Collections.Generic;
using SCGESP.Clases;

namespace SCGESP.Controllers.EleAPI
{
    public class EliminaRequisicionCabeceraController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
            public string RmReqId { get; set; }
        }

        public class RequisicionEncabezadoResult
        {
            public int Resultado { get; set; }

        }

        public XmlDocument Post(datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada();
            entrada.Usuario = UsuarioDesencripta;  
            entrada.Origen = "AdminWEB";  //Datos.Origen; 
            entrada.Transaccion = 120760;
			entrada.Operacion = 11; // ya no se eliminan el registro solo se cancela;

            entrada.agregaElemento("RmReqId", Datos.RmReqId);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                return respuesta.Documento;
            }
            else
            {

				string msnError = "";
				XmlDocument xmErrores = new XmlDocument();
				XmlDocument xmError = new XmlDocument();
				xmErrores.LoadXml(respuesta.Errores.InnerXml);

				XmlNodeList elemList = xmErrores.GetElementsByTagName("Descripcion");
				for (int i = 0; i < elemList.Count; i++)
				{
					msnError += (elemList[i].InnerXml) + " ";
				}

				xmError.LoadXml("<Salida><Resultado>0</Resultado><Error>" + msnError + "</Error></Salida>");

				return xmError;

				//var errores = respuesta.Errores;

				//return null;
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


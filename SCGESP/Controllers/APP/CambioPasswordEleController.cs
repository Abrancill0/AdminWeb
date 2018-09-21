using SCGESP.Clases;
using Ele.Generales;
using System.Web.Http;
using System.Xml;
using System.Collections.Generic;
using System;

namespace SCGESP.Controllers.APP
{
    public class CambioPasswordEleController : ApiController
    {
        public class datos
        {
            public string usuario { get; set; }
            public string contrasena { get; set; }
        }

        public class ObtieneEmpleadoResult
        {
            public int SgUsuEmpleado { get; set; }
        }

        public XmlDocument Post(datos Datos)
        {

            var fechaVencimiento = DateTime.Today.Date.AddDays(1000);
            var pswEncriptado = Encrypter.Encrypt(Datos.contrasena, Datos.usuario.ToUpper());

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.usuario,
                Origen = "Login Usuario",
                Transaccion = 100004,
                Operacion = 9

            };


            entrada.agregaElemento("SgUsuId", Datos.usuario);
            entrada.agregaElemento("SgUsuClaveAcceso", pswEncriptado);
            entrada.agregaElemento("SgUsuFechaVencimiento", fechaVencimiento);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                return respuesta.Documento;
            }
            else
            {
                var errores = respuesta.Errores;

				return respuesta.Documento;
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


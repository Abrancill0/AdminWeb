﻿using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class ConsultaTareasPendientesController : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
        }

        public class TareasPendientesResult
        {
            public int Resultado { get; set; }
        }

        public XmlDocument Post(Datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencripta,
                Origen = "Programa CGE",  //Datos.Origen; 
                Transaccion = 120484, //Tareas pendientes
                Operacion = 1//regresa una tabla con todos los campos de la tabla ( La cantidad de registros depende del filtro enviado)
            };

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                return respuesta.Documento;
            }
            else
            {
                //var errores = respuesta.Errores;

                return respuesta.Documento;
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

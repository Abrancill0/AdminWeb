using Ele.Generales;
using System.Xml;
using System.Web.Http;
using System.Collections.Generic;
using System;
using SCGESP.Clases;

namespace SCGESP.Controllers.EleAPI
{
    public class RequisicionDetalleEliminaController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
            public string RmRdeId { get; set; }
            public string RmRdeRequisicion { get; set; }
            
        }

        public string Post(datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada();
            entrada.Usuario = UsuarioDesencripta;  
            entrada.Origen = "Programa CGE";  //Datos.Origen; 
            entrada.Transaccion = 120762;
            entrada.Operacion = 4;

            entrada.agregaElemento("RmRdeRequisicion", Datos.RmRdeRequisicion);
            entrada.agregaElemento("RmRdeId", Datos.RmRdeId);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
               
                return "OK";
            }
            else
            {
                var errores = respuesta.Errores;

                return null;
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


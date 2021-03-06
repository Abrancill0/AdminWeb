﻿using Ele.Generales;
using System.Xml;
using System.Web.Http;
using System.Collections.Generic;
using SCGESP.Clases;

namespace SCGESP.Controllers.AppNew
{
    public class RechazaOCController : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string RmOcoId { get; set; }
            public string RmOcoRequisicion { get; set; }
            public string RmOcoComentario { get; set; }
        }

        public class RechazaRequisicionResult
        {
            public int Resultado { get; set; }
        }

        public XmlDocument Post(Datos Datos)
        {
           
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminApp",  //Datos.Origen; 
                Transaccion = 120768,
                Operacion = 14 //rechazar requisiciones
            };
            entrada.agregaElemento("RmOcoId", Datos.RmOcoId);
            entrada.agregaElemento("RmOcoComentarios", Datos.RmOcoComentario);
            entrada.agregaElemento("RmOcoRequisicion", Datos.RmOcoRequisicion);
            

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                return respuesta.Documento;
            }
            else
            {
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



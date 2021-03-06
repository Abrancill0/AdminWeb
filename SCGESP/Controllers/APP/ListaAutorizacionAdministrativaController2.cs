﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class ListaAutorizacionAdministrativa2Controller : ApiController
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
            public string RmOcoOficinaNombre { get; set; }
            public string RmOcoSubramoNombre { get; set; }
            public string RmOcoSolicitoNombre { get; set; }
            public string RmReqJustificacion { get; set; }
            public string RmOcoProveedorNombre { get; set; }
            public string RmOcoSubtotal { get; set; }
            public string RmOcoIva { get; set; }
            public string RmOcoTotal { get; set; }

        }
        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        //public XmlDocument Post(ParametrosEntrada Datos)
        public XmlDocument Post(ParametrosEntrada Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencripta,
                Origen = "AdminAPP",
                Transaccion = 120768,
                Operacion = 1,
            };

            entrada.agregaElemento("estatus", 2);

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                if (respuesta.Resultado == "1")
                {
                    return respuesta.Documento;
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
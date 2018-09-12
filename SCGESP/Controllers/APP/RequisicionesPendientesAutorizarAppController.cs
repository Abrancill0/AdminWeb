﻿using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.APP
{
    public class RequisicionesPendientesAutorizarAppController : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
        }

        public class RequisicionesPorAutorizarResult
        {
            public string RmReqId { get; set; }
            public string RmReqEstatusNombre { get; set; }
            public string RmReqTipoRequisicion { get; set; }
            public double RmReqTotal { get; set; }
            public string RmReqSolicitanteNombre { get; set; }
            public string RmReqSubramo { get; set; }
            public string RmReqJustificacion { get; set; }
            public string RmReqOficinaNombre { get; set; }
            public string RmReqEstatus { get; set; }
            public string RmReqMonedaNombre { get; set; }
            public string RmReqTipoGastoNombre { get; set; }
            public string RmReqProveedorNombre { get; set; }
            public string RmReqCentroNombre { get; set; }
            public string RmReqEmpleadoObligado  {get; set; }
            public string RmReqEmpleadoObligadoNombre { get; set; }
        }

        public List<RequisicionesPorAutorizarResult> Post(Datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencripta,
                Origen = "Programa CGE",  //Datos.Origen; 
                Transaccion = 120760,
                Operacion = 1//regresa una tabla con todos los campos de la tabla ( La cantidad de registros depende del filtro enviado)
            };

            entrada.agregaElemento("proceso", "2");

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            DataTable DTRequisiciones = new DataTable();

            if (respuesta.Resultado == "1")
            {
                DTRequisiciones = respuesta.obtieneTabla("Catalogo");

                List<RequisicionesPorAutorizarResult> lista = new List<RequisicionesPorAutorizarResult>();

                foreach (DataRow row in DTRequisiciones.Rows)
                {
                    RequisicionesPorAutorizarResult ent = new RequisicionesPorAutorizarResult
                    {
                        RmReqId = Convert.ToString(row["RmReqId"]), //OK
                        RmReqEstatusNombre = Convert.ToString(row["RmReqEstatusNombre"]),
                        RmReqJustificacion = Convert.ToString(row["RmReqJustificacion"]),
                        RmReqOficinaNombre = Convert.ToString(row["RmReqOficinaNombre"]),
                        RmReqSolicitanteNombre = Convert.ToString(row["RmReqSolicitanteNombre"]),
                        RmReqTotal = Convert.ToDouble(row["RmReqTotal"]),
                        RmReqEstatus = Convert.ToString(row["RmReqEstatus"]),
                        RmReqMonedaNombre = Convert.ToString(row["RmReqMonedaNombre"]),
                        RmReqTipoGastoNombre = Convert.ToString(row["RmReqTipoGastoNombre"]),
                        RmReqProveedorNombre = Convert.ToString(row["RmReqProveedorNombre"]),
                        RmReqCentroNombre = Convert.ToString(row["RmReqCentroNombre"]),
                        RmReqTipoRequisicion = Convert.ToString(row["RmReqTipoRequisicion"]),
                        RmReqSubramo = Convert.ToString(row["RmReqSubramo"]),
                        RmReqEmpleadoObligado = Convert.ToString(row["RmReqEmpleadoObligado"]),
                        RmReqEmpleadoObligadoNombre = Convert.ToString(row["RmReqEmpleadoObligadoNombre"]),
                    };
                    lista.Add(ent);
                }


                return lista;
            }
            else
            {
                var errores = respuesta.Errores;

                return null;
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

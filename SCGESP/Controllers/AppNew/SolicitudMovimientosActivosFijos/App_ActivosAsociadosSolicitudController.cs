﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;
using Newtonsoft.Json.Linq;
using SCGESP.Clases;

namespace SCGESP.Controllers.AppNew
{
    public class App_ActivosAsociadosSolicitudController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string AfVrdID { get; set; }
        }
        //Parametros Salida
        public class ObtieneParametrosSalida
        {

            public string AfVrdID { get; set; }
            public string AfVrdFolioActivoFijo { get; set; }
            public string AfVrdFolioActivoFijoNombre { get; set; }
            public string AfVrdFechaVentaDestruccion { get; set; }
            public string AfVrdFacturaVenta { get; set; }
            public string AfVrdValorVenta { get; set; }
            public string AfVrdOficioDestruccion { get; set; }
            public string AfInvFactura { get; set; }
            public string AfInvFechaAdquisicion { get; set; }
            public string AfInvValorAdquisicion { get; set; }
            public string CalculoValorNetoActual { get; set; }
            public string AfInvMarca { get; set; }
            public string AfInvModelo { get; set; }
            public string AfInvNumeroSerie { get; set; }
            public string AfInvAsignacionCentro { get; set; }
            public string AfInvAsignacionCentroNombre { get; set; }
            public string Utilidad { get; set; }
            public string Perdida { get; set; }
            public string AfInvDeprContAcumulada { get; set; }
            public string AfVrdComentario { get; set; }

    }
        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        //public XmlDocument Post(ParametrosEntrada Datos)
        public JObject Post(ParametrosEntrada Datos)
        {
            //string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120338,
                Operacion = 1,
            };

            entrada.agregaElemento("AfVrdID", Datos.AfVrdID);

            DataTable DTListaAdministrativos = new DataTable();

            try
            {
                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                if (respuesta.Resultado == "1")
                {
                    DTListaAdministrativos = respuesta.obtieneTabla("Catalogo");

                    List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                    foreach (DataRow row in DTListaAdministrativos.Rows)
                    {
                        ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                        {

                            AfVrdID = Convert.ToString(row["AfVrdID"]),
                            AfVrdFolioActivoFijo = Convert.ToString(row["AfVrdFolioActivoFijo"]),
                            AfVrdFolioActivoFijoNombre = Convert.ToString(row["AfVrdFolioActivoFijoNombre"]),
                            AfVrdFechaVentaDestruccion = Convert.ToString(row["AfVrdFechaVentaDestruccion"]),
                            AfVrdFacturaVenta = Convert.ToString(row["AfVrdFacturaVenta"]),
                            AfVrdValorVenta = Convert.ToString(row["AfVrdValorVenta"]) == "" ? "0" :Convert.ToString(row["AfVrdValorVenta"]),
                            AfVrdOficioDestruccion = Convert.ToString(row["AfVrdOficioDestruccion"]),
                            AfVrdComentario = Convert.ToString(row["AfVrdComentario"]),
                            AfInvFactura = Convert.ToString(row["AfInvFactura"]),
                            AfInvFechaAdquisicion = Convert.ToString(row["AfInvFechaAdquisicion"]),
                            AfInvValorAdquisicion = Convert.ToString(row["AfInvValorAdquisicion"]) == "" ? "0" : Convert.ToString(row["AfInvValorAdquisicion"]),
                            CalculoValorNetoActual = Convert.ToString(row["CalculoValorNetoActual"]) == ""? "0" : Convert.ToString(row["CalculoValorNetoActual"]),
                            AfInvMarca = Convert.ToString(row["AfInvMarca"]),
                            AfInvModelo = Convert.ToString(row["AfInvModelo"]),
                            AfInvNumeroSerie = Convert.ToString(row["AfInvNumeroSerie"]),
                            AfInvAsignacionCentro = Convert.ToString(row["AfInvNumeroSerie"]),
                            AfInvAsignacionCentroNombre = Convert.ToString(row["AfInvAsignacionCentroNombre"]),
                            Utilidad = Convert.ToString(row["Utilidad"]) == "" ? "0" : Convert.ToString(row["Utilidad"]),
                            Perdida = Convert.ToString(row["Perdida"]) == "" ? "0" : Convert.ToString(row["Perdida"]),
                            AfInvDeprContAcumulada = Convert.ToString(row["AfInvDeprContAcumulada"]) == "" ? "0" : Convert.ToString(row["AfInvDeprContAcumulada"]),

                        };
                        lista.Add(ent);
                    }

                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = "OK",
                        estatus = 1,
                        Result = lista

                    });


                    return Resultado;

                }
                else
                {
                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = "No existen datos",
                        estatus = 1,


                    });


                    return Resultado;
                }


            }
            catch (Exception ex)
            {
          
                JObject Resultado = JObject.FromObject(new
                {
                    mensaje = ex.ToString(),
                    estatus = 1,


                });


                return Resultado;
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
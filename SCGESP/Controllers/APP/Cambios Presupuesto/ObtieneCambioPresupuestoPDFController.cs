﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;

namespace SCGESP.Controllers
{
    public class ObtieneCambioPresupuestoPDFController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string PrPtiAnio { get; set; }
            public string PrPtiFolio { get; set; }
        }

        public class ObtieneParametrosSalida
        {
            public string PDF { get; set; } //PDF
        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120623,
                Operacion = 22,
            };

            entrada.agregaElemento("PrPtiAnio", Datos.PrPtiAnio);
            entrada.agregaElemento("PrPtiFolio", Datos.PrPtiFolio);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);
  
            try
            {

                if (respuesta.Resultado == "1")
            {

                    DataTable DT = new DataTable();
                   // DT = respuesta.obtieneValor("Valores");
                    string cosa = respuesta.obtieneValor("ArchivoPdf");
                    List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                    string result = "";
                    string format = ".pdf";
                    string path = HttpContext.Current.Server.MapPath("/PDF/SolicitudCambio/");
                    string name = DateTime.Now.ToString("yyyyMMddhhmmss");

                   // foreach (DataRow row in DT.Rows)
                   // {
                        byte[] data = Convert.FromBase64String(cosa);

                        //ArchivoPdf
                        MemoryStream ms = new MemoryStream(data, 0, data.Length);
                        ms.Write(data, 0, data.Length);
                        string rutacompleta = path + name + format;
                        File.WriteAllBytes(rutacompleta, data);
                        result = "PDF/SolicitudCambio/" + name + format;
                    //}
                    
                    ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                    {
                        PDF = result
                    };
                    lista.Add(ent);

                    return lista;
            }
            else
            {
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                {
                    PDF = Convert.ToString("no encontro ningun registro"),
                    
                };
                lista.Add(ent);
                
                return lista;
            }

            }
            catch (Exception ex)
            {
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                {

                    PDF = ex.ToString()

                };
                lista.Add(ent);
                
                return lista;
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

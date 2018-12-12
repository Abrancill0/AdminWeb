using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;

namespace SCGESP.Controllers
{
    public class DetalleTraspasoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string PrTdeTraspaso { get; set; }
        }

        public class ObtieneParametrosSalida
        {
            public string PrTdeTraspaso { get; set; } //No Traspaso
            public string PrTdeLinea { get; set; } //Línea
            public string PrTdeCentro { get; set; } //Departameneto
            public string PrTdeCentroNombre { get; set; } //Departamento Nombre
            public string PrTdeOficina { get; set; } // Oficina
            public string PrTdeOficinaNombre { get; set; } //Oficina Nombre
            public string PrTdeSubramo { get; set; } // Subramo
            public string  PrTdeSubramoNombre { get; set; } //Subramo Nombre
            public string PrTdeCuenta { get; set; } //Cuenta
            public string PrTdeCuentaNombre { get; set; } //Cuenta Nombre
            public string PrTdeCargo { get; set; } //Disminuciones
            public string PrTdeAbono { get; set; } //Aumentos
        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120698,
                Operacion = 1,
            };
            
            entrada.agregaElemento("PrTdeTraspaso", Datos.PrTdeTraspaso);
            
             DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            DataTable DTLista = new DataTable();
            
            try
            {

                if (respuesta.Resultado == "1")
            {
                DTLista = respuesta.obtieneTabla("Catalogo");

                int NumOCVobo = DTLista.Rows.Count;

                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                foreach (DataRow row in DTLista.Rows)
                {
                    ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                    {
                        PrTdeTraspaso = Convert.ToString(row["PrTdeTraspaso"]),
                        PrTdeLinea = Convert.ToString(row["PrTdeLinea"]),
                        PrTdeCentro = Convert.ToString(row["PrTdeCentro"]),
                        PrTdeCentroNombre = Convert.ToString(row["PrTdeCentroNombre"]),
                        PrTdeOficina = Convert.ToString(row["PrTdeOficina"]),
                        PrTdeOficinaNombre = Convert.ToString(row["PrTdeOficinaNombre"]),
                        PrTdeSubramo = Convert.ToString(row["PrTdeSubramo"]),
                        PrTdeSubramoNombre = Convert.ToString(row["PrTdeSubramoNombre"]),
                        PrTdeCuenta = Convert.ToString(row["PrTdeCuenta"]),
                        PrTdeCuentaNombre = Convert.ToString(row["PrTdeCuentaNombre"]),
                        PrTdeCargo = Convert.ToString(row["PrTdeCargo"]),
                        PrTdeAbono = Convert.ToString(row["PrTdeAbono"]),
                    };
                    lista.Add(ent);
                }
                return lista;
            }
            else
            {
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                {
                    PrTdeTraspaso = Convert.ToString("no encontro ningun registro"),
                    
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

                    PrTdeTraspaso = ex.ToString()

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

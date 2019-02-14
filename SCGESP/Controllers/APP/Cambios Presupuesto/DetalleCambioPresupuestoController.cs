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
    public class DetalleCambioPresupuestoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
            public string PrPdeAnio { get; set; }
            public string PrPdeFolio { get; set; }
        }

        public class ObtieneParametrosSalida
        {
            public string PrPdeFolio { get; set; } //Folio del Cambio
            public string PrPdeConsecutivo { get; set; } // Consecutivo
            public string PrPdeCentroNombre { get; set; } //Departameneto
            public string PrPdeOficinaNombre { get; set; } //Oficina
            public string PrPdeSubramoNombre { get; set; } //Subramo
            public string PrPdeCuentaNombre { get; set; } //Cuenta
            public string PrPdeCuenta { get; set; }
            public string PrPdeAfectacion { get; set; } //tipo de afectación( 1= aumenta, -1 = desminuye)
            public string PrPdeValorTotal { get; set; } //Total del movimiento en el año(Te muestra además una columna por cada mes pero yo creo que con sólo mostrar la anual es suficiente)
        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        {
            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120625,
                Operacion = 1,
            };
            
            entrada.agregaElemento("PrPdeAnio", Datos.PrPdeAnio);
            entrada.agregaElemento("PrPdeFolio", Datos.PrPdeFolio);
           
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
                        PrPdeFolio = Convert.ToString(row["PrPdeFolio"]),
                        PrPdeConsecutivo = Convert.ToString(row["PrPdeConsecutivo"]),
                        PrPdeCentroNombre = Convert.ToString(row["PrPdeCentroNombre"]),
                        PrPdeOficinaNombre = Convert.ToString(row["PrPdeOficinaNombre"]),
                        PrPdeSubramoNombre = Convert.ToString(row["PrPdeSubramoNombre"]),
                        PrPdeCuentaNombre = Convert.ToString(row["PrPdeCuentaNombre"]),
                        PrPdeCuenta = Convert.ToString(row["PrPdeCuenta"]),
                        PrPdeAfectacion = Convert.ToString(row["PrPdeAfectacion"]),
                        PrPdeValorTotal = Convert.ToString(row["PrPdeValorTotal"]),  
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
                    PrPdeOficinaNombre = Convert.ToString("no encontro ningun registro"),
                    
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

                    PrPdeOficinaNombre = ex.ToString()

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

using SCGESP.Clases;
using Ele.Generales;
using System.Web.Http;
using System.Xml;
using System.Collections.Generic;
using System;
using System.Data;

namespace SCGESP.Controllers.APP
{
    public class LoginEleController : ApiController
    {
        public class ParametrosSalida
        {
            // public int i_uresponsable { get; set; }
            public string cosa { get; set; }
            public string cosa2 { get; set; }
            public string cosa3 { get; set; }
            public string GrEmpCentro { get; set; }
            public string GrEmpOficina { get; set; }
            public string GrEmpTipoGasto { get; set; }
            public string GrEmpTarjetaToka { get; set; }
            public string SgUsuMostrarGraficaAlAutorizar { get; set; }
            public string SgUsuFechaVencimiento { get; set; }
            //public string responsable { get; set; }
        }

        public class datos
        {
            public string usuario { get; set; }
            public string contrasena { get; set; }
        }

        public class ObtieneEmpleadoResult
        {
            public int SgUsuEmpleado { get; set; }
        }

        public List<ParametrosSalida> Post(datos Datos)
        {
            try
            {
                DocumentoEntrada entrada = new DocumentoEntrada
                {
                    Usuario = Datos.usuario,
                    Origen = "Programa CGE",  //Datos.Origen; 
                    Transaccion = 100004,
                    Operacion = 17
                };
                entrada.agregaElemento("SgUsuId", Datos.usuario);
                entrada.agregaElemento("SgUsuClaveAcceso", Datos.contrasena);

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                DataTable DTLista = new DataTable();

                if (respuesta.Resultado == "1")//&& idUsuario.Trim() != ""
                {
                    DTLista = respuesta.obtieneTabla("Catalogo");

                    List<ParametrosSalida> lista = new List<ParametrosSalida>();

                    foreach (DataRow row in DTLista.Rows)
                    {
                        ParametrosSalida ent = new ParametrosSalida
                        {
                            cosa = Convert.ToString(row["FiCscSolicitud"]),
                            cosa2 = Convert.ToString(row["FiCscSolicitud"]),
                            GrEmpCentro = Convert.ToString(row["FiCscSolicitud"]),
                            GrEmpOficina = Convert.ToString(row["FiCscSolicitud"]),
                            GrEmpTipoGasto = Convert.ToString(row["FiCscSolicitud"]),
                            cosa3 = Convert.ToString(row["FiCscSolicitud"]),
                            GrEmpTarjetaToka = Convert.ToString(row["FiCscSolicitud"]),
                            SgUsuFechaVencimiento = Convert.ToString(row["FiCscSolicitud"]),
                            SgUsuMostrarGraficaAlAutorizar = Convert.ToString(row["FiCscSolicitud"]),

                        };
                        lista.Add(ent);
                    }


                    return lista;

                }
                else
                {
                    DTLista = respuesta.obtieneTabla("Catalogo");
                    
                    string resultado = respuesta.Documento.InnerText;
                    List<ParametrosSalida> lista = new List<ParametrosSalida>();

                   
                        ParametrosSalida ent = new ParametrosSalida
                        {
                            cosa = resultado,

                        };
                        lista.Add(ent);
                 
                    return lista;
                }
            }
            catch (Exception ex)
            {
             
                List<ParametrosSalida> lista = new List<ParametrosSalida>();
                
                    ParametrosSalida ent = new ParametrosSalida
                    {
                        cosa = ex.ToString()

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

        public string ConsultaValorRow(string dato, XmlElement tablarow)
        {
            XmlElement TablaRow = tablarow;
            XmlNode valor = TablaRow.SelectSingleNode("Llave/NewDataSet/Llave/" + dato);

            string respuesta = "";

            try
            {
                if (valor != null)
                    respuesta = valor.InnerText.Trim();
                else
                    respuesta = "";
            }
            catch (InvalidCastException e)
            {
                var error = Convert.ToString(e);
                respuesta = "";
            }
            return respuesta;
        }
    }
}


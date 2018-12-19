using SCGESP.Clases;
using Ele.Generales;
using System.Web.Http;
using System.Xml;
using System.Collections.Generic;
using System;
using System.Data;

namespace SCGESP.Controllers.APP
{
    public class LoginAppController : ApiController
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
            public bool CambiaContrasena { get; set; }
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
                    DTLista = respuesta.obtieneTabla("Llave");

                    List<ParametrosSalida> lista = new List<ParametrosSalida>();
                    
                    foreach (DataRow row in DTLista.Rows)
                    {
                        string cadenaCosa = Convert.ToString(row["SgUsuId"]);
                        string cadenaCosa2 = Convert.ToString(row["SgUsuEmpleado"]);

                        cadenaCosa = cadenaCosa.Replace(" ", "");
                        cadenaCosa2 = cadenaCosa2.Replace(" ", "");

                        string cosa = Seguridad.Encriptar(cadenaCosa);
                        string cosa2 = Seguridad.Encriptar(cadenaCosa2);

                        DateTime dt = DateTime.Today;

                        DateTime FechaVencimiento = Convert.ToDateTime(Convert.ToString(row["SgUsuFechaVencimiento"]));

                        bool CambiaContrasena = false;

                        if (FechaVencimiento <= DateTime.Today.Date)
                         {
                            CambiaContrasena = true;
                         }
                        else
                        {
                            CambiaContrasena = false;
                        }
                        
                        ParametrosSalida ent = new ParametrosSalida
                        {
                            cosa = cosa,
                            cosa2 = cosa2,
                            GrEmpCentro = Convert.ToString(row["GrEmpCentro"]),
                            GrEmpOficina = Convert.ToString(row["GrEmpOficina"]),
                            GrEmpTipoGasto = Convert.ToString(row["GrEmpTipoGasto"]),
                            cosa3 = Convert.ToString(row["SgUsuNombre"]),
                            GrEmpTarjetaToka = Convert.ToString(row["GrEmpTarjetaToka"]),
                            CambiaContrasena = CambiaContrasena,
                            SgUsuMostrarGraficaAlAutorizar = Convert.ToString(row["SgUsuMostrarGraficaAlAutorizar"]),

                        };
                        lista.Add(ent);
                    }


                    return lista;

                }
                else
                {
                    //DTLista = respuesta.obtieneTabla("Catalogo");
                    
                   // string resultado = respuesta.Errores.InnerXml;
                    string resultado2 = respuesta.Errores.InnerText;
                   // string resultado3 = respuesta.Errores.OuterXml.;

                    List<ParametrosSalida> lista = new List<ParametrosSalida>();
                    
                        ParametrosSalida ent = new ParametrosSalida
                        {
                            cosa = resultado2,
                            CambiaContrasena=false

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


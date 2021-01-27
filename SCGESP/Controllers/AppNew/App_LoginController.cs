using SCGESP.Clases;
using Ele.Generales;
using System.Web.Http;
using System.Xml;
using System.Collections.Generic;
using System;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace SCGESP.Controllers.AppNew
{
    public class App_LoginController : ApiController
    {
        public class ParametrosSalida
        {
            public string SgUsuId { get; set; }
            public string SgUsuEmpleado { get; set; }
            public string SgUsuNombre { get; set; }
            public string GrEmpCentro { get; set; }
            public string GrEmpOficina { get; set; }
            public string GrEmpTipoGasto { get; set; }
            public string GrEmpTarjetaToka { get; set; }
            public string SgUsuMostrarGraficaAlAutorizar { get; set; }
            public bool CambiaContrasena { get; set; }
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

        public JObject Post(datos Datos)
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

                string Mensaje = "";
                int Estatus = 0;

                if (respuesta.Resultado == "1")//&& idUsuario.Trim() != ""
                {
                     Mensaje = "OK";
                     Estatus = 1;

                    DTLista = respuesta.obtieneTabla("Llave");

                    List<ParametrosSalida> lista = new List<ParametrosSalida>();

                    foreach (DataRow row in DTLista.Rows)
                    {
                        string SgUsuId = Convert.ToString(row["SgUsuId"]);
                        string SgUsuEmpleado = Convert.ToString(row["SgUsuEmpleado"]);

                        SgUsuId = SgUsuId.Replace(" ", "");
                        SgUsuEmpleado = SgUsuEmpleado.Replace(" ", "");

                        //string cosa = Seguridad.Encriptar(cadenaCosa);
                        //string cosa2 = Seguridad.Encriptar(cadenaCosa2);

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
                            SgUsuId = SgUsuId,
                            SgUsuEmpleado = SgUsuEmpleado,
                            GrEmpCentro = Convert.ToString(row["GrEmpCentro"]),
                            GrEmpOficina = Convert.ToString(row["GrEmpOficina"]),
                            GrEmpTipoGasto = Convert.ToString(row["GrEmpTipoGasto"]),
                            SgUsuNombre = Convert.ToString(row["SgUsuNombre"]),
                            GrEmpTarjetaToka = Convert.ToString(row["GrEmpTarjetaToka"]),
                            CambiaContrasena = CambiaContrasena,
                            SgUsuMostrarGraficaAlAutorizar = Convert.ToString(row["SgUsuMostrarGraficaAlAutorizar"]),

                        };

                        lista.Add(ent);
                    }

                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = Mensaje,
                        estatus = Estatus,
                        Result = lista

                    });


                    return Resultado;

                }
                else
                {

                    //  < Error >< Concepto > SgUsuClaveAcceso </ Concepto >< Descripcion > CONTRASEÑA INVÁLIDA </ Descripcion ></ Error >
                    Mensaje = respuesta.Errores.InnerText;
                    XDocument doc = XDocument.Parse(respuesta.Documento.InnerXml);
                    XElement Salida = doc.Element("Salida");
                    XElement Errores = Salida.Element("Errores");
                    XElement Error = Errores.Element("Error");
                    XElement Descripcion = Error.Element("Descripcion");
                    Estatus = 0;

                    string resultado2 = respuesta.Errores.InnerText;

                    JObject Resultado = JObject.FromObject(new
                    {
                        mensaje = Descripcion.Value,
                        estatus = Estatus,
                    });

                    return Resultado;
                  
                }
            }
            catch (Exception ex)
            {
             
                JObject Resultado = JObject.FromObject(new
                {
                    mensaje = ex.ToString(),
                    estatus = 0,
                    CambiaContrasena = false,
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


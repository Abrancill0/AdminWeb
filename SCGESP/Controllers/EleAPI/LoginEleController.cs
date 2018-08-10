using SCGESP.Clases;
using Ele.Generales;
using System.Web.Http;
using System.Xml;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace SCGESP.Controllers.EleAPI
{
    public class LoginAppController : ApiController
    {
        public class ListResult
        {
            // public int i_uresponsable { get; set; }
            public string cosa { get; set; }
            public string cosa2 { get; set; }
            public string cosa3 { get; set; }
            public string GrEmpCentro { get; set; }
            public string GrEmpOficina { get; set; }
            public string GrEmpTipoGasto { get; set; }
            public string GrEmpTarjetaToka { get; set; }
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

        public IEnumerable<ListResult> Post(datos Datos)
        {

            try
            {
				EnvioCorreosELE.Envio("", "hector.ramos@trascenti.com", "", "", "", "prueba", "mensaje prueba", 0);

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

                if (respuesta.Resultado == "1")//&& idUsuario.Trim() != ""
                {
                    XmlElement tabla = respuesta.Tablas;
                    string SgUsuId = ConsultaValorRow("SgUsuId", tabla);
                    string SgUsuEmpleado = ConsultaValorRow("SgUsuEmpleado", tabla);
                    string GrEmpCentro = ConsultaValorRow("GrEmpCentro", tabla);
                    string GrEmpOficina = ConsultaValorRow("GrEmpOficina", tabla);
                    string GrEmpTipoGasto = ConsultaValorRow("GrEmpTipoGasto", tabla);
                    string SgUsuNombre = ConsultaValorRow("SgUsuNombre", tabla);
                    string GrEmpTarjetaToka = ConsultaValorRow("GrEmpTarjetaToka", tabla);


                    string cosa = Seguridad.Encriptar(SgUsuId);
                    string cosa2 = Seguridad.Encriptar(Convert.ToString(SgUsuEmpleado));

                    List<ListResult> lista = new List<ListResult>();

                    ListResult ent = new ListResult
                    {
                        cosa = cosa,
                        cosa2 = cosa2,
                        cosa3 = SgUsuNombre,
                        GrEmpCentro = GrEmpCentro,
                        GrEmpOficina = GrEmpOficina,
                        GrEmpTipoGasto = GrEmpTipoGasto,
                        GrEmpTarjetaToka = GrEmpTarjetaToka
                    };
                    lista.Add(ent);

                    return lista;

                }
                else
                {
                    List<ListResult> lista = new List<ListResult>();

                    ListResult ent = new ListResult
                    {
                        cosa = "error",
                        cosa2 = "",
                        cosa3 = "",
                        GrEmpCentro = "",
                        GrEmpOficina = "",
                        GrEmpTipoGasto = "",
                        GrEmpTarjetaToka = ""
                    };
                    lista.Add(ent);

                    return lista;
                }
            }
            catch (Exception ex)
            {

                List<ListResult> lista = new List<ListResult>();

                ListResult ent = new ListResult
                {
                    cosa = ex.ToString(),
                    cosa2 = "",
                    cosa3 = "",
                    GrEmpCentro = "",
                    GrEmpOficina = "",
                    GrEmpTipoGasto = "",
                    GrEmpTarjetaToka = ""
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

        public string ConsultaValorRow(string dato, XmlElement tablarow) {
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


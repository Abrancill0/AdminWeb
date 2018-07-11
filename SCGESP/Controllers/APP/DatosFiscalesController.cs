using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.APP
{
    public class DatosFiscalesAppController : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
            public string GrConId { get; set; }
        }

        public class EmpresaIDResult
        {
            public string GrConRfc { get; set; }
            public string GrConRazonSocial { get; set; }
            public string GrConCalle { get; set; }
            public string GrConNumExt { get; set; }
            public string GrConColonia { get; set; }
            public string GrConEstado { get; set; }
            public string GrConCiudad { get; set; }
            public string GrConCodigoPostal { get; set; }
        }

        public List<EmpresaIDResult> Post(Datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencripta,
                Origen = "Programa CGE",  //Datos.Origen; 
                Transaccion = 120099,
                Operacion = 6//verifica si existe una llave y regresa una tabla de un renglon con todos los campos de la tabla
            };
            entrada.agregaElemento("GrConId", Datos.GrConId);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                string[] GrConRfc = new string[100];
                string[] GrConRazonSocial = new string[100];
                string[] GrConCalle = new string[100];
                string[] GrConNumExt = new string[100];
                string[] GrConColonia = new string[100];
                string[] GrConEstado = new string[100];
                string[] GrConCiudad = new string[100];
                string[] GrConCodigoPostal = new string[100];

                string TipoNodo = "";
                int indice;

                indice = 0;

                string xmlData = respuesta.Documento.InnerXml;

                StringBuilder output = new StringBuilder();

                using (XmlReader reader = XmlReader.Create(new StringReader(xmlData)))
                {
                    XmlWriterSettings ws = new XmlWriterSettings();
                    ws.Indent = true;
                    using (XmlWriter writer = XmlWriter.Create(output, ws))
                    {
                        while (reader.Read())
                        {
                            // Only detect start elements.
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element: // The node is an element.

                                    if (reader.Name == "GrConRfc")
                                    {
                                        TipoNodo = "GrConRfc";
                                    }
                                    else if (reader.Name == "GrConRazonSocial")
                                    {
                                        TipoNodo = "GrConRazonSocial";
                                    }
                                    else if (reader.Name == "GrConCalle")
                                    {
                                        TipoNodo = "GrConCalle";
                                    }
                                    else if (reader.Name == "GrConNumExt")
                                    {
                                        TipoNodo = "GrConNumExt";
                                    }
                                    else if (reader.Name == "GrConColonia")
                                    {
                                        TipoNodo = "GrConColonia";
                                    }
                                    else if (reader.Name == "GrConEstado")
                                    {
                                        TipoNodo = "GrConEstado";
                                    }
                                    else if (reader.Name == "GrConCiudad")
                                    {
                                        TipoNodo = "GrConCiudad";
                                    }
                                    else if (reader.Name == "GrConCodigoPostal")
                                    {
                                        TipoNodo = "GrConCodigoPostal";
                                    }

                                    break;

                                case XmlNodeType.Text:
                                    if (TipoNodo == "GrConRfc")
                                    {
                                        GrConRfc[indice] = Convert.ToString(reader.Value);
                                        TipoNodo = "";

                                    }
                                    else if (TipoNodo == "GrConRazonSocial")
                                    {
                                        GrConRazonSocial[indice] = Convert.ToString(reader.Value);
                                        TipoNodo = "";
                                    }
                                    else if (TipoNodo == "GrConCalle")
                                    {
                                        GrConCalle[indice] = Convert.ToString(reader.Value);
                                        TipoNodo = "";
                                    }
                                    else if (TipoNodo == "GrConNumExt")
                                    {
                                        GrConNumExt[indice] = Convert.ToString(reader.Value);

                                        TipoNodo = "";
                                    }
                                    else if (TipoNodo == "GrConColonia")
                                    {
                                        GrConColonia[indice] = Convert.ToString(reader.Value);

                                        TipoNodo = "";
                                    }
                                    else if (TipoNodo == "GrConEstado")
                                    {
                                        GrConEstado[indice] = Convert.ToString(reader.Value);
                                        indice = indice + 1;
                                        TipoNodo = "";
                                    }
                                    else if (TipoNodo == "GrConCiudad")
                                    {
                                        GrConCiudad[indice] = Convert.ToString(reader.Value);
                                        TipoNodo = "";
                                    }
                                    else if (TipoNodo == "GrConCodigoPostal")
                                    {
                                        GrConCodigoPostal[indice] = Convert.ToString(reader.Value);
                                        indice = indice + 1;
                                        TipoNodo = "";
                                    }
                                    break;
                            }
                        }
                    }

                    List<EmpresaIDResult> lista = new List<EmpresaIDResult>();

                    for (int i = 0; i <= indice - 1; i++)
                    {
                        EmpresaIDResult ent = new EmpresaIDResult
                        {
                            GrConRfc = GrConRfc[i],
                            GrConRazonSocial = GrConRazonSocial[i],
                            GrConCalle = GrConCalle[i],
                            GrConNumExt = GrConNumExt[i],
                            GrConColonia = GrConColonia[i],
                            GrConEstado = GrConEstado[i],
                            GrConCiudad = GrConCiudad[i],
                            GrConCodigoPostal = GrConCodigoPostal[i]
                        };

                        lista.Add(ent);
                    }


                    return lista;
                }      
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

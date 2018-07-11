using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.APP
{
    public class ConsultaMaterialAppController : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Requisicion { get; set; }
            public string Valida { get; set; }
        }

        public class ObtieneCategoriaResult
        {
            public string GrMatId { get; set; }
            public string GrMatNombre  { get; set; }
            public string GrMatPrecio { get; set; }
            public string GrMatIva { get; set; }
            public string GrMatGrupo { get; set; }
            public string GrMatUnidadMedida { get; set; }
        }
        
        public List<ObtieneCategoriaResult> Post(Datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencripta,
                Origen = "Programa CGE",  //Datos.Origen; 
                Transaccion = 120796,
                Operacion = 16//regresa una tabla con todos los campos de la tabla ( La cantidad de registros depende del filtro enviado)
            };

        
            entrada.agregaElemento("requisicion", Datos.Requisicion);
            entrada.agregaElemento("valida", Datos.Valida);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {

                string[] GrMatId = new string[100];
                string[] GrMatNombre = new string[100];
                string[] GrMatPrecio = new string[100];
                string[] GrMatIva = new string[100];
                string[] GrMatGrupo = new string[100];
                string[] GrMatUnidadMedida = new string[100];
                
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

                                    if (reader.Name == "GrMatId")
                                    {
                                        TipoNodo = "GrMatId";
                                    }
                                    else if (reader.Name == "GrMatNombre")
                                    {
                                        TipoNodo = "GrMatNombre";
                                    }
                                    else if (reader.Name == "GrMatPrecio")
                                    {
                                        TipoNodo = "GrMatPrecio";
                                    }
                                    else if (reader.Name == "GrMatIva")
                                    {
                                        TipoNodo = "GrMatIva";
                                    }
                                    else if (reader.Name == "GrMatGrupo")
                                    {
                                        TipoNodo = "GrMatGrupo";
                                    }
                                    else if (reader.Name == "GrMatUnidadMedida")
                                    {
                                        TipoNodo = "GrMatUnidadMedida";
                                    }
                                    
                                    break;

                                case XmlNodeType.Text:
                                    if (TipoNodo == "GrMatId")
                                    {
                                        GrMatId[indice] = Convert.ToString(reader.Value);
                                        TipoNodo = "";

                                    }
                                    else if (TipoNodo == "GrMatNombre")
                                    {
                                        GrMatNombre[indice] = Convert.ToString(reader.Value);
                                        TipoNodo = "";
                                    }
                                    else if (TipoNodo == "GrMatPrecio")
                                    {
                                        GrMatPrecio[indice] = Convert.ToString(reader.Value);
                                        TipoNodo = "";
                                    }
                                    else if (TipoNodo == "GrMatIva")
                                    {
                                        GrMatIva[indice] = Convert.ToString(reader.Value);
                                     
                                        TipoNodo = "";
                                    }
                                    else if (TipoNodo == "GrMatGrupo")
                                    {
                                        GrMatGrupo[indice] = Convert.ToString(reader.Value);
                                       
                                        TipoNodo = "";
                                    }
                                    else if (TipoNodo == "GrMatUnidadMedida")
                                    {
                                        GrMatUnidadMedida[indice] = Convert.ToString(reader.Value);
                                        indice = indice + 1;
                                        TipoNodo = "";
                                    }
                                    break;
                            }
                        }
                    }

                    List<ObtieneCategoriaResult> lista = new List<ObtieneCategoriaResult>();

                    for (int i = 0; i <= indice-1; i++)
                    {
                        ObtieneCategoriaResult ent = new ObtieneCategoriaResult
                        {
                            GrMatId = GrMatId[i],
                            GrMatNombre = GrMatNombre[i],
                            GrMatPrecio = GrMatPrecio[i],
                            GrMatIva = GrMatIva[i],
                            GrMatGrupo = GrMatGrupo[i],
                            GrMatUnidadMedida = GrMatUnidadMedida[i],
                           
                        };

                        lista.Add(ent);
                    }

                  
                    return lista;
                }

                

               // return respuesta.Documento;
            }
            else
            {
                var errores = respuesta.Documento;

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

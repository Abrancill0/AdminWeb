using Ele.Generales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace SCGESP.Clases
{
    public class EnvioCorreosELE
    {

        public class AutorizaRequisicionResult
        {
            public int Resultado { get; set; }
        }

        public static XmlDocument Envio(string Usuario, string correo, string EmpleadoID, string UsuarioID, string correoCopia, string Asunto, string Mensaje, int usuEncriptado)
        {
            try
            {
                string UsuarioDesencripta = "";
                if (usuEncriptado == 1)
                {
                    UsuarioDesencripta = Clases.Seguridad.DesEncriptar(Usuario);
                }
                else {
                    UsuarioDesencripta = Usuario;
                }
                

                if (correo == "" && EmpleadoID == "" && UsuarioID == "") {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml("<Error>No se cuenta con un correo o con una forma para recuperarlo.</Error>");
                    return xml;
                }

                DocumentoEntrada entrada = new DocumentoEntrada
                {
                    Usuario = UsuarioDesencripta,
                    Origen = "Programa CGE",
                    Transaccion = 3
                };

                string Empleado = "";
                string Correo = "";

                DataTable DTCorreo = new DataTable();

                if (correo == "")
                {
                    if (EmpleadoID == "")
                    {
                        Empleado = ObtieneEmpelado(UsuarioID, UsuarioDesencripta);

                        Correo = ObtieneCorreo(Empleado, UsuarioDesencripta);
                    }
                    else
                    {
                        Correo = ObtieneCorreo(EmpleadoID, UsuarioDesencripta);
                    }

                }
                else
                {
                    Correo = correo;
                }

                entrada.agregaElemento("Para", Correo);
                entrada.agregaElemento("Copia", correoCopia);
                entrada.agregaElemento("Asunto", Asunto);
                entrada.agregaElemento("Mensaje", Mensaje);

                DocumentoSalida respuesta = PeticionGeneral(entrada.Documento);

                if (respuesta.Resultado == "1")
                {
                    return respuesta.Documento;
                }
                else
                {
                    return respuesta.Documento;
                }
            }
            catch (System.Exception ex)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml("<Error>" + ex.ToString() + "</Error>");

                return xml;
            }

        }

        public static DocumentoSalida PeticionGeneral(XmlDocument doc)
        {
            Localhost.Elegrp ws = new Localhost.Elegrp();
            ws.Timeout = -1;
            string respuesta = ws.PeticionGeneral(doc);
            return new DocumentoSalida(respuesta);
        }

        public static string ObtieneEmpelado(string UsuarioID, string UsuarioDesencriptado)
        {

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencriptado,
                Origen = "Programa CGE",  //Datos.Origen; 
                Transaccion = 100004,
                Operacion = 6//verifica si existe una llave y regresa una tabla de un renglon con todos los campos de la tabla
            };
            entrada.agregaElemento("SgUsuId", UsuarioID);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            DataTable DTEmpleado = new DataTable();

            string EmpleadoResult = "";

            if (respuesta.Resultado == "1")
            {

                DTEmpleado = respuesta.obtieneTabla("Catalogo");

                for (int i = 0; i < DTEmpleado.Rows.Count; i++)
                {
                    EmpleadoResult = Convert.ToString(DTEmpleado.Rows[i]["GrEmpId"]);
                }

                return EmpleadoResult;

            }
            else
            {
                var errores = respuesta.Errores;

                return "";
            }
        }

        public static string ObtieneCorreo(string Empelado, string UsuarioDesencriptado)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(UsuarioDesencriptado);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencripta,
                Origen = "Programa CGE",  //Datos.Origen; 
                Transaccion = 120037,
                Operacion = 6//verifica si existe una llave y regresa una tabla de un renglon con todos los campos de la tabla
            };
            entrada.agregaElemento("GrEmpId", Empelado);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            DataTable DTCorreo = new DataTable();
            string CorreoResult = "";

            if (respuesta.Resultado == "1")
            {
                DTCorreo = respuesta.obtieneTabla("Catalogo");

                for (int i = 0; i < DTCorreo.Rows.Count; i++)
                {
                    CorreoResult = Convert.ToString(DTCorreo.Rows[i]["GrEmpCorreoElectronico"]);
                }

                return CorreoResult;
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
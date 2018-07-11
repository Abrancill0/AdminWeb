using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;
using System;

namespace SCGESP.Controllers
{
    public class RequisicionesListaPendientesUsuController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string Empleado { get; set; }
            public string Origen { get; set; }
        }

        public class RequisicionEncabezadoResult
        {
            public int Resultado { get; set; }

        }

        public XmlDocument Post(datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            string EmpleadoDesencripta = Seguridad.DesEncriptar(Datos.Empleado);

            DocumentoEntrada entrada = new DocumentoEntrada();
            entrada.Usuario = UsuarioDesencripta; //Datos.Usuario;  
            entrada.Origen = "Programa CGE";  //Datos.Origen; 
            entrada.Transaccion = 120760;
            entrada.Operacion = 1;
            //entrada.agregaElemento("proceso", 9);
            entrada.agregaElemento("RmReqSolicitante", Convert.ToInt32(EmpleadoDesencripta));



            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                return respuesta.Documento;
            }
            else
            {
                var errores = respuesta.Errores;

                return null;
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


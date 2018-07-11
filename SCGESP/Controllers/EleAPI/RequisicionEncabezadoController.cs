using Ele.Generales;
using System.Xml;
using System.Web.Http;
using System.Collections.Generic;
using System;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class RequisicionEncabezadoController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }

            public string Empleado { get; set; }
            public string Origen { get; set; }
            public string RmReqId { get; set; }
            public string RmReqEstatus { get; set; }
            public string RmReqTipoRequisicion { get; set; }
            public string RmReqFechaRequisicion { get; set; }
            public string RmReqFechaRequrida { get; set; }
            public string RmReqFechaFinal { get; set; }
            public string RmReqSolicitante { get; set; }
            public string RmReqTipoDeGasto { get; set; }
            public string RmReqCentro { get; set; }
            public string RmReqOficina { get; set; }
            public string RmReqSubramo { get; set; }
            public string RmReqJustificacion { get; set; }
        }

      

        public string Post(datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);
            string EmpleadoDesencripta = Seguridad.DesEncriptar(Datos.Empleado);
            
            DocumentoEntrada entrada = new DocumentoEntrada();
            entrada.Usuario = UsuarioDesencripta;  
            entrada.Origen = "Programa CGE";  //Datos.Origen; 
            entrada.Transaccion = 120760;
            entrada.Operacion = 2;

            entrada.agregaElemento("RmReqId", Datos.RmReqId);
            entrada.agregaElemento("RmReqEstatus", Datos.RmReqEstatus);
            entrada.agregaElemento("RmReqTipoRequisicion", Datos.RmReqTipoRequisicion);
            entrada.agregaElemento("RmReqFechaRequisicion", Datos.RmReqFechaRequisicion);
            entrada.agregaElemento("RmReqFechaRequerida", Datos.RmReqFechaRequrida);
            entrada.agregaElemento("RmReqFechaFinal", Datos.RmReqFechaFinal);
            entrada.agregaElemento("RmReqSolicitante", EmpleadoDesencripta); 
            entrada.agregaElemento("RmReqTipoGasto", Datos.RmReqTipoDeGasto);
            entrada.agregaElemento("RmReqCentro", Datos.RmReqCentro);
            entrada.agregaElemento("RmReqOficina", Datos.RmReqOficina);
            entrada.agregaElemento("RmReqSubramo", Datos.RmReqSubramo);
            entrada.agregaElemento("RmReqJustificacion", Datos.RmReqJustificacion);
            entrada.agregaElemento("RmReqEstatusSiguiente", 3);
            entrada.agregaElemento("RmReqUrgente", 0);
            entrada.agregaElemento("RmReqMoneda", 1);
            entrada.agregaElemento("RmReqTipoCambio", 1);
            
            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                var resultado = respuesta.obtieneValor("RmReqId");

                return resultado;
            }
            else
            {
                var errores = respuesta.Errores;
                XmlElement tablaError = errores;
                string error = ConsultaValorRow("Descripcion", tablaError);
                return error;
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
            XmlNode valor = TablaRow.SelectSingleNode("Error/" + dato);

            string respuesta = "";

            try
            {
                if (valor != null)
                    respuesta = valor.InnerText.Trim();
                else
                    respuesta = "Error.";
            }
            catch (InvalidCastException e)
            {
                var error = Convert.ToString(e);
                respuesta = error;
            }
            return respuesta;
        }
    }
}

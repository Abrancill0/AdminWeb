using Ele.Generales;
using System.Xml;
using System.Web.Http;
using System.Collections.Generic;
using System;
using SCGESP.Clases;

namespace SCGESP.Controllers.EleAPI
{
    public class RequisicionDetalleActualizaController : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
            public string RmRdeRequisicion { get; set; }
            public string RmRdeId { get; set; }
            public string RmRdeMaterial { get; set; }
            public string RmRdeEstatus { get; set; }
            public string RmRdeCantidadSolicitada { get; set; }
            public string RmRdeDescripcion { get; set; }
            public string RmRdeUnidadSolicitada { get; set; }
            public string RmRdeGrupoMaterial { get; set; }
            public string RmRdeCuenta { get; set; }
            public string RmRdePrecioUnitario { get; set; }
            public string RmRdePorcIva { get; set; }
        }

        public string Post(datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada();
            entrada.Usuario = UsuarioDesencripta; //Datos.Usuario;  
            entrada.Origen = "Programa CGE";  //Datos.Origen; 
            entrada.Transaccion = 120762;
            entrada.Operacion = 3;

            entrada.agregaElemento("RmRdeRequisicion", Datos.RmRdeRequisicion);
            entrada.agregaElemento("RmRdeId", Datos.RmRdeId);
            entrada.agregaElemento("RmRdeMaterial", Datos.RmRdeMaterial);
            entrada.agregaElemento("RmRdeEstatus", Datos.RmRdeEstatus);
            entrada.agregaElemento("RmRdeCantidadSolicitada", Datos.RmRdeCantidadSolicitada);
            entrada.agregaElemento("RmRdeDescripcion", Datos.RmRdeDescripcion);
            entrada.agregaElemento("RmRdeUnidadSolicitada", Datos.RmRdeUnidadSolicitada);
            entrada.agregaElemento("RmRdeGrupoMaterial", Datos.RmRdeGrupoMaterial);
            entrada.agregaElemento("RmRdeCuenta", Datos.RmRdeCuenta);
            entrada.agregaElemento("RmRdePrecioUnitario", Datos.RmRdePrecioUnitario);
            //double Iva = Convert.ToDouble(Datos.RmRdePrecioUnitario) * 0.16;
            entrada.agregaElemento("RmRdePorcIva", Datos.RmRdePorcIva);


            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                var resultado = respuesta.obtieneValor("RmRdeId");

                return resultado;
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


using Ele.Generales;
using System.Xml;
using System.Web.Http;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class RequisicionDotacionController : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
            public string RmReqId { get; set; }
            public string FechaPago { get; set; }
            public decimal Importe { get; set; }
            public string Benificiario { get; set; }

        }
        public XmlDocument Post(Datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencripta,
                Origen = "Programa CGE",  //Datos.Origen; 
                Transaccion = 120760,
                Operacion = 14//regresa una tabla con todos los campos de la tabla ( La cantidad de registros depende del filtro enviado)
            };

            entrada.agregaElemento("RmReqId", Datos.RmReqId);
            entrada.agregaElemento("FechaPago", Datos.FechaPago);
            entrada.agregaElemento("Importe", Datos.Importe);
            entrada.agregaElemento("MetodoPago", 4);
            entrada.agregaElemento("Benificiario", Datos.Benificiario);
            entrada.agregaElemento("CuentaBancaria", 1);
            entrada.agregaElemento("NumeroCheque", 0);
            entrada.agregaElemento("TarjetaToka", "5117650402441001");

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                return respuesta.Documento;
            }
            else
            {
                var errores = respuesta.Documento;

                return errores;
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

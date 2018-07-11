using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.EleAPI
{
    public class RequisicionReembolsoController : ApiController
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
            entrada.agregaElemento("RmReqEstatus", Datos.RmReqEstatus);//1
            entrada.agregaElemento("RmReqTipoRequisicion", Datos.RmReqTipoRequisicion);//reembolso
            entrada.agregaElemento("RmReqFechaRequisicion", Datos.RmReqFechaRequisicion);//fecha hoy
            entrada.agregaElemento("RmReqFechaRequerida", Datos.RmReqFechaRequrida);//pendiente
            entrada.agregaElemento("RmReqFechaFinal", Datos.RmReqFechaFinal);//pendiente
            entrada.agregaElemento("RmReqSolicitante", EmpleadoDesencripta);//si lo traemos
            entrada.agregaElemento("RmReqTipoGasto", Datos.RmReqTipoDeGasto);//checarlo
            entrada.agregaElemento("RmReqCentro", Datos.RmReqCentro);//viene del ususario
            entrada.agregaElemento("RmReqOficina", Datos.RmReqOficina);//viene del ususario
            entrada.agregaElemento("RmReqSubramo", Datos.RmReqSubramo);//viene del ususario
            entrada.agregaElemento("RmReqJustificacion", Datos.RmReqJustificacion);//remboslol
            entrada.agregaElemento("RmReqEstatusSiguiente", 3);//checar con pepe
            entrada.agregaElemento("RmReqUrgente", 0);//

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                var RmReqId = respuesta.obtieneValor("RmReqId");
                
                return "";
            }
            else
            {
                var errores = respuesta.Errores;

                return null;
            }
            
        }


        public static string Detalle(datos Datos)
        {

            //

            SqlCommand comando = new SqlCommand("ObtieneGastosEfectivo");
            comando.CommandType = CommandType.StoredProcedure;

            //Declaracion de parametros
            comando.Parameters.Add("@idinforme", SqlDbType.Int);

            //Asignacion de valores a parametros
            comando.Parameters["@idinforme"].Value = 1;

            comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
            comando.CommandTimeout = 0;
            comando.Connection.Open();
            //DA.SelectCommand = comando;
            // comando.ExecuteNonQuery();

            DataTable DT = new DataTable();
            SqlDataAdapter DA = new SqlDataAdapter(comando);
            comando.Connection.Close();
            DA.Fill(DT);


           // List<ListResult> lista = new List<ListResult>();

            if (DT.Rows.Count > 0)
            {

            }





                string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada();
            entrada.Usuario = UsuarioDesencripta; //Datos.Usuario;  
            entrada.Origen = "Programa CGE";  //Datos.Origen; 
            entrada.Transaccion = 120762;
            entrada.Operacion = 2;

            //entrada.agregaElemento("RmRdeRequisicion", Datos.RmRdeRequisicion);
            //entrada.agregaElemento("RmRdeMaterial", Datos.GrMatId);
            //entrada.agregaElemento("RmRdeId", Datos.RmRdeId);
            //entrada.agregaElemento("RmRdeEstatus", Datos.RmRdeEstatus);//1 por default 
            //entrada.agregaElemento("RmRdeCantidadSolicitada", Datos.RmRdeCantidadSolicitada);
            ////entrada.agregaElemento("RmRdeDescripcion", Datos.RmRdeDescripcion);
            //entrada.agregaElemento("RmRdeUnidadSolicitada", Datos.RmRdeUnidadSolicitada);
            //entrada.agregaElemento("RmRdeGrupoMaterial", Datos.RmRdeGrupoMaterial);
            //entrada.agregaElemento("RmRdeCuenta", Datos.RmRdeCuenta);
            //entrada.agregaElemento("RmRdePrecioUnitario", Datos.RmRdePrecioUnitario);//pend
            //// double Iva = Convert.ToDouble(Datos.RmRdePrecioUnitario) * 0.16;//pend
            //entrada.agregaElemento("RmRdePorcIva", Convert.ToString(Datos.RmRdePorcIva));//pend

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


        public static string GeneraRequisicion(datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada();
            entrada.Usuario = UsuarioDesencripta;
            entrada.Origen = "Programa CGE";  //Datos.Origen; 
            entrada.Transaccion = 120760;
            entrada.Operacion = 9;

            entrada.agregaElemento("RmReqId", Datos.RmReqId);

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            if (respuesta.Resultado == "1")
            {
                //var resultado = "Requisicion Generadacorrectamente";

                return "";
            }
            else
            {
                var errores = respuesta;

                return "";
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

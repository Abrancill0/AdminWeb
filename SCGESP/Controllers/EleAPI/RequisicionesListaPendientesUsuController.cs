using Ele.Generales;
using System.Xml;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using SCGESP.Clases;
using System;
using System.Linq;

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
            public DataTable Resultado { get; set; }

        }

        public DataTable Post(datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            string EmpleadoDesencripta = Seguridad.DesEncriptar(Datos.Empleado);

			DataTable Requisiciones = new DataTable();

			try
			{
				DataTable RequisicionesTrabajando = ObtenerRequisicionesTrabajando(UsuarioDesencripta);
				DataTable RequisicionesSolicitante = ObtenerRequisicionesSolicitante(UsuarioDesencripta, EmpleadoDesencripta);
				Requisiciones =  MisRequisiciones(RequisicionesTrabajando, RequisicionesSolicitante);
				return Requisiciones;
			}
			catch (Exception ex)
			{
				return Requisiciones;
			}
			
		}
		
		private DataTable ObtenerRequisicionesTrabajando(string Usuario)
		{
			DataTable DTRequisiciones = new DataTable();
			try
			{
				DocumentoEntrada entrada = new DocumentoEntrada
				{
					Usuario = Usuario, //Datos.Usuario;  
					Origen = "AdminWEB",  //Datos.Origen; 
					Transaccion = 120760,
					Operacion = 1
				};
				entrada.agregaElemento("proceso", 1);

				DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

				if (respuesta.Resultado == "1")
				{

					DTRequisiciones = respuesta.obtieneTabla("Catalogo");
				}
				return DTRequisiciones;
			}
			catch (Exception)
			{

				return DTRequisiciones;
			}
		}

		private DataTable ObtenerRequisicionesSolicitante(string Usuario, string Empleado)
		{
			DataTable DTRequisiciones = new DataTable();
			try
			{
				string fechaInicial = (DateTime.Today.Subtract(TimeSpan.FromDays(120))).ToString("dd/MM/yyyy");
				string fechaFinal = (DateTime.Today.AddDays(30)).ToString("dd/MM/yyyy");

				DocumentoEntrada entrada = new DocumentoEntrada();
				entrada.Usuario = Usuario; //Datos.Usuario;  
				entrada.Origen = "AdminWEB";  //Datos.Origen; 
				entrada.Transaccion = 120760;
				entrada.Operacion = 1;
				entrada.agregaElemento("RmReqSolicitante", Convert.ToInt32(Empleado));
				entrada.agregaElemento("FechaInicial", fechaInicial);//fechaInicial.ToString("dd/MM/yyyy")
				entrada.agregaElemento("FechaFinal", fechaFinal);
				
				DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

				if (respuesta.Resultado == "1")
				{

					DTRequisiciones = respuesta.obtieneTabla("Catalogo");
				}

				if (respuesta.Resultado == "1")
				{

					DTRequisiciones = respuesta.obtieneTabla("Catalogo");
				}
				return DTRequisiciones;
			}
			catch (Exception)
			{

				return DTRequisiciones;
			}
		}

		private DataTable MisRequisiciones(DataTable RequisicionesTrabajando, DataTable RequisicionesSolicitante)
		{
			DataTable DTRequisiciones = new DataTable();
			try
			{
				if (RequisicionesTrabajando.Rows.Count > 0 && RequisicionesSolicitante.Rows.Count > 0)
				{
					RequisicionesTrabajando.Merge(RequisicionesSolicitante);
					string[] TobeDistinct = {
						"RmReqId", "RmReqSolicitante", "RmReqEstatusNombre", "RmReqEstatus", "RmReqJustificacion",
						"RmReqSolicitanteNombre", "RmReqOficinaNombre", "RmReqSubramoNombre" };
					RequisicionesTrabajando = RequisicionesTrabajando
						.DefaultView.ToTable(true, TobeDistinct)
						.AsEnumerable()
						.CopyToDataTable();
					DataView VwRequisiciones = RequisicionesTrabajando.DefaultView;
					VwRequisiciones.Sort = "RmReqId DESC";
					DTRequisiciones = VwRequisiciones.ToTable();
				}
				else if (RequisicionesTrabajando.Rows.Count > 0 && RequisicionesSolicitante.Rows.Count == 0)
				{
					DTRequisiciones = RequisicionesTrabajando;
				}
				else if (RequisicionesTrabajando.Rows.Count == 0 && RequisicionesSolicitante.Rows.Count > 0)
				{
					DTRequisiciones = RequisicionesSolicitante;
				}
				return DTRequisiciones;
			}
			catch (Exception)
			{
				return DTRequisiciones;
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


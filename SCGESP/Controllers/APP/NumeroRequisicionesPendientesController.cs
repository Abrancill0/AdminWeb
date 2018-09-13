using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml;

namespace SCGESP.Controllers.APP
{
    public class NumeroRequisicionesPendientesAppController : ApiController
    {
        public class Datos
        {
            public string Usuario { get; set; }
            public string Origen { get; set; }
        }

        public class NUmeroRequisicionesResult
		{
            public string Tipo { get; set; }
			public int NumeroRequisiciones { get; set; }

		}

        public List<NUmeroRequisicionesResult> Post(Datos Datos)
        {
            string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = UsuarioDesencripta,
                Origen = "AdminApp",  
                Transaccion = 120760,
                Operacion = 1
            };

            entrada.agregaElemento("proceso", "2");

            DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

            DataTable DTRequisiciones = new DataTable();

            if (respuesta.Resultado == "1")
            {
                DTRequisiciones = respuesta.obtieneTabla("Catalogo");

				int NumReq = DTRequisiciones.Rows.Count;

                List<NUmeroRequisicionesResult> lista = new List<NUmeroRequisicionesResult>();
				
					NUmeroRequisicionesResult ent = new NUmeroRequisicionesResult
					{
                        Tipo = "Requisiciones Pendientes",
						NumeroRequisiciones = NumReq

					};
                    lista.Add(ent);
                
                return lista;
            }
            else
            {
				List<NUmeroRequisicionesResult> lista = new List<NUmeroRequisicionesResult>();

				NUmeroRequisicionesResult ent = new NUmeroRequisicionesResult
				{
					Tipo = "Requisiciones Pendientes",
					NumeroRequisiciones = 0

				};
				lista.Add(ent);

				return lista;
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

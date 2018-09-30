using Ele.Generales;
using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

            List<NUmeroRequisicionesResult> lista = new List<NUmeroRequisicionesResult>();

            try
            {
                SqlCommand comando = new SqlCommand("CountInformeApp");
                comando.CommandType = CommandType.StoredProcedure;

                //Declaracion de parametros
                comando.Parameters.Add("@uconsulta", SqlDbType.VarChar);
                //comando.Parameters.Add("@idempresa", SqlDbType.Int);

                //Asignacion de valores a parametros
                comando.Parameters["@uconsulta"].Value = UsuarioDesencripta;

                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();
                //DA.SelectCommand = comando;
                //comando.ExecuteNonQuery();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);
                
                if (DT.Rows.Count > 0)
                {

                    foreach (DataRow row in DT.Rows)
                    {
                        NUmeroRequisicionesResult ent = new NUmeroRequisicionesResult
                        {
                            Tipo = "Informes Pendientes",
                            NumeroRequisiciones = Convert.ToInt32(row["NumeroInformes"])
                        };

                        lista.Add(ent);
                        
                    };
                    
                }
                else
                {
                    NUmeroRequisicionesResult ent = new NUmeroRequisicionesResult
                    {
                        Tipo = "Informes Pendientes",
                        NumeroRequisiciones =0
                    };

                    lista.Add(ent);
                }
                
            }
            catch (Exception ex)
            {
                NUmeroRequisicionesResult ent = new NUmeroRequisicionesResult
                {
                    Tipo = "Informes Pendientes",
                    NumeroRequisiciones = 0
                };

                lista.Add(ent);
            }

            
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

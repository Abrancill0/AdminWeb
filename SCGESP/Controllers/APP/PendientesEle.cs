using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class PendientesEleController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }

        }

        public class ObtieneParametrosSalida
        {
            public string Proceso { get; set; }
            public string ProcesoNombre { get; set; }
            public string Registros { get; set; }
        }

        public class AdminWebPendientesSalida
        {
            public string Tipo { get; set; }
            public int Numero { get; set; }

        }


        //public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        {
            try
            {
               //string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

                var ObtienePendientes = InformesPendientes(Datos.Usuario);

                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                {
                    Proceso = "Proceso AdminWeb 01",
                    ProcesoNombre = "Informes Pendientes",
                    Registros = Convert.ToString(ObtienePendientes.Where(p => p.Tipo== "Informes").Select(p => p.Numero).SingleOrDefault()),

                };
                lista.Add(ent);
                

                DocumentoEntrada entrada = new DocumentoEntrada
                {
                    Usuario = Datos.Usuario,
                    Origen = "AdminApp",  //Datos.Origen; 
                    Transaccion = 100004, //usuario
                    Operacion = 18
                };
                entrada.agregaElemento("SgUsuId", Datos.Usuario);
                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                DataTable DTLista = new DataTable();

                if (respuesta.Resultado == "1")
                {
                    DTLista = respuesta.obtieneTabla("Pendientes");

                    int NumOCVobo = DTLista.Rows.Count;

                   // List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                    foreach (DataRow row in DTLista.Rows)
                    {
                        ObtieneParametrosSalida ent1 = new ObtieneParametrosSalida
                        {
                            Proceso = Convert.ToString(row["Proceso"]),
                            ProcesoNombre = Convert.ToString(row["ProcesoNombre"]),
                            Registros = Convert.ToString(row["Registros"]),

                        };
                        lista.Add(ent1);
                    }
                    return lista;
                }
                else
                {
                    //List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                    ObtieneParametrosSalida ent2 = new ObtieneParametrosSalida
                    {
                        Proceso = "Error",

                    };

                    lista.Add(ent2);

                    return lista;

                }
            }
            catch (System.Exception)
            {

                return null;
            }



        }

        public List<AdminWebPendientesSalida> InformesPendientes(string Usuario)
        {
            List<AdminWebPendientesSalida> lista = new List<AdminWebPendientesSalida>();

            try
            {
                SqlCommand comando = new SqlCommand("CountInformeApp");
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@uconsulta", SqlDbType.VarChar);

                comando.Parameters["@uconsulta"].Value = Usuario;

                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();
             
                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow row in DT.Rows)
                    {
                        AdminWebPendientesSalida ent = new AdminWebPendientesSalida
                        {
                            Tipo = Convert.ToString(row["Tipo"]),
                            Numero = 0 //Convert.ToInt32(row["NumeroInformes"])
                        };

                        lista.Add(ent);
                        
                    };

                    return lista;
                }
                else
                {
                    AdminWebPendientesSalida ent = new AdminWebPendientesSalida
                    {
                        Tipo = "Informes",
                        Numero = 0
                    };

                    lista.Add(ent);

                    return lista;
                }

            }
            catch (Exception ex)
            {
                AdminWebPendientesSalida ent = new AdminWebPendientesSalida
                {
                    Tipo = "Informes",
                    Numero = 0
                };

                lista.Add(ent);

                return lista;
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

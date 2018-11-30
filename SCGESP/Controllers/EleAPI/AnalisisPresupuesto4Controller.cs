using Ele.Generales;
using SCGESP.Clases;
using System.Data;
using System.Web.Http;
using System.Xml;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;
using System;

namespace SCGESP.Controllers.EleAPI
{
    public class AnalisisPresupuesto4Controller : ApiController
    {
        public class datos
        {
            public string Usuario { get; set; }
            public string RmReqId { get; set; }
        }

        public class AnalisisPresupuestoResult
        {
            public string DimCentro { get; set; }
            public string Centro { get; set; }
            public string CentroNombre { get; set; }
            public string DimOficina { get; set; }
            public string Oficina { get; set; }
            public string OficinaNombre { get; set; }
            public string DimRamo { get; set; }
            public string Ramo { get; set; }
            public string RamoNombre { get; set; }
            public string DimCuenta { get; set; }
            public string Cuenta { get; set; }
            public string CuentaNombre { get; set; }
            public string ModificadoMes { get; set; }
            public string ModificadoAcumulado { get; set; }
            public string ModificadoAnual { get; set; }
            public string DisponibleMes { get; set; }
            public string DisponibleAcumulado { get; set; }
            public string DisponibleAnual { get; set; }
            public string PrecomprometidoMes { get; set; }
            public string PrecomprometidoAcumulado { get; set; }
            public string PrecomprometidoAnual { get; set; }
            public string ComprometidoMes { get; set; }
            public string ComprometidoAcumulado { get; set; }
            public string ComprometidoAnual { get; set; }
            public string DevengadoMes { get; set; }
            public string DevengadoAcumulado { get; set; }
            public string DevengadoAnual { get; set; }
            public string PagadoMes { get; set; }
            public string PagadoAcumulado { get; set; }
            public string PagadoAnual { get; set; }
            public string Requerido { get; set; }

            public string Anio { get; set; }
            public string Mes { get; set; }
            public string Dia { get; set; }
        }

        public List<AnalisisPresupuestoResult> Post(datos Datos)
        {
            try
            {
                string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

                DocumentoEntrada entrada = new DocumentoEntrada
                {
                    Usuario = UsuarioDesencripta,
                    Origen = "Programa CGE",
                    Transaccion = 120760,
                    Operacion = 16
                };

                //entrada.agregaElemento("SgUsuMostrarGraficaAlAutorizar", 1);
                entrada.agregaElemento("RmReqId", Datos.RmReqId);

                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                DataTable DTResultado = new DataTable();

                DataTable DTAnalisis = new DataTable();

                if (respuesta.Resultado == "1")
                {
                    DTAnalisis = respuesta.obtieneTabla("AnalisisPresupuesto");

                    DTResultado = respuesta.obtieneTabla("Salida");

                    List<AnalisisPresupuestoResult> lista = new List<AnalisisPresupuestoResult>();

                    for (int i = 0; i < DTAnalisis.Rows.Count; i++)
                    {
                        AnalisisPresupuestoResult ent = new AnalisisPresupuestoResult
                        {
                            DimCentro = Convert.ToString(DTAnalisis.Rows[i]["DimCentro"]),
                            Centro = Convert.ToString(DTAnalisis.Rows[i]["Centro"]),
                            CentroNombre = Convert.ToString(DTAnalisis.Rows[i]["CentroNombre"]),
                            DimOficina = Convert.ToString(DTAnalisis.Rows[i]["DimOficina"]),
                            Oficina = Convert.ToString(DTAnalisis.Rows[i]["Oficina"]),
                            OficinaNombre = Convert.ToString(DTAnalisis.Rows[i]["OficinaNombre"]),
                            DimRamo = Convert.ToString(DTAnalisis.Rows[i]["DimRamo"]),
                            Ramo = Convert.ToString(DTAnalisis.Rows[i]["Ramo"]),
                            RamoNombre = Convert.ToString(DTAnalisis.Rows[i]["RamoNombre"]),
                            DimCuenta = Convert.ToString(DTAnalisis.Rows[i]["DimCuenta"]),
                            Cuenta = Convert.ToString(DTAnalisis.Rows[i]["Cuenta"]),
                            CuentaNombre = Convert.ToString(DTAnalisis.Rows[i]["CuentaNombre"]),
                            ModificadoMes = Convert.ToString(DTAnalisis.Rows[i]["ModificadoMes"]),
                            ModificadoAcumulado = Convert.ToString(DTAnalisis.Rows[i]["ModificadoAcumulado"]),
                            ModificadoAnual = Convert.ToString(DTAnalisis.Rows[i]["ModificadoAnual"]),
                            DisponibleMes = Convert.ToString(DTAnalisis.Rows[i]["DisponibleMes"]),
                            DisponibleAcumulado = Convert.ToString(DTAnalisis.Rows[i]["DisponibleAcumulado"]),
                            DisponibleAnual = Convert.ToString(DTAnalisis.Rows[i]["DisponibleAnual"]),
                            PrecomprometidoMes = Convert.ToString(DTAnalisis.Rows[i]["PrecomprometidoMes"]),
                            PrecomprometidoAcumulado = Convert.ToString(DTAnalisis.Rows[i]["PrecomprometidoAcumulado"]),
                            PrecomprometidoAnual = Convert.ToString(DTAnalisis.Rows[i]["PrecomprometidoAnual"]),
                            ComprometidoMes = Convert.ToString(DTAnalisis.Rows[i]["ComprometidoMes"]),
                            ComprometidoAcumulado = Convert.ToString(DTAnalisis.Rows[i]["ComprometidoAcumulado"]),
                            ComprometidoAnual = Convert.ToString(DTAnalisis.Rows[i]["ComprometidoAnual"]),
                            DevengadoMes = Convert.ToString(DTAnalisis.Rows[i]["DevengadoMes"]),
                            DevengadoAcumulado = Convert.ToString(DTAnalisis.Rows[i]["DevengadoAcumulado"]),
                            DevengadoAnual = Convert.ToString(DTAnalisis.Rows[i]["DevengadoAnual"]),
                            PagadoMes = Convert.ToString(DTAnalisis.Rows[i]["PagadoMes"]),
                            PagadoAcumulado = Convert.ToString(DTAnalisis.Rows[i]["PagadoAcumulado"]),
                            PagadoAnual = Convert.ToString(DTAnalisis.Rows[i]["PagadoAnual"]),
                            Requerido = Convert.ToString(DTAnalisis.Rows[i]["Requerido"]),


                            Anio = Convert.ToString(DTResultado.Rows[0]["Anio"]),
                            Mes = Convert.ToString(DTResultado.Rows[0]["Mes"]),
                            Dia = Convert.ToString(DTResultado.Rows[0]["Dia"]),

                        };

                        lista.Add(ent);
                    }

                    return lista;
                }
                else
                {
                    List<AnalisisPresupuestoResult> lista = new List<AnalisisPresupuestoResult>();

                    AnalisisPresupuestoResult ent = new AnalisisPresupuestoResult
                    {
                        DimCentro = Convert.ToString("Error"),
                        Centro = Convert.ToString("Error"),
                        CentroNombre = Convert.ToString("Error"),
                        DimOficina = Convert.ToString("Error"),
                        Oficina = Convert.ToString("Error"),
                        OficinaNombre = Convert.ToString("Error"),
                    };

                    lista.Add(ent);

                    return lista;
                }
            }
            catch (System.Exception ex)
            {
                List<AnalisisPresupuestoResult> lista = new List<AnalisisPresupuestoResult>();

                AnalisisPresupuestoResult ent = new AnalisisPresupuestoResult
                {
                    DimCentro = Convert.ToString("Error"),
                    Centro = Convert.ToString("Error"),
                    CentroNombre = Convert.ToString(ex.ToString()),
                    DimOficina = Convert.ToString("Error"),
                    Oficina = Convert.ToString("Error"),
                    OficinaNombre = Convert.ToString("Error"),
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


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Ele.Generales;
using SCGESP.Clases;

namespace SCGESP.Controllers
{
    public class SolicitudMovimientoActivoFijoController : ApiController
    {
        //Parametros Entrada
        public class ParametrosEntrada
        {
            public string Usuario { get; set; }
        }
        //Parametros Salida
        public class ObtieneParametrosSalida
        {
            public string AfVarID { get; set; }
            public string AfVarEstatus { get; set; }
            public string AfVarEstatusNombre { get; set; }
            public string AfVarTipoMovimiento { get; set; }
            public string AfVarTipoMovimientoNombre { get; set; }
            public string AfVarMotivoMovimiento { get; set; }
            public string AfVarMotivoMovimientoNombre { get; set; }
            public string AfVarFechaSolicitud { get; set; }
            public string AfVarEmpleadoSolicita { get; set; }
            public string AfVarEmpleadoSolicitaNombre { get; set; }
            public string AfVarRubroActivo { get; set; }
            public string AfVarRubroActivoNombre { get; set; }
            public string AfVarEmpleado { get; set; }
            public string AfVarEmpleadoNombre { get; set; }
            public string AfVarComentarios { get; set; }
            public string AfVarFechaFin { get; set; }
            public string AfVarFechaDev { get; set; }
            public string AfVarProveedor { get; set; }
            public string AfVarProveedorNombre { get; set; }
            public string AfVarDireccion { get; set; }
            public string AfVarTelefono { get; set; }
            public string AfVarCorreoElectronico { get; set; }
            public string AfVarFechaAsignacion { get; set; }
            public string AfVarCentroOrigen { get; set; }
            public string AfVarCentroOrigenNombre { get; set; }
            public string AfVarCentro { get; set; }
            public string AfVarCentroNombre { get; set; }
            public string AfVarUbicacion { get; set; }
            public string AfVarUbicacionNombre { get; set; }
            public string AfVarTipoIdentificacion { get; set; }
            public string AfVarTipoIdentificacionNombre { get; set; }
            public string AfVarIdentificacion { get; set; }
            public string AfVarUsuarioAlta { get; set; }
            public string AfVarUsuarioAltaNombre { get; set; }
            public string AfVarFechaAlta { get; set; }
            public string AfVarUsuarioUltMod { get; set; }
            public string AfVarUsuarioUltModNombre { get; set; }
            public string AfVarFechaUltMod { get; set; }
            public string AfVarFechaVentaDestruccion { get; set; }
            public string AfVarLicitacionVenta { get; set; }
            public string AfVarValorVenta { get; set; }
            public string AfVarFacturaVenta { get; set; }
            public string AfVarLoteVenta { get; set; }
            public string AfVarOficioDestruccion { get; set; }
            public string AfVarEnlaceInventarioOrig { get; set; }
            public string AfVarEnlaceInventarioOrigNombre { get; set; }
            public string AfVarResponsableCentroOrig { get; set; }
            public string AfVarResponsableCentroOrigNombre { get; set; }
            public string AfVarEnlaceInventario { get; set; }
            public string AfVarEnlaceInventarioNombre { get; set; }
            public string AfVarResponsableCentro { get; set; }
            public string AfVarResponsableCentroNombre { get; set; }
            public string AfVarJefeInventario { get; set; }
            public string AfVarJefeInventarioNombre { get; set; }
            public string AfVarCoordinadorRecursos { get; set; }
            public string AfVarCoordinadorRecursosNombre { get; set; }
            public string AfVarJefeSistemas { get; set; }
            public string AfVarJefeSistemasNombre { get; set; }
            public string AfVarJefeMantenimiento { get; set; }
            public string AfVarJefeMantenimientoNombre { get; set; }

        }
       
        public List<ObtieneParametrosSalida> Post(ParametrosEntrada Datos)
        {
            // string UsuarioDesencripta = Seguridad.DesEncriptar(Datos.Usuario);

            DocumentoEntrada entrada = new DocumentoEntrada
            {
                Usuario = Datos.Usuario,
                Origen = "AdminAPP",
                Transaccion = 120341,
                Operacion = 1,
            };

            //entrada.agregaElemento("estatus", 2);
            // entrada.agregaElemento("estatus", 2);

            DataTable DTListaAdministrativos = new DataTable();

            try
            {
                DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

                if (respuesta.Resultado == "1")
                {
                    DTListaAdministrativos = respuesta.obtieneTabla("Catalogo");

                    List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                    foreach (DataRow row in DTListaAdministrativos.Rows)
                    {
                        ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                        {

                            AfVarID = Convert.ToString(row["AfVarID"]),
                            AfVarEstatus = Convert.ToString(row["AfVarEstatus"]),
                            AfVarEstatusNombre = Convert.ToString(row["AfVarEstatusNombre"]),
                            AfVarTipoMovimiento = Convert.ToString(row["AfVarTipoMovimiento"]),
                            AfVarTipoMovimientoNombre = Convert.ToString(row["AfVarTipoMovimientoNombre"]),
                            AfVarMotivoMovimiento = Convert.ToString(row["AfVarMotivoMovimiento"]),
                            AfVarMotivoMovimientoNombre = Convert.ToString(row["AfVarMotivoMovimientoNombre"]),
                            AfVarFechaAsignacion = Convert.ToString(row["AfVarFechaAsignacion"]),
                            AfVarFechaSolicitud = Convert.ToString(row["AfVarFechaSolicitud"]),
                            AfVarEmpleadoSolicita = Convert.ToString(row["AfVarEmpleadoSolicita"]),
                            AfVarEmpleadoSolicitaNombre = Convert.ToString(row["AfVarEmpleadoSolicitaNombre"]),
                            AfVarCentroOrigen = Convert.ToString(row["AfVarCentroOrigen"]),
                            AfVarCentroOrigenNombre = Convert.ToString(row["AfVarCentroOrigenNombre"]),
                            AfVarCentro = Convert.ToString(row["AfVarCentro"]),
                            AfVarCentroNombre = Convert.ToString(row["AfVarCentroNombre"]),
                            AfVarUbicacion = Convert.ToString(row["AfVarUbicacion"]),
                            AfVarUbicacionNombre = Convert.ToString(row["AfVarUbicacionNombre"]),
                            AfVarEmpleado = Convert.ToString(row["AfVarEmpleado"]),
                            AfVarEmpleadoNombre = Convert.ToString(row["AfVarEmpleadoNombre"]),
                            AfVarComentarios = Convert.ToString(row["AfVarComentarios"]),
                            AfVarFechaFin = Convert.ToString(row["AfVarFechaFin"]),
                            AfVarProveedor = Convert.ToString(row["AfVarProveedor"]),
                            AfVarProveedorNombre = Convert.ToString(row["AfVarProveedorNombre"]),
                            AfVarDireccion = Convert.ToString(row["AfVarDireccion"]),
                            AfVarTelefono = Convert.ToString(row["AfVarTelefono"]),
                            AfVarCorreoElectronico = Convert.ToString(row["AfVarCorreoElectronico"]),
                            AfVarTipoIdentificacion = Convert.ToString(row["AfVarTipoIdentificacion"]),
                            AfVarTipoIdentificacionNombre = Convert.ToString(row["AfVarTipoIdentificacionNombre"]),
                            AfVarIdentificacion = Convert.ToString(row["AfVarIdentificacion"]),
                            AfVarFechaVentaDestruccion = Convert.ToString(row["AfVarFechaVentaDestruccion"]),
                            AfVarLicitacionVenta = Convert.ToString(row["AfVarLicitacionVenta"]),
                            AfVarValorVenta = Convert.ToString(row["AfVarValorVenta"]),
                            AfVarFacturaVenta = Convert.ToString(row["AfVarFacturaVenta"]),
                            AfVarLoteVenta = Convert.ToString(row["AfVarLoteVenta"]),
                            AfVarOficioDestruccion = Convert.ToString(row["AfVarOficioDestruccion"]),
                            AfVarEnlaceInventarioOrig = Convert.ToString(row["AfVarEnlaceInventarioOrig"]),
                            AfVarEnlaceInventarioOrigNombre = Convert.ToString(row["AfVarEnlaceInventarioOrigNombre"]),
                            AfVarResponsableCentroOrig = Convert.ToString(row["AfVarResponsableCentroOrig"]),
                            AfVarResponsableCentroOrigNombre = Convert.ToString(row["AfVarResponsableCentroOrigNombre"]),
                            AfVarEnlaceInventario = Convert.ToString(row["AfVarEnlaceInventario"]),
                            AfVarEnlaceInventarioNombre = Convert.ToString(row["AfVarEnlaceInventarioNombre"]),
                            AfVarResponsableCentro = Convert.ToString(row["AfVarResponsableCentro"]),
                            AfVarResponsableCentroNombre = Convert.ToString(row["AfVarResponsableCentroNombre"]),
                            AfVarJefeInventario = Convert.ToString(row["AfVarJefeInventario"]),
                            AfVarJefeInventarioNombre = Convert.ToString(row["AfVarJefeInventarioNombre"]),
                            AfVarCoordinadorRecursos = Convert.ToString(row["AfVarCoordinadorRecursos"]),
                            AfVarCoordinadorRecursosNombre = Convert.ToString(row["AfVarCoordinadorRecursosNombre"]),
                            AfVarJefeSistemas = Convert.ToString(row["AfVarJefeSistemas"]),
                            AfVarJefeSistemasNombre = Convert.ToString(row["AfVarJefeSistemasNombre"]),
                            AfVarJefeMantenimiento = Convert.ToString(row["AfVarJefeMantenimiento"]),
                            AfVarJefeMantenimientoNombre = Convert.ToString(row["AfVarJefeMantenimientoNombre"]),
                            AfVarRubroActivo = Convert.ToString(row["AfVarRubroActivo"]),
                            AfVarRubroActivoNombre = Convert.ToString(row["AfVarRubroActivoNombre"]),
                            AfVarUsuarioAlta = Convert.ToString(row["AfVarUsuarioAlta"]),
                            AfVarUsuarioAltaNombre = Convert.ToString(row["AfVarUsuarioAltaNombre"]),
                            AfVarUsuarioUltMod = Convert.ToString(row["AfVarUsuarioUltMod"]),
                            AfVarUsuarioUltModNombre = Convert.ToString(row["AfVarUsuarioUltModNombre"]),
                            AfVarFechaAlta = Convert.ToString(row["AfVarFechaAlta"]),
                            AfVarFechaDev = Convert.ToString(row["AfVarFechaDev"]),
                            AfVarFechaUltMod = Convert.ToString(row["AfVarFechaUltMod"]),

    };
                        lista.Add(ent);
                    }

                    return lista;
                }
                else
                {
                    List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                    ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                    {

                        AfVarTipoMovimientoNombre = Convert.ToString("no encontro nada"),


                    };
                    lista.Add(ent);



                    return lista;
                }


            }
            catch (Exception ex)
            {
                List<ObtieneParametrosSalida> lista = new List<ObtieneParametrosSalida>();

                ObtieneParametrosSalida ent = new ObtieneParametrosSalida
                {

                    AfVarTipoMovimientoNombre = ex.ToString()

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
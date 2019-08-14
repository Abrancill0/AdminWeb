
/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 * Requisiciones
 */
var tabla = "";
var PRequisiciones = [];// Seguridad.permisos(3);
var ctaPres = [];
var ctaImp = [];
var presupuestado = 0;
var UsuarioActivo = localStorage.getItem("cosa");
var EmpeladoActivo = localStorage.getItem("cosa2");
$(function () {
    try {
        cargaInicialReqPorAut();
    } catch (err) {
        console.log("Input is ", err);
        setTimeout(cargaInicialReqPorAut, 100);
    }
});

$("#refreshTbl").click(ObtenerRequisiciones);
function cargaInicialReqPorAut() {
    //Seguridad.sessionActiva();
    
    tabla = crearTabla("#tblRequisiciones", 0, "desc");
    ObtenerRequisiciones();
}

function ObtenerRequisiciones() {

    $("#aAutoriza, #aRechazar").hide();

    $('#tblRequisiciones tbody').empty();
    //console.log("consulta browse");
    //if (!tabla)
    //    tabla = $('#tblRequisiciones').DataTable();
    //return false;
    $.ajax({
        async: true,
        type: "POST",
        url: "/api/RequisicionesPorAutorizar",
        data: JSON.stringify({ 'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cargando();
        },
        success: function (result) {
            //console.log(result);
            var resultado = result.Salida.Tablas.Catalogo.NewDataSet.Catalogo;
            if (result.Salida.Resultado === "1") {
                tabla
                    .clear()
                    .draw();

                var nrow = 1;

                var nreq = 0;
                try {
                    nreq = resultado.length;
                } catch (err) {
                    nreq = 0;
                }
                if (nreq > 0) {
                    $.each(resultado, function (key, value) {
                        tabla.row.add(newRowReqAut(value, nrow)).draw(false);
                        nrow++;
                    });
                } else {
                    tabla.row.add(newRowReqAut(resultado, nrow)).draw(false);
                    nrow++;
                }
            } else {
                $.notify("Error al cargar las Requisiciones", { globalPosition: 'top center', className: 'error' });
            }
            cargado();
        },
        complete: function () {
            $("#tblRequisiciones tbody tr").each(function () {
                $(this)[0].cells[6].className = "text-right";
            });
        },
        error: function (result) {
            cargado();
            console.log(result)
            $.notify("Error al cargar las Requisiciones", { globalPosition: 'top center', className: 'error' });
        }
    });
}
function newRowReqAut(datos, nrow) {
    try {
        var RmReqId = datoEle(datos.RmReqId) * 1;
        var RmReqEstatusNombre = datoEle(datos.RmReqEstatusNombre);
        var RmReqEstatus = datoEle(datos.RmReqEstatus);
        var RmReqJustificacion = datoEle(datos.RmReqJustificacion);
        var RmReqSolicitante = datoEle(datos.RmReqSolicitante);
        var RmReqSolicitanteNombre = datoEle(datos.RmReqSolicitanteNombre);
        var RmReqOficinaNombre = datoEle(datos.RmReqOficinaNombre);
        var RmReqSubramoNombre = datoEle(datos.RmReqSubramoNombre);
        var RmReqTotal = datoEle(datos.RmReqTotal) * 1;

        var datos = {
            'IdResponsable': RmReqSolicitante,
            'RmReqId': RmReqId,
            'Usuario': UsuarioActivo,
            'RmReqEstatus': RmReqEstatus,
            'RmReqTotal': RmReqTotal,
            'RmReqComentarios': ""
        };

        var btnVer = "<button type='button' onclick='verRequisiciones(\"u\", " + RmReqId + "," + RmReqEstatus + ")' class='btn btn-success btn-sm'><span class='glyphicon glyphicon-eye-open'></span> Ver</button>";
        var btnAutoriza = "<a id='aAutoriza" + nrow + "' onclick='confAutorizarRequisicion(" + JSON.stringify(datos) + ")' class='btn btn-success btn-sm' aria-disabled='false' href='#' role='button'><span class='glyphicon glyphicon-ok'></span> Autorizar</a>";
        var btnRechazar = "<a id='aRechazar" + nrow + "' onclick='confRechazarRequisicion(" + JSON.stringify(datos) + ")' class='btn btn-danger btn-sm' aria-disabled='false' href='#' role='button'><span class='glyphicon glyphicon-repeat'></span> Rechazar</a>";

        var inpChk = chk2("chkReq" + RmReqId, "chkReq", "", "muestraOcultaBtnAutRec()", JSON.stringify(datos), "18", "default", "default", "");

        var newrow = [
            RmReqId,
            RmReqJustificacion,
            RmReqSolicitanteNombre,
            RmReqOficinaNombre,
            RmReqSubramoNombre,
            RmReqEstatusNombre,
            formatNumber.new(RmReqTotal.toFixed(2), "$ "),
            btnVer,
            btnAutoriza,
            btnRechazar,
            inpChk
        ];
    } catch (err) {
        var newrow = ["", "", "", "", "", "", "", "", "", "", ""];
    }
    
    return newrow;
}

function verRequisiciones(ac, id, estatus) {
    estatus = valorVacio(estatus) ? 1 : estatus;
    id = id * 1;
    $("#guardara, #inputDocumento, #inputResponsable").show();
    $("#GeneraRequisicion").hide();

    if (estatus === 1) {
        $(".textotd").hide();
        $(".inputtd").show();
    } else {
        $(".textotd").show();
        $(".inputtd").hide();
    }

    $("#liDetalleReq, #liAutoriza").removeAttr("class").attr("class", "");
    $("a[href$='#tabDetalleReq'], a[href$='#tabAutoriza']").removeAttr("class").attr("class", "btn disabled");
    $("#tabDetalleReq, #tabAutoriza").removeAttr("class").attr("class", "tab-pane fade");

    $("#liSolicitud").removeAttr("class").attr("class", "active");
    $("#tabSolicitud").removeAttr("class").attr("class", "tab-pane fade in active");

    $("#stAutorizado").val("0");
    $("#tdIdRequisicion, #tdComentarios").empty();
    if (ac === "u" || id > 0) {
        //obtenerInformes();
        //obtenerReponsables();
        rangoFechas("repde2", "repa2", "reporte2", "diasViaje()");
        $("#asignar").show();

        $("a[href$='#tabDetalleReq']").removeAttr("class").attr("class", "btn");

        SelectRequisicion(id);
    }

    $("#verRequisiciones").modal({
        show: true,
        keyboard: false,
        backdrop: "static"
    });
    
}

function SelectRequisicion(id) {
    var estatus = 0;
    presupuestado = 0;
    var idresponsable = "";
    var responsable = "";
    var idestatusInf = "";
    var estatusInf = "";
    var ProyInf = "";

    $("#monto").removeAttr("disabled");
    var datos = { 'Usuario': UsuarioActivo, 'RmReqId': id, 'Empleado': EmpeladoActivo };
    $("#trDevolucion").hide();
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaRequisicionIDCabecera',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cargando();
            $("#tblAutorizadoresReq tbody, #tblDetalleRequisicion tbody").remove();
            $("#categoria, #cantidadcat, #montocat").val("");
        },
        success: function (result) {
            console.log(result);
            var resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
            //console.log(resultado);
            var RmReqId = resultado.RmReqId;
            var RmReqFechaRequerida = (resultado.RmReqFechaRequerida).split("T");
            var RmReqFechaFinal = datoEle(resultado.RmReqFechaFinal);
            RmReqFechaFinal = valorVacio(RmReqFechaFinal) ? resultado.RmReqFechaRequerida : RmReqFechaFinal;
            RmReqFechaFinal = RmReqFechaFinal.split("T");
            var fechaini = formatFecha(RmReqFechaRequerida[0], "");
            var fechafin = formatFecha(RmReqFechaFinal[0], "");
            var RmReqJustificacion = datoEle(resultado.RmReqJustificacion);
            var RmReqOficina = datoEle(resultado.RmReqOficina);
            var RmReqOficinaNombre = datoEle(resultado.RmReqOficinaNombre);
            var RmReqSolicitante = datoEle(resultado.RmReqSolicitante);
            var RmReqSolicitanteNombre = datoEle(resultado.RmReqSolicitanteNombre);
            var RmReqSubramo = datoEle(resultado.RmReqSubramo);
            var RmReqSubramoNombre = datoEle(resultado.RmReqSubramoNombre);
            var RmReqTipoGasto = datoEle(resultado.RmReqTipoGasto);
            var RmReqTipoGastoNombre = datoEle(resultado.RmReqTipoGastoNombre);
            var RmReqTipoRequisicion = datoEle(resultado.RmReqTipoRequisicion);
            var RmReqTipoRequisicionNombre = datoEle(resultado.RmReqTipoRequisicionNombre);
            var RmReqCentro = datoEle(resultado.RmReqCentro);
            var RmReqCentroNombre = datoEle(resultado.RmReqCentroNombre);
            var RmReqEstatus = datoEle(resultado.RmReqEstatus);
            RmReqEstatus = RmReqEstatus === "98" ? "1" : RmReqEstatus;
            var RmReqEstatusNombre = datoEle(resultado.RmReqEstatusNombre);
            var RmReqComentarios = datoEle(resultado.RmReqComentarios);
            var RmReqTipoRequisicion = datoEle(resultado.RmReqTipoRequisicion);
            var RmReqTipoRequisicionNombre = datoEle(resultado.RmReqTipoRequisicionNombre);
            var RmReqTotal = datoEle(resultado.RmReqTotal) * 1;

            $("#idreq").val(RmReqId);

            ".tdIdRequisicion".AsHTML(RmReqId);

            
            "#tdJustificacion .textotd".AsHTML(RmReqJustificacion);

            
            "#tdTipoRequisicion .textotd".AsHTML(RmReqTipoRequisicion + " - " + RmReqTipoRequisicionNombre);

            
            "#tdTipoGasto .textotd".AsHTML(RmReqTipoGasto + " - " + RmReqTipoGastoNombre);

            
            "#tdCentro .textotd".AsHTML(RmReqCentro + " - " + RmReqCentroNombre);

            
            "#tdOficina .textotd".AsHTML(RmReqOficina + " - " + RmReqOficinaNombre);

            
            "#tdSubramo .textotd".AsHTML(RmReqSubramo + " - " + RmReqSubramoNombre);

            $("#idestatus").val(RmReqEstatus);
            "#tdEstatus".AsHTML(RmReqEstatusNombre);

            "#tdComentarios".AsHTML(RmReqComentarios);

            $("#repde2").val(fechaini);
            $("#repa2").val(fechafin);

            if (RmReqEstatus !== "1" && RmReqTotal > 0) {
                $("#monto").val(RmReqTotal.toFixed(2));
                "#ReqMonto".AsHTML(formatNumber.new(RmReqTotal.toFixed(2), "$ "));
            }

        },
        complete: function () {
            var RmReqEstatus = $("#idestatus").val() * 1;
            if (RmReqEstatus === 1) {
                ConsultaMaterial2();
                //$("#guardara, #selCtaPre").show();
            } else {
                destroyRangoFechas("repde2", "repa2");
                $("#guardara, #selCtaPre").hide();
                if (RmReqEstatus >= 3) {
                    SelectProcesosRequisicion();
                    $("a[href$='#tabAutoriza']").removeAttr("class").attr("class", "btn");
                }
            }

            diasViaje();
            browseDetalleRequisicion();
            cargado();
        },
        error: function (result) {
            console.log("error", result);
            cargado();
            $.notify("Error al cargar los Requisiciones", { globalPosition: 'top center', className: 'error' });
        }
    });
}
function browseDetalleRequisicion() {
    var RmRdeRequisicion = $("#idreq").val() * 1;
    $("#GeneraRequisicion").hide();
    if (RmRdeRequisicion > 0) {
        var RmReqEstatus = $("#idestatus").val() * 1;
        var datos = {
            'RmRdeRequisicion': RmRdeRequisicion,
            'Usuario': UsuarioActivo,
            'Empleado': EmpeladoActivo
        };
        $.ajax({
            async: true,
            type: "POST",
            url: '/api/ConsultaRequisicionDetalle',
            data: JSON.stringify(datos),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            cache: false,
            beforeSend: function () {
                //cargando();
                $("#tblDetalleRequisicion tbody").remove();
                $("#tblDetalleRequisicion tfoot").empty();
            },
            success: function (result) {
                var resultado = result.Salida.Tablas.Catalogo.NewDataSet.Catalogo;
                var ncategorias = 0;
                try {
                    ncategorias = resultado.length;
                } catch (err) {
                    ncategorias = 1;
                    //valorVacio(resultado.length) ? 1 : resultado.length;
                }

                var RmReqTotal = 0, RmReqTotalIVA = 0, i = 1;;
                $("#tblDetalleRequisicion").append("<tbody>");
                if (ncategorias > 0) {
                    $.each(resultado, function (key, value) {
                        var newrow = newRowDetalleRequisicion(value, RmRdeRequisicion, RmReqEstatus, i);
                        RmReqTotal += newrow.RmReqTotal;
                        RmReqTotalIVA += newrow.RmReqTotalIVA;
                        $("#tblDetalleRequisicion tbody").append(newrow.row);
                        i++;
                    });
                } else {
                    var newrow = newRowDetalleRequisicion(resultado, RmRdeRequisicion, RmReqEstatus, i);
                    RmReqTotal += newrow.RmReqTotal;
                    RmReqTotalIVA += newrow.RmReqTotalIVA;
                    $("#tblDetalleRequisicion tbody").append(newrow.row);
                }
                $("#tblDetalleRequisicion").append("</tbody>");
                var row = "<tr>";
                row += "<td colspan='3'></td>";
                row += "<td align='right'>Total:</td>";
                row += "<td align='right'>" + formatNumber.new(RmReqTotal.toFixed(2), "$ ") + "</td>";
                row += "<td align='right'>" + formatNumber.new(RmReqTotalIVA.toFixed(2), "$ ") + "</td>";
                row += "<td></td>";
                $("#tblDetalleRequisicion tfoot").empty()
                    .append(row);
                $("#monto").val(RmReqTotalIVA.toFixed(2));
                "#ReqMonto".AsHTML(formatNumber.new(RmReqTotalIVA.toFixed(2), "$ "));

            },
            complete: function () {
                //cargado();
                var totareq = $("#monto").val() * 1;
                if (totareq > 0 && RmReqEstatus === 1)
                    $("#GeneraRequisicion").show();
                else
                    $("#GeneraRequisicion").hide();
            },
            error: function (result) {
                //cargado();
                console.log("error", result);
            }
        });
    }
}
function SelectProcesosRequisicion() {
    var RmReqId = $("#idreq").val() * 1;
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/RequisicionProcesos',
        data: JSON.stringify({ 'RmReqId': RmReqId, 'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
            $("#tblAutorizadoresReq tbody").remove();
            $("#tblAutorizadoresReq tfoot").empty();
        },
        success: function (result) {
            //console.log(result);
            var exito = result.Salida.Resultado * 1;
            var resultado = [];
            if (exito === 1) {
                resultado = result.Salida.Tablas.FlujoRequisicion.NewDataSet.FlujoRequisicion;
                if (!valorVacio(resultado)) {
                    var i = 1;
                    $("#tblAutorizadoresReq").append("<tbody>");
                    var TerminadoOld = "";
                    $.each(resultado, function (key, value) {
                        var proceso = (datoEle(value.Proceso)).split(" - ");
                        var autorizador = proceso[1];
                        var Terminado = datoEle(value.Terminado);
                        var IdResponsable = datoEle(value.IdResponsable);
                        var Responsable = datoEle(value.Responsable);
                        var txtAutorizador = valorVacio(autorizador) ? "" : " (" + autorizador + ")";
                        txtAutorizador = valorVacio(Responsable) ? autorizador : txtAutorizador;
                        var ok = Terminado === "SI" ? 1 : 0;
                        var status = ok === 1 ? SiNo2(ok) : "";
                        var esautorizador = (datoEle(autorizador).toLowerCase()).indexOf("autoriza");
                        var btnAutoriza = "", btnDeclinar = "";
                        if (esautorizador !== -1) {
                            if (ok === 0 && (TerminadoOld === "" || TerminadoOld === "SI")) {
                                var idEmpleado = encriptaDesencriptaEle(EmpeladoActivo, 0);
                                if (idEmpleado === IdResponsable) {
                                    //var UsuarioActivo = encriptaDesencriptaEle(UsuarioActivo, 0);
                                    var datos = { 'IdResponsable': IdResponsable, 'RmReqId': RmReqId, 'Usuario': UsuarioActivo, 'RmReqEstatus': $("#idestatus").val() };
                                    btnAutoriza = "<a id='aAutoriza" + i + "' onclick='confAutorizarRequisicionVer(" + JSON.stringify(datos) + ")' class='btn btn-success btn-sm' aria-disabled='false' href='#' role='button'><span class='glyphicon glyphicon-ok'></span> Autorizar</a>";
                                    btnDeclinar = "<a id='aRechazar" + i + "' onclick='confRechazarRequisicionVer(" + JSON.stringify(datos) + ")' class='btn btn-danger btn-sm' aria-disabled='false' href='#' role='button'><span class='glyphicon glyphicon-repeat'></span> Rechazar</a>";
                                }
                            }
                        }
                        var row = "<tr>";
                        row += "<td>" + i + "</td>";
                        row += "<td>" + Responsable + txtAutorizador + "</td>";
                        row += "<td>" + btnAutoriza + "</td>";
                        row += "<td>" + btnDeclinar + "</td>";
                        row += "<td>" + status + "</td></tr>";
                        $("#tblAutorizadoresReq tbody").append(row);
                        TerminadoOld = Terminado;
                        i++;
                    });
                    $("#tblAutorizadoresReq").append("</tbody>");
                }
            } else {
                $.notify("Error: Al consultar Procesos de la requisición.", { globalPosition: 'top center', className: 'error' });
            }
        },
        complete: function () {
            //cargado();
        },
        error: function (result) {
            $("#tblAutorizadoresReq tbody").remove();
            $("#tblAutorizadoresReq tfoot").empty();
            //cargado();
            console.log("error", result);
        }
    });
    //return resultado;
}
function diasViaje() {
    var dias = difDiaFecha($("input#repde2").val(), $("input#repa2").val());
    dias++;
    $("#dias").val(dias);
}
function confAutorizarRequisicionVer(datos) {
    var botones = [];
    botones[0] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    botones[1] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            autorizarRequisicionVer(datos);
        }
    };
    Seguridad.confirmar("Autorizar Requisición: " + datos.RmReqId + "?", botones, " Autorizar Requisicion.", "#verRequisiciones");
}

function autorizarRequisicionVer(datos) {
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/AutorizaRequisicion',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            var exito = (valorVacio(result) ? 0 : result.Salida.Resultado) * 1;
            if (exito === 1) {
                $.notify("Requisición Autorizada.", { globalPosition: 'top center', className: 'success' });
                SelectProcesosRequisicion();
            } else {
                if (valorVacio(result)) {
                    $.notify("Error: Al autorizar Requisición.", { globalPosition: 'top center', className: 'error' });
                } else {
                    var Descripcion = result.Salida.Errores.Error.Descripcion;
                    Descripcion = Descripcion.replace("Requisición", "Requisición [<b>" + datos.RmReqId + "</b>]");
                    Seguridad.alerta(Descripcion, "#verRequisiciones", "Error: Al autorizar!");
                }
            }

        },
        complete: function () {
            //cargado();
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
        }
    });
}

function confRechazarRequisicionVer(datos) {
    var botones = [];
    botones[0] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    botones[1] = {
        text: "Si", click: function () {
            var RmReqComentarios = $("#RmReqComentarios").val();
            if (valorVacio(RmReqComentarios)) {
                $("#alerta").notify("Especifica el motivo del rechazo.", { globalPosition: 'top center', className: 'error' });
            } else {
                datos["RmReqComentarios"] = RmReqComentarios;
                $(this).dialog("close");
                rechazarRequisicionVer(datos);
            }
        }
    };
    var msn = "Rechazar Requisición: " + datos.RmReqId + "?<br />";
    msn += "Motivo:<br />";
    msn += "<input type='text' id='RmReqComentarios' name='RmReqComentarios' style='width: 100 %' class='form- control' placeholder='Motivo del rechazo'>";
    Seguridad.confirmar(msn, botones, " Rechazar Requisicion.", "#verRequisiciones");
}

function rechazarRequisicionVer(datos) {
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/RechazaRequisicion',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            var exito = (valorVacio(result) ? 0 : result.Salida.Resultado) * 1;
            if (exito === 1) {
                $("#verRequisiciones").modal("hide");
                $.notify("Requisición Rechazada.", { globalPosition: 'top center', className: 'success' });
                //SelectProcesosRequisicion();
                setTimeout(function () {
                    verRequisiciones("u", datos.RmReqId, 1);
                }, 500)
            } else {
                if (valorVacio(result)) {
                    $.notify("Error: Al rechazar Requisición.", { globalPosition: 'top center', className: 'error' });
                } else {
                    var Descripcion = result.Salida.Errores.Error.Descripcion;
                    Descripcion = Descripcion.replace("Requisición", "Requisición [<b>" + datos.RmReqId + "</b>]");
                    Seguridad.alerta(Descripcion, "#verRequisiciones", "Error: Al Rechazar!");
                }
            }

        },
        complete: function () {
            //cargado();
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
        }
    });
}

function newRowDetalleRequisicion(datos, RmRdeRequisicion, RmReqEstatus, i) {
    var newrow = [];
    try {
        var RmRdeCantidadSolicitada = datos.RmRdeCantidadSolicitada * 1;
        var RmRdePrecioUnitario = datos.RmRdePrecioUnitario * 1;
        var RmRdePorcIva = (valorVacio(datos.RmRdePorcIva) ? 0 : datos.RmRdePorcIva) * 1;
        var RmRdeId = datos.RmRdeId * 1;
        var cantidadcat = "", montocat = "";
        var RmRdeMaterial = datoEle(datos.RmRdeMaterial);
        var RmRdeMaterialNombre = datoEle(datos.RmRdeMaterialNombre);
        var RmRdeEstatus = datoEle(datos.RmRdeEstatus);
        var RmRdeUnidadSolicitada = datoEle(datos.RmRdeUnidadSolicitada);
        var RmRdeGrupoMaterial = datoEle(datos.RmRdeGrupoMaterial);
        var RmRdeCuenta = datoEle(datos.RmRdeCuenta);

        var RmRdeSubtotalSis = datoEle(datos.RmRdeSubtotal) * 1;
        var RmRdeIvaSis = datoEle(datos.RmRdeIva) * 1;
        
        var RmRdeSubtotal = 0, PorSubTotal = 0, RmReqTotalDetalle = 0,
            RmReqTotalDetalleIVA = 0, RmReqTotal = 0, RmReqTotalIVA = 0;

        if (RmReqEstatus === 1) {
            RmRdeSubtotal = RmRdeCantidadSolicitada * RmRdePrecioUnitario;
            PorSubTotal = 100;
            if (RmRdePorcIva > 0) {
                PorSubTotal = 100 - RmRdePorcIva;
            }

            RmReqTotalDetalle = Math.round(RmRdeSubtotal);
            RmReqTotalDetalleIVA = Math.round(RmRdeSubtotal * (1 + (RmRdePorcIva / 100)));

            RmReqTotal = RmReqTotalDetalle;
            RmReqTotalIVA = RmReqTotalDetalleIVA;
        } else {

            RmReqTotalDetalle = RmRdeSubtotalSis;
            RmReqTotalDetalleIVA = RmRdeSubtotalSis + RmRdeIvaSis;

            RmReqTotal = RmReqTotalDetalle;
            RmReqTotalIVA = RmReqTotalDetalleIVA;

        }

        var btnEli = "";
        var datosCat = {
            'RmRdeRequisicion': RmRdeRequisicion,
            'RmRdeId': RmRdeId,
            'RmRdeMaterial': RmRdeMaterial,
            'RmRdeMaterialNombre': RmRdeMaterialNombre,
            'RmRdeEstatus': RmRdeEstatus,
            'RmRdeUnidadSolicitada': RmRdeUnidadSolicitada,
            'RmRdeGrupoMaterial': RmRdeGrupoMaterial,
            'RmRdeCuenta': RmRdeCuenta,
            'RmRdePorcIva': RmRdePorcIva
        };
        var fc = "actualizaDetalleReq(" + JSON.stringify(datosCat) + ")";

        var RmRdePrecioUnitario2 = (RmReqTotalDetalleIVA / RmRdeCantidadSolicitada).toFixed(2);
        if (RmReqEstatus === 1) {
            cantidadcat = "<input type='number' id='cantidadcat" + RmRdeId + "' cantidadcat='" + RmRdeCantidadSolicitada + "' name='cantidadcat" + RmRdeId + "' onchange='" + fc + "' value='" + RmRdeCantidadSolicitada + "' class='form-control' />";
            montocat = "<input type='number' id='montocat" + RmRdeId + "' montocat='" + RmRdePrecioUnitario2 + "' name='montocat" + RmRdeId + "' onchange='" + fc + "' value='" + RmRdePrecioUnitario2 + "' class='form-control' />";
            btnEli = "<button type='button' class='btn btn-danger btn-sm' onclick='confEliminarDetalleReq(" + RmRdeRequisicion + "," + RmRdeId + ", \"" + RmRdeMaterialNombre + "\")'><span class='glyphicon glyphicon-trash'></span> Eliminar</button>";
        } else {
            cantidadcat = RmRdeCantidadSolicitada.toFixed(0);
            montocat = formatNumber.new(RmRdePrecioUnitario2, "$ ");
        }
        var row = "<tr>";
        row += "<td>" + i + "</td>";
        row += "<td>" + RmRdeMaterialNombre + "</td>";
        row += "<td align='center'>" + cantidadcat + "</td>";
        row += "<td align='right'>" + montocat + "</td>";
        row += "<td align='right'>" + formatNumber.new(RmReqTotalDetalle.toFixed(2), "$ ") + "</td>";
        row += "<td align='right'>" + formatNumber.new(RmReqTotalDetalleIVA.toFixed(2), "$ ") + "</td>";
        row += "<td>" + btnEli + "</td>";
        row += "</tr>";
        newrow['row'] = row;
        newrow['RmReqTotal'] = RmReqTotal * 1;
        newrow['RmReqTotalIVA'] = RmReqTotalIVA * 1;

    } catch (err) {
        var row = "<tr>";
        row += "<td></td>";
        row += "<td></td>";
        row += "<td></td>";
        row += "<td></td>";
        row += "<td></td>";
        row += "<td></td>";
        row += "<td></td>";
        row += "</tr>";
        newrow['row'] = row;
        newrow['RmReqTotal'] = 0;
        newrow['RmReqTotalIVA'] = 0;
    }
    return newrow;
}

function muestraOcultaBtnAutRec() {
    var ocultar = true;
    var tmonto = 0;
    $("input:checkbox[name=chkReq]").each(function () {
        if ($(this).is(':checked') && ocultar === true) {
            ocultar = false;
        } else if (ocultar === true) {
            ocultar = true;
        }
    });
    if (ocultar) {
        $("#aAutoriza, #aRechazar").hide();
    } else {
        $("#aAutoriza, #aRechazar").show();
    }
    //$.notify("Total: " + formatNumber.new(tmonto.toFixed(2), "$ "), "info");
}

$("#tblRequisiciones").on("click, mouseenter", "tbody tr", function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    } else {
        $('#tblRequisiciones tbody tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});

$("#aAutoriza").click(function () {
    var i = 0, total = 0, datoReq = [];
    $("input:checkbox[name=chkReq]").each(function () {
        if ($(this).is(':checked')) {   
            var datos = JSON.parse($(this).val());
            total += datos.RmReqTotal;
            datoReq[i] = datos;
            i++;
        }
    });
    var resultadoAut = [], nautorizaciones = 0, textError;
    var botones = [];
    botones[0] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    botones[1] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            for (var ii = 0; ii < i; ii++) {
                var datos = datoReq[ii];
                //console.log(datos);
                resultadoAut = autorizarRequisicion(datos, 2);
                var ok = resultadoAut['ok'] * 1;
                nautorizaciones += ok;
                if (ok === 0) {
                    var error = datoEle(resultadoAut['error']);
                    textError += valorVacio(error) ? "" : (" " + error);
                }
            }

            if (nautorizaciones === i) {
                Seguridad.alerta("Se Autorizaron <b> " + i + "</b> Requisiciones.");
            } else {
                var errores = valorVacio(textError) ? "." : ("<br />Errores: <br/>" + textError + ".");
                Seguridad.alerta("Se Autorizaron <b> " + nautorizaciones + "</b> Requisiciones de <b> " + i + "</b>" + errores);
            }
            ObtenerRequisiciones();
        }
    };
    Seguridad.confirmar("Autorizar <b>" + i + "</b> Requisiciones<br />por un importe total de: <b>" + (formatNumber.new(total.toFixed(2), "$ ")) + "</b>?", botones, " Autorizar Requisiciones.");
});

function confAutorizarRequisicion(datos) {
    var botones = [];
    botones[0] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    botones[1] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            autorizarRequisicion(datos, 1);
        }
    };
    Seguridad.confirmar("Autorizar Requisición: <b>" + datos.RmReqId + "</b><br />por un importe de: <b>" + (formatNumber.new((datos.RmReqTotal).toFixed(2), "$ ")) + "</b>?", botones, " Autorizar Requisicion.");
}

function autorizarRequisicion(datos, origen) {
    var reqAut=[];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/AutorizaRequisicion',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            var exito = (valorVacio(result) ? 0 : result.Salida.Resultado) * 1;
            
            if (exito === 1) {
                reqAut['ok'] = 1;
                reqAut['error'] = "";
                if (origen === 1) {
                    ObtenerRequisiciones();
                    $.notify("Requisición Autorizada.", { globalPosition: 'top center', className: 'success' });
                }
            } else {
                reqAut['ok'] = 0;
                if (origen === 1) {
                    if (valorVacio(result)) {
                        $.notify("Error: Al autorizar Requisición.", { globalPosition: 'top center', className: 'error' });
                    } else {
                        var Descripcion = result.Salida.Errores.Error.Descripcion;
                        Descripcion = Descripcion.replace("Requisición", "Requisición [<b>" + datos.RmReqId + "</b>]");
                        Seguridad.alerta(Descripcion);
                    }
                } else {
                    if (valorVacio(result)) {
                        reqAut['error'] = "Error: Al autorizar Requisición  [<b>" + datos.RmReqId + "</b>].";
                    } else {
                        var Descripcion = result.Salida.Errores.Error.Descripcion;
                        Descripcion = Descripcion.replace("Requisición", "Requisición [<b>" + datos.RmReqId + "</b>]");
                        reqAut['error'] = Descripcion;
                        Seguridad.alerta(Descripcion);
                    }
                }
            }
        },
        complete: function () {
            //cargado();
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
        }
    });
    return reqAut;
}

$("#aRechazar").click(function () {
    var i = 0, total = 0, datoReq = [];
    $("input:checkbox[name=chkReq]").each(function () {
        if ($(this).is(':checked')) {
            var datos = JSON.parse($(this).val());
            total += datos.RmReqTotal;
            datoReq[i] = datos;
            i++;
        }
    });
    var resultadoRec = [], nrechazos = 0, textError = "";
    var botones = [];
    botones[0] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    botones[1] = {
        text: "Si", click: function () {
            var RmReqComentarios = $("#RmReqComentariosTodos").val();
            if (valorVacio(RmReqComentarios)) {
                $("#alerta").notify("Especifica el motivo del rechazo.", { globalPosition: 'top center', className: 'error' });
            } else {
                $(this).dialog("close");
                for (var ii = 0; ii < i; ii++) {
                    var datos = datoReq[ii];
                    datos["RmReqComentarios"] = RmReqComentarios;
                    //console.log(datos);
                    resultadoRec = rechazarRequisicion(datos, 2);
                    var ok = resultadoRec['ok'] * 1;
                    nrechazos += ok;
                    if (ok === 0) {
                        var error = datoEle(resultadoRec['error']);
                        textError += valorVacio(error) ? "" : (" " + error);
                    }
                }

                if (nrechazos === i) {
                    Seguridad.alerta("Se Rechazaron <b> " + i + "</b> Requisiciones.");
                } else {
                    var errores = valorVacio(textError) ? "." : ("<br />Errores: <br/>" + textError + ".");
                    Seguridad.alerta("Se Rechazaron <b> " + nautorizaciones + "</b> Requisiciones de <b> " + i + "</b>" + errores);
                }
                ObtenerRequisiciones();

            }

        }
    };
    var msn = "Rechazar <b>" + i + "</b> Requisiciones<br />por un importe total de: <b>" + (formatNumber.new(total.toFixed(2), "$ ")) + "</b>?<br />";
    msn += "Motivo*:<br />";
    msn += "<input type='text' id='RmReqComentariosTodos' name='RmReqComentarios' style='width: 100%' class='form- control' placeholder='Motivo del rechazo'>";
    msn += "*El motivo aplicara para todas las requisiciones seleccionadas.";
    Seguridad.confirmar(msn, botones, " Rechazar Requisicion.");
    
});

function confRechazarRequisicion(datos) {
    var botones = [];
    botones[0] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    botones[1] = {
        text: "Si", click: function () {
            var RmReqComentarios = $("#RmReqComentarios").val();
            if (valorVacio(RmReqComentarios)) {
                $("#alerta").notify("Especifica el motivo del rechazo.", { globalPosition: 'top center', className: 'error' });
            } else {
                datos["RmReqComentarios"] = RmReqComentarios;
                $(this).dialog("close");
                rechazarRequisicion(datos, 1);
            }
        }
    };
    var msn = "Rechazar Requisición: <b>" + datos.RmReqId + "</b><br />por un importe de: <b>" + (formatNumber.new((datos.RmReqTotal).toFixed(2), "$ ")) + "</b>?<br />";
    msn += "Motivo:<br />";
    msn += "<input type='text' id='RmReqComentarios' name='RmReqComentarios' style='width: 100 %' class='form- control' placeholder='Motivo del rechazo'>";
    Seguridad.confirmar(msn, botones, " Rechazar Requisicion.");
}

function rechazarRequisicion(datos, origen) {
    var reqRec = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/RechazaRequisicion',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            var exito = (valorVacio(result) ? 0 : result.Salida.Resultado) * 1;
            if (exito === 1) {
                reqRec['ok'] = 1;
                reqRec['error'] = "";
                if (origen === 1) {
                    ObtenerRequisiciones();
                    $.notify("Requisición Rechazada.", { globalPosition: 'top center', className: 'success' });
                }
            } else {
                reqRec['ok'] = 0;
                if (origen === 1) {
                    if (valorVacio(result)) {
                        $.notify("Error: Al rechazar Requisición.", { globalPosition: 'top center', className: 'error' });
                    } else {
                        var Descripcion = result.Salida.Errores.Error.Descripcion;
                        Descripcion = Descripcion.replace("Requisición", "Requisición [<b>" + datos.RmReqId + "</b>]");
                        Seguridad.alerta(Descripcion);
                    }
                } else {
                    if (valorVacio(result)) {
                        reqRec['error'] = "Error: Al rechazar Requisición  [<b>" + datos.RmReqId + "</b>].";
                    } else {
                        var Descripcion = result.Salida.Errores.Error.Descripcion;
                        Descripcion = Descripcion.replace("Requisición", "Requisición [<b>" + datos.RmReqId + "</b>]");
                        reqRec['error'] = Descripcion;
                        Seguridad.alerta(Descripcion);
                    }
                }
            }

        },
        complete: function () {
            //cargado();
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
        }
    });
    return reqRec;
}
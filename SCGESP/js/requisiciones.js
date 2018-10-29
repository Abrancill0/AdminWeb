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
        cargaInicialReq();
    } catch (err) {
        console.log("Input is ", err);
        setTimeout(cargaInicialReq, 100);
    }
});
$("#btndotacion").click(function () {
    var datos = {
        'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo,
        'RmReqId': ($("#idreq").val() * 1),
        'Beneficiario': localStorage.getItem("nmbemp"),
        'FechaPago': '21/02/2018',
        'Importe': ($("#monto").val() * 1)
    }
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/RequisicionDotacion',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        success: function (result) {
            console.log("success", result);
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
        }
    });

});
function cargaInicialReq() {
    //Seguridad.sessionActiva();
    tabla = crearTabla("#tblRequisiciones", 0, "desc");
    ObtenerRequisiciones();
    //, 'Usuario': UsuarioActivo 
    ConsultaCatalogo(UsuarioActivo, 'ConsultaTipoGasto', 'RmReqTipoDeGasto');
    ConsultaCatalogo(UsuarioActivo, 'ConsultaCentroUsuario', 'RmReqCentro');
    ConsultaCatalogo(UsuarioActivo, 'ConsultaOficinaUsuario', 'RmReqOficina');
    ConsultaCatalogo(UsuarioActivo, 'ConsultaSubramos', 'RmReqSubramo');
    obtenerTipoRequisicion();
}
function ConsultaCatalogo(usuario, catalogo, menuselect) {
    var datos = { 'Usuario': usuario, 'Empleado': EmpeladoActivo };
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/' + catalogo,
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            $("#" + menuselect).empty();
            $("#" + menuselect).append("<option value=''> - Seleccionar - </option>");
        },
        success: function (result) {
            //console.log("succees " + catalogo, result);
            var stresultado = result.Salida.Resultado;
            if (stresultado === '1') {
                var resultTablas = result.Salida.Tablas;
                var resultado = [];
                if (resultTablas.Descripciones) {
                    resultado = resultTablas.Descripciones.NewDataSet.Descripciones;
                } else {
                    resultado = resultTablas.Catalogo.NewDataSet.Catalogo;
                }
                var nelementos = valorVacio(resultado.length) ? 1 : resultado.length;
                var llave = '', descripcion = '', option = '';
                if (nelementos === 1) {
                    switch (menuselect) {
                        case 'RmReqTipoDeGasto':
                            llave = resultado.Llave;
                            descripcion = resultado.Descripcion;
                            break;
                        case 'RmReqCentro':
                            llave = resultado.SgUceCentro;
                            descripcion = resultado.SgUceCentroNombre;
                            break;
                        case 'RmReqOficina':
                            llave = resultado.SgUofOficina;
                            descripcion = resultado.SgUofOficinaNombre;
                            break;
                        case 'RmReqSubramo':
                            llave = resultado.Llave;
                            descripcion = resultado.Descripcion;
                            break;
                    }
                    option = "<option value='" + llave + "'>" + llave + " - " + descripcion + "</option>";
                    $("#" + menuselect).append(option);
                } else {
                    $.each(resultado, function (key, value) {
                        switch (menuselect) {
                            case 'RmReqTipoDeGasto':
                                llave = value.Llave;
                                descripcion = value.Descripcion;
                                break;
                            case 'RmReqCentro':
                                llave = value.SgUceCentro;
                                descripcion = value.SgUceCentroNombre;
                                break;
                            case 'RmReqOficina':
                                llave = value.SgUofOficina;
                                descripcion = value.SgUofOficinaNombre;
                                break;
                            case 'RmReqSubramo':
                                llave = value.Llave;
                                descripcion = value.Descripcion;
                                break;
                        }
                        option = "<option value='" + llave + "'>" + llave + " - " + descripcion + "</option>";
                        $("#" + menuselect).append(option);
                    });
                }
            } else {
                console.log("error c/succees ", result);
                $.notify("Error al cargar el catalogo: " + catalogo, { globalPosition: 'top center', className: 'error' });
            }

        },
        complete: function () {

        },
        error: function (result) {
            console.log("error", result);
            $("#" + menuselect).empty();
            $.notify("Error al cargar el catalogo: " + catalogo, { globalPosition: 'top center', className: 'error' });
        }
    });
}
$("#refreshTbl").click(function () {
    ObtenerRequisiciones();
});
function ObtenerRequisiciones() {
    $("#anuevoa").show();
    
    //console.log("consulta browse");
    //if (!tabla)
    //    tabla = $('#tblRequisiciones').DataTable();
    $.ajax({
        async: true,
        type: "POST",
        url: "/api/RequisicionesListaPendientesUsu",
        data: JSON.stringify({ 'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cargando();
        },
        success: function (result) {
            //console.log(result.Salida);
            var resultado = result.Salida.Tablas.Catalogo.NewDataSet.Catalogo;
            if (result.Salida.Resultado === "1") {
                $('#tblRequisiciones tbody').empty();
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
                        tabla.row.add(newRowRequisiciones(value, nrow)).draw(false);
                        nrow++;
                    });
                } else {
                    tabla.row.add(newRowRequisiciones(resultado, nrow)).draw(false);
                    nrow++;
                }
            } else {
                $.notify("Error al cargar las Requisiciones", { globalPosition: 'top center', className: 'error' });
            }
            cargado();
        },
        error: function (result) {
            cargado();
            console.log(result)
            $.notify("Error al cargar las Requisiciones", { globalPosition: 'top center', className: 'error' });
        }
    });
}
function newRowRequisiciones(datos, nrow) {
    try {
        var RmReqId = datoEle(datos.RmReqId);
        var idresponsable = datoEle(datos.RmReqSolicitante) * 1;
        var RmReqEstatusNombre = datoEle(datos.RmReqEstatusNombre);
        var RmReqEstatus = datoEle(datos.RmReqEstatus);
        RmReqEstatus = RmReqEstatus === "98" ? "1" : RmReqEstatus;
        var RmReqJustificacion = datoEle(datos.RmReqJustificacion);
        var RmReqSolicitanteNombre = datoEle(datos.RmReqSolicitanteNombre);
        var RmReqOficinaNombre = datoEle(datos.RmReqOficinaNombre);
        var RmReqSubramoNombre = datoEle(datos.RmReqSubramoNombre);
        var btnVer = "";
        btnVer = "<button type='button' onclick='verRequisiciones(\"u\", " + RmReqId + "," + RmReqEstatus + ")' class='btn btn-success btn-sm'><span class='glyphicon glyphicon-eye-open'></span> Ver</button>";

        var btnEli = "";
        if (RmReqEstatus === "1" || RmReqEstatusNombre === "Captura")
            btnEli = "<button type='button' class='btn btn-danger btn-sm' onclick='confCancelarRequisiciones(" + RmReqId + ", \"" + (RmReqId + ".- " + RmReqJustificacion) + "\")'><span class='glyphicon glyphicon-ban-circle'></span> Cancelar</button>";

        var newrow = [
            RmReqId,
            RmReqJustificacion,
            RmReqSolicitanteNombre,
            RmReqOficinaNombre,
            RmReqSubramoNombre,
            RmReqEstatusNombre,
            btnVer,
            btnEli,
        ]

    } catch (err) {
        var newrow = ["", "", "", "", "", "", "", ""];
    }
    return newrow;
}
$("#tblRequisiciones").on("click, mouseenter", "tbody tr", function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    } else {
        $('#tblRequisiciones tbody tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});
function confCancelarRequisiciones(id, nombre) {
    var datos = { 'Usuario': UsuarioActivo, 'RmReqId': id, 'Empleado': EmpeladoActivo };
    var botones = [];
    botones[0] = {
        text: "Si", click: function () {
            var permite = permitirAjustarReq(id);
            if (permite.actualizaOk === false) {
                var msnError = "No puedes cancelar la requisición cuando el estatus es: ";
                msnError += permite.idestatus + ".- " + permite.nmbestatus + ".";
                $.notify(msnError, { position: "top center", autoHideDelay: 4000, className: "error" });
                return false;
            } else {
                $(this).dialog("close");
                CancelarRequisiciones(datos, nombre);
            }
        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Cancelar Requisicion:<br /><b>" + nombre + "</b>?", botones, " Eliminar Requisicion.");

}
function CancelarRequisiciones(datos, nombre) {
    //var monto = $("#monto").val() * 1;
    var datosd = { 'RmRdeRequisicion': datos.RmReqId, 'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo };
    if (1 === 2) {
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaRequisicionDetalle',
        data: JSON.stringify(datosd),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargando();
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

            if (ncategorias > 0) {
                var i = 1;
                $.each(resultado, function (key, value) {
                    var RmRdeId = value.RmRdeId * 1;
                    eliminarDetalleReq(datosd.RmRdeRequisicion, RmRdeId, "", 0);

                });
            } else {
                var RmRdeId = resultado.RmRdeId * 1;
                eliminarDetalleReq(datosd.RmRdeRequisicion, RmRdeId, "", 0);
            }

        },
        error: function (result) {
            //cargado();
            console.log("error", result);
        }
    });
    }
//la operacion del API EliminaRequisicionCabecera cambio a solo cancelar
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/EliminaRequisicionCabecera',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        success: function (result) {

            var stResultado = result.Salida.Resultado * 1;

            if(stResultado === 1){
                ObtenerRequisiciones();
                $("#tblRequisiciones").notify("Requisicion " + nombre + " Cancelada.", { position: "top center", autoHideDelay: 3000 }, "success");
            }else{
                $("#tblRequisiciones").notify("Error al Cancelar Requisicion " + nombre + ".", { position: "top center", autoHideDelay: 2000 }, "error");
                Seguridad.alerta(result.Salida.Error, "#tblRequisiciones");
                //
            }
        },
        error: function (result) {
            console.log(result);
            $("#tblRequisiciones").notify("Error al Cancelar Requisicion.", { position: "top center", autoHideDelay: 2000 }, "error");
        }
    });
}
$("#anuevoa").click(function () {
    $("#btnAux").empty();
    $("#idreq").val("0");
    $("#monto").val(0);
    "#ReqMonto".AsHTML(formatNumber.new(0, "$ "));
    $("#RmReqJustificacion").val("");
    $("#tdGastado").empty();
    $("#tdDisponible").empty();
    $("#tdEstatus").empty();
    $("#idestatus").val("");
    //$("#formaPago").val("").selectpicker('refresh');

    $("#monto").removeAttr("disabled");
    $("#RmReqJustificacion").removeAttr("disabled");

    $("#documento").val("");
    $("#responsableInf").val("");

    $("#guardara").empty();
    $("#guardara").append("<span class='glyphicon glyphicon-floppy-saved'></span> Guardar");
    verRequisiciones("i", 0);
});
function verRequisiciones(ac, id, estatus) {
    estatus = valorVacio(estatus) ? 1 : estatus;
    id = id * 1;
    $("#guardara, #inputResponsable").show();
    $("#GeneraRequisicion").hide();

    if (ac === "i") {
        $("#liDetalleReq").attr("onclick", "guardadoAutomatico()");
    } else {
        $("#liDetalleReq").removeAttr("onclick");
    }

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
    $("#liAutoriza").hide();
    if (ac === "i") {
        rangoFechas("repde2", "repa2", "reporte2", "diasViaje()");
        var fecha = fechaActual();

        $("#categoria, #categoria, #montocat").val("");
        $("#tblAutorizadoresReq tbody, #tblDetalleRequisicion tbody").remove();


        $("input#repde2").val(fecha);
        $("input#repa2").val(fecha);

        catDefault = selectCatalogosDefault(encriptaDesencriptaEle(EmpeladoActivo, 0));

        diasViaje();

    } else if (ac === "u" || id > 0) {
        //obtenerInformes();
        //obtenerReponsables();
        rangoFechas("repde2", "repa2", "reporte2", "diasViaje()");
        $("a[href$='#tabDetalleReq']").removeAttr("class").attr("class", "btn");

        SelectRequisicion(id);
    }

    $("#verRequisiciones").modal({
        show: true,
        keyboard: false,
        backdrop: "static"
    });

    $("#formaPago, #RmReqCentro, #RmReqTipoDeGasto, #RmReqOficina, #RmReqSubramo, #RmReqTipoRequisicion").select2({
        dropdownParent: $("#tabSolicitud")
    });
    $("#categoria").select2({
        dropdownParent: $("#tabDetalleReq")
    });

    $("#responsable").select2({
        dropdownParent: $("#informe")
    });
}
function selectCatalogosDefault(usuario) {
    var datos = { 'GrEmpID': usuario, 'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo };
    var catDefault = JSON.parse(localStorage.getItem("default"));

    $("#RmReqTipoDeGasto").val(catDefault.GrEmpTipoGasto);
    $("#RmReqCentro").val(catDefault.GrEmpCentro);
    $("#RmReqOficina").val(catDefault.GrEmpOficina);
    $("#RmReqSubramo").val('333');
    $("#RmReqCentro, #RmReqTipoDeGasto, #RmReqOficina, #RmReqSubramo").select2({
        dropdownParent: $("#tabSolicitud")
    });
    return catDefault;
    /*$.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaCatalogosDefault',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {

        },
        success: function (result) {
            //console.log(result);
            catDefault = result[0];
            //console.log(catDefault);
        },
        complete: function () {
            $("#RmReqTipoDeGasto").val(catDefault.GrEmpTipoGasto);
            $("#RmReqCentro").val(catDefault.GrEmpCentro);
            $("#RmReqOficina").val(catDefault.GrEmpOficina);
            $("#RmReqSubramo").val('');
            $("#RmReqCentro, #RmReqTipoDeGasto, #RmReqOficina, #RmReqSubramo").select2({
                dropdownParent: $("#tabSolicitud")
            });
        },
        error: function (result) {
            catDefault = [];
            console.log("error", result);
            $.notify("Error al cargar el catalogo default", { globalPosition: 'top center', className: 'error' });
        }
    });*/
}
function diasViaje() {
    var dias = difDiaFecha($("input#repde2").val(), $("input#repa2").val());
    dias++;
    $("#dias").val(dias);
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
            //console.log(result);
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

            $("#RmReqJustificacion").val(RmReqJustificacion);
            "#tdJustificacion .textotd".AsHTML(RmReqJustificacion);

            $("#RmReqTipoRequisicion").val(RmReqTipoRequisicion);
            "#tdTipoRequisicion .textotd".AsHTML(RmReqTipoRequisicion + " - " + RmReqTipoRequisicionNombre);

            $("#RmReqTipoDeGasto").val(RmReqTipoGasto);
            "#tdTipoGasto .textotd".AsHTML(RmReqTipoGasto + " - " + RmReqTipoGastoNombre);

            $("#RmReqCentro").val(RmReqCentro);
            "#tdCentro .textotd".AsHTML(RmReqCentro + " - " + RmReqCentroNombre);

            $("#RmReqOficina").val(RmReqOficina);
            "#tdOficina .textotd".AsHTML(RmReqOficina + " - " + RmReqOficinaNombre);

            $("#RmReqSubramo").val(RmReqSubramo);
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
                $("#guardara, #selCtaPre").show();
            } else {
                destroyRangoFechas("repde2", "repa2");
                $("#guardara, #selCtaPre").hide();
                if (RmReqEstatus >= 3) {
                    SelectProcesosRequisicion();
                    $("a[href$='#tabAutoriza']").removeAttr("class").attr("class", "btn");
                    $("#liAutoriza").show();
                }
            }
            $("#RmReqCentro, #RmReqTipoDeGasto, #RmReqOficina, #RmReqSubramo, #RmReqTipoRequisicion").select2({
                dropdownParent: $("#tabSolicitud")
            });

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
function bloqueaElementosRequisicion(ProyInf, estatusInf, responsable) {
    destroyRangoFechas("repde2", "repa2");

    $("#nuevoInforme").empty();
    $("#monto").attr("disabled", true);
    $("#RmReqJustificacion").attr("disabled", true);
    $("#inputDocumento").hide();//$("#documento").attr("disabled", true).selectpicker('refresh');
    $("#inputResponsable").hide();//$("#responsableInf").attr("disabled", true).selectpicker('refresh');

    if (!valorVacio(estatusInf)) {
        $("#labelDocumento").append(ProyInf + " <span class='label label-info'>" + estatusInf + "</span>");
    }
    $("#labelResponsable").append(responsable);

    $("#guardara, #guardara2, #selCtaPre").hide();

}
$("#responsable").change(function () {
    $("#responsableInf").val($("#responsable").val());
});
$("#frmInforme .close, .cerrar").click(function () {
    $("#frmInforme").modal("hide");
    $("#VerRequisicion").modal("show");
    $("#responsableInf").val($("#responsable").val());
});
$("#guardara, #guardara2").click(function () {
    guardarRequisicion("Requisicion", "");
});
function guardarRequisicion(origen, doc) {
    var idrep = $("#idreq").val() * 1;
    var ac = (idrep === 0) ? "InsertRequisicion" : "UpdateRequisicion";

    var documento = "";
    if (origen === "Requisicion") {
        documento = $("#documento").val() === "NA" ? "" : $("#documento").val();
    } else {
        documento = doc;
    }
    var monto = 1; $("#monto").val() * 1;
    var formaPago = $("#formaPago").val();
    var RmReqCentro = $("#RmReqCentro").val();
    var justificacion = $("#RmReqJustificacion").val();
    var dias = $("#dias").val() * 1;
    var finicio = $("#repde2").val();
    var ffin = $("#repa2").val();
    var RmReqTipoDeGasto = $("#RmReqTipoDeGasto").val();
    var RmReqOficina = $("#RmReqOficina").val();
    var RmReqSubramo = $("#RmReqSubramo").val();
    var RmReqEstatus = "1";
    var RmReqTipoRequisicion = $("#RmReqTipoRequisicion").val(); //"99";
    var error = 0;

    if (valorVacio(justificacion)) {
        $("#RmReqJustificacion").notify("Indica una justificacion para el Requisición.", { position: "top", autoHideDelay: 2000 }, "error");
        error = 1;
    }
    if (valorVacio(RmReqTipoRequisicion)) {
        $("#RmReqTipoRequisicion").notify("Indica el tipo de Requisición.", { position: "top", autoHideDelay: 2000 }, "error");
        error = 1;
    }
    if (monto <= 0 || !/[-+]?([0-9]*\.[0-9]+|[0-9]+)/.test(monto)) {
        $("#monto").notify("Ingresa el monto ($) del Requisición.", { position: "top", autoHideDelay: 2000 }, "error");
        error = 1;
    }
    if (valorVacio(RmReqCentro) === "") {
        $("#RmReqCentro").notify("Selecciona un centro.", { position: "top", autoHideDelay: 2000 }, "error");
        $("#RmReqCentro").removeAttr("disabled");
        error = 1;
    }
    if (valorVacio(RmReqTipoDeGasto)) {
        $("#RmReqTipoDeGasto").notify("Seleciona el tipo de gasto.", { position: "top", autoHideDelay: 2000 }, "error");
        error = 1;
    }
    if (valorVacio(RmReqOficina)) {
        $("#RmReqOficina").notify("Seleciona la oficina.", { position: "top", autoHideDelay: 2000 }, "error");
        error = 1;
    }
    if (valorVacio(RmReqSubramo)) {
        $("#RmReqSubramo").notify("Seleciona el subramo.", { position: "top", autoHideDelay: 2000 }, "error");
        error = 1;
    }
    var SgUsuId = UsuarioActivo;
    var datosEmp = SelectUsuario(SgUsuId);
    var SgUsuEmpleado = encriptaDesencriptaEle(EmpeladoActivo, 0);
    //datoEle(datosEmp.SgUsuEmpleado);
    if (valorVacio(SgUsuEmpleado)) {
        $("#RmReqJustificacion").notify("Indica una justificacion para el Requisicion.", { position: "top", autoHideDelay: 2000 }, "error");
        error = 1;
    }
    if (error === 1)
        return false;

    idrep = idrep === 0 ? "" : idrep;

    var datos = {
        'RmReqId': idrep,
        'RmReqEstatus': RmReqEstatus,
        'RmReqTipoRequisicion': RmReqTipoRequisicion,
        'RmReqFechaRequisicion': (fechaActual().replace(/-/gi, "/")) + " 00:00:00",
        'RmReqFechaRequrida': finicio.replace(/-/gi, "/") + " 00:00:00",
        'RmReqFechaFinal': ffin.replace(/-/gi, "/") + " 23:59:59",
        'RmReqSolicitante': SgUsuEmpleado, //UsuarioActivo,//responsableInf
        'RmReqTipoDeGasto': RmReqTipoDeGasto,
        'RmReqCentro': RmReqCentro,
        'RmReqOficina': RmReqOficina,
        'RmReqSubramo': RmReqSubramo,
        'RmReqJustificacion': justificacion,
        'formaPago': formaPago,
        'dias': dias,
        'Usuario': UsuarioActivo,
        'Empleado': EmpeladoActivo
    };

    var accion = (idrep * 1) === 0 ? "RequisicionEncabezado" : "RequisicionEncabezadoActualiza";
    if (accion === "RequisicionEncabezadoActualiza") {
        var permite = permitirAjustarReq(idrep);
        if (permite.actualizaOk === false) {
            var msnError = "No puedes actualizar la requisición cuando el estatus es: ";
            msnError += permite.idestatus + ".- " + permite.nmbestatus + ".";
            $.notify(msnError, { position: "top center", autoHideDelay: 4000, className: "error" });
            return false;
        }
    }
    var terminarPreloader = 1;
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/' + accion,
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            $("#verRequisiciones").modal("hide");
            cargando();
        },
        success: function (result) {
            var stresultado = 0;
            if (accion === "RequisicionEncabezado")
                stresultado = ((result * 1) > 0) ? 1 : 0;
            else
                stresultado = result * 1;

            if (stresultado === 1) {
                var msn = (accion === "RequisicionEncabezado" ? "Se creo Requisicion." : "Se actualizo el Requisicion.");
                $.notify(msn, { position: 'top center', className: 'success' });
                ObtenerRequisiciones();
                terminarPreloader = 0;
            } else {
                $.notify("Error al crear/actualizar la requisición.", { position: 'top center', className: 'error' });

                setTimeout(function () {
                    $("#verRequisiciones").modal({
                        show: true,
                        keyboard: false,
                        backdrop: "static"
                    });
                    setTimeout(function () {
                        Seguridad.alerta("No puede generar la requisición porque:<br />" + result, "#verRequisiciones");
                    }, 400);
                }, 600);
            }
        },
        complete: function () {
            if (terminarPreloader === 1)
                cargado();
        },
        error: function (result) {
            cargado();
            console.log(result);
        }
    });
    //}
}
function permitirAjustarReq(id) {
    var drequisicion = DatosRequisicion(id);
    var RmReqEstatus = datoEle(drequisicion.RmReqEstatus);
    var RmReqEstatusNombre = datoEle(drequisicion.RmReqEstatusNombre);
    var actualizaOk = false;
    var resultado = [];
    if (RmReqEstatus === "1" || RmReqEstatus === "98") {
        resultado = {
            actualizaOk: true,
            idestatus: RmReqEstatus,
            nmbestatus: RmReqEstatusNombre
        };
    } else {
        resultado = {
            actualizaOk: false,
            idestatus: RmReqEstatus,
            nmbestatus: RmReqEstatusNombre
        };
    }
    return resultado;
}
function ConsultaMaterial() {
    var RmReqId = $("#idreq").val() * 1;
    //var RmReqTipoDeGasto = $("#RmReqTipoDeGasto").val();
    var datos = { 'RmRdeRequisicion': RmReqId, 'TipoRequisicion': 99, 'valida': 1, 'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo };
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaMaterial',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            $("#categoria").empty();
            $("#categoria").append("<option value='' data-informacion=''> - Categoria - </option>");
        },
        success: function (result) {
            $.each(result, function (key, value) {
                var GrMatId = datoEle(value.GrMatId);
                var GrMatNombre = datoEle(value.GrMatNombre);
                var GrMatPrecio = datoEle(value.GrMatPrecio) * 1;
                var GrMatIva = datoEle(value.GrMatIva) * 1;
                var GrMatGrupo = datoEle(value.GrMatGrupo);
                var cuenta = datoEle(value.GrGmaCuentaAdquisicion);
                var GrMatUnidadMedida = datoEle(value.GrMatUnidadMedida);

                $("#categoria").append("<option value = '" + GrMatId + "'" +
                    " data-rmrdematerialnombre = '" + GrMatNombre + "'" +
                    " data-rmrdepreciounitario = '" + GrMatPrecio + "'" +
                    " data-rmrdeporciva = '" + GrMatIva + "'" +
                    " data-rmrdegrupomaterial = '" + GrMatGrupo + "'" +
                    " data-rmrdecuenta = '" + cuenta + "'" +
                    " data-rmrdeunidadsolicitada = '" + GrMatUnidadMedida + "'" +
                    " >" + GrMatNombre + "</option>");
            });
        },
        complete: function () {
            cargado();
            $("#categoria").select2({
                dropdownParent: $("#tabDetalleReq")
            });
        },
        error: function (result) {
            cargado();
            console.log(result);
        }
    });
}
function ConsultaMaterial2() {
    var RmReqId = $("#idreq").val() * 1;
    //var RmReqTipoDeGasto = $("#RmReqTipoDeGasto").val();
    var datos = {
        'RmRdeRequisicion': RmReqId,
        'Requisicion': RmReqId,
        'RmTrmTipoRequisicion': 99,
        'valida': 1,
        'Usuario': UsuarioActivo,
        'Empleado': EmpeladoActivo
    };
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaMaterial2',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            $("#categoria").empty();
            $("#categoria").append("<option value='' data-informacion=''> - Categoria - </option>");
        },
        success: function (result) {
            //console.log(result);
            var stResultado = result.Salida.Resultado;
            if (stResultado === "1") {
                var resultado = result.Salida.Tablas.Materiales.NewDataSet.Materiales;

                var nres = 0;
                try {
                    nres = resultado.length;
                } catch (err) {
                    nres = 0;
                }
                if (nres > 0) {
                    $.each(resultado, function (key, value) {
                        var GrMatId = datoEle(value.GrMatId);
                        var GrMatNombre = datoEle(value.GrMatNombre);
                        var GrMatPrecio = datoEle(value.GrMatPrecio) * 1;
                        var GrMatIva = datoEle(value.GrMatIva) * 1;
                        var GrMatGrupo = datoEle(value.GrMatGrupo);
                        var GrMatUnidadMedida = datoEle(value.GrMatUnidadMedida);
                        var option = "<option value = '" + GrMatId + "'" +
                            " data-rmrdematerialnombre = '" + GrMatNombre + "'" +
                            " data-rmrdepreciounitario = '" + GrMatPrecio + "'" +
                            " data-rmrdeporciva = '" + GrMatIva + "'" +
                            " data-rmrdegrupomaterial = '" + GrMatGrupo + "'" +
                            " data-rmrdeunidadsolicitada = '" + GrMatUnidadMedida + "'" +
                            " >" + GrMatNombre + "</option>";
                        //console.log(option);
                        $("#categoria").append(option);
                    });
                } else {
                    try {
                        if (!valorVacio(resultado.GrMatId)) {
                            var GrMatId = datoEle(resultado.GrMatId);
                            var GrMatNombre = datoEle(resultado.GrMatNombre);
                            var GrMatPrecio = datoEle(resultado.GrMatPrecio) * 1;
                            var GrMatIva = datoEle(resultado.GrMatIva) * 1;
                            var GrMatGrupo = datoEle(resultado.GrMatGrupo);
                            var GrMatUnidadMedida = datoEle(resultado.GrMatUnidadMedida);
                            var option = "<option value = '" + GrMatId + "'" +
                                " data-rmrdematerialnombre = '" + GrMatNombre + "'" +
                                " data-rmrdepreciounitario = '" + GrMatPrecio + "'" +
                                " data-rmrdeporciva = '" + GrMatIva + "'" +
                                " data-rmrdegrupomaterial = '" + GrMatGrupo + "'" +
                                " data-rmrdeunidadsolicitada = '" + GrMatUnidadMedida + "'" +
                                " >" + GrMatNombre + "</option>";
                            //console.log(option);
                            $("#categoria").append(option);
                        } else {
                            var option = "<option value = ''>Sin Categorias</option>";
                            //console.log(option);
                            $("#categoria").append(option);
                        }
                    } catch (err) {
                        var option = "<option value = ''>Sin Categorias</option>";
                        //console.log(option);
                        $("#categoria").append(option);
                    }
                }

            } else {
                $("#categoria").empty();
            }
        },
        complete: function () {
            $("#categoria").select2({
                dropdownParent: $("#tabDetalleReq")
            });
        },
        error: function (result) {
            console.log(result);
        }
    });
}
$("#categoria").change(function () {
    var elemento = $(this)[0];
    var opIndex = elemento.selectedIndex;
    var idCategoria = $(this).val();
    var nmbCategoria = elemento.options[opIndex].text;
    var datos = elemento.options[opIndex].dataset;
    if (idCategoria > 0) {
        if (datos.rmrdecuenta !== "") {
            $("#montocat").focus();
            $("#montocat").val(""); //(datos.rmrdepreciounitario);
            setTimeout(function () {
                $("#cantidadcat").val("");
                $("#cantidadcat").focus();
                var dias = $("#dias").val() * 1;
                if (dias > 0)
                    $("#cantidadcat").val(1);//dias
                $("#montocat").focus();
            }, 500);
        } else {
            $("#montocat").val("");
            $("#cantidadcat").val("");
        }
    } else {
        $("#montocat").val("");
        $("#cantidadcat").val("");
    }

});
$("#cantidadcat, #montocat").change(function () {
    var RmReqId = $("#idreq").val() * 1;
    var cantidadcat = 1; //$("#cantidadcat").val() * 1;
    var montocat = $("#montocat").val() * 1;
    var elementoCat = $("#categoria")[0];
    var idCategoria = $("#categoria").val() * 1;
    var opIndex = elementoCat.selectedIndex;
    //var nmbCategoria = elementoCat.options[opIndex].text;
    var datosCat = elementoCat.options[opIndex].dataset;
    if (RmReqId === 0) {
        $.notify("Error no puedes agregar detalles a la requisición.", { position: "top center", autoHideDelay: 2000, className: "error" });
        return false;
    }

    if (idCategoria === 0) {
        $("#categoria").notify("Seleciona una categoria.", { position: "top center", autoHideDelay: 2000, className: "error" });
        return false;
    }

    if (montocat === 0) {
        $("#montocat").notify("Indica un importe mayor a 0.", { position: "top center", autoHideDelay: 2000, className: "error" });
        return false;
    }

    if (cantidadcat === 0) {
        $("#cantidadcat").notify("Indica una cantidad mayor a 0.", { position: "top center", autoHideDelay: 2000, className: "error" });
        return false;
    }

    if (RmReqId > 0 && idCategoria > 0 &&
        cantidadcat > 0 && montocat > 0) {
        //guardarDetalleRequisicion(RmReqId, idCategoria, cantidadcat, datosCat);
        /*
        var botones = [];
        botones[0] = {
            text: "Si", click: function () {
                $(this).dialog("close");
                guardarDetalleRequisicion(RmReqId, idCategoria, cantidadcat, datosCat);
            }
        };
        botones[1] = {
            text: "No", click: function () {
                $(this).dialog("close");
            }
        };
        Seguridad.confirmar("Agregar Categoria [" + datosCat.rmrdematerialnombre + "] al detalle de la Requisición [" + RmReqId + "]?", botones, " Agregar Categoria.", "#tblDetalleRequisicion");
        */
    }
});
$("#guardaDetalleReq").click(function () {
    var RmReqId = $("#idreq").val() * 1;
    var cantidadcat = 1; //$("#cantidadcat").val() * 1;
    var montocat = $("#montocat").val() * 1;
    var elementoCat = $("#categoria")[0];
    var idCategoria = $("#categoria").val() * 1;
    var opIndex = elementoCat.selectedIndex;
    //var nmbCategoria = elementoCat.options[opIndex].text;
    var datosCat = elementoCat.options[opIndex].dataset;
    
    if (RmReqId === 0) {
        $.notify("Error no puedes agregar detalles a la requisición.", { position: "top center", autoHideDelay: 2000, className: "error" });
        return false;
    }

    if (idCategoria === 0) {
        $("#categoria").notify("Seleciona una categoria.", { position: "top center", autoHideDelay: 2000, className: "error" });
        return false;
    }

    if (montocat === 0) {
        $("#montocat").notify("Indica un importe mayor a 0.", { position: "top center", autoHideDelay: 2000, className: "error" });
        return false;
    }

    if (cantidadcat === 0) {
        $("#cantidadcat").notify("Indica una cantidad mayor a 0.", { position: "top center", autoHideDelay: 2000, className: "error" });
        return false;
    }

    if (RmReqId > 0 && idCategoria > 0 &&
        cantidadcat > 0 && montocat > 0) {

        var permite = permitirAjustarReq(RmReqId);
        if (permite.actualizaOk === false) {
            var msnError = "No puedes hacer ajustes en la requisición cuando el estatus es: ";
            msnError += permite.idestatus + ".- " + permite.nmbestatus + ".";
            $.notify(msnError, { position: "top center", autoHideDelay: 4000, className: "error" });
            return false;
        } else {
            guardarDetalleRequisicion(RmReqId, idCategoria, cantidadcat, datosCat);
        }

        
    }
});
function guardarDetalleRequisicion(RmReqId, idCategoria, cantidadcat, datosCat) {
    var RmRdePrecioUnitario = $("#montocat").val() * 1;
    var RmRdePorcIva = datoEle(datosCat.rmrdeporciva) * 1;
    var RmRdeSubtotal = RmRdePrecioUnitario;
    var RmRdeIva = RmRdePorcIva > 0 ? (RmRdeSubtotal * RmRdePorcIva / 100) : 0;
    RmRdePrecioUnitario = RmRdePrecioUnitario / (1 + (RmRdePorcIva / 100)); //RmRdePrecioUnitario - RmRdeIva;
    var datos = {
        'RmRdeRequisicion': RmReqId,
        'RmRdeId': "",
        'RmRdeEstatus': 1,
        'GrMatId': idCategoria,
        'RmRdeCantidadSolicitada': cantidadcat,
        'RmRdeMaterialNombre': datosCat.RmRdeMaterialNombre ? datosCat.RmRdeMaterialNombre : datosCat.rmrdematerialnombre,
        'RmRdeUnidadSolicitada': datosCat.rmrdeunidadsolicitada,
        'RmRdeGrupoMaterial': datosCat.rmrdegrupomaterial,
        'RmRdeCuenta': datosCat.rmrdecuenta,
        'RmRdePrecioUnitario': RmRdePrecioUnitario,
        'RmRdePorcIva': RmRdePorcIva,
        'Usuario': UsuarioActivo,
        'Empleado': EmpeladoActivo
    };
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/RequisicionDetalle',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargando();
            //$("[data-toggle='tooltip']").tooltip("destroy");
        },
        success: function (result) {
            var stResultado = (result.substr(0, 1)) === "<" ? 0 : 1;
            if (stResultado === 1) {
                $("#montocat").val("");
                $("#cantidadcat").val("");
                browseDetalleRequisicion();
                ConsultaMaterial2();
                $("#tblDetalleRequisicion").notify("Categoria agregada.", { position: "top center", autoHideDelay: 2500, className: "success" });
            } else {
                var resultadoXML = $.parseXML(result);
                var resultaJSON = xmlToJson(resultadoXML);
                //console.log(resultadoXML, resultaJSON);
                var error = "";
                try {
                    error = resultaJSON.Error.Descripcion["#text"];
                    error = "Error: [" + datos.RmRdeMaterialNombre + "] " + error;
                } catch (err) {
                    error = "Error al agregar la categoria [" + datos.RmRdeMaterialNombre + "] a la requisición."
                }
                $("#tblDetalleRequisicion").notify(error, { position: "top center", autoHideDelay: 4000, className: "error" });
            }
        },
        complete: function () {
            //cargado();
            //$("[data-toggle='tooltip']").tooltip();
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
        }
    });

}
function actualizaDetalleReq(datos) {
    var cantidadcat = $("#cantidadcat" + datos.RmRdeId).val() * 1;
    var cantidadcatold = $("#cantidadcat" + datos.RmRdeId).attr("cantidadcat") * 1;
    var montocat = $("#montocat" + datos.RmRdeId).val() * 1;
    var montocatold = $("#montocat" + datos.RmRdeId).attr("montocat") * 1;

    var RmRdePorcIva = datos.RmRdePorcIva * 1;
    var RmRdePrecioUnitario = montocat;
    var RmRdeSubtotal = RmRdePrecioUnitario;
    var RmRdeIva = RmRdePorcIva > 0 ? (RmRdeSubtotal * RmRdePorcIva / 100) : 0;
    RmRdePrecioUnitario = RmRdePrecioUnitario / (1 + (RmRdePorcIva / 100)); //RmRdePrecioUnitario - RmRdeIva;

    if (cantidadcat > 0 && montocat > 0) {
        datos['RmRdeCantidadSolicitada'] = cantidadcat;
        datos['RmRdePrecioUnitario'] = RmRdePrecioUnitario; //montocat;

        datos['Usuario'] = UsuarioActivo;
        datos['Empleado'] = EmpeladoActivo;
        var permite = permitirAjustarReq(datos.RmRdeRequisicion);
        if (permite.actualizaOk === false) {
            var msnError = "No puedes hacer ajustes en la requisición cuando el estatus es: ";
            msnError += permite.idestatus + ".- " + permite.nmbestatus + ".";
            $.notify(msnError, { position: "top center", autoHideDelay: 4000, className: "error" });
            return false;
        }
        $.ajax({
            async: true,
            type: "POST",
            url: '/api/RequisicionDetalleActualiza',
            data: JSON.stringify(datos),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            cache: false,
            beforeSend: function () {
                //cargando();
            },
            success: function (result) {
                //console.log("success", result);
            },
            complete: function () {
                //cargado();
                browseDetalleRequisicion();
                $("#montocat").val("");
                $("#cantidadcat").val("");
                $("#tblDetalleRequisicion").notify("Categoria actualizada.", { position: "top center", autoHideDelay: 2500, className: "success" });
            },
            error: function (result) {
                //cargado();
                console.log("error", result);
            }
        });
    } else {
        $("#tblDetalleRequisicion").notify("El importe y cantidad solicitada deben ser mayor a cero.", { position: "top center", autoHideDelay: 2000 }, "error");
        if (cantidadcat === 0)
            $("#cantidadcat" + datosCat.RmRdeId).val(cantidadcatold);
        if (montocat === 0)
            $("#montocat" + datosCat.RmRdeId).val(montocatold);

    }
}
function confEliminarDetalleReq(RmRdeRequisicion, RmReqDetalleId, RmRdeMaterialNombre) {
    var botones = [];
    botones[0] = {
        text: "Si", click: function () {
            var permite = permitirAjustarReq(RmRdeRequisicion);
            if (permite.actualizaOk === false) {
                var msnError = "No puedes hacer ajustes en la requisición cuando el estatus es: ";
                msnError += permite.idestatus + ".- " + permite.nmbestatus + ".";
                $.notify(msnError, { position: "top center", autoHideDelay: 4000, className: "error" });
                return false;
            } else {
                $(this).dialog("close");
                eliminarDetalleReq(RmRdeRequisicion, RmReqDetalleId, RmRdeMaterialNombre, 1);
            }
            
        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Eliminar Categoria [" + RmRdeMaterialNombre + "] de la Requisición [" + RmRdeRequisicion + "]?", botones, " Eliminar Requisicion.", "#tblDetalleRequisicion");
}
function eliminarDetalleReq(RmRdeRequisicion, RmReqDetalleId, RmRdeMaterialNombre, valida) {
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/RequisicionDetalleElimina',
        data: JSON.stringify({ 'RmRdeRequisicion': RmRdeRequisicion, 'RmRdeId': RmReqDetalleId, 'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargando();
        },
        success: function (result) {
            //console.log("success", result);
        },
        complete: function () {
            //cargado();
            if (valida === 1) {
                $("#montocat").val("");
                $("#cantidadcat").val("");
                browseDetalleRequisicion();
                ConsultaMaterial2();
                $("#tblDetalleRequisicion").notify("Categoria eliminada de la requisición.", { position: "top center", autoHideDelay: 2500, className: "success" });
            }
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
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
                //console.log(result);
                var resultado = result.Salida.Tablas.Catalogo.NewDataSet.Catalogo;
                var ncategorias = 0;
                try {
                    ncategorias = resultado.length;
                } catch (err) {
                    ncategorias = 1;
                    //valorVacio(resultado.length) ? 1 : resultado.length;
                }

                var RmReqTotal = 0, RmReqTotalIVA = 0, i = 1;
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

                if (RmReqEstatus === 1) {
                    $("#monto").val(RmReqTotalIVA.toFixed(2));
                    "#ReqMonto".AsHTML(formatNumber.new(RmReqTotalIVA.toFixed(2), "$ "));
                }

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
$("#GeneraRequisicion").click(function () {
    var RmReqId = $("#idreq").val() * 1;
    if (RmReqId > 0) {
        var botones = [];
        botones[0] = {
            text: "Si", click: function () {
                $(this).dialog("close");
                GeneraRequisicion(RmReqId);
            }
        };
        botones[1] = {
            text: "No", click: function () {
                $(this).dialog("close");
            }
        };
        Seguridad.confirmar("Generar Requisición [" + RmReqId + "]?", botones, " Generar Requisición.", "#verRequisiciones");

    } else {
        $.notify("Error: Al generar requisición", { globalPosition: 'top center', className: 'error' });
    }
});
function GeneraRequisicion(RmReqId) {
    if (RmReqId > 0) {
        $.ajax({
            async: true,
            type: "POST",
            url: '/api/GenerarRequisicion',
            data: JSON.stringify({ 'RmReqId': RmReqId, 'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            cache: false,
            beforeSend: function () {
                $("#verRequisiciones").modal("hide");
                cargando();
            },
            success: function (result) {
                //console.log("success", result);
                var Resultado = result.Resultado * 1;
                if (Resultado === 1) {
                    $.notify("Requisición generada.", { globalPosition: 'top center', className: 'success' });
                    setTimeout(function () {
                        verRequisiciones("u", RmReqId, 3);
                    }, 600);
                    ObtenerRequisiciones();
                } else {
                    //console.log(result);

                    setTimeout(function () {
                        $("#verRequisiciones").modal({
                            show: true,
                            keyboard: false,
                            backdrop: "static"
                        });
                    }, 600);
                    var errores = result.Errores.Errores.Error;
                    nerrores = valorVacio(errores.length) ? 0 : errores.length;
                    var strError = "";
                    if (nerrores > 0) {
                        $.each(errores, function (key, value) {
                            //console.log(value);
                            var descripcion = "";
                            if (value.Descripcion.indexOf("Partida") >= 0) {
                                descripcion = value.Descripcion.split("Partida");
                                strError += descripcion[0] + "<br />";
                            } else {
                                descripcion = value.Descripcion;
                                strError += descripcion + "<br />";
                            }
                        });
                    } else {
                        //console.log(errores);
                        var descripcion = "";
                        if (errores.Descripcion.indexOf("Partida") >= 0) {
                            descripcion = errores.Descripcion.split("Partida");
                            strError = descripcion[0] + "<br />";
                        } else {
                            descripcion = errores.Descripcion;
                            strError += descripcion + "<br />";
                        }
                    }

                    var CuentaNoDisponible = 0, SolicitaTraspaso = 0;
                    if (!valorVacio(result.Valores.Valores)) {
                        CuentaNoDisponible = datoEle(result.Valores.Valores.CuentaNoDisponible) * 1;
                        SolicitaTraspaso = datoEle(result.Valores.Valores.SolicitaTraspaso) * 1;
                    }

                    if (CuentaNoDisponible === 1 || SolicitaTraspaso === 1) {
                        setTimeout(function () { CrearRequisicionPeticionTraspaso(strError); }, 1000);
                    } else {
                        setTimeout(function () {
                            Seguridad.alerta("No puede generar la requisición porque:<br />" + strError, "#verRequisiciones");
                        }, 1000);

                    }

                    //console.log(strError);
                }
            },
            complete: function () {
                setTimeout(function () {
                    cargado();
                }, 600);
            },
            error: function (result) {
                cargado();
                console.log("error", result);
            }
        });
    } else {
        $.notify("Error: Al generar requisición", { globalPosition: 'top center', className: 'error' });
    }
}
function CrearRequisicionPeticionTraspaso(strError) {
    var RmReqId = $("#idreq").val() * 1;
    //Seguridad.alerta(strError, "#verRequisiciones");

    if (RmReqId > 0) {

        var permite = permitirAjustarReq(RmReqId);
        if (permite.actualizaOk === false) {
            var msnError = "No puedes hacer ajustes en la requisición cuando el estatus es: ";
            msnError += permite.idestatus + ".- " + permite.nmbestatus + ".";
            $.notify(msnError, { position: "top center", autoHideDelay: 4000, className: "error" });
            return false;
        }

        var botones = [];
        botones[0] = {
            text: "Si", click: function () {
                $(this).dialog("close");
                $.ajax({
                    async: true,
                    type: "POST",
                    url: '/api/RequisicionPeticionTraspaso',
                    data: JSON.stringify({ 'RmReqId': RmReqId, 'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo }),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    cache: false,
                    beforeSend: function () {
                        //cargando();
                        $("#verRequisiciones").modal("hide");
                    },
                    success: function (result) {
                        //console.log("success", result);
                        setTimeout(function () {
                            verRequisiciones("u", RmReqId, 2);
                        }, 600);

                        $("#verRequisiciones").notify("Peticion de Traspaso Creada.", { position: "top center", autoHideDelay: 2500, className: "success" });
                        ObtenerRequisiciones();;
                    },
                    complete: function () {
                        //cargado();
                        browseDetalleRequisicion();
                    },
                    error: function (result) {
                        //cargado();
                        console.log("error", result);
                    }
                });
            }
        };
        botones[1] = {
            text: "No", click: function () {
                $(this).dialog("close");
            }
        };
        setTimeout(function () {
            Seguridad.confirmar("No puede generar la requisición porque:<br />" + strError + "¿Desea Colocar la Solicitud de Traspaso para la Requisición [" + RmReqId + "]?", botones, " Solicitud de Traspaso.", "#verRequisiciones");
        }, 400);

    } else {
        $.notify("Error: Al Crear Petición de Traspaso.", { globalPosition: 'top center', className: 'error' });
    }

}
function SelectUsuario(SgUsuId) {
    var resultado = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaUsuarioID',
        data: JSON.stringify({ 'SgUsuId': SgUsuId, 'Usuario': SgUsuId, 'Empleado': EmpeladoActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            var exito = result.Salida.Resultado * 1;
            if (exito === 1) {
                resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
            } else {
                $.notify("Error: Al consultar Usuario.", { globalPosition: 'top center', className: 'error' });
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
    return resultado;
}
function SelectEmpleado(GrEmpID, UsuarioActivo) {
    var resultado = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaEmpleadoID',
        data: JSON.stringify({ 'GrEmpID': GrEmpID, 'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            //console.log(result);
            var exito = result.Salida.Resultado * 1;
            if (exito === 1) {
                resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
            } else {
                $.notify("Error: Al consultar Empleado.", { globalPosition: 'top center', className: 'error' });
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
    return resultado;
}
function SelectProcesosRequisicion() {
    var RmReqId = $("#idreq").val() * 1;
    $.ajax({
        async: false,
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
                        var proceso = (datoEle(value.Proceso)).replace("-", " - ");
                        proceso = proceso.split(" - ");
                        var autorizador = $.trim(proceso[1]);
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
                                    btnAutoriza = "<a id='aAutoriza" + i + "' onclick='confAutorizarRequisicion(" + JSON.stringify(datos) + ")' class='btn btn-success btn-sm' aria-disabled='false' href='#' role='button'><span class='glyphicon glyphicon-ok'></span> Autorizar</a>";
                                    btnDeclinar = "<a id='aRechazar" + i + "' onclick='confRechazarRequisicion(" + JSON.stringify(datos) + ")' class='btn btn-danger btn-sm' aria-disabled='false' href='#' role='button'><span class='glyphicon glyphicon-repeat'></span> Rechazar</a>";
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
function confAutorizarRequisicion(datos) {
    var botones = [];
    botones[0] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            autorizarRequisicion(datos);
        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Autorizar Requisición: " + datos.RmReqId + "?", botones, " Autorizar Requisicion.", "#verRequisiciones");
}
function autorizarRequisicion(datos) {
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
function confRechazarRequisicion(datos) {
    var botones = [];
    botones[0] = {
        text: "Si", click: function () {
            var RmReqComentarios = $("#RmReqComentarios").val();
            if (valorVacio(RmReqComentarios)) {
                $("#alerta").notify("Especifica el motivo del rechazo.", { globalPosition: 'top center', className: 'error' });
            } else {
                datos["RmReqComentarios"] = RmReqComentarios;
                $(this).dialog("close");
                rechazarRequisicion(datos);
            }
        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    var msn = "Rechazar Requisición: " + datos.RmReqId + "?<br />";
    msn += "Motivo:<br />";
    msn += "<input type='text' id='RmReqComentarios' name='RmReqComentarios' style='width: 100 %' class='form- control' placeholder='Motivo del rechazo'>";
    Seguridad.confirmar(msn, botones, " Rechazar Requisicion.", "#verRequisiciones");
}
function rechazarRequisicion(datos) {
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
function obtenerTipoRequisicion() {
    var datos = {
        'RutaProceso': 4,
        'Usuario': UsuarioActivo,
        'Empleado': EmpeladoActivo
    };
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaTipoReqGastoViaje',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
            $("#RmReqTipoRequisicion").empty();
        },
        success: function (result) {
            var stResultado = result.Salida.Resultado;
            if (stResultado === '1') {
                var resultado = result.Salida.Tablas.Catalogo.NewDataSet.Catalogo;
                var nres = 0;
                try {
                    nres = resultado.length;
                } catch (err) {
                    nres = 0;
                }

                if (nres > 0) {
                    $.each(resultado, function (key, value) {
                        //if (value.RmTirRutaProceso === "4") {
                        var option = "<option value = '" + value.RmTirId + "'>" + value.RmTirNombre + "</option>";
                        $("#RmReqTipoRequisicion").append(option);
                        //}
                    });
                } else {
                    //if (resultado.RmTirRutaProceso === "4") {
                    var option = "<option value = '" + resultado.RmTirId + "'>" + resultado.RmTirNombre + "</option>";
                    $("#RmReqTipoRequisicion").append(option);
                    //}                    
                }

            } else {
                $("#RmReqTipoRequisicion").empty();
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
//guardado automatico
function guardadoAutomatico() {
    var idReq = $("#idreq").val() * 1;
    if (idReq === 0) {
        
        var justificacion = $.trim($("#RmReqJustificacion").val());
        var RmReqCentro = $("#RmReqCentro").val();
        var RmReqTipoDeGasto = $("#RmReqTipoDeGasto").val();
        var RmReqOficina = $("#RmReqOficina").val();
        var RmReqSubramo = $("#RmReqSubramo").val();
        var RmReqTipoRequisicion = $("#RmReqTipoRequisicion").val();
        var finicio = $("#repde2").val();
        var ffin = $("#repa2").val();
        var SgUsuEmpleado = encriptaDesencriptaEle(EmpeladoActivo, 0);
        var RmReqEstatus = "1";
        if (!valorVacio(justificacion) && !valorVacio(RmReqCentro) &&
            !valorVacio(RmReqTipoDeGasto) && !valorVacio(RmReqOficina) &&
            !valorVacio(RmReqSubramo) && !valorVacio(RmReqTipoRequisicion) &&
            !valorVacio(SgUsuEmpleado)) {
            var datos = {
                'RmReqId': idReq,
                'RmReqEstatus': RmReqEstatus,
                'RmReqTipoRequisicion': RmReqTipoRequisicion,
                'RmReqFechaRequisicion': (fechaActual().replace(/-/gi, "/")) + " 00:00:00",
                'RmReqFechaRequrida': finicio.replace(/-/gi, "/") + " 00:00:00",
                'RmReqFechaFinal': ffin.replace(/-/gi, "/") + " 23:59:59",
                'RmReqSolicitante': SgUsuEmpleado, 
                'RmReqTipoDeGasto': RmReqTipoDeGasto,
                'RmReqCentro': RmReqCentro,
                'RmReqOficina': RmReqOficina,
                'RmReqSubramo': RmReqSubramo,
                'RmReqJustificacion': justificacion,
                'Usuario': UsuarioActivo,
                'Empleado': EmpeladoActivo
            };
            gAutomaticoRequisicion(datos);

        } else {
            $("#liSolicitud").notify("Todo los campos de la vista 'Solicitud' necesitan un valor.", { position: "top", autoHideDelay: 2000 }, "error");
        }
    }
}
function gAutomaticoRequisicion(datos) {
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/RequisicionEncabezado',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cargando();
            $("#liDetalleReq").hide();
        },
        success: function (result) {
            var stresultado = 0;
            var idReq = result * 1;
            stresultado = (idReq > 0) ? 1 : 0;

            if (stresultado === 1) {

                rangoFechas("repde2", "repa2", "reporte2", "diasViaje()");
                
                $("#liSolicitud, #liAutoriza").removeAttr("class").attr("class", "");
                $("a[href$='#tabSolicitud'], a[href$='#tabDetalleReq']").removeAttr("class").attr("class", "btn");
                $("a[href$='#tabAutoriza']").removeAttr("class").attr("class", "btn disabled");
                $("#tabSolicitud").removeAttr("class").attr("class", "tab-pane fade");

                $("#liDetalleReq").removeAttr("class").attr("class", "active");
                $("#tabDetalleReq").removeAttr("class").attr("class", "tab-pane fade in active");


                $("#liDetalleReq").notify("Requisición guardada en automatico.", { position: "top", autoHideDelay: 4000, className: "success" });
                $("#liDetalleReq").removeAttr("onclick");
                SelectRequisicion(idReq);
            } else {
                $("#liSolicitud").notify("No se puede guardar la requisición.", { position: "top", autoHideDelay: 2000, className: "error" });
            }
        },
        complete: function () {
            cargado();
            $("#liDetalleReq").show();
            ObtenerRequisiciones();
        },
        error: function (result) {
            cargado();
        }
    });
}

function DatosRequisicion(RmReqId) {
    var datos = { 'Usuario': UsuarioActivo, 'RmReqId': RmReqId, 'Empleado': EmpeladoActivo };
    var resultado = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaRequisicionIDCabecera',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
        },
        success: function (result) {
            resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
        },
        error: function (result) {
            console.log("error", result);
        }
    });
    return resultado;
}
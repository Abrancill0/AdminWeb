/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 * Informes
 */
var tablaInformes = "";
var tabla = "";
var estatus = 0;
var cargaPag = false;
var tblinput_ag = "";
var UsuarioActivo = localStorage.getItem("cosa");
$(function () {
    //informe js cargado
    try {
        $("#AlertInfoDisp").hide();
        cargaInicialInf();
    } catch (err) {
        console.log("Input is ", err);
        setTimeout(cargaInicialInf, 100);
    }
});

//OK
function OpcionesMenuMes(mesactual, ac) {
    var fini = FechaMasMenos(mesactual, 2, "m", "-");
    var ffin = FechaMasMenos(mesactual, 2, "m", "+");
    if (ac === "u") {
        var fecha = fechaActual();
        fecha = "01" + fecha.substr(2, 10);
        ffin = FechaMasMenos(mesactual, 2, "m", "+");
    }

    var difmes = Math.ceil(difDiaFecha(fini, ffin) / 30);

    $("#mes").empty();
    $("#mesInfVer").empty();
    for (var i = 0; i < difmes; i++) {
        var fechanueva = FechaMasMenos(fini, i, "m", "+");
        var fechaymd = fechanueva.split("-");
        var fechan = fechaymd[2] + "-" + fechaymd[1] + "-" + fechaymd[0];
        var fechanmb = formatFecha(fechan, "mmm, yyyy");
        $("#mes").append("<option value='" + fechanueva + "'>" + fechanmb + "</option>");
        $("#mesInfVer").append("<option value='" + fechanueva + "'>" + fechanmb + "</option>");
    }
}
//OK
function cargaInicialInf() {
    //crearTabla("#tblProyectos", 0, "desc");
    //crearTablaDetalleRow("#tblGastos", 0, "asc", 14, false);
    //crearTabla("#tblGastos", 0, "asc");


    //ObtenerEstatusResponsables();
    //ObtenerResponsables();
    //ObtenerTipoInf();
    ObtenerInformes();
    //   ObtenerTipoInf();

    ////////$("[]").tooltip();
}
//OK
function ObtenerInformes() {
    $("#AlertInfoDisp").hide();

    var UsuarioActivo = localStorage.getItem("cosa");

    var datos = {
        "estatus": 3,
        "uresponsable": UsuarioActivo,
        "uconsulta": UsuarioActivo
    };

    $('#tblProyectos tbody').empty();

    if (!tablaInformes) {
        tablaInformes = $("#tblProyectos").DataTable({
            "order": [[0, "desc"]],
            "processing": true,
            "scrollCollapse": false,
            scrollX: false,
            paging: false,
            searching: false,
            "ordering": false,
            "language": {
                "lengthMenu": "_MENU_ Registros Por Página",
                "zeroRecords": "No se encontraron Registros",
                "info": "",
                "infoEmpty": "No hay registros para mostrar.",
                "infoFiltered": "(_TOTAL_ Registros de _MAX_)",
                "search": "Buscar:",
                "processing": "Cargando Información",
                "decimal": ".",
                "thousands": ",",
                "paginate": {
                    "first": "Primera Página",
                    "last": "Ultima Página",
                    "next": "Siguiente",
                    "sPrevious": "Anterior"
                }
            },
            fixedColumns: {
                leftColumns: 3//Le indico que deje fijas solo las 3 primeras columnas
            },
            initComplete: function (settings, json) {
                setTimeout(function () { $("#tblProyectos").DataTable().draw(); }, 200);
            },
            "autoWidth": false,
            "fixedHeader": {
                "header": true,
                "footer": true
            }
        });
    }
    var f = new Date();
    var fh = f.getDate() + '' + f.getMonth() + '' + f.getFullYear() + '' + f.getHours() + '' + f.getMinutes() + '' + f.getSeconds();
    $.ajax({
        type: 'POST',
        url: '/api/BrowseInformeAutoriza',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cerrarfrmInforme();
            cargando();
        },
        success: function (result) {
            /*ver listado
             * 0: ninguno
             * 1: todos
             * 2: solo 1 (login)
             */

            tablaInformes
                .clear()
                .draw();
            var ii = 0;
            $.each(result, function (i, value) {
                //console.log(value);
                var btnEdit = "";
                var finicio = valorVacio(value.i_finicio) ? "" : formatFecha(new Date(value.i_finicio), "dd/mm/yyyy");
                var ffin = valorVacio(value.i_ffin) ? "" : formatFecha(new Date(value.i_ffin), "dd/mm/yyyy");

                btnEdit = "<button type='button' class='btn btn-success btn-sm' onclick='verInformeGastos(" + value.i_id + ", " + 0 + ", " + value.i_estatus + ")' data-dismiss='modal'><i class='zmdi zmdi-eye zmdi-hc-lg' style='padding: 3px 0px'></i> Ver</button>";
                var btnVer = "<a href='/ver_informe_autorizacion?" + fh + "&item=" + value.i_id + "' class='btn btn-success btn-sm'><i class='zmdi zmdi-eye zmdi-hc-lg' style='padding: 3px 0px'></i> Ver</a>";
                //if (todos === 1) {
                var just = (value.i_nmb).split("");
                var njust = just.length;
                var justificacion = "";
                if (njust >= 30) {
                    var ll = 0;
                    for (var jj = 0; jj <= njust; jj++) {
                        if (just[jj]) {
                            justificacion += just[jj];
                            if (ll === 30) {
                                justificacion += "<br />";
                                ll = 0;
                            } else {
                                ll++;
                            }
                        }
                    }
                } else {
                    justificacion = value.i_nmb;
                }

                tablaInformes.row.add([
                    value.r_idrequisicion,
                    value.i_ninforme,
                    justificacion,
                    formatNumber.new((value.r_montorequisicion * 1).toFixed(2), "$ "),
                    value.responsable,
                    value.e_estatus,
                    btnVer
                ]).draw(false);
                ii++;


            });
            if (ii > 0) {
                $("#tblProyectos tbody tr").each(function () {
                    $(this)[0].cells[3].className = "text-right";
                });
            }
            cargado();
        }, complete: function () {

        },
        error: function (result) {
            cargado();
            $.notify("Error al cargar los informes", { globalPosition: 'top center', className: 'error' });
        }
    });
    //ObtenerEstatusResponsables();
    $('#divListInformes').css('display', 'block');
}

$("#aguarda").click(function () {

    //console.log($("#verinforme").serialize() + "&responsable=" + $("#verinforme #responsablever").val());
    var error = 0;
    if ($.trim($("#verinforme #proyecto").val()) === "") {
        $("#verinforme #proyecto").notify("Ingresa el nombre del Proyecto", { position: "top" }, "error");
        error = 1;
    }
    if (error === 1) {
        $("#aenvia").hide();
        $("#aedit").hide();
        $("#aagregarg").hide();
        $("#aanticipos").hide();
        $("#aguarda").show();
        $("#acancela").show();
        return false;
    } else {
        $("#aenvia").show();
        $("#aedit").show();
        $("#aagregarg").show();
        $("#aanticipos").show();
        $("#aguarda").hide();
        $("#acancela").hide();

        var Id = $("#idinforme").val();
        var tipo = $("#tipoInfVer").val();
        var i_nmb = $("#verinforme #proyecto").val();
        var Empresa = localStorage.getItem("IDEmpresa");
        var UsuarioActivo = localStorage.getItem("cosa");
        var mes = $("#mesInfVer").val();


        var datos = {
            "id": Id,
            "motivo": "",
            "i_nmb": i_nmb,
            "umodifico": UsuarioActivo,
            "notas": "",
            "idempresa": Empresa,
            "proycontable": "",
            "tipo": tipo,
            "mes": mes,
        };

        $.ajax({
            async: true,
            type: 'POST',
            url: '/api/UpdateInforme',
            data: JSON.stringify(datos),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            beforeSend: function () {
                cargando();
            },
            success: function (result) {
                cargado();

                "#lblproyecto".AsHTML($("#verinforme #proyecto").val());

                $("#lblproyecto").show();

                $("#lblTipoInfVer").show();
                $("#divTipoInfVer").hide();
                "#lblTipoInfVer".AsHTML($('#tipoInfVer option:selected').text());

                $("#lblMesInfVer").show();
                $("#divMesInfVer").hide();
                "#lblMesInfVer".AsHTML($('#mesInfVer option:selected').text());

                $("#verinforme #proyecto").attr("type", "hidden");

                $.notify("Datos Actualizados.", { globalPosition: 'top center', className: 'success' });

                //actualiza lista de informes
                ObtenerInformes();
            },
            complete: function () {
                var idinforme = $("#verinforme #idinforme").val();
                var idproyecto = $("#verinforme #idproyecto").val();
                Seguridad.bitacora("UpdateInforme", 4, idproyecto + "," + idinforme, "Se actualizo el informe.", 1);
            },
            error: function (result) {
                console.log(result);
                $.notify("Error al Actualizar el informe.", { globalPosition: 'top center', className: 'error' });
            }
        });

    }

});

$(".cerrarGA").click(function () {
    $("#gastosAutorizar").modal("hide");
    $("#verInformeGastos").modal("show");
    $("#verQuienAutoriza").modal({
        show: true,
        keyboard: false
    });
});

function verInformeGastos(id, idproyecto, st) {
    $("#verinforme #proyecto").attr("readonly", true);
    $("#idinforme").val(id);
    $("#idproyecto").val(idproyecto);

    tblinput_ag = "";
    fecUltgastos = "";
    estatus = 3; //st;
    selectInforme(id, 1);
    consultaInfoGastos(id, st, 1);

    $("#verInformeGastos").modal({
        show: true,
        keyboard: false,
        backdrop: "static"
    });

}

function consultaInfoGastos(id, st, actControles) {
    st = 3;
    selectInforme(id, actControles);
    obtenerGastosInforme(id, idproyecto, st);
}

function selectInforme(id, actControles) {
    //estatus = 0;
    //alert(id + ", " + idproyecto);
    var reembolso = 0;
    var uresponsable = 0;
    var DesactivaControl = 0;
    var hAutorizarComprobar = "";
    //var Empresa = localStorage.getItem("IDEmpresa");

    var datos = {
        "id": id
    };
    $("#usuResponsable").val();
    $.ajax({
        async: false,
        type: 'POST',
        url: '/api/SelectInforme',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            cargando();
        },
        success: function (result) {
            $("#verinforme #proyecto").attr("type", "hidden");
            $("#divresponsablever").hide();
            $("#divTipoInfVer").hide();
            $("#divMesInfVer").hide();
            $.each(result, function (key, value) {
                $("#tdninforme").empty();
                $("#tdestatus").empty();
                $("#tddel").empty();
                $("#tdal").empty();
                $("#tdcomAut").empty();
                $("#tdConXML").empty();
                $("#lblRequisicion").empty();
                $("#tdConBancaria").empty();
                $("#tdConConvenios").empty();
                $("#tdContabilizar").empty();
                $("#tdanticipo").empty();
                $("#tdgastado").empty();
                $("#tddisponible").empty();
                $("#tdpagarares").empty();
                $("#tdpagore").empty();
                $("#tdtsreeembolsable").empty();
                $("#tdtnreeembolsable").empty();

                hAutorizarComprobar = value.hAutorizarComprobar;
                //tdcomAut
                $("#verinforme #proyecto").val(value.p_nmb);

                document.getElementById("HFRmRdeRequisicion").value = value.r_idrequisicion;
                DatosRequisicion();

                estatus = value.i_estatus * 1;
                $("#solAnticipo").hide();

                if (value.i_comentarioaut !== "" && value.i_comentarioaut !== null) {
                    $("#tdcomAut").append("<b>Comentario: </b> " + value.i_comentarioaut);
                    $("#comentarioaut").val(value.i_comentarioaut);
                }
                var lblCon = "<span style='font-size: 11px' class='label label-success'><span class='glyphicon glyphicon-ok'></span></span>";
                if ((value.i_conciliacionbancos * 1) === 1) {
                    $("#tdConBancaria").append("<b>Confrontación: </b> " + lblCon);
                }

                uresponsable = $.trim(value.i_uresponsable);
                $("#usuResponsable").val(uresponsable);
                $("#tdninforme").append(value.i_ninforme);
                $("#lblRequisicion").append(value.r_idrequisicion);
                $("#tdestatus").append(value.e_estatus);

                var finicio = valorVacio(value.del) ? "" : formatFecha(new Date(value.del), "dd/mm/yyyy");
                var ffin = valorVacio(value.al) ? "" : formatFecha(new Date(value.al), "dd/mm/yyyy");

                $("#tddel").append(finicio);
                $("#tdal").append(ffin);


                $("#verinforme #responsablever").val(uresponsable);

                /*inicio tipo informe*/
                var idtipo = value.i_tipo * 1;
                var clave = value.clavetipo;
                var nombre = value.tipo;
                var tipo = valorVacio(clave) ? nombre : (clave + " - " + nombre);
                $("#verinforme #tipoInfVer").val(idtipo);
                "#lblTipoInfVer".AsHTML(tipo);
                //fin tipo informe

                reembolso = value.reembolso * 1;
                var anticipo = value.r_montorequisicion * 1;
                var gastado = value.MontoGastado * 1;
                var disponible = value.Disponible * 1;
                //var pagarares = value.PagarResponsable * 1;//por pagar a responsable
                var tsreembolso = value.i_tsreembolso * 1;
                var tnreembolso = value.i_tnreembolso * 1;
                $("#HFMontoRequisicion").val(anticipo);
                $("#tdanticipo").append("<span style='font-size: 16px;' class='label label-primary'>" + formatNumber.new(anticipo.toFixed(2), "$ ") + "</span>");
                $("#tdgastado").append("<span style='font-size: 16px;' class='label label-danger'>" + formatNumber.new(gastado.toFixed(2), "$ ") + "</span>");

                $(".pagore").hide();
                $(".trtreeembolsable").hide();

                $("#disAnticipo").val(disponible);

                "#lblCabeceraFormaPago".AsHTML(value.i_tarjetatoka);
                $("#inputCabeceraFormaPago").val(value.i_tarjetatoka);
                "#lblproyecto".AsHTML($("#verinforme #proyecto").val());
                "#lblresponsablever".AsHTML(value.responsable);

                /*inicio totales*/
                $("#lbltotalg").empty();
                $("#lblmontog").empty();
                $("#lbltotalg").append(formatNumber.new((value.i_totalg * 1).toFixed(2), "$ "));
                $("#lblmontog").append(formatNumber.new((value.i_total * 1).toFixed(2), "$ "));
                $("#totalg").val(value.i_totalg);
                $("#montog").val(value.i_total);
                /*fin totales*/
                DesactivaControl = value.DesactivaControl;
            });
            $("#tipoInfVer, #mesInfVer").select2({
                dropdownParent: $("#verinforme")
            });


            if (actControles === 1)
                habilitaControlesInfo(idproyecto, id, estatus, uresponsable, DesactivaControl, hAutorizarComprobar);

            cargado();
        },
        error: function (result) {
            console.log(result);
        }
    });

}

function habilitaControlesInfo(idproyecto, id, estatus, uresponsable, DesactivaControl, hAutorizarComprobar) {
    $("#aenvia").hide();
    $("#aguarda").hide();
    $("#acancela").hide();
    $("#aanticipos").show();
    $("#aautoriza").hide();
    $("#verinforme #idinforme").val(id);
    $("#verinforme #idproyecto").val(idproyecto);
    $("#comentarioaut").val("");
    $("#aexportarxls").hide();
    $("#aexportarpdf").hide();
    $("#aterminar").hide();
    $("#apago").hide();
    $("#aexportarxls").show();
    $("#aexportarxls").attr("onclick", "informexls(" + idproyecto + ", " + id + ")");
    $("#aagregarg").hide();
    if (estatus <= 2) {

        $("#aagregarg").show();
        $("#aenvia").show();
        $("#aedit").show();
    } else if (estatus === 3 || estatus === 7 || estatus === 5 || estatus === 4 || estatus === 8) {//3 = por autorizar, 7 = autorizado parcial, 5 = autorizado

        if (hAutorizarComprobar === "comprobar") {
            $("#aenvia").show();//envia a comprobacion
        } else if (hAutorizarComprobar === "autorizar") {
            $("#aautoriza").show();//autoriza
        }
        $("#arechaza").show();//rechaza

        $("#aedit").hide();
        $("#aguarda").hide();
        $("#acancela").hide();


        if ((DesactivaControl * 1) === 1) {
            $("#autorizadores").hide();
        }
        else {
            $("#autorizadores").show();
        }
    }

}

$("#aedit").click(function () {
    $("#aenvia").hide();
    $("#aedit").hide();
    $("#aanticipos").hide();
    $("#aagregarg").hide();
    $("#aguarda").show();
    $("#acancela").show();

    $("#verinforme #proyecto").attr("readonly", false);

    $("#lblproyecto").hide();
    $("#lblTipoInfVer").hide();
    $("#divTipoInfVer").show();
    $("#lblMesInfVer").hide();
    $("#divMesInfVer").show();

    $("#verinforme #proyecto").attr("type", "text");
});

function abrirfrmInforme(ac) {

    //$("#frmInforme").modal('show');

}

function cerrarfrmInforme() {
    //$("#frmInforme").modal('hide');
}
//OK
function GuardarInforme(ac) {
    if (ac === "I") {
        ac = 'InsertInforme';
    }
    var error = 0;
    if (valorVacio($.trim($("#proyecto").val()))) {
        $("#proyecto").notify("Ingresa el nombre del Proyecto", { position: "top" }, "error");
        error = 1;
    }
    if (valorVacio($("#responsable").val())) {
        $("#responsable").notify("Selecciona a un Responsable.", { position: "bottom center" }, "error");
        error = 1;
    }
    if (error === 1) {
        return false;
    }

    var Empresa = localStorage.getItem("IDEmpresa");
    var Usuario = localStorage.getItem("IDUsuario");
    var uresponsable = $("#responsable").val();
    var motivo = "";
    var viaje = $("#proyecto").val();
    var tipo = $("#tipoInf").val();
    var mes = $("#mes").val();

    var datos = {
        "idproyecto": 0,
        "ucrea": Usuario,
        "uresponsable": uresponsable,
        "motivo": "",
        "viaje": viaje,
        "pago": 0,
        "estatus": 1,
        "idapp": "WEB",
        "tipo": tipo,
        "idempresa": Empresa,
        "mes": mes
    };

    $.ajax({
        async: true,
        type: 'POST',
        url: '/api/GuardaInforme',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $("#frmInforme").modal('hide');
            cargando();
        },
        success: function (result) {
            cargado();
            $.each(result, function (i, value) {
                //$.notify("Informe Creado! No. [" + value.NINFORME + "]", {globalPosition: 'top center', className: 'success'});
                $("#tblProyectos").notify("Informe Guardado! No. [" + value.NINFORME + "]", { position: "top center", className: "success" });
                $("#proyecto").val("");
                Seguridad.bitacora("InsertInforme", 4, value.idproyecto + "," + value.idinforme, "Se Creo Informe No. [" + value.NINFORME + "].", 1);
                //ObtenerEstatusResponsables();
            });
            ObtenerInformes();
        },
        error: function (result) {
            alert(result);
            //console.log(result);
        }
    });
}
//OK
function ObtenerResponsables() {
    $("#responsable").empty();
    $("#responsablever").empty();

    var Empresa = localStorage.getItem("IDEmpresa");
    var UsuarioActivo = localStorage.getItem("IDUsuario");

    var datos = {
        "idempresa": Empresa,
        "uactivo": UsuarioActivo
    };

    $.ajax({
        type: 'POST',
        url: '/api/ObtieneResponsable',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            $("#responsable").append("<option value=''>- Todos -</option>");
            $("#responsablever").append("<option value=''>- Todos -</option>");
            $.each(result, function () {
                $("#responsable").append("<option data-icon='glyphicon-user' value='" + result[0].u_id + "' data-subtext='" + result[0].u_usuario + "'>&nbsp;" + result[0].nombre + "</option>");
                $("#responsable").val(result[0].uactivo);
                $("#responsablever").append("<option data-icon='glyphicon-user' value='" + result[0].u_id + "' data-subtext='" + result[0].u_usuario + "'>&nbsp;" + result[0].nombre + "</option>");
            });
        },
        error: function (result) {
            alert(result);
        }
    });
}
//OK
function ObtenerTipoInf() {
    var Empresa = localStorage.getItem("IDEmpresa");

    var datos = { "idempresa": Empresa };

    $.ajax({
        type: 'POST',
        url: '/api/ObtenerTipoInforme',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $("#tipoInf").empty();
            $("#tipoInfVer").empty();
            $("#tipoInf").append("<option value=''>- Tipo -</option>");
            $("#tipoInfVer").append("<option value=''>- Tipo -</option>");
        },
        success: function (result) {
            $.each(result, function (key, value) {
                var clave = result[0].c_clave;
                var nombre = result[0].c_nmb;
                var tipo = valorVacio(clave) ? nombre : (clave + " - " + nombre);

                $("#tipoInf").append("<option value='" + result[0].c_id + "'>" + tipo + "</option>");
                $("#tipoInfVer").append("<option value='" + result[0].c_id + "'>" + tipo + "</option>");

            });
        },
        error: function (result) {
            alert(result);
        }
    });
}
//OK
function ObtenerEstatusResponsables() {
    //console.log($("input#repde").val(), $("input#repa").val(), $("#estatusInf").val(), $("#responsableInf").val());
    //responsables informes creados
    var datos = { "idempresa": 0 };
    //estatus informes creados
    $("#estatusInf").empty();
    $("#estatusInf").append("<option value=''>- Todos -</option>");
    $.ajax({
        type: 'POST',
        url: '/api/EstatusInforme',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            $.each(result, function (key, value) {
                $("#estatusInf").append("<option value='" + result[0].i_estatus + "'>" + result[0].e_estatus + "</option>");
            });
        },
        error: function (result) {
            alert(result);
        }
    });
}

$("#aagregarg").click(function () {
    //verInformeGastos
    $("#mAgregarGastoInf").modal({
        show: true,
        keyboard: false,
        backdrop: "static"
    });

    $('#hgasto').timepicker({
        minuteStep: 1,
        appendWidgetTo: 'body',
        showSeconds: false,
        showMeridian: false,
        template: 'dropdown',
        modalBackdrop: true,
        defaultTime: horaActual("hh:mm"),
        clickShow: false
    });
    $("#hgasto").timepicker('setTime', horaActual("hh:mm"));

    $("#inpustGasto").css({
        opacity: 1,
        "background-color": "transparent"
    });
    $("#preInputGasto").hide('slow');


    $("#inpDA").hide();
    $("#btnInpDA").attr('datos-adicionales', 'false');

    $("#mAgregarGastoInf").css({ 'z-index': 2000 });
    opacityModalVerInfoG();

    $("#formapago, #categoria").select2({
        dropdownParent: $("#mAgregarGastoInf")
    });
    $("#fileotro:file").filestyle({
        input: false,
        buttonText: "&nbsp;Imagen", //"OTRO",
        btnClass: "btn-primary"
        //size: "sm"
    });
    $("#preInputGasto").hide();
    $('#fileotro').filestyle('clear');
    var fecha = fechaActual();// (fecUltgastos === "") ? fechaActual() : fecUltgastos;
    $("#fgasto").val(fecha);
    $("#fgasto").datepicker({
        dateFormat: "dd-mm-yy",
        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
        dayNamesShort: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"],
        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
        changeMonth: true,
        changeYear: true,
        onSelect: function (selectedDate) {
            var fechaymd = selectedDate.split("-");
            var fecha = fechaymd[2] + "-" + fechaymd[1] + "-" + fechaymd[0];
            var fcontablegasto = formatFecha(fecha, "01-mm-yyyy");
            //if (fcontablegasto !== $("#verinforme #mesInfVer").val()) {
            //    $("#fgasto").notify("La fecha de gasto NO corresponde al mes contable del informe.", { position: "top" }, "error");
            //}
        },
        beforeShow: function (input, inst) {
            var calendar = inst.dpDiv;
            setTimeout(function () {
                calendar.css({
                    'z-index': 2000
                });
            }, 0);
        }
    });
    setTimeout(function () {
        $("#fgasto").focus();
        $("#mAgregarGastoInf").css({
            color: 'black'
        });
    }, 400);
});

$("#mAgregarGastoInf, #anticiposInforme, #verComprobante, " +
    " #verDatosAdicionales, #verQuienAutoriza, #crearpago, " +
    "#MapUbicacionGasto").on('hidden.bs.modal', function () {

        $("#verInformeGastos").css({ 'z-index': 1041 });

    });

function opacityModalVerInfoG() {
    $("#verInformeGastos").css({ 'z-index': 1040 });
}

$("#btnGuardaGasto").click(function () {
    guardarGasto();
});

$("#tblProyectos").on("click, mouseenter", "tbody tr", function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    } else {
        $('#tblProyectos tbody tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});

$("#averhorag").click(function () {
    hideColHoraGasto("btn");
});

function hideColHoraGasto(origen) {
    var rowTotales = $('#tblGastos tfoot');
    var table = $('#tblGastos').DataTable();
    var column = table.column(2);
    // Toggle the visibility
    if (origen === "btn") {
        column.visible(!column.visible());
        if (column.visible() === false) {
            $("#tdcolsdatgasto").attr("colspan", 3);
            "#averhorag".AsHTML("<span class='zmdi zmdi-time'></span> Ver Hora Gasto");
        } else {
            $("#tdcolsdatgasto").attr("colspan", 4);
            "#averhorag".AsHTML("<span class='zmdi zmdi-timer-off'></span> Ocultar Hora Gasto");
        }
    }
    else {
        column.visible(false);
        $("#tdcolsdatgasto").attr("colspan", 3);
        "#averhorag".AsHTML("<span class='zmdi zmdi-time'></span> Ver Hora Gasto");
    }
    /*console.log(rowTotales)
    setTimeout(function () {
        $('#tblGastos tfoot').remove();
        $('#tblGastos').append("<tfoot></tfoot>");
        $('#tblGastos tfoot').append(rowTotales);

    }, 1000);*/
}

$("#aenvia").click(function () {
    var idinforme = $("#idinforme").val();
    var UsuarioActivo = localStorage.getItem("cosa");
    var req = DatosRequisicion();
    var datos =
        {
            "idinforme": idinforme,
            "Usuario": UsuarioActivo
        };

    var totalg = $("#totalg").val() * 1;//monto a comprobar informe
    var RmReqImporteComprobar = req.RmReqImporteComprobar * 1;//monto a comprobar requisicion
    var estatus = req.RmReqEstatus * 1;
    var estatusObligatorioReq = "Fondo Retirado";
    var estatusActualReq = req.RmReqEstatusNombre;

    if (estatus === 52) {
        if (totalg.toFixed(2) === RmReqImporteComprobar.toFixed(2)) {
            confEnviarAAutorizacion(datos);
        } else {
            Seguridad.alerta("No puedes enviar a comprobación el Informe.<br />El importe Gastado del Informe (" +
                formatNumber.new(totalg.toFixed(2), "$ ") + ") debe ser igual al importe comprobado en la requisición " +
                "(" + formatNumber.new(RmReqImporteComprobar.toFixed(2), "$ ") + ").", "#verInformeGastos");
        }
    } else {
        Seguridad.alerta("No puedes enviar a comprobación el Informe.<br />Tu requisición necesita estar en estatus <b>'" +
            estatusObligatorioReq + "'</b><br />" +
            "Estatus Actual de la requisición <b>'" + estatusActualReq + "'</b>.", "#verInformeGastos");
    }

});

function confEnviarAAutorizacion(datos) {
    var botones = [];
    botones[0] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            cargando();

            if ($("#ChkVoBo").is(":checked")) {
                //console.log("enviado a vobo");
                enviarVoBo();
            } else {
                //console.log("enviado a adminerp");

                var respuesta = enviarAAutorizacion(datos);
                //if (respuesta.stResultado === 1) {
                if (respuesta.stResultado === 1) {
                    $.notify(respuesta.descripcion, { globalPosition: 'top center', className: 'success' });
                } else {
                    //"Error al enviar comprobacion,favor de verificar"
                    $.notify(respuesta.descripcion, { globalPosition: 'top center', className: 'error' });
                }
                cargado();
            }

        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };

    //var inputchk = "<br /><label for='ChkVoBo'>" + chk2("ChkVoBo", "ChkVoBo", "", "", "", "18", "default", "default", "") + " Solicitar VoBo</label>";

    var inputchk = "<div id='divChkVoBo'>";
    inputchk += "<label class='custom-control custom-checkbox'>";
    inputchk += "<input type='checkbox' id='ChkVoBo' class='custom-control-input'>";
    inputchk += "<span class='custom-control-indicator'></span>";
    inputchk += "<span class='custom-control-description'>Solicitar VoBo</span>";
    inputchk += "</label>";
    inputchk += "</div>";

    Seguridad.confirmar("Desea enviar el informe a comprobacion? " + inputchk, botones, "Enviar Comprobante.", "#verInformeGastos");
    habilitaChkVoBo();
}

function habilitaChkVoBo() {
    var UsuarioEncriptadoActivo = localStorage.getItem("cosa");
    var UsuariDesencriptadoActivo = encriptaDesencriptaEle(UsuarioEncriptadoActivo, 0);
    var idinforme = $("#idinforme").val() * 1;
    $.ajax({
        async: true,
        type: 'POST',
        url: '/api/VistoBueno',
        data: JSON.stringify({ 'c_accion': 'VOBO', 'idinforme': idinforme, 'usuarioActual': UsuariDesencriptadoActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {

            var resultado = result[0];

            if (resultado.Resultado == 'OK') {
                $("#divChkVoBo").show();

                c_usuario_nombre = resultado.c_usuario_nombre;
                c_correo = resultado.c_correo;
                c_usuario = resultado.c_usuario;
                c_valor_default = resultado.c_valor_default * 1;
                c_chk_bloqueado = resultado.c_chk_bloqueado * 1;
                c_duracion_ini = resultado.c_duracion_ini;
                c_duracion_fin = resultado.c_duracion_fin;

                if (c_valor_default === 1) {
                    $("#ChkVoBo").attr("checked", true);
                } else {
                    $("#ChkVoBo").removeAttr("checked");
                }

                if (c_chk_bloqueado === 1) {
                    $("#ChkVoBo").attr("disabled", true);
                } else {
                    $("#ChkVoBo").removeAttr("disabled");
                }

                $("#HFUsuariovobo").val(c_usuario);

            }
            else {
                $("#divChkVoBo").hide();
                $("#ChkVoBo").removeAttr("checked");
                $("#ChkVoBo").removeAttr("disabled");
            }


        },
        error: function (result) {
            console.log(result);
        }

    });
}

function enviarVoBo() {
    var usuarioActual = localStorage.getItem("cosa");;
    var usuariovobo = $("#HFUsuariovobo").val();
    var idinforme = $("#idinforme").val();

    var datos = {
        "usuarioActual": usuarioActual,
        "usuariovobo": usuariovobo,
        "idinforme": idinforme,
    };

    $.ajax({
        async: true,
        type: 'POST',
        url: '/api/Enviovobo',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            cargando();
        },
        success: function (result) {

            $.notify("Se ha enviado correctamente a Visto Bueno.", { globalPosition: 'top center', className: 'success' });
            $("#verInformeGastos").modal("hide");
            ObtenerInformes();
        },
        error: function (result) {
            $.notify("Error al enviar Visto Bueno.", { globalPosition: 'top center', className: 'error' });

            console.log(result);
        }
    });
}

function DatosRequisicion() {
    var RmReqId = $("#HFRmRdeRequisicion").val() * 1;
    var datos = { 'Usuario': UsuarioActivo, 'RmReqId': RmReqId, 'Empleado': EmpeladoActivo };
    "#tddecrementado".AsHTML("<span style='font-size: 16px;' class='label label-warning'>$ 0.00</span>");
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
            "#tddecrementado".AsHTML("<span style='font-size: 16px;' class='label label-warning'>$ 0.00</span>");
        },
        success: function (result) {
            resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
            console.log(resultado);

            try {
                var fInicio = ((resultado.RmReqFechaRequerida).split("T"))[0];
                var fFin = ((resultado.RmReqFechaFinal).split("T"))[0];
                fInicio = formatFecha(fInicio, "dd-mm-yyyy");
                fFin = formatFecha(fFin, "dd-mm-yyyy");
                localStorage.setItem('fechasReq', JSON.stringify({ 'fInicio': fInicio, 'fFin': fFin }));

                //RmReqTotal - RmReqImporteComprobar = RmReqImporteDecrementado
                var RmReqTotal = resultado.RmReqTotal * 1;
                var RmReqImporteComprobar = resultado.RmReqImporteComprobar * 1;
                var RmReqImporteDecrementado = 0;
                if (RmReqImporteComprobar > 0)
                    RmReqImporteDecrementado = RmReqTotal - RmReqImporteComprobar;

                "#tddecrementado".AsHTML("<span style='font-size: 16px;' class='label label-warning'>" + formatNumber.new(RmReqImporteDecrementado.toFixed(2), "$ ") + "</span>");

                var disponible = $("#disAnticipo").val() * 1;

                disponible = (RmReqImporteDecrementado <= disponible) ? (disponible - RmReqImporteDecrementado) : 0;

                "#tddisponible".AsHTML("<span style='font-size: 16px;' class='label label-success'>" + formatNumber.new(disponible.toFixed(2), "$ ") + "</span>");
            }
            catch (err) {
                "#tddecrementado".AsHTML("<span style='font-size: 16px;' class='label label-warning'>$ 0.00</span>");
            }



        },
        error: function (result) {
            console.log("error", result);
            "#tddisponible".AsHTML("<span style='font-size: 16px;' class='label label-success'>$ 0.00</span>");
        }
    });
    return resultado;
}

function enviarAAutorizacion(datos) {
    var respuesta = [];
    $.ajax({
        async: false,
        type: "POST",
        url: "/api/Comprobacion",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            $("#verInformeGastos").modal('hide');
        },
        success: function (result) {
            //var stResultado = result.Salida.Resultado * 1;
            //if (stResultado === 1) {
            var stResultado = result;
            if (stResultado === 'OK') {
                respuesta = {
                    'stResultado': 1,
                    'descripcion': 'El informe se envio a comprobación.'
                };
                ObtenerInformes();
            } else {
                var error = stResultado; //datoEle(result.Salida.Errores.Error.Descripcion);
                if (valorVacio(error)) {
                    error = "Error el informe no se pudo enviar a comprobación.";
                }
                respuesta = {
                    'stResultado': 0,
                    'descripcion': error
                };
            }
        },
        error: function (result) {
            console.log(result);
            cargado();
            respuesta = {
                'stResultado': 0,
                'descripcion': result
            };
        }
    });
    return respuesta;
}

$("#arechaza").click(function () {
    var idinforme = $("#idinforme").val();
    var UsuarioActivo = localStorage.getItem("cosa");
    var botones = [];
    $("#ComentariosRechazp").val("");
    botones[0] = {
        text: "Si", click: function () {

            var Comentarios = $.trim($("#ComentariosRechazp").val());
            if (Comentarios === "") {
                $.notify("Se requiere un motivo de rechazo.", { globalPosition: 'top center', className: 'error', autoHideDelay: 2000 });
                return false;
            }

            var datos =
                {
                    "idinforme": idinforme,
                    "comentarioaut": Comentarios,
                    "usuario": UsuarioActivo
                };

            $.ajax({
                async: true,
                type: "POST",
                url: "/api/RechazarInforme",
                data: JSON.stringify(datos),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                cache: false,
                success: function (result) {


                    $.notify("El informe se ha rechazado correctamente", { globalPosition: 'top center', className: 'success', autoHideDelay: 8000 });

                    ObtenerInformes();
                    $("#verInformeGastos").modal('hide');

                }
            });

            $(this).dialog("close");

        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };

    var InputComentario = "<input id='ComentariosRechazp' data-toggle='tooltip' data-placement='top' data-html='true' aria-hidden='true' type='text' title='Comentarios de rechazo' />";

    tblcomentAut = "<table cellpadding='0' width='100%' cellspacing='0' border='0'>";
    tblcomentAut += "<tr><td>Comentario de rechazo</td></tr><tr><td>" + InputComentario + "</td></tr></table>";

    Seguridad.confirmar(tblcomentAut, botones, "Rechazar Informe", "#verInformeGastos");



});

$("#aautoriza").click(function () {
    var idinforme = $("#idinforme").val();
    var UsuarioActivo = localStorage.getItem("cosa");

    var datos =
        {
            "idinforme": idinforme,
            "Usuario": UsuarioActivo
        };

    var botones = [];

    botones[0] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            $.ajax({
                async: true,
                type: "POST",
                url: "/api/AutorizaInforme",
                data: JSON.stringify(datos),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                cache: false,
                success: function (result) {

                    $.notify("Informe Autorizado correctamente", { globalPosition: 'top center', className: 'success' });
                    $("#verInformeGastos").modal('hide');

                    ObtenerInformes();

                }
            });
        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Desea autorizar el informe?", botones, "Autorizar", "#verInformeGastos");

});

$("#autorizadores").click(function () {

    $("#tabAutoriza").css({ 'z-index': 4000 });

    $("#tabAutoriza").modal({
        show: true,
        keyboard: false,
        backdrop: "static"
    });
    if (localStorage.getItem('autOpcInf')) {
        localStorage.removeItem('autOpcInf');
    }
    //SelectProcesosRequisicion();
    selectUsuarios([]);

});

function SelectProcesosRequisicion() {
    var RmReqId = document.getElementById("HFRmRdeRequisicion").value;
    var listAutDefault = [];
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
                        var IdResponsable = datoEle(value.IdResponsable);//numero empleado
                        var Responsable = datoEle(value.Responsable);
                        var UsuarioAut = datoEle(value.Usuario);
                        var SgUsuIdAut = datoEle(value.SgUsuId);
                        var UAutInforme = valorVacio(UsuarioAut) ? SgUsuIdAut : UsuarioAut;
                        UAutInforme = $.trim(UAutInforme);
                        var txtAutorizador = valorVacio(autorizador) ? "" : " (" + autorizador + ")";
                        txtAutorizador = valorVacio(Responsable) ? autorizador : txtAutorizador;
                        var ok = Terminado === "SI" ? 1 : 0;
                        var status = ok === 1 ? SiNo2(ok) : "";
                        var esautorizador = (datoEle(autorizador).toLowerCase()).indexOf("autoriza");
                        var btnAutoriza = "", btnDeclinar = "";

                        if (!valorVacio(UAutInforme))
                            listAutDefault.push(UAutInforme);

                        var chkIncluir = chk2("ChkAutorizador" + IdResponsable, "ChkAutorizador", "checked", "", UAutInforme, "18", "default", "default", "");
                        if (i > 1 && txtAutorizador != 'Dotación de Anticipo' && !valorVacio(UAutInforme)) {
                            var row = "<tr>";
                            row += "<td>" + i + "</td>";
                            row += "<td>" + Responsable + txtAutorizador + "</td>";
                            row += "<td>" + chkIncluir + "</td></tr>";
                            $("#tblAutorizadoresReq tbody").append(row);
                        }
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
            selectUsuarios(listAutDefault);
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
function selectUsuarios(listAutDefault) {
    var listUsuarios = [];
    var UsuariosLista = false;
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaCatalogoUsuarios',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ 'Usuario': UsuarioActivo }),
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            var exito = result.Salida.Resultado * 1;
            if (exito === 1) {
                var resultado = result.Salida.Tablas.Catalogo.NewDataSet.Catalogo;
                var nusuarios = 0;
                try {
                    nusuarios = resultado.length;
                } catch (err) {
                    nusuarios = 0;
                }
                if (nusuarios > 0) {
                    $.each(resultado, function (key, value) {
                        if ($.inArray($.trim(value.SgUsuId), listAutDefault) === -1 &&
                            !valorVacio($.trim(value.SgUsuId)) &&
                            value.SgUsuActivo === "true" &&
                            !valorVacio(datoEle(value.SgUsuEmpleado))) {
                            listUsuarios.push(value);
                            UsuariosLista = true;
                        }
                    });
                } else {
                    if ($.inArray($.trim(resultado.SgUsuId), listAutDefault) === -1 &&
                        !valorVacio($.trim(resultado.SgUsuId)) &&
                        resultado.SgUsuActivo === "true" &&
                        !valorVacio(datoEle(resultado.SgUsuEmpleado))) {
                        listUsuarios.push(resultado);
                        UsuariosLista = true;
                    }
                }
            } else {
                $.notify("Error: Al consultar Usuarios.", { globalPosition: 'top center', className: 'error' });
            }
        },
        complete: function () {
            //cargado();
            //selectEmpleado(listUsuarios);
            $("#mAutOpcional").empty();
            $("#tblAutOpcional").hide();
            if (UsuariosLista === true) {
                $("#mAutOpcional").append("<option value=''> - Seleccionar Para Agregar - </option>");
                $.each(listUsuarios, function (key, value) {
                    var usu = $.trim(value.SgUsuId);
                    var nmb = $.trim(value.SgUsuNombre);
                    var option = "<option value='" + usu + "'>" + nmb + "</option>";
                    $("#mAutOpcional").append(option);
                });
                $("#mAutOpcional").select2({
                    dropdownParent: $("#lblAutOpcional")
                });
            }
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
        }
    });
}
$("#mAutOpcional").change(function () {
    var usuario = $("#mAutOpcional").val();
    var mAutOpcional = $("#mAutOpcional");
    var nombre = mAutOpcional[0].options[mAutOpcional[0].selectedIndex].text;
    var autOpc = [];
    if (!localStorage.getItem('autOpcInf')) {
        localStorage.setItem('autOpcInf', []);
    } else {
        autOpc = JSON.parse(localStorage.getItem('autOpcInf'));
    }
    var usuValido = validaUsuarioSel(usuario, autOpc);
    if (usuValido.ok === true) {
        autOpc.push({ 'usuario': usuario, 'nombre': nombre });
    } else {
        $("#mAutOpcional").notify(usuValido.descripcion, { globalPosition: 'top center', className: 'error' });
    }
    localStorage.setItem('autOpcInf', JSON.stringify(autOpc));
    tablaAutOpc(autOpc);
});
function tablaAutOpc(autOpc) {
    $("#tblAutOpcional tbody").empty();
    var i = 1;
    $.each(autOpc, function (key, value) {
        if (!valorVacio(datoEle(value.usuario)) && !valorVacio(datoEle(value.nombre))) {
            var inputs = "<input type='hidden' id='usuOpc" + value.usuario + "' name='usuOpc' value='" + value.usuario + "' />";
            inputs += "<button type='button' class='btn btn-danger btn-sm glyphicon glyphicon-trash DelGas' onclick='eliminarUsuario(\"" + value.usuario + "\", \"" + value.nombre + "\")'></button>"
            var row = "<tr>";
            row += "<td>" + i + "</td>";
            row += "<td>" + value.nombre + "</td>";
            row += "<td>" + inputs + "</td></tr>";
            $("#tblAutOpcional tbody").append(row);
            i++;
        }
    });
    if (i > 1)
        $("#tblAutOpcional").show();

    $("#tblAutOpcional").hide();
}
function eliminarUsuario(usuario, nombre) {
    var botones = [];
    botones[0] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            $("#tblAutOpcional tbody").empty();
            var autOpc = JSON.parse(localStorage.getItem('autOpcInf'));
            $.each(autOpc, function (key, value) {
                if (value.usuario === usuario) {
                    autOpc[key] = { 'usuario': '', 'nombre': '' };
                    //delete autOpc[key];
                }
            });
            localStorage.setItem('autOpcInf', JSON.stringify(autOpc));
            tablaAutOpc(autOpc);
        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Elimina usuario: <b>" + nombre + "</b><br />como autorizador del informe?", botones, " Elimina Usuario Autorizador.", "#tblAutOpcional");
}
function validaUsuarioSel(usuario, autOpc) {
    var seleccionado = [];
    if ($("#usuResponsable").val() !== usuario) {
        seleccionado['ok'] = true;
        seleccionado['descripcion'] = "Usuario agregado como autorizador.";
        $.each(autOpc, function (key, value) {
            if (value.usuario === usuario) {
                $("#mAutOpcional").val("");
                seleccionado['ok'] = false;
                seleccionado['descripcion'] = "No puedes agregar el mismo usuario más de una vez.";
                return seleccionado;
            }
        });
    } else {
        $("#mAutOpcional").val("");
        seleccionado['ok'] = false;
        seleccionado['descripcion'] = "El responsable del informe no puede ser su propio autorizador.";
        return seleccionado;
    }
    return seleccionado;
}

$("#tabAutoriza").on('hidden.bs.modal', function () {
    localStorage.removeItem('autOpcInf');
});


function selectEmpleado(listUsuarios) {
    //arma menu con empleados (posibles autorizadores)
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaCatalogoEmpleados',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ 'Usuario': UsuarioActivo }),
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            console.log("empleados: ", result);
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

$("#EnviarAutorizadores").click(function () {
    var idinforme = $("#idinforme").val()
    var RmReqId = document.getElementById("HFRmRdeRequisicion").value;
    var autorizadores = [];
    if (valorVacio($("#mAutOpcional").val())) {
        $.notify("Se requiere seleccionar un autorizador.", { globalPosition: 'top center', className: 'error' });
        return false;
    }
    autorizadores.push($("#mAutOpcional").val());
    /*
    $("input[type=hidden][name=usuOpc]").each(function () {
        var usuario = $(this).val();
        if (!valorVacio(usuario))
            autorizadores.push(usuario);
    });
    $("input:checkbox[name=ChkAutorizador]").each(function () {
        if ($(this).is(':checked')) {
            autorizadores.push($(this).val());
        }
    });
    */
    if (autorizadores.length > 0) {
        $.ajax({
            async: false,
            type: "POST",
            url: '/api/EnviaAutorizadores',
            data: JSON.stringify({ 'RmReqId': RmReqId, 'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo, 'idinforme': idinforme, 'autorizadores': autorizadores }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            cache: false,
            beforeSend: function () {

            },
            success: function (result) {
                $.notify("El informe se ha enviado al proceso de autorizadores", { globalPosition: 'top center', className: 'success' });
                ObtenerInformes();
                $("#tabAutoriza").modal('hide');
                $("#verInformeGastos").modal('hide');
            }
        });
    } else {
        $.notify("Se requiere seleccionar almenos a un autorizador.", { globalPosition: 'top center', className: 'error' });
    }
});
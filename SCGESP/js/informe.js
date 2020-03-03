/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 * Informes
 */
var tabla = "";
var tablaInformes = "";
var estatus = 0;
var cargaPag = false;
var tblinput_ag = "";
var UsuarioActivo = localStorage.getItem("cosa");
var EmpeladoActivo = localStorage.getItem("cosa2");
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
function cargaInicialInf() {
    //crearTabla("#tblProyectos", 0, "desc");
    //crearTabla("#tblGastos", 1, "asc");



    //crearTablaDetalleRow("#tblGastos", 0, "asc", 15, false);

    ObtenerInformes();

    ////////$("[]").tooltip();
}

function Historico() {
    
    var datos = {
        "Usuario": UsuarioActivo,
        "FechaInicio": '01/06/2018',
        "FechaFinal": '01/07/2018',
        "Origen": 'App'
    };


    $.ajax({
        type: 'POST',
        url: '/api/HistoricoApp',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
        },
        success: function (result) {
            
        }
    });

}


function ObtenerInformes() {
    $("#AlertInfoDisp").hide();

    var UsuarioActivo = localStorage.getItem("cosa");
    var EmpeladoActivo = localStorage.getItem("cosa2");
    //var estatusInf = $("#estatusInf").val();
    // var responsableInf = $("#responsableInf").val();

    var datos = {
        "estatus": 0,
        "uresponsable": UsuarioActivo,
        "uconsulta": UsuarioActivo,
        "empleadoactivo": EmpeladoActivo,
        "ExcluirEstatusReq": "Contabilizada"
    };

    $('#tblProyectos tbody').empty();
    if (!tablaInformes) {
        tablaInformes = crearTabla("#tblProyectos", 0, "desc");
    }

    var f = new Date();
    var fh = f.getDate() + '' + f.getMonth() + '' + f.getFullYear() + '' + f.getHours() + '' + f.getMinutes() + '' + f.getSeconds();

    $.ajax({
        type: 'POST',
        url: '/api/BrowseInforme',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cerrarfrmInforme();
            cargando();
        },
        success: function (result) {
            console.log(result);
            /*ver listado
             * 0: ninguno
             * 1: todos
             * 2: solo 1 (login)
             */

            tablaInformes
                .clear()
                .draw();
            $.each(result, function (i, value) {
                //console.log(value);
                var btnEdit = "";
                var ImpAutorizado = value.MontoRequisicion * 1;//Autorizado = importe solicitado en la requisicion
                var ImpCapturado = value.i_totalg * 1;//gastado=capturado
                var ImpComprobado = value.i_total * 1;//comprobado
                var ImpDisponible = (ImpAutorizado - ImpCapturado);//comprobado
                var iestatus = value.i_estatus * 1;//estatus informe
                var estatus = value.e_estatus;//estatus informe string
                var rechazado = value.rechazado * 1;//estatus informe rechazado

                btnEdit = "<button type='button' class='btn btn-success btn-sm' onclick='verInformeGastos(" + value.i_id + ", " + value.i_idproyecto + ", " + value.i_estatus + ")' data-dismiss='modal'><i class='zmdi zmdi-eye zmdi-hc-lg' style='padding: 3px 0px'></i> Ver</button>";
                var btnVer = "<a href='/ver_informe_responsable?" + fh + "&item=" + value.i_id + "' class='btn btn-success btn-sm'><i class='zmdi zmdi-eye zmdi-hc-lg' style='padding: 3px 0px'></i> Ver</a>";
                //if (todos === 1) {

                //var finicio = valorVacio(value.i_finicio) ? "" : formatFecha(new Date(value.i_finicio), "dd/mm/yyyy");
                //var ffin = valorVacio(value.i_ffin) ? "" : formatFecha(new Date(value.i_ffin), "dd/mm/yyyy");

                estatus = rechazado === 1 ? "Rechazado" : estatus;
                //var just = (value.i_nmb).split("");
                //var njust = just.length;
                //var justificacion = "<p style='color:black; word-wrap: break-word !important; width: 200px !important;'>" + value.i_nmb + "</p>";
                var justificacion = value.i_nmb;
                /*if (njust >= 30) {
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
                }*/

                tablaInformes.row.add([value.r_idrequisicion,
                value.i_ninforme,
                    justificacion,
                formatNumber.new(ImpAutorizado.toFixed(2), "$ "),
                value.responsable,
                    estatus,
                    btnVer
                ]).draw(false);
            });
            cargado();
        }, complete: function () {
            $("#tblProyectos tbody tr").each(function () {
                $(this)[0].cells[3].className = "text-right";
                $(this)[0].cells[2].className = "valJustificacion";
                console.log($(this)[0].cells[2]);
            });
        },
        error: function (result) {
            console.log(result);
            cargado();
            $.notify("Error al cargar los informes", { globalPosition: 'top center', className: 'error' });
        }
    });

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
$("#acancela").click(function () {
    $("#aenvia, #arelaciona, #aedit, #aagregarg, #aanticipos").show();
    $("#aguarda, #acancela").hide();
    $("#verinforme #proyecto").attr("readonly", true);

    $("#lblproyecto, #lblTipoInfVer, #lblMesInfVer").show();
    $("#divTipoInfVer, #divMesInfVer").hide();

    $("#verinforme #proyecto").attr("type", "hidden");

});
//inicio excel
$("#aexportarxls").click(function () {
    var idinforme = $("#idinforme").val() * 1;
    cargando();
    $("#verInformeGastos").modal("hide");
    if (idinforme > 0) {
        var datos = {
            'IdInforme': idinforme
        };

        var informe = selectInformeExcel(datos.IdInforme);
        if (informe.ok === true) {
            datos['NoInforme'] = informe.datos.i_ninforme;
            datos['NmbSolicitante'] = informe.datos.responsable;
            var requisicion = SelectRequisicionExcel(informe.datos.r_idrequisicion);
            if (requisicion.ok === true) {
                datos['TipoReq'] = datoEle(requisicion.datos.RmReqTipoRequisicionNombre);
                datos['Departamento'] = "";
                datos['Puesto'] = "";
                datos['Area'] = "";
                datos['Oficina'] = datoEle(requisicion.datos.RmReqOficinaNombre);
                datos['Centro'] = datoEle(requisicion.datos.RmReqCentroNombre);
            }
            generaExcel(datos);
        } else {
            console.log("error");
            $.notify("Error al generar excel.", { globalPosition: 'top center', className: 'error', autoHideDelay: 3000 });
        }
    } else {
        $.notify("Error al generar excel.", { globalPosition: 'top center', className: 'error', autoHideDelay: 3000 });
    }
    cargado();
    $("#verInformeGastos").modal({
        show: true,
        keyboard: false,
        backdrop: "static"
    });

});
function generaExcel(datos) {
    var rutaEli = "";
    ; $.ajax({
        async: false,
        type: 'POST',
        url: '/api/ExportarExcel',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            //console.log("SUCCESS", result)
            var rutas = result.split(",");
            var rutades1 = rutas[1];
            var rDescarga = rutades1.replace("api/ExportarExcel", "temp/") + rutas[2];
            rutaEli = rutas[0];
            window.location = rDescarga;
        },
        complete: function () {
            setTimeout(function () {
                if (rutaEli !== "") {
                    eliminaExcel(rutaEli);
                }
            }, 4000);
        },
        error: function (result) {
            console.log(result)
        }
    });
}
function eliminaExcel(rutaEli) {
    $.ajax({
        async: true,
        type: 'POST',
        url: '/api/EliminaExcel',
        data: JSON.stringify({ 'RutaExcel': rutaEli }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
        },
        error: function (result) {
            console.log(result)
        }
    });
}
function selectInformeExcel(id) {
    var datos = {
        "id": id
    };
    var datosInf = [];
    $.ajax({
        async: false,
        type: 'POST',
        url: '/api/SelectInforme',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            datosInf['ok'] = true;
            datosInf['datos'] = result[0];
        },
        error: function (result) {
            console.log(result);
            datosInf['ok'] = false;
            datosInf['datos'] = "Error al consultar informe";
        }
    });
    return datosInf;
}
function SelectRequisicionExcel(id) {
    var datos = { 'Usuario': UsuarioActivo, 'RmReqId': id };
    var datosReq = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaRequisicionIDCabecera',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        success: function (result) {
            var stResultado = result.Salida.Resultado;
            if (stResultado === "1") {
                var resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
                datosReq['ok'] = true;
                datosReq['datos'] = resultado;
            } else {
                datosReq['ok'] = false;
                datosReq['datos'] = "Error al consultar requisicion";
            }
        },
        error: function (result) {
            console.log(result);
            datosReq['ok'] = false;
            datosReq['datos'] = "Error al consultar requisicion";
        }
    });
    return datosReq;
}
//fin excel
function verInformeGastos(id, idproyecto, st) {
    $("#verinforme #proyecto").attr("readonly", true);
    $("#idinforme").val(id);
    $("#idproyecto").val(idproyecto);

    tblinput_ag = "";
    fecUltgastos = "";
    estatus = st;
    selectInforme(id);

    consultaInfoGastos(id, st, 1);

    $("#verInformeGastos").modal({
        show: true,
        keyboard: false,
        backdrop: "static"
    });

}
function consultaInfoGastos(id, st, actControles) {
    selectInforme(id, actControles);
    obtenerGastosInforme(id, idproyecto, st);
    //setTimeout(function () {
    //    DatosRequisicion(); //RmReqTotal - RmReqImporteComprobar = RmReqImporteDecrementado, RmReqMonedaNombre = 'Peso'
    //}, 3000);
}

function selectInforme(id, actControles) {
    $("#HFMontoRequisicion").val(0);
    //estatus = 0;
    //alert(id + ", " + idproyecto);
    var reembolso = 0;
    //var uresponsable = 0;
    var uresponsable = 0;

    //var Empresa = localStorage.getItem("IDEmpresa");

    //tarjeta toka 
    localStorage.setItem("tarjetatoka", "");

    var datos = {
        "id": id
    };
    var conciliacionOk = 0;
    $("#ConfBanco").val(conciliacionOk);
    $("#estatus").val(0);
    $("#inputCabeceraFormaPago").val("");
    //$("#usuResponsable").val("");
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
                //$("#aenvia");
                $("#tdninforme").empty();
                $("#lblRequisicion").empty();
                $("#tdestatus").empty();
                $("#tddel").empty();
                $("#tdal").empty();
                $("#tdcomAut").empty();
                $("#tdConXML").empty();
                $("#tdConBancaria").empty();
                $("#tdConConvenios").empty();
                $("#tdContabilizar").empty();
                $("#tdanticipo").empty();
                $("#tdgastado").empty();
                $("#tddisponible").empty();
                //$("#tdpagarares").empty();
                $("#tdpagore").empty();
                $("#tdtsreeembolsable").empty();
                $("#tdtnreeembolsable").empty();

                //tarjeta toka
                localStorage.setItem("tarjetatoka", datoEle(value.i_tarjetatoka));

                var str_estatus = value.e_estatus;//estatus informe string
                var rechazado = value.rechazado * 1;//estatus informe rechazado

                //tdcomAut
                $("#verinforme #proyecto").val(value.p_nmb);

                //document.getElementById("HFRmRdeRequisicion").value = value.r_idrequisicion;
                $("#HFRmRdeRequisicion").val(value.r_idrequisicion);

                estatus = value.i_estatus * 1;
                $("#estatus").val(estatus);
                $("#solAnticipo").hide();

                if (value.i_comentarioaut !== "" && value.i_comentarioaut !== null) {
                    var comRechazo = (value.i_comentarioaut).replace(/;;/gi, "<br />");
                    $("#tdcomAut").append("<b>Comentario de Rechazo: </b> " + comRechazo);
                    $("#comentarioaut").val(value.i_comentarioaut);
                }
                var lblCon = "<span style='font-size: 11px' class='label label-success'><span class='glyphicon glyphicon-ok'></span></span>";

                conciliacionOk = value.i_conciliacionbancos * 1;
                if (conciliacionOk === 1) {
                    $("#tdConBancaria").append("<b>Confrontación: </b> " + lblCon);
                }

                uresponsable = value.i_uresponsable;
                str_estatus = rechazado === 1 ? "Rechazado" : str_estatus;
                //$("#usuResponsable").val(uresponsable);
                $("#tdninforme").append(value.i_ninforme);
                $("#lblRequisicion").append(value.r_idrequisicion);
                $("#tdestatus").append(str_estatus);


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
                var disponible = parseFloat(value.Disponible);
                //var pagarares = value.PagarResponsable * 1;//por pagar a responsable
                var tsreembolso = value.i_tsreembolso * 1;
                var tnreembolso = value.i_tnreembolso * 1;
                $("#HFMontoRequisicion").val(anticipo);
                $("#tdanticipo").append("<span style='font-size: 16px;' class='label label-primary'>" + formatNumber.new(anticipo.toFixed(2), "$ ") + "</span>");
                $("#tdgastado").append("<span style='font-size: 16px;' class='label label-danger'>" + formatNumber.new(gastado.toFixed(2), "$ ") + "</span>");
                $("#tddisponible").append("<span style='font-size: 16px;' class='label label-success'>" + formatNumber.new(disponible.toFixed(2), "$ ") + "</span>");
                //$("#tdpagarares").append("<span style='font-size: 16px;' class='label label-warning'>" + formatNumber.new(pagarares.toFixed(2), "$ ") + "</span>");

                $(".pagore").hide();
                $(".trtreeembolsable").hide();

                $("#disAnticipo").val(disponible);

                "#lblCabeceraFormaPago".AsHTML(value.i_tarjetatoka);
                $("#inputCabeceraFormaPago").val(value.i_tarjetatoka);
                "#lblproyecto".AsHTML($("#verinforme #proyecto").val());
                "#lblresponsablever".AsHTML(value.responsable);

                $("#divresponsablever").hide();

                /*inicio totales*/
                $("#lbltotalg").empty();
                $("#lblmontog").empty();
                $("#lbltotalg").append(formatNumber.new((value.i_totalg * 1).toFixed(2), "$ "));
                $("#lblmontog").append(formatNumber.new((value.i_total * 1).toFixed(2), "$ "));
                $("#totalg").val(value.i_totalg);
                $("#montog").val(value.i_total);
                /*fin totales*/

            });
            $("#tipoInfVer, #mesInfVer").select2({
                dropdownParent: $("#verinforme")
            });

            if (actControles === 1)
                habilitaControlesInfo(idproyecto, id, estatus, uresponsable, reembolso);

            if (estatus === 2 && conciliacionOk === 1) {
                $("#aenvia").show();

                if (estatus === 2) {
                    $("#aconfrontar, #cancelarConfrontacion").show();
                    $("#confrontarInforme").hide();
                } else {
                    $("#aconfrontar").hide();
                }
            }
            else if (estatus === 2 && conciliacionOk === 0) {
                $("#aenvia, #cancelarConfrontacion, #confrontarInforme").hide();
                $("#aconfrontar").show();
            }

            if (conciliacionOk === 1) {
                $("#aagregarg").hide();
            }

            if (conciliacionOk === 0) {
                $("#aenvia").hide();
            }

            $("#ConfBanco").val(conciliacionOk);

            cargado();
        },
        error: function (result) {
            console.log(result);
        }
    });
    consultaDecremento();
}

function habilitaControlesInfo(idproyecto, id, estatus, uresponsable, reembolso) {
    $("#aenvia").hide();
    $("#aguarda").hide();
    $("#acancela").hide();
    $("#aanticipos").show();
    $("#verinforme #idinforme").val(id);
    $("#verinforme #idproyecto").val(idproyecto);
    $("#comentarioaut").val("");
    $("#aexportarxls").hide();
    $("#aexportarpdf").hide();
    $("#aterminar").hide();
    $("#apago").hide();
    $("#aexportarxls").show();
    //$("#aexportarxls").attr("onclick", "informexls(" + idproyecto + ", " + id + ")");
    $("#aagregarg").hide();

    if (estatus <= 2) {

        $("#aagregarg").show();
        $("#aenvia").show();
        $("#aedit").show();
    } else if (estatus === 3 || estatus === 7 || estatus === 5 || estatus === 8) {//3 = por autorizar, 7 = autorizado parcial, 5 = autorizado
        $("#aenvia").hide();
        $("#aedit").hide();
        $("#aguarda").hide();
        $("#acancela").hide();

        $("#auautoriza").show();

        if (estatus === 5)
            $("#aterminar").show();
        else
            $("#aterminar").hide();

        if (estatus === 8 && reembolso > 0)
            $("#apago").show();
        else
            $("#apago").hide();
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
            });
            ObtenerInformes();
        },
        error: function (result) {
            alert(result);
            //console.log(result);
        }
    });
}
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
$("#aagregarg").click(function () {
    localStorage.removeItem('comensales');
    var fechasReq = JSON.parse(localStorage.getItem("fechasReq"));
    $("#ncomensales").val("");
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
        minDate: fechasReq.fInicio,
        maxDate: fechasReq.fFin,
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
    "#MapUbicacionGasto, #confrontacion").on('hidden.bs.modal', function () {

        $("#verInformeGastos").css({ 'z-index': 1041 });

        localStorage.removeItem('comensales');
        $("#ncomensales, #ncomensalesda").val("");

    });

$("#verInformeGastos").on('hidden.bs.modal', function () {
    //tarjeta toka
    localStorage.setItem("tarjetatoka", "");
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
$("#averubicaciones").click(function () {
    hideColUbicaciones("btn");
});
function hideColUbicaciones(origen) {
    var table = $('#tblGastos').DataTable();
    var column = table.column(12);
    // Toggle the visibility
    if (origen === "btn") {
        column.visible(!column.visible());
        if (column.visible() === false) {
            $("#tdcolstotales").attr("colspan", 4);
            "#averubicaciones".AsHTML("<span class='zmdi zmdi-pin-drop'></span> Ver Ubicaciones");
        } else {
            $("#tdcolstotales").attr("colspan", 5);
            "#averubicaciones".AsHTML("<span class='zmdi zmdi-pin-off'></span> Ocultar Ubicaciones");
        }
    }
    else {
        column.visible(false);
        $("#tdcolstotales").attr("colspan", 4);
        "#averubicaciones".AsHTML("<span class='zmdi zmdi-pin-drop'></span> Ver Ubicaciones");
    }
}
$("#averhorag").click(function () {
    hideColHoraGasto("btn");
});
function hideColHoraGasto(origen) {
    var table = $('#tblGastos').DataTable();
    var column = table.column(3);
    // Toggle the visibility
    if (origen === "btn") {
        column.visible(!column.visible());
        if (column.visible() === false) {
            $("#tdcolsdatgasto").attr("colspan", 4);
            "#averhorag".AsHTML("<span class='zmdi zmdi-time'></span> Ver Hora Gasto");
        } else {
            $("#tdcolsdatgasto").attr("colspan", 5);
            "#averhorag".AsHTML("<span class='zmdi zmdi-timer-off'></span> Ocultar Hora Gasto");
        }
    }
    else {
        column.visible(false);
        $("#tdcolsdatgasto").attr("colspan", 4);
        "#averhorag".AsHTML("<span class='zmdi zmdi-time'></span> Ver Hora Gasto");
    }
}

function hideColFormaPago() {
    var table = $('#tblGastos').DataTable();
    var column = table.column(7);
    //console.log(table, column, column.visible());
    column.visible(false);//!column.visible()
}

$("#aenvia").click(function () {
    var ConfBanco = $("#ConfBanco").val() * 1;
    if (ConfBanco === 0) {
        Seguridad.alerta("No puedes enviar a autorización.<br />Informe NO Confrontado.", "#verInformeGastos");
        return false;
    }
    var vComensalesObjetivo = validaComensalesObjetivoEnGastos();
    if (vComensalesObjetivo.estatus === false) {
        Seguridad.alerta("No puedes enviar a autorización el informe.<br />" + vComensalesObjetivo.mensaje, "#verInformeGastos");
        return false;
    }
    var valida = validaExistenComprobantes();
    var req = DatosRequisicion();
    //console.log(req);
    //RmReqImporteComprobar, RmReqEstatus
    var totalg = $("#totalg").val() * 1;//monto a comprobar informe
    var RmReqImporteComprobar = req.RmReqImporteComprobar * 1;//monto a comprobar requisicion
    var estatus = req.RmReqEstatus * 1;
    var estatusObligatorioReq = "Fondo Retirado";
    var estatusActualReq = req.RmReqEstatusNombre;
    if (valida.NoGastosConComprobante != valida.NoGastos) {
        Seguridad.alerta("Todos los gastos deben contener almenos un comprobante.", "#verInformeGastos");
    } else {
        if (estatus === 52) {
            if (valida.NoGastosNoDeducible > 0) {
                confGastosNoDeducibles(totalg, RmReqImporteComprobar);
            } else if (totalg.toFixed(2) != RmReqImporteComprobar.toFixed(2)) {
                //ValidacionMensajeMontos();
                Seguridad.alerta("No puedes enviar a autorización el Informe.<br />" +
                    "Existen diferencias entre el importe gastado (" + formatNumber.new((totalg * 1).toFixed(2), "$ ") + ") " +
                    "y el importe comprobado (" + formatNumber.new((RmReqImporteComprobar * 1).toFixed(2), "$ ") + ") en la requisicion.", "#verInformeGastos");
            }
            else {
                enviarAAutorizacion();
            }
        } else {
            Seguridad.alerta("No puedes enviar a autorización el Informe.<br />Tu requisición necesita estar en estatus <b>'" +
                estatusObligatorioReq + "'</b><br />" +
                "Estatus Actual de la requisición <b>'" + estatusActualReq + "'</b>.", "#verInformeGastos");
        }
    }
});
function DatosRequisicion() {
    var RmReqId = $("#HFRmRdeRequisicion").val() * 1;
    var datos = { 'Usuario': UsuarioActivo, 'RmReqId': RmReqId, 'Empleado': EmpeladoActivo };
    var resultado = [];
    localStorage.removeItem('fechasReq');
    
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
            //console.log(resultado);
            var fInicio = ((resultado.RmReqFechaRequerida).split("T"))[0];
            var fFin = ((resultado.RmReqFechaFinal).split("T"))[0];
            fInicio = formatFecha(fInicio, "dd-mm-yyyy");
            fFin = formatFecha(fFin, "dd-mm-yyyy");
            localStorage.setItem('fechasReq', JSON.stringify({ 'fInicio': fInicio, 'fFin': fFin }));

            //RmReqTotal - RmReqImporteComprobar = RmReqImporteDecrementado
            var RmReqTotal = resultado.RmReqTotal * 1;
            var RmReqImporteComprobar = datoEle(resultado.RmReqImporteComprobar) * 1;
            var RmReqImporteDecrementado = 0;
            var RmReqEstatus = resultado.RmReqEstatus * 1;

            //disponible sin considerar decremento
            if (($("#totalg").val() * 1) > 0 && !valorVacio($("#totalg").val())) {
                $("#disAnticipo").val(RmReqTotal - ($("#totalg").val() * 1));
            }

            if (RmReqImporteComprobar > 0) {
                RmReqImporteDecrementado = RmReqTotal - RmReqImporteComprobar;
            } else if (RmReqImporteComprobar === 0 && RmReqEstatus === 53) {
                RmReqImporteDecrementado = RmReqTotal;
            }

            "#tddecrementado".AsHTML("<span style='font-size: 16px;' class='label label-warning'>" + formatNumber.new(RmReqImporteDecrementado.toFixed(2), "$ ") + "</span>");

            var disponible = parseFloat($("#disAnticipo").val());

            disponible = (RmReqImporteDecrementado <= disponible) ? (disponible - RmReqImporteDecrementado) : 0;

            "#tddisponible".AsHTML("<span style='font-size: 16px;' class='label label-success'>" + formatNumber.new(disponible.toFixed(2), "$ ") + "</span>");
            $("#disAnticipo").val(disponible);

        },
        error: function (result) {
            console.log("error", result);
        }
    });
    return resultado;
}

function consultaDecremento() {
    var RmReqId = $("#HFRmRdeRequisicion").val() * 1;
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
            //console.log(resultado);
            //RmReqTotal - RmReqImporteComprobar = RmReqImporteDecrementado
            var RmReqTotal = resultado.RmReqTotal * 1;
            var RmReqImporteComprobar = datoEle(resultado.RmReqImporteComprobar) * 1;
            var RmReqImporteDecrementado = 0;
            var RmReqEstatus = resultado.RmReqEstatus * 1;
            if (RmReqImporteComprobar > 0) {
                RmReqImporteDecrementado = RmReqTotal - RmReqImporteComprobar;
            } else if (RmReqImporteComprobar === 0 && RmReqEstatus === 53) {
                RmReqImporteDecrementado = RmReqTotal;
            }
            //console.log(resultado, RmReqImporteDecrementado, RmReqEstatus, RmReqTotal);

            "#tddecrementado".AsHTML("<span style='font-size: 16px;' class='label label-warning'>" + formatNumber.new(RmReqImporteDecrementado.toFixed(2), "$ ") + "</span>");
            
        },
        error: function (result) {
            console.log("error", result);
        }
    });
}

function validaExistenComprobantes() {
    var resultado = [];

    var idinforme = $("#idinforme").val();

    var datos =
        {
            "idinforme": idinforme
        };

    $.ajax({
        async: false,
        type: "POST",
        url: "/api/ValidaExistenComprobantes",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (result) {
            resultado = result;
        },
        error: function (result) {
            console.log(result);
        }
    });

    return resultado;

}
function confGastosNoDeducibles(totalg, RmReqImporteComprobar) {
    var botones = [];
    botones[0] = {
        text: "Aceptar", click: function () {
            $(this).dialog("close");
            if (totalg.toFixed(2) != RmReqImporteComprobar.toFixed(2)) {
                //ValidacionMensajeMontos();

                Seguridad.alerta("No puedes enviar a autorización el Informe.<br />" +
                    "Existen diferencias entre el importe gastado (" + formatNumber.new((totalg * 1).toFixed(2), "$ ") + ") " +
                    "y el importe comprobado (" + formatNumber.new((RmReqImporteComprobar * 1).toFixed(2), "$ ") + ") en la requisicion.", "#verInformeGastos");
            }
            else {
                enviarAAutorizacion();
            }
        }
    };
    botones[1] = {
        text: "Cancelar", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Existen Gastos sin comprobante Fiscal,<br />estos gastos se clasificaran como no deducibles.", botones, "Enviar a Autorización.", "#verInformeGastos");
}
function ValidacionMensajeMontos() {
    var botones = [];
    botones[0] = {
        text: "Aceptar", click: function () {
            $(this).dialog("close");
            enviarAAutorizacion();
        }
    };
    botones[1] = {
        text: "Cancelar", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Existen diferencias entre el importe a comprobar y el importe de la requisicion<br />Desea Continuar?.", botones, "Enviar a Autorización.", "#verInformeGastos");
}
function enviarAAutorizacion() {
    var idinforme = $("#idinforme").val();
    var idproyecto = $("#idproyecto").val();

    var datos =
        {
            "idinforme": idinforme
        };

    var botones = [];
    botones[0] = {
        text: "Aceptar", click: function () {
            $.ajax({
                async: true,
                type: "POST",
                url: "/api/EnviaAutorizacion",
                data: JSON.stringify(datos),
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                beforeSend: function () {

                    cargando();
                },
                success: function (result) {
                    $.notify("Informe enviado correctamente.", { globalPosition: 'top center', className: 'success', autoHideDelay: 6000 });
                    consultaInfoGastos(idinforme, idproyecto, 3, 1);
                    //actualiza lista de informes
                    ObtenerInformes();
                    $("#verInformeGastos").modal('hide');

                },
                complete: function () {

                    cargado();
                },
                error: function (result) {
                    cargado();
                    console.log(result);
                }
            });
            $(this).dialog("close");
        }
    };
    botones[1] = {
        text: "Cancelar", click: function () {
            $(this).dialog("close");
        }
    };

    var botones1 = [];

    botones1[0] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            Seguridad.confirmar("Enviar a Autorización?<br />Una vez enviado ya no podras hacer ningun cambio.", botones, "Enviar a Autorización.", "#verInformeGastos");
        }
    };
    botones1[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };

    var totalg = $("#totalg").val() * 1;
    var montog = $("#montog").val() * 1;

    Seguridad.confirmar("Enviar a Autorización?<br />Una vez enviado ya no podras hacer ningun cambio.", botones, "Enviar a Autorización.", "#verInformeGastos");
}
//confrontación
$("#aconfrontar").click(function () {
    //if ($("#tabsConfrontar").tabs()) {
    //  $("#tabsConfrontar").tabs("destroy").tabs();
    //}
    var IdInforme = $("#idinforme").val() * 1;
    var valida = validaExistenComprobantes();

    if (valida.NoGastosConComprobante != valida.NoGastos) {
        Seguridad.alerta("Para Confrontar, todos los gastos deben contener almenos un comprobante.", "#verInformeGastos");
        return false;
    }
    var vComensalesObjetivo = validaComensalesObjetivoEnGastos();
    if (vComensalesObjetivo.estatus === false) {
        Seguridad.alerta("No puedes confrontar el informe.<br />" + vComensalesObjetivo.mensaje, "#verInformeGastos");
        return false;
    }

    obtenerGastosInforme(IdInforme, 0, 2);
    $("#filebanco:file").filestyle({
        input: false,
        buttonName: "btn-success",
        buttonText: "&nbsp; Cargar Movimientos*"
    });
    var datos = JSON.parse(localStorage.getItem("listGastos"));
    var importeMin = 99999;
    var importeMax = 0;

    var FechaMin = "";
    var FechaMax = "";
    var tarjeta = "";
    $.each(datos, function (key, value) {
        importeMax = value.TGastado > importeMax ? value.TGastado : importeMax;
        importeMin = value.TGastado < importeMin ? value.TGastado : importeMin;

        tarjeta = value.FormaPago;

        var f1 = (value.FGasto).split("-");
        var fgasto = new Date(f1[2] + "-" + f1[1] + "-" + f1[0]);
        if (FechaMax === "" && FechaMin === "") {
            FechaMax = f1[2] + "-" + f1[1] + "-" + f1[0];
            FechaMin = f1[2] + "-" + f1[1] + "-" + f1[0];
        } else {
            var fMin = new Date(FechaMin);
            var fMax = new Date(FechaMax);
            FechaMax = fgasto.getTime() > fMax.getTime() ? f1[2] + "-" + f1[1] + "-" + f1[0] : FechaMax;
            FechaMin = fgasto.getTime() < fMin.getTime() ? f1[2] + "-" + f1[1] + "-" + f1[0] : FechaMin;
        }
    });

    FechaMax = formatFecha(FechaMax, 'dd-mm-yyyy');
    FechaMin = formatFecha(FechaMin, 'dd-mm-yyyy');

    $("#msnmb").empty();

    rangoFechas("repde2", "repa2", "reporte2", "BuscarMovBancariosParaConfrontar('" + tarjeta + "')");
    $("#repde2, #repde2 + span span").datepicker("destroy");

    var fechafin = FechaMax;
    var fechaini = FechaMin;
    $("input#repde2").val(fechaini);
    $("input#repa2").val(fechafin);


    $("#repa2").datepicker("option", "minDate", fechaini);

    $("#importede").val(Math.ceil(importeMin));
    $("#importea").val(Math.ceil(importeMax));


    $("#tabsConfrontar").tabs();
    $("#confrontacion").modal({
        show: true,
        keyboard: false,
        backdrop: "static"
    });
    BuscarMovBancariosParaConfrontar(tarjeta);
    opacityModalVerInfoG();
    $("#confrontacion").css({ 'z-index': 2000 });
});
$("#importede, #importea").change(function () {
    BuscarMovBancariosParaConfrontar("");
});
$("#btnBuscarMovBanco").click(function () {
    var IdInforme = $("#idinforme").val() * 1;
    obtenerGastosInforme(IdInforme, 0, 2);
    setTimeout(function () {
        BuscarMovBancariosParaConfrontar("");
    }, 2000);
    
});
$("#confrontarInforme").click(function () {
    var ConfBanco = $("#ConfBanco").val() * 1;
    if (ConfBanco === 0) {
        var IdInforme = $("#idinforme").val() * 1;
        var ImporteRequisicion = $("#HFMontoRequisicion").val() * 1;
        var ImporteMovBanco = $("#totalMovBanco").val() * 1;
        var ImporteGastado = $("#totalg").val() * 1;
        var vComensalesObjetivo = validaComensalesObjetivoEnGastos();
        if (vComensalesObjetivo.estatus === false) {
            Seguridad.alerta("No puedes confrontar el informe.<br />" + vComensalesObjetivo.mensaje, "#tabConfrontacion");
            return false;
        }
        var valida = validaExistenComprobantes();
        if (valida.NoGastosConComprobante != valida.NoGastos) {
            Seguridad.alerta("Para Confrontar, todos los gastos deben contener almenos un comprobante.", "#tabConfrontacion");
            return false;
        }

        if (ImporteGastado.toFixed(2) !== ImporteMovBanco.toFixed(2)) {
            Seguridad.alerta("No se puede confrontar el informe.<br />" +
                "El importe gastado y confrontado son diferentes.<br />" +
                "Importe a confrontar: <b>" + formatNumber.new(ImporteMovBanco.toFixed(2), "$ ") + "</b><br />" +
                "Importe gastado: <b>" + formatNumber.new(ImporteGastado.toFixed(2), "$ ") + "</b>",
                "#tabConfrontacion");
            return false;
        }

        var ImporteFondeo = ImporteRequisicion - ImporteMovBanco;
        ImporteFondeo = ImporteFondeo.toFixed(2);

        var msnFondeo = "Importe a confrontar: <b>" + formatNumber.new(ImporteMovBanco.toFixed(2), "$ ") + "</b><br />" +
            "Importe requisición: <b>" + formatNumber.new(ImporteRequisicion.toFixed(2), "$ ") + "</b><br />" +
            "Importe gastado: <b>" + formatNumber.new(ImporteGastado.toFixed(2), "$ ") + "</b><br />" +
            //"Al confrontar tu informe se solicitara un retiro por: <b>" + formatNumber.new(ImporteFondeo, "$ ") + "</b> (solo en caso necesario)<br />" +
            "Al confrontar ya no podras modificar tus gastos.";

        var botones = [];
        botones[0] = {
            text: "Si", click: function () {
                var datos = {
                    'IdInforme': IdInforme,
                    'ImporteRequisicion': ImporteRequisicion,
                    'ImporteMovBanco': ImporteMovBanco,
                    'ImporteGastado': ImporteGastado,
                    'ImporteFondeo': ImporteFondeo
                };
                confrontarInforme(datos);
                $(this).dialog("close");
            }
        };
        botones[1] = {
            text: "No", click: function () {
                $(this).dialog("close");
            }
        };
        Seguridad.confirmar("Confrontar Informe?<br />" + msnFondeo, botones, " Confrontar Informe.", "#tabConfrontacion");
    } else {
        Seguridad.alerta("El informe ya fue confrontado.", "#tabConfrontacion");
    }
});
function confrontarInforme(datos) {
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConfrontarInforme',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            //console.log(result)
            if (result.ConfrontacionOk === true) {
                $.notify(result.Descripcion, { position: "top center", className: "success" });
                consultaInfoGastos(datos.IdInforme, 2, 1);
                $("#confrontacion").modal("hide");
            } else {
                $.notify(result.Descripcion, { position: "top center", className: "error" });
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
function BuscarMovBancariosParaConfrontar(tarjeta) {
    var IdInforme = $("#idinforme").val() * 1;
    var repde = $("#repde2").val();
    var repa = $("#repa2").val();
    var importede = $("#importede").val() * 1;
    var importea = $("#importea").val() * 1;

    tarjeta = $.trim(tarjeta);
    if (valorVacio(tarjeta)) {
        var datos = JSON.parse(localStorage.getItem("listGastos"));
        tarjeta = datos[0]['FormaPago'];
    }
    repde = repde.split("-");
    repa = repa.split("-");

    var repde1 = repde[2] + "-" + repde[1] + "-" + repde[0];
    var repa1 = repa[2] + "-" + repa[1] + "-" + repa[0];
    var ConfBanco = $("#ConfBanco").val() * 1;
    var datos = {
        'RepDe': repde1,
        'RepA': repa1,
        'ImporteDe': importede,
        'ImporteA': importea,
        'Tarjeta': tarjeta,
        'IdInforme': IdInforme
    };
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaMovBancariosParaConfrontar',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargando();
        },
        success: function (result) {
            //console.log(result);
            $("#tblMovBanco tbody").empty();
            var i = 1;
            if (valorVacio(result))
                $("#confrontarInforme").hide();
            else
                $("#confrontarInforme").show();
            var tImporte = 0;
            $.each(result, function (key, value) {
                value["IdInf"] = IdInforme;
                var opc = menuOpcionesConfrontar(value);
                tImporte += value.Importe * 1;
                var row = "<tr>";
                row += "<td>" + value.Banco + "</td>";
                row += "<td>" + value.Descripcion + "</td>";
                row += "<td>" + value.Fecha + "</td>";
                row += "<td style='text-align: right'>" + formatNumber.new((value.Importe).toFixed(2), "$ ") + "</td>";
                row += "<td align='center'>" + opc.opciones + "</td>";
                row += "</tr>";
                $("#tblMovBanco tbody").append(row);
                if (opc.menu === true)
                    $("#confrontarInforme").hide();
            });
            $("#totalMovBanco").val(tImporte);
            "#tdTotalMovBanco".AsHTML(formatNumber.new(tImporte.toFixed(2), "$ "));
        },
        complete: function () {

            if (ConfBanco === 1) {
                $("#cancelarConfrontacion").show();
            }
            $("#tblMovSinGasto tbody tr").each(function () {
                try {
                    //$(this)[0].cells[5].className = "text-right";
                } catch (err) {
                }
            });
        },
        error: function (result) {
            console.log(result);
        }
    });
}
function menuOpcionesConfrontar(datos) {
    if (datos.Conciliacion === 0) {
        var menu = "<div class='dropdown'>";
        menu += "<button class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Opciones</button>";
        menu += "<div class='dropdown-menu'>";
        menu += "<a href='#' onclick='confLigarGasto(" + JSON.stringify(datos) + ")' class='dropdown-item'>Ligar a Gasto</a>";
        menu += "<a href='#' onclick='confAgregarGastoMovBanco(" + JSON.stringify(datos) + ")' class='dropdown-item'>Agregar como un nuevo Gasto</a>";
        menu += "</div>";
        menu += "</div>";
        var opciones = {
            'opciones': menu,
            'menu': true
        };
        return opciones;
    } else {
        var gasto = selGastoStorage(datos);
        gasto = "<h6><label class='label' style='font-size: 11px; margin:3px; color:black'>Gasto: " + gasto + "</label></h6>";
        var opciones = {
            'opciones': gasto,
            'menu': false
        };
        return opciones;
    }

}
function selGastoStorage(datos) {
    var listGastos = JSON.parse(localStorage.getItem("listGastos"));
    var gasto = "";
    $.each(listGastos, function (key, value) {
        if (datos.IdGasto === value.IdGasto) {
            gasto = value.Concepto + " / " + value.Negocio;
        }
    });
    return gasto;
}
function confLigarGasto(datos) {
    var datosMovBan = datos;
    var listGastos = JSON.parse(localStorage.getItem("listGastos"));
    var opcionGasto = "<ol style='list-style-type: none;'>";
    var checked = "checked";
    var nopciones = 0;
    $.each(listGastos, function (key, value) {
        if ((value.IdMovBanco * 1) === 0) {
            var total = formatNumber.new((value.TGastado).toFixed(2), "$ ");
            opcionGasto += "<li><label><input type='radio' name='radioGasto' " + checked + " value='" + JSON.stringify(value) + "'> " + value.Concepto + " <b>" + total + "</b></label></li>";
            checked = "";
            nopciones++;
        }
    });
    if (nopciones === 0) {
        Seguridad.alerta("No existen gastos para ligar.",
            "#tabConfrontacion");
        return false;
    }

    opcionGasto += "</ol>";
    var botones = [];
    botones[0] = {
        text: "Aceptar", click: function () {
            var datosGastos = $("input:radio[name='radioGasto']:checked").val();
            datosGastos = JSON.parse(datosGastos);
            ligarGasto(datosMovBan, datosGastos);
            $(this).dialog("close");
        }
    };
    botones[1] = {
        text: "Cancelar", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Selecciona un gasto:" + opcionGasto, botones, "Ligar Gasto.", "#tabConfrontacion");
}
function ligarGasto(datosMovBan, datosGastos) {
    //console.log("info mov banco => ", datosMovBan, "Info gasto => ", datosGastos);

    cargando();
    var datos = {
        'IdMovBanco': datosMovBan.IdMovBanco,
        'IdInforme': datosMovBan.IdInf,
        'IdGasto': datosGastos.IdGasto,
        'Importe': datosMovBan.Importe
    };

    crearLigaGasto(datos);
    obtenerGastosInforme(datosMovBan.IdInf, 0, 2);
    BuscarMovBancariosParaConfrontar(datosMovBan.Tarjeta);
    cargado();
}
function crearLigaGasto(datos) {
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/RelacionaGastoMovBanco',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
        },
        success: function (result) {
            var tipomsn = result.RelacionOk === true ? "success" : "error";
            $.notify(result.Descripcion, { position: "top center", className: tipomsn });
        },
        complete: function () {

        },
        error: function (result) {
        }
    });
}
function confAgregarGastoMovBanco(datos) {

    var disponible = $("#disAnticipo").val() * 1;

    if (datos.Importe > disponible) {
        Seguridad.alerta("No puedes agregar el gasto.<br />" +
            "El importe del gasto es mayor al importe disponible (por comprobar):<br />" +
            "Imp. disponible: <b>" + formatNumber.new(disponible.toFixed(2), "$ ") + "</b><br />" +
            "Imp. gasto: <b>" + formatNumber.new((datos.Importe).toFixed(2), "$ ") + "</b>",
            "#tabConfrontacion");
        return false;
    }

    var datosMovBan = datos;
    var botones = [];
    botones[0] = {
        text: "Aceptar", click: function () {
            agregarGastoMovBanco(datosMovBan);
            $(this).dialog("close");
        }
    };
    botones[1] = {
        text: "Cancelar", click: function () {
            $(this).dialog("close");
        }
    };
    var gasto = datosMovBan.Descripcion + " / " + formatNumber.new((datosMovBan.Importe).toFixed(2), "$ ");
    Seguridad.confirmar("Agregar movimiento bancario como un gasto:<br />" + gasto, botones, "Agregar Gasto.", "#tabConfrontacion");
}
function agregarGastoMovBanco(datosMovBan) {

    var elementoCat = $("#categoria")[0];
    var Categoria = elementoCat.options[0].value;
    var NombreCategoria = elementoCat.options[0].text;
    var datosCat = elementoCat.options[0].dataset;
    var ivacategoria = datosCat.grmativa;

    var ugasto = localStorage.getItem("cosa");
    var datos = {
        "idinforme": datosMovBan.IdInf,
        "idproyecto": 0,
        "ugasto": ugasto,
        "concepto": datosMovBan.Descripcion,
        "negocio": datosMovBan.Banco,
        "observaciones": "Movimiento Bancario",
        "rfc": "",
        "contacto": "",
        "telefono": "",
        "fgasto": (datosMovBan.Fecha).replace("/", "-"),
        "hgasto": horaActual("hh:mm"),
        "total": datosMovBan.Importe,
        "formapago": datosMovBan.Tarjeta,
        "categoria": Categoria,
        "correo": "",
        "fileotros": "",
        "nombreCategoria": NombreCategoria,
        "ivaCategoria": ivacategoria,
        "ncomensales": 0,
        "nmbcomensales": ""
    };
    //console.log("datos insert=>", datos);
    cargando();
    $.ajax({
        type: "POST",
        url: "/api/InsertGasto",
        data: JSON.stringify(datos), //checar con hector{'dirxml': dirxml, 'dirpdf': dirpdf, 'dirotros': dirotros},
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {

        },
        success: function (result) {
            $.notify("Gasto Guardado.", { globalPosition: 'top center', className: 'success' });
            var datosLiga = {
                'IdMovBanco': datosMovBan.IdMovBanco,
                'IdInforme': datosMovBan.IdInf,
                'IdGasto': result[0].IdGasto,
                'Importe': datosMovBan.Importe
            };
            crearLigaGasto(datosLiga);
        },
        complete: function () {
            cargado();
            consultaInfoGastos(datosMovBan.IdInf, 2, 1);
            BuscarMovBancariosParaConfrontar(datosMovBan.Tarjeta);
        },
        error: function (result) {
            console.log(result);
            $.notify("Error al Guardar", { globalPosition: 'top center', className: 'error' });
        }
    });

}
//confrontación carga excel
function preparaCarga() {
    $("#trIntrucciones").show();
    $("#trMovimientoEdoCuenta").hide();
}
function cargaBanco() {
    var IdInforme = $("#idinforme").val() * 1;
    cargando();
    var banco = $("#banco").val();
    if (!valorVacio(banco)) {
        resetInformeConfrontado();
        var file = $("#filebanco").get(0).files[0];
        var r = new FileReader();
        var nombre = file.name;
        var extFile = (nombre.substring(nombre.lastIndexOf(".") + 1)).toLowerCase();
        if (extFile === "xlsx") {
            r.onload = function () {
                var binimage = r.result;
                nombre = nombre.replace("." + extFile, "");
                guardarExcelBanco(banco, nombre, extFile, binimage, IdInforme);
            };
            r.readAsDataURL(file);
        } else {
            $("#filebanco").notify("El formato del archivo no es valido.", { globalPosition: 'top center', className: 'error' });
            $("#filebanco").filestyle('clear');
        }

    } else {
        $("#banco").notify("Seleciona un banco.", { globalPosition: 'top center', className: 'error' });
        $("#filebanco").filestyle('clear');
    }
}
function guardarExcelBanco(banco, nombre, extFile, binimage, IdInforme) {
    var datos = {
        'Usuario': encriptaDesencriptaEle(UsuarioActivo, 0),
        'ArchivoNmb': nombre,
        'ArchivoExt': extFile,
        'Archivo': binimage
    };
    var tablaMov = "";
    var total = 0;
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/CargarExcelBancoClosedXML', // CargarExcelBanco,
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            $("#filebanco").filestyle('clear');
            $("#tdTarjeta, #tdNombre, #tdNomina").empty();
            $("#tblMovimientos tbody").empty().remove();
            tablaMov = $("#tblMovimientos").DataTable()
            tablaMov.destroy();
            tablaMov = crearTablaReportes("#tblMovimientos", 0, "desc", false, '');
        },
        success: function (result) {
            if (!valorVacio(result)) {
                var resultado = result[0];
                if (resultado.ArchivoOk === true) {
                    "#tdNombre".AsHTML(resultado.Nombre);
                    "#tdNomina".AsHTML(resultado.Nomina);
                    $("#tblMovimientos").append("<tbody>");
                    var nmovimientos = 0;
                    try {
                        nmovimientos = resultado.RowExcel.length;
                    } catch (err) {
                        nmovimientos = 0;
                    }
                    if (nmovimientos > 0) {
                        var i = 0;
                        var Tarjeta = "", Descripcion = "", Fecha = "", Importe = 0, Tipo = "";
                        $.each(resultado.RowExcel, function (key, value) {
                            if (value.Fecha !== null) {
                                value['Nombre'] = resultado.Nombre;
                                value['Nomina'] = resultado.Nomina;
                                value['Embosado'] = resultado.Embosado;
                                value['IdInforme'] = IdInforme;
                                Tarjeta = value.Tarjeta;
                                Descripcion = value.Descripcion;
                                Fecha = ((value.Fecha).split(" "))[0];
                                Importe = value.Importe;
                                total += Importe;
                                //Tipo = value.Tipo;
                                //var fc = 'chkMovimiento($(this))';
                                var inpChk = chk("movBanco" + i, "movBanco", "checked", "", JSON.stringify(value), "18", "success", "danger", "");
                                var spanDuplicado = "<span id='tdDuplicado" + i + "' align='center'></span>";
                                var spanFechaDuplicado = "<span id='tdFDuplicado" + i + "' align='center'></span>";
                                var spanChk = "<span id='tdChk" + i + "' align='center'>" + inpChk + "</span>";
                                var newRow = [
                                    Tarjeta,
                                    Fecha,
                                    Descripcion,
                                    formatNumber.new(Importe.toFixed(2), "$ "),
                                    //spanDuplicado,
                                    //spanFechaDuplicado,
                                    spanChk
                                ];
                                tablaMov.row.add(newRow).draw(false);
                                i++;
                            }
                        });
                        "#tdTarjeta".AsHTML(Tarjeta);
                    }
                    $("#tblMovimientos").append("</tbody>");
                    //$("#totalMovBanco").val()
                    "#tdTotalConfrontar".AsHTML(formatNumber.new(total.toFixed(2), "$ "));
                    $("#trIntrucciones").hide();
                    $("#trMovimientoEdoCuenta").show();



                } else {
                    $.notify("Error al cargar archivo.", { position: "top", className: 'error' });
                }
            }
        },
        complete: function () {
            cargado();
            $("#tblMovimientos tbody tr").each(function () {
                $(this)[0].cells[3].className = "text-right";
            });
            validaDuplicados();
        },
        error: function (result) {
            cargado();
            console.log("error", result);
        }
    });
}
function validaDuplicados() {
    $("input:checkbox[name=movBanco]").each(function () {
        var id = ($(this)[0].id).replace("movBanco", "");
        var datos = JSON.parse($(this).val());
        datos['Banco'] = "TOKA";
        datos['IdChk'] = id;
        var duplicado = movBancoEsDuplicado(datos);
        datos['Duplicado'] = duplicado.Duplicado === "No" ? 0 : 1;
        datos['IdMovimiento'] = duplicado.IdMovimiento;
        datos['Usuario'] = encriptaDesencriptaEle(UsuarioActivo, 0);
        ("#tdDuplicado" + id).AsHTML(duplicado.Duplicado);
        ("#tdFDuplicado" + id).AsHTML(datoEle(duplicado.Fecha));

        $(this).val(JSON.stringify(datos));
    });
}
function movBancoEsDuplicado(datos) {
    var resultado = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/MovBancoEsDuplicado',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            resultado = result[0];

        },
        complete: function () {
            //cargado();
        },
        error: function (result) {
            //cargado();
            resultado = {
                'Duplicado': "No",
                'Fecha': null,
                'IdMovimiento': 0
            };
            console.log("error", result);
        }
    });
    return resultado;
}
$("#guardarMBanco").click(function () {
    var elementosSel = false;
    var IdInforme = $("#idinforme").val() * 1;
    //resetInformeConfrontado();
    $("input:checkbox[name=movBanco]").each(function () {
        if ($(this).is(':checked')) {
            elementosSel = true;
            var datos = $(this).val(); //JSON.parse($(this).val());
            var resultado = guardarMBanco(datos);
            if (resultado.Exito.GuardadoOk === true) {
                var idchk = resultado.DatosGuardados.IdChk;
                ("#tdChk" + idchk).AsHTML("<span style='font-size: 11px' class='label label-success'><span class='glyphicon glyphicon-ok'></span> Cargado</span>");
            } else {
                ("#tdChk" + idchk).AsHTML("<span style='font-size: 11px' class='label label-danger'><span class='glyphicon glyphicon-remove'></span> Cargado</span>");
            }
        }
    });

    if (elementosSel === false) {
        $.notify("Seleccionar movimiento(s).", { position: "top center", className: 'error' });
    } else {
        obtenerGastosInforme(IdInforme, 0, 2);
        $.notify("Información guardada.", { position: "top center", className: 'success' });
    }
});
function resetInformeConfrontado() {
    var IdInforme = $("#idinforme").val() * 1;
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ResetInformeConfrontado',
        data: JSON.stringify({ 'IdInforme': IdInforme }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            BuscarMovBancariosParaConfrontar("");
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
            resultado = null;
        }
    });
}
function guardarMBanco(datos) {
    var IdInforme = $("#idinforme").val() * 1;
    var resultado = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/GuardarMovBanco',
        data: datos,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            resultado = {
                'DatosGuardados': JSON.parse(datos),
                'Exito': result
            };
        },
        complete: function () {
            //cargado();
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
            resultado = null;
        }
    });
    return resultado;
}
$("#cancelarConfrontacion").click(function () {
    var IdInforme = $("#idinforme").val() * 1;
    var botones = [];
    botones[0] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            resetInformeConfrontado();
            consultaInfoGastos(IdInforme, 2, 1);
        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Cancelar la confrontación del informe?", botones, " Cancelar Confrontación.", "#tabConfrontacion");
});

$("#arefresh").click(function () {
    var IdInforme = $("#idinforme").val() * 1;
    var estatus = $("#estatus").val() * 1;

    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ActualizarTotalesInforme',
        data: JSON.stringify({ 'IdInforme': IdInforme }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            //
        },
        complete: function () {
            //cargado();
            consultaInfoGastos(IdInforme, estatus, 1);
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
            resultado = null;
        }
    });

    $.notify("Informe Actualizado", { globalPosition: 'top center', className: 'success' });
});
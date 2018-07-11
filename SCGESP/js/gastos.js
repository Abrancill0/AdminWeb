/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 * GASTOS
 */
var tablaGastos = "";
var disabled = "";
var trueFalse = false;
var fecUltgastos = "";
var UsuarioActivo = localStorage.getItem("cosa");
var EmpeladoActivo = localStorage.getItem("cosa2");

//var PGastos = Seguridad.permisos(5);

function formatoInputFile() {
    $(":file.filestyle[id!='fileotro']").filestyle({
        input: false,
        buttonText: "",
        size: "xs"
    });
}
function obtenerGastosInforme(id, idproyecto, estatusinf) {

    estatusinf = estatusinf * 1;
    $("#divListGastos").hide();
    if (!tablaGastos) {
        tablaGastos = $("#tblGastos").DataTable({
            "order": [[1, "desc"]],
            "processing": true,
            "scrollY": "200px",
            "scrollCollapse": true,
            scrollX: true,
            paging: false,
            searching: false,
            "ordering": false,
            "language": {
                "lengthMenu": "_MENU_ Registros Por Página",
                "zeroRecords": "No se encontraron Registros",
                "info": "Página _PAGE_ de _PAGES_ (_MAX_ Registros)",
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
                setTimeout(function () { $("#tblGastos").DataTable().draw(); }, 200);
            },
            "autoWidth": false,
            "fixedHeader": {
                "header": true,
                "footer": true
            }
        });
    }

    var datos = {
        "idinforme": id,
        "idproyecto": 0
    };
    var i = 0;
    var uresponsable = 0;
    var UpdateGasto = 0;
    var UpdateMiGasto = 0;
    var SelectDatosAdicional = 0;
    var DeleteGasto = 0;
    var uidlogin = 0;
    var listGastos = [];
    var total = 0, totalmonto = 0;

    $('#totalg').val(0);
    $('#montog').val(0);

    $.ajax({
        async: false,
        type: "POST",
        url: "/api/browseGastosInforme",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        success: function (result) {
            var menuFpago = menuFormaPago();
            var menucategoria = menucategorias2();
            var numGastos = result.length;
            tablaGastos
                .clear()
                .draw();
            $.each(result, function (key, value) {
                var menuFpago2 = menuFpago;
                var menucategoria2 = menucategoria;

                i++;
                var txtfgasto = "", txtconcepto = "", txtnegocio = "", txttotal = ""
                    , filexml = "", filepdf = "", fileotro = "", txtobservaciones = ""
                    , chkAplica = "", tblBtn = "", txtgorigen = "", txtmonto = "", txthgasto = "";

                var aplica = value.g_aplica * 1;

                //variables de tipo de gasto
                var tipoajuste = value.tipoajuste * 1;
                var najustes = value.najustes * 1;

                var ngasto_orden = value.orden;
                var valmaxpropina = value.valmaxpropina * 1;

                var gcomprobante = value.g_comprobante * 1;
                var latitud = valorVacio(value.g_latitud) ? "" : value.g_latitud;
                var logitud = valorVacio(value.g_longitud) ? "" : value.g_longitud;
                var btnVerUbi = "";
                uresponsable = value.i_uresponsable * 1;

                var confrontacionBan = (value.g_conciliacionbancos * 1);
                if (tipoajuste !== 1) {
                    filexml = "<span id='inputfilexml" + value.g_id + "'>";
                    filexml += inputfilexml(id, 0, value.g_id, aplica, value.g_dirxml, value.g_dirpdf, value.g_dirotros, estatusinf, value.g_idgorigen, tipoajuste, confrontacionBan);
                    filexml += "</span>";
                }

                filepdf = "<span id='inputfilepdf" + value.g_id + "'>";
                filepdf += inputfilepdf(id, 0, value.g_id, aplica, value.g_dirxml, value.g_dirpdf, value.g_dirotros, estatusinf, confrontacionBan);
                filepdf += "</span>";

                fileotro = "<span id='inputfileotro" + value.g_id + "'>";
                fileotro += inputfileotro(id, 0, value.g_id, aplica, value.g_dirxml, value.g_dirpdf, value.g_dirotros, estatusinf, confrontacionBan);
                fileotro += "</span>";


                if (gcomprobante === 1) {
                    if (value.MONTO == 0 && tipoajuste !== 1) {
                        txtmonto = "<div id='monto" + value.g_id + "' style='display: table-cell; text-align: right; width: 80px; word-wrap: break-word'>" + formatNumber.new((value.g_total * 1).toFixed(2), "$ ") + "</div>";
                    }
                    else {
                        txtmonto = "<div id='monto" + value.g_id + "' style='display: table-cell; text-align: right; width: 80px; word-wrap: break-word'>" + formatNumber.new((value.MONTO * 1).toFixed(2), "$ ") + "</div>";
                    }
                } else {
                    txtmonto = "<div id='monto" + value.g_id + "' style='display: table-cell; text-align: right; width: 80px; word-wrap: break-word'>$ 0.00</div>";
                }


                var hrmm = (valorVacio(value.g_hgasto) ? "0:00" : value.g_hgasto).split(":");
                var hgasto = "0:00";
                if ((hrmm[0] * 1) === 0 && (hrmm[1] * 1) === 0) {
                    hgasto = "0:00";
                } else {
                    hgasto = (hrmm[0] * 1) + ":" + hrmm[1];
                }
                //console.log(value.g_conciliacionbancos, "<-conciliacion");
                if (estatusinf <= 2 && (value.g_conciliacionbancos * 1) === 0) {//&& tipoajuste === 0
                    txtfgasto = inputTextGasto("fgasto" + value.g_id, value.g_id + "fgasto", value.g_fgasto, "actualizarGasto(" + value.g_id + ", " + id + ", " + 0 + ", \"" + value.g_dirxml + "\", \"" + value.g_dirpdf + "\", \"" + value.g_dirotros + "\", " + estatusinf + ", \"txtFecha\")", 'width: 80px;', "", "readonly", 'form-control fgasto', "fgasto='" + value.g_fgasto + "'");
                    txtconcepto = inputTextGasto("concepto" + value.g_id, "concepto" + value.g_id, value.g_concepto, "actualizarGasto(" + value.g_id + ", " + id + ", " + 0 + ", \"" + value.g_dirxml + "\", \"" + value.g_dirpdf + "\", \"" + value.g_dirotros + "\", " + estatusinf + ", \"text\")", "width: 100px;", "", "", "", "");
                    txtnegocio = inputTextGasto("negocio" + value.g_id, "negocio" + value.g_id, value.g_negocio, "actualizarGasto(" + value.g_id + ", " + id + ", " + 0 + ", \"" + value.g_dirxml + "\", \"" + value.g_dirpdf + "\", \"" + value.g_dirotros + "\", " + estatusinf + ", \"text\")", "width: 100px;", "", "", "", "");

                    //si existen gastos de ajuste el importe gastado ya no se podra modificar
                    //if (najustes === 0)
                    txttotal = inputTextGasto("total" + value.g_id, "total" + value.g_id, value.g_total, "actualizarGasto(" + value.g_id + ", " + id + ", " + 0 + ", \"" + value.g_dirxml + "\", \"" + value.g_dirpdf + "\", \"" + value.g_dirotros + "\", " + estatusinf + ", \"txtTotal\")", "width: 100%;", "", "", "form-control text-right inp-total-gastado", "onkeypress='return justNumbers(event);' valueOld='" + value.g_total + "'");
                    //else
                    //    txttotal = formatNumber.new((value.g_total * 1).toFixed(2), "$ ") + "<input type='hidden' id='total" + value.g_id + "' name='total" + value.g_id + "' value='" + value.g_total + "' />";

                    txthgasto = inputTextGasto("hgasto" + value.g_id, "hgasto" + value.g_id, hgasto, "", "width: 60px;", "", "readonly", "form-control input-small hgasto", "hgasto='" + hgasto + "' onTimeChange='actualizarGasto(" + value.g_id + ", " + id + ", " + idproyecto + ", \"" + value.g_dirxml + "\", \"" + value.g_dirpdf + "\", \"" + value.g_dirotros + "\", " + estatusinf + ", \"txtHgasto\")'");

                    var functionActfp = "actualizarGasto(" + value.g_id + ", " + id + ", " + 0 + ", \"" + value.g_dirxml + "\", \"" + value.g_dirpdf + "\", \"" + value.g_dirotros + "\", " + estatusinf + ", \"selectfp\")";
                    var functionActc = "actualizarGasto(" + value.g_id + ", " + id + ", " + 0 + ", \"" + value.g_dirxml + "\", \"" + value.g_dirpdf + "\", \"" + value.g_dirotros + "\", " + estatusinf + ", \"selectc\")";
                    menuFpago2 = menuFpago.replace("formapago", "formapago" + value.g_id).replace("onchange", "onchange='" + functionActfp + "'");
                    menucategoria2 = menucategoria.replace("categoria", "categoria" + value.g_id).replace("onchange", "onchange='" + functionActc + "' " + ((aplica === 0) ? 'disabled' : ''));

                    chkAplica = chk("chk_" + 0 + "_" + id + "_" + value.g_id
                        , "chk_" + 0 + "_" + id + "_" + value.g_id, (aplica === 1 ? "checked" : "")
                        , "aplicaGasto(this.value, \"" + value.g_dirxml + "\", \"" + value.g_dirpdf + "\", \"" + value.g_dirotros + "\", \"" + value.g_categoria + "\", " + estatusinf + ", " + (value.g_total * 1) + ", " + (value.MONTO * 1) + ", " + value.g_idgorigen + ")", idproyecto + "_" + id + "_" + value.g_id, "18", "success", "danger", "");

                    txtobservaciones = inputTextGasto("observaciones" + value.g_id, "observaciones" + value.g_id, value.g_observaciones, "actualizarGasto(" + value.g_id + ", " + id + ", " + 0 + ", \"" + value.g_dirxml + "\", \"" + value.g_dirpdf + "\", \"" + value.g_dirotros + "\", " + estatusinf + ", \"text\")", "width: 130px;", "", "", "", "");

                    var disa = "";
                    var bgcolor = "";
                    var lblTooltipOri = "";
                    if (!valorVacio(value.g_dirxml)) {
                        disa = "readonly";
                        bgcolor = "background-color: lightgray;";
                    } else {
                        lblTooltipOri = "<div style='width: 170px; text-align: left'>Indica el numero de gasto<br /> con el que se relaciona este gasto</div>";
                    }
                    txtgorigen = "<input id='gorigen" + value.g_id + "' " + disa + " ngasact='" + i + "' class='txtgorigen' onkeypress='return justNumbers(event);' onchange='relaciona(this.value, " + 0 + ", " + id + ", " + value.g_id + ", " + estatusinf + ", " + value.g_idgorigen + ", " + numGastos + ")' data-toggle='tooltip' data-placement='top' data-html='true' " +
                        " title=\"" + lblTooltipOri + "\" aria-hidden='true' value='' name='gorigen" + value.g_id + "' type='text' style='width: 20px;" + bgcolor + "' />";
                } else {
                    var fg = (value.g_fgasto).split('-');
                    txtfgasto = formatFecha(fg[2] + '-' + fg[1] + '-' + fg[0], 'dd mmm'); //
                    txtconcepto = value.g_concepto;
                    txtnegocio = value.g_negocio;
                    txttotal = formatNumber.new((value.g_total * 1).toFixed(2), "$ ");
                    txthgasto = hgasto;
                    menuFpago2 = value.g_formapago;
                    /*
                    if (estatusinf === 2 && (tipoajuste === 1 || tipoajuste === 2) && (value.g_conciliacionbancos * 1) === 0) {
                        var datosUp = {
                            'IdInforme': id,
                            'IdGasto': value.g_id,
                            'DirXML': value.g_dirxml,
                            'TipoAjuste': tipoajuste,
                            'TGastado': 0,
                            'TComprobar': (value.MONTO * 1)
                        };
                        var functionActc = "actualizarCategoriaGastoAjuste(" + JSON.stringify(datosUp) + ")";
                        menucategoria2 = menucategoria.replace("categoria", "categoria" + value.g_id).replace("onchange", "onchange='" + functionActc + "' ");
                        menucategoria2 = menucategoria2;
                    } else {
                        menucategoria2 = value.g_nombreCategoria;
                    }
                    */
                    menucategoria2 = value.g_nombreCategoria;
                    chkAplica = aplica === 1 ? "<span style='font-size: 11px' class='label label-success'><span class='glyphicon glyphicon-ok'></span></span>" : "<span style='font-size: 11px' class='label label-danger'><span class='glyphicon glyphicon-remove'></span></span>";
                    txtobservaciones = "<div style='display: table-cell; width: 200px; word-wrap: break-word'>" + value.g_observaciones + "</div>";
                    txtgorigen = "";

                    var txttype = "text";
                    if (value.g_id === value.g_idgorigen) {
                        txttype = "hidden";
                    }

                    txtgorigen = "<span id='tooltip" + value.g_id + "' data-toggle='tooltip' data-placement='top' data-html='true' " +
                        " title=\"<div style='width: 170px;'>El gasto esta relacionado</div>\" aria-hidden='true'><input id='gorigen" + value.g_id + "' disabled class='txtgorigen' value='' name='gorigen" + value.g_id + "' type='" + txttype + "' style='width: 20px;' /></span>";

                }

                fecUltgastos = value.g_fgasto;

                var id_gasto = lblNGasto(value.g_id, value.g_total, value.MONTO, i, value.g_idgorigen, ngasto_orden, tipoajuste);

                tblBtn = tblBtnGasto(0, id, value.g_id, SelectDatosAdicional, DeleteGasto, estatusinf, uresponsable, uidlogin, value.g_dirxml, value.g_dirpdf, value.g_dirotros, (value.g_concepto + " " + value.g_negocio), confrontacionBan);

                total += value.g_total * 1;
                totalmonto += (gcomprobante === 1 ? value.MONTO : 0) * 1; //(((value.MONTO * 1) > 0) ? value.MONTO : value.g_total) * 1;
                
                var datosGasto = [];
                if (tipoajuste === 0) {
                    datosGasto = {
                        'IdInforme': id, 'IdGasto': value.g_id,
                        'FGasto': value.g_fgasto, 'HGasto': value.g_hgasto,
                        'Concepto': value.g_concepto, 'Negocio': value.g_negocio,
                        'FormaPago': value.g_formapago, 'IdCategoria': value.g_categoria,
                        'Categoria': value.g_nombreCategoria, 'IvaCategoria': value.g_ivaCategoria,
                        'TGastado': (value.g_total * 1), 'TComprobar': (value.MONTO * 1),
                        'ConciliacionBanco': value.g_conciliacionbancos, 'IdMovBanco': value.g_idmovbanco,
                        'Observaciones': value.g_observaciones, 'UGasto': value.g_ugasto,
                        'ValMaxPropina': (value.g_total * 1), 'DirXML': value.g_dirxml
                    };//valmaxpropina
                }
                var menuAdicional = "";
                if (tipoajuste === 0 && (value.g_conciliacionbancos * 1) === 0) //valida que el movimiento sea un gasto (no ajuste)
                    menuAdicional = menuOpcionesAdicionales(datosGasto);

                var linkComensales = "";
                var ncomensales = value.ncomensales * 1;
                var nmbcomensales = ncomensales > 0 ? value.nmbcomensales : "";
                //console.log(ncomensales, nmbcomensales);
                if (ncomensales === 0 && tipoajuste === 0 && estatusinf === 2) {
                    linkComensales = inputTextGasto("comensal_" + value.g_id, "comensal_" + value.g_id, "", "actualizaComensales(this.value," + value.g_id + ", " + id + ")", "width: 130px;", "", "", "", "");
                } else if (ncomensales === 1) {
                    if (tipoajuste === 0 && estatusinf === 2 && (value.g_conciliacionbancos * 1) === 0) {
                        linkComensales = inputTextGasto("comensal_" + value.g_id, "comensal_" + value.g_id, nmbcomensales, "actualizaComensales(this.value," + value.g_id + ", " + id + ")", "width: 130px;", "", "", "", "");
                    } else {
                        linkComensales = nmbcomensales !== "" ? nmbcomensales : "";
                    }
                } else if (ncomensales > 1) {
                    //linkComensales = "<button type='button' class='btn btn-link' onclick='verComensale(" + value.g_id + ", " + id + "," + ncomensales + ", \"" + nmbcomensales + "\", " + estatusinf + ",  " + (value.g_conciliacionbancos * 1) + ")'>Ver " + ncomensales +" Comensales</button>";
                    linkComensales = "<button type='button' class='btn btn-link' onclick='verDatosAdicionales(" + value.g_id + ", " + id + ", " + 0 + ", " + estatusinf + ")'>Click para ver <b>" + ncomensales + "</b> Comensales</button>";
                }
                var inpComensales = "<input type='hidden' id='inpComensalesGasto" + value.g_id + "' tipoajuste='" + tipoajuste + "' value='" + nmbcomensales + "' />";
                tablaGastos.row.add([menuAdicional,
                    id_gasto,
                    txtfgasto,
                    txthgasto,
                    txtconcepto,
                    txtnegocio,
                    txttotal,
                    menuFpago2,
                    filexml, //XML
                    filepdf, //PDF
                    fileotro, //OTRO
                    txtmonto,
                    menucategoria2,
                    linkComensales + inpComensales,
                    txtobservaciones,
                    tblBtn
                    , { 'IdInforme': id, 'idgasto': value.g_id }
                ]).draw(false);


                txttotal = formatNumber.new((value.g_total * 1).toFixed(2), "$");
                menuFpago2 = value.g_formapago;
                menucategoria2 = value.g_nombreCategoria;

                if (tipoajuste === 0) {
                    listGastos.push(datosGasto);
                }

                var IdformaPago = value.g_formapago === 'Efectivo' ? 1 : 2;

                $("#formapago" + value.g_id).val(IdformaPago);
                var selCue = ((value.g_categoria * 1) === 0) ? "9999999" : value.g_categoria;
                $("#categoria" + value.g_id).val(selCue);

            });
             
            $('#totalg').val(total);
            $('#montog').val(totalmonto);

            localStorage.setItem('listGastos', JSON.stringify(listGastos));
        },
        complete: function () {
            if (estatusinf <= 2) {
                formatoInputFile();
                var fechasReq = JSON.parse(localStorage.getItem("fechasReq"));
                $(".fgasto").datepicker({
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
                        if ($(this).attr("fgasto") !== selectedDate) {
                            var ejecuta = $(this).attr("onchange");
                            setTimeout(ejecuta, 500);
                            $(this).attr("fgasto", selectedDate);
                        }
                        if (fcontablegasto !== $("#verinforme #mesInfVer").val()) {
                            $(this).notify("La fecha de gasto NO corresponde al mes contable del informe.", { position: "top" }, "error");
                        }
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

                $(".hgasto").timepicker({
                    minuteStep: 1,
                    appendWidgetTo: 'body',
                    showSeconds: false,
                    showMeridian: false,
                    template: 'dropdown',
                    modalBackdrop: true,
                    clickShow: false,
                    defaultTime: false,
                    onTimeChange: true
                });
            }
            obtenNumGasto();
            ////$("[data-toggle='tooltip']").tooltip();

            $('#tblGastos tbody tr input').css({ height: '15px' });
            var colores = [];
            i = 0;
            //hideColUbicaciones('inicio');
            hideColHoraGasto('inicio');
            hideColFormaPago();

            asignaValMaxTotalGastado();

        },
        error: function (result) {
            console.log(result);
        }
    });
    $('#nuevoGasto').css('display', 'block');
    setTimeout(function () {
        tablaGastos
            .order([1, 'asc'])
            .draw();

        setTimeout(function () {
            tablaGastos.columns.adjust().draw();
            tdtotales(total, totalmonto);
        }, 1000);

    }, 2000);
    $("#divListGastos").show();
}
function asignaValMaxTotalGastado() {
    $(".inp-total-gastado").each(function () {
        var total = $(this).val() * 1;
        var disponible = $("#disAnticipo").val();
        disponible = parseFloat(disponible);
        disponible = disponible.toFixed(2);
        disponible = parseFloat(disponible);

        var valmaximo = total + disponible;
        valmaximo = valmaximo.toFixed(2)

        $(this).attr("max", valmaximo);

    });
}
function validaComensalesObjetivoEnGastos() {
    var respuesta = {
        'estatus': true,
        'mensaje': 'ok'
    };
    $(".menu-categoria").each(function () {
        var id = ($(this)[0].id).replace("categoria", "");

        var CategoriaSelect = document.getElementById("categoria" + id);
        var NombreCategoria = CategoriaSelect.options[CategoriaSelect.selectedIndex].text;
        var comensales = $.trim($("#inpComensalesGasto" + id).val());
        comensales = comensales.replace(/ /gi, "").replace(/,/gi, "");
        var observaciones = $.trim($("#observaciones" + id).val());
        //console.log(comensales);
        var tipoajuste = $("#inpComensalesGasto" + id).attr("tipoajuste") * 1;
        if (tipoajuste === 0) {
            if ((NombreCategoria.toLowerCase()).indexOf("alimenta") > -1) {
                if (valorVacio(comensales) && respuesta.estatus === true) {
                    respuesta.estatus = false;
                    respuesta.mensaje = "Los comensales son obligatorios.";
                }
            } else if ((NombreCategoria.toLowerCase()).indexOf("sesion") > -1 && respuesta.estatus === true) {
                if (valorVacio(comensales)) {
                    respuesta.estatus = false;
                    respuesta.mensaje = "Los comensales son obligatorios.";
                } else if (valorVacio(observaciones)) {
                    respuesta.estatus = false;
                    respuesta.mensaje = "El Objetivo es obligatorio para sesiones de trabajo.";
                }
            }
        }
    });
    return respuesta;
}
function actualizaComensales(comensales, idgasto, idinforme) {
    var existeComa = comensales.indexOf(",");
    var nmbcomensales = "";
    var ncomensales = 0;
    if (existeComa < 0) {
        ncomensales = 1;
        nmbcomensales = $.trim(comensales);
    } else {
        var nmbcomensal = comensales.split(",");
        ncomensales = nmbcomensal.length;
        nmbcomensales = comensales.replace(/,  /gi, ", ");
        nmbcomensales = nmbcomensales.replace(/, /gi, ",");
        nmbcomensales = nmbcomensales.replace(/,/gi, ", ");
    }

    var datos = {
        'IdInforme': idinforme,
        'IdGasto': idgasto,
        'NComensales': ncomensales,
        'Nmbcomensales': (ncomensales === 0 ? "" : nmbcomensales)
    };
    $.ajax({
        type: "POST",
        url: "/api/UpdateComensalesGasto",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.ComensalesOK) {
                $.notify(result.Descripcion, { globalPosition: 'top center', className: 'success' });
            } else {
                $.notify(result.Descripcion, { globalPosition: 'top center', className: 'success' });
            }

        },
        complete: function () {
            obtenerGastosInforme(idinforme, 0, 2);
        },
        error: function (result) {
            console.log(result);
            $.notify("Error al Guardar Comensales.", { globalPosition: 'top center', className: 'error' });
        }
    });
}

function inputTextGasto(id, name, value, onchange, style, disabled, readonly, clase, otros) {
    clase = valorVacio(clase) ? "form-control" : clase;
    var tipo = "text";
    var origen = "";
    origen = id.substr(0, 5);
    tipo = origen === "total" ? "number" : tipo;
    var input = "";
    input = "<div class='form-group'>";
    input += "<input type='" + tipo + "' id='" + id + "' name='" + name + "' " +
        " value='" + value + "' ";
    if (!valorVacio(disabled))
        input += "disabled='" + disabled + "' ";
    if (!valorVacio(onchange))
        input += " onchange='" + onchange + "'";
    if (!valorVacio(readonly))
        input += " readonly='" + readonly + "'";

    input += " class='" + clase + "' ";

    if (!valorVacio(style))
        input += " style='" + style + "'";
    if (!valorVacio(otros))
        input += " " + otros + " ";
    input += " />";
    input += "<i class='form-group__bar'></i>";
    input += "</div>";
    return input;
}
function obtenNumGasto() {
    var NumGastoId = [];
    $(".lblngastos").each(function () {
        var nid = ($(this)[0].id).split("_");
        var ngasto = 0;
        if (nid[1] !== nid[3]) {
            ngasto = nid[2];
            $(".lblngastos").each(function () {
                var nid2 = ($(this)[0].id).split("_");
                if (nid2[1] === nid[3]) {
                    ngasto = nid2[2];
                    return true;
                }
            });
        } else {
            ngasto = "";
        }
        NumGastoId[nid[1]] = ngasto;
    });
    //console.log(NumGastoId);
    $(".txtgorigen").each(function () {
        var txtgorigenid = ($(this)[0].id).replace("gorigen", "") * 1;
        var ngasto = NumGastoId[txtgorigenid];
        $(this).val(ngasto);
        $(this).attr('ngasto', ngasto);
        $("#tooltip" + txtgorigenid).attr("title", "<div style='width: 170px;'>El gasto esta relacionado con el gasto: " + ngasto + "</div>");
    });
}
function tblBtnGasto(idproyecto, idinforme, id, SelectDatosAdicional, DeleteGasto, estatusinf, uresponsable, uidlogin, dirxml, dirpdf, dirotros, nombregasto, ConfrontacionBanco) {
    var tbl = "<table class='display' cellspacing='0' width='100%'><tr>";

    tbl += "<td><button type='button' class='btn btn-success btn-xs glyphicon glyphicon-plus' onclick='verDatosAdicionales(" + id + ", " + idinforme + ", " + 0 + ", " + estatusinf + ")'></button></td>";
    if (estatusinf <= 2 && ConfrontacionBanco === 0)
        tbl += "<td><button type='button' class='btn btn-danger btn-xs glyphicon glyphicon-trash DelGas' onclick='eliminarGasto(" + id + ", " + idinforme + ", " + 0 + ", " + estatusinf + ", \"" + dirxml + "\", \"" + dirpdf + "\", \"" + dirotros + "\", \"" + nombregasto + "\")'></button></td>";

    tbl += "</tr></table>";

    return tbl;
}
function lblNGasto(id, total, MONTO, n, idgorigen, ngasto_orden, tipoajuste) {
    var colorLlabel = "label label-success";
    total = Math.round((total * 1).toFixed(2));
    MONTO = Math.round((MONTO * 1).toFixed(2));
    MONTO = MONTO === 0 ? total : MONTO;
    //console.log(total , MONTO);
    var lbl = "";
    if (total === MONTO && tipoajuste === 0) {
        colorLlabel = "label label-success";
    } else if (total !== MONTO && tipoajuste === 0) {
        colorLlabel = "label label-warning";
    }
    lbl = "<h4><span id='gasto_" + id + "_" + n + "_" + idgorigen + "' ngasto='" + n + "' idgasto='" + id + "' class='lblngastos " + colorLlabel + "'>" + ngasto_orden + "</span></h4>";
    return lbl;
}
function inputfileotro(idinforme, idproyecto, id, aplica, dirxml, dirpdf, dirotros, estatusinf, confrontacionBan) {
    var fileotro = "";
    var colorver = "";
    if (valorVacio(dirotros) && estatusinf <= 2) {
        if (aplica === 0 || confrontacionBan === 1) {
            fileotro = "<a data-toggle='tooltip' class='btn btn-default btn-xs disabled glyphicon glyphicon-folder-open' aria-disabled='false' role='button'></a>";
        } else {
            fileotro = "<input id='fileotro" + id + "' accept='image/*' onchange='actualizarGastoComOTRO(" + id + ", " + idinforme + ", " + 0 + ", \"" + dirxml + "\", \"" + dirpdf + "\", \"" + dirotros + "\", " + estatusinf + ", \"file\")' name='fileotro" + id + "' type='file' class='filestyle' />";
        }
    } else {
        habilitarLinkComprobante(dirotros);
        colorver = (disabled === "disabled") ? "default" : "success";
        if (aplica === 0) {
            disabled = "disabled";
            trueFalse = true;
        }
        fileotro = "<a href='#' data-toggle='tooltip' title='Ver Otros Comprobantes' class='btn btn-" + colorver + " btn-xs " + disabled + " glyphicon glyphicon-eye-open' aria-disabled='" + trueFalse + "' onclick='verComprobante(\"OTRO\", " + 0 + ", " + idinforme + ", " + id + ", \"" + dirotros + "\", " + estatusinf + ", " + confrontacionBan + ")' role='button'></a>";

    }
    return fileotro;
}
function inputfilepdf(idinforme, idproyecto, id, aplica, dirxml, dirpdf, dirotros, estatusinf, confrontacionBan) {
    var filepdf = "";
    var colorver = "";
    if (valorVacio(dirpdf) && estatusinf <= 2) {
        if (aplica === 0 || confrontacionBan === 1) {
            filepdf = "<a data-toggle='tooltip' class='btn btn-default btn-xs disabled glyphicon glyphicon-folder-open' aria-disabled='false' role='button'></a>";
        } else {
            // filepdf = "<input id='filepdf" + id + "' accept='.pdf'  name='filepdf" + id + "' type='file'/>";
            filepdf = "<input id='filepdf" + id + "' accept='.pdf' onchange='actualizarGastoComPDF(" + id + ", " + idinforme + ", " + 0 + ", \"" + dirxml + "\", \"" + dirpdf + "\", \"" + dirotros + "\", " + estatusinf + ", \"file\")' name='filepdf" + id + "' type='file' class='filestyle' />";
        }
    } else {
        habilitarLinkComprobante(dirpdf);
        colorver = (disabled === "disabled") ? "default" : "success";
        if (aplica === 0) {
            disabled = "disabled";
            trueFalse = true;
        }
        filepdf = "<a href='#' title='Ver PDF' class='btn btn-" + colorver + " btn-xs " + disabled + " glyphicon glyphicon-eye-open' aria-disabled='" + trueFalse + "' onclick='verComprobante(\"PDF\", " + 0 + ", " + idinforme + ", " + id + ", \"" + dirpdf + "\", " + estatusinf + ", " + confrontacionBan + ")' role='button'></a>";
    }
    return filepdf;
}
function inputfilexml(idinforme, idproyecto, id, aplica, dirxml, dirpdf, dirotros, estatusinf, idgorigen, tipoajuste, confrontacionBan) {
    id = id * 1;
    idgorigen = idgorigen * 1;
    var filexml = "";
    var colorver = "";
    if (valorVacio(dirxml) && estatusinf <= 2) {
        if (aplica === 0 || id !== idgorigen || confrontacionBan === 1) {
            filexml = "<a id='verxml" + id + "' data-xml='no' dirxml='' data-toggle='tooltip' class='btn btn-default btn-xs disabled glyphicon glyphicon-folder-open' aria-disabled='false' role='button'></a>";
        } else {
            filexml = "<input id='filexml" + id + "' accept='.xml' onchange='actualizarGastoComXML(" + id + ", " + idinforme + ", " + 0 + ", \"" + dirxml + "\", \"" + dirpdf + "\", \"" + dirotros + "\", " + estatusinf + ", \"file\")' name='filexml" + id + "' type='file' class='filestyle' />";
        }
    } else {
        habilitarLinkComprobante(dirxml);
        colorver = (disabled === "disabled") ? "default" : "success";
        if ((aplica === 0 || id !== idgorigen) && tipoajuste === 0) {
            disabled = "disabled";
            trueFalse = true;
        }
        filexml = "<a id='verxml" + id + "' data-xml='si' dirxml='" + dirxml + "' href='#' data-toggle='tooltip' title='Ver XML' class='btn btn-" + colorver + " btn-xs " + disabled + " glyphicon glyphicon-eye-open' aria-disabled='" + trueFalse + "' onclick='verComprobante(\"XML\", " + 0 + ", " + idinforme + ", " + id + ", \"" + dirxml + "\", " + estatusinf + ", " + confrontacionBan + ")' role='button'></a>";
    }
    return filexml;
}
function habilitarLinkComprobante(dir) {
    disabled = (dir) ? '' : 'disabled';
    trueFalse = (dir) ? false : true;
}
function verDatosAdicionales(id, idinforme, idproyecto, estatusinf) {
    $("#verDatosAdicionales").modal('show');
    $("#verDatosAdicionales").css({ 'z-index': 2000 });
    opacityModalVerInfoG();

    localStorage.removeItem('comensales');
    $("#ncomensales, #ncomensalesda").val("");
    $("#estatusInformeDA").val(estatusinf);

    $("#frmDatosAdicionales #rfc").val("");
    $("#frmDatosAdicionales #contacto").val("");
    $("#frmDatosAdicionales #telefono").val("");
    $("#frmDatosAdicionales #correo").val("");

    if (estatusinf === 2) {//Seguridad.permiso(5, "SaveDatosAdicionales") === 1)
        $("#btnDA").show();
        $("#frmDatosAdicionales #rfc").attr("disabled", false);
        $("#frmDatosAdicionales #contacto").attr("disabled", false);
        $("#frmDatosAdicionales #telefono").attr("disabled", false);
        $("#frmDatosAdicionales #correo").attr("disabled", false);
        $("#ncomensalesda").attr("disabled", false);
        $(".btnmasmenos").removeClass("disabled").show();
    } else {
        $("#btnDA").hide();
        $("#frmDatosAdicionales #rfc").attr("disabled", true);
        $("#frmDatosAdicionales #contacto").attr("disabled", true);
        $("#frmDatosAdicionales #telefono").attr("disabled", true);
        $("#frmDatosAdicionales #correo").attr("disabled", true);
        $("#ncomensalesda").attr("disabled", true);
        $(".btnmasmenos").addClass("disabled").hide();
    }

    var datos = {
        "id": id,
        "idinforme": idinforme,
        "idproyecto": 0
    };


    //seleciona datos adicionales
    $.ajax({
        async: false,
        type: "POST",
        url: "/api/SelectDatosAdicionales",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            if (result.length > 0) {
                $("#frmDatosAdicionales #rfc").val(result[0]['g_rfc']);
                $("#frmDatosAdicionales #contacto").val(result[0]['g_contacto']);
                $("#frmDatosAdicionales #telefono").val(result[0]['g_telefono']);
                $("#frmDatosAdicionales #correo").val(result[0]['g_correo']);

                var ncomensales = result[0]['ncomensales'] * 1;
                $("#ncomensalesda").val(ncomensales);
                var comensales1 = "";
                var comensales = [];
                if (ncomensales > 0) {
                    if (ncomensales > 1) {
                        comensales1 = (result[0]['nmbcomensales']).replace(",", ", ");
                        comensales1 = (result[0]['nmbcomensales']).split(", ");
                        for (var i = 0; i < ncomensales; i++) {
                            comensales.push($.trim(comensales1[i]));
                        }
                    } else {
                        var c = (result[0]['nmbcomensales']).replace(",", "");
                        comensales.push($.trim(c));
                    }
                }

                creaInputsComensalesda(comensales);
                localStorage.setItem('comensales', JSON.stringify(comensales));
            }
        },
        error: function (result) {
            console.log(result);
        }
    });

    $("#frmDatosAdicionales #idgasto").val(id);
    $("#frmDatosAdicionales #idinforme").val(idinforme);
    $("#frmDatosAdicionales #idproyecto").val(0);
}
function GuardarDatosAdicionales() {
    var error = 0;
    var contacto = $("#frmDatosAdicionales #contacto").val();
    var idgasto = $("#frmDatosAdicionales #idgasto").val();
    var idinforme = $("#frmDatosAdicionales #idinforme").val();
    //var idproyecto = $("#frmDatosAdicionales #idproyecto").val();
    var rfc = $("#frmDatosAdicionales #rfc").val();
    if (!valorVacio(rfc)) {
        if (ValidaRFC(rfc) === false) {
            $("#frmDatosAdicionales #rfc").notify("El formato del R.F.C. es incorrecto.", { position: "top" }, "error");
            error = 1;
        }
    }
    else {
        rfc = '';
    }

    var correo = $("#frmDatosAdicionales #correo").val();
    if (!valorVacio(correo)) {
        if (ValidaEMail(correo) === false) {
            $("#frmDatosAdicionales #correo").notify("El correo no es valido.", { position: "top" }, "error");
            error = 1;
        }
    }

    var telefono = $("#frmDatosAdicionales #telefono").val();
    if (!valorVacio(telefono)) {
        if (ValidaTelefono(telefono) === false) {
            $("#frmDatosAdicionales #telefono").notify("El telefono no es valido.", { position: "top" }, "error");
            error = 1;
        }
    }

    //inicio comensales
    var listComensales = [];
    var comensales = "";
    var ncomensales = 0;
    try {
        listComensales = JSON.parse(localStorage.getItem('comensales'));
        ncomensales = listComensales.length;
        if (ncomensales > 0) {
            for (var i = 0; i < ncomensales; i++) {
                comensales += valorVacio(comensales) ? listComensales[i] : (", " + listComensales[i]);
            }
        }
    } catch (err) {
        ncomensales = 0;
    }
    //fin comensales

    var datos = {
        'idgasto': idgasto, 'idinforme': idinforme, 'idproyecto': 0,
        'contacto': contacto, 'rfc': rfc.replace(/-/gi, ""), 'correo': correo,
        'telefono': telefono.replace(/-/gi, ""),
        "ncomensales": ncomensales,
        "nmbcomensales": comensales
    };

    if (error === 1) {
        return false;
    }

    $.ajax({
        async: false,
        type: "POST",
        url: "/api/SaveDatosAdicionalesGasto",
        data: datos, //$("#frmDatosAdicionales").serialize(),
        dataType: "json",
        success: function (result) {
            $("#ncomensalesda").val("");
            localStorage.removeItem('comensales');
            creaInputsComensalesda([]);

            $.notify("Datos Guardados.", { globalPosition: 'top center', className: 'success' });

            obtenerGastosInforme(idinforme, 0, 2);
            //Seguridad.bitacora("SaveDatosAdicionales", 5, 0 + "," + idinforme + "," + idgasto, "Se guardaron datos adicionales.", 1);
        },
        error: function (result) {
            $.notify("Error al Guardar", { globalPosition: 'top center', className: 'error' });
            //Seguridad.bitacora("SaveDatosAdicionales", 5, 0 + "," + idinforme + "," + idgasto, "Error: Al guardar datos adicionales. " + JSON.stringify(datos) + " " + JSON.stringify(result), 0);
        }
    });
    $("#verDatosAdicionales").modal('hide');
}
function eliminarGastoCom(comprobante, idproyecto, idinforme, idgasto, dircomp, estatus) {

    var datos = {
        'id': idgasto,
        'idinforme': idinforme,
        'comprobante': comprobante
    };

    var botones = [];

    botones[0] = {
        text: "Si", click: function () {
            $.ajax({
                async: false,
                type: "POST",
                url: "/api/EliminaComprobantes",
                data: datos,
                dataType: "json",
                beforeSend: function () {
                    cargando();
                },
                success: function (result) {
                    //console.log(result);
                    $.notify("Comprobante de Gasto Borrado.", { globalPosition: 'top center', className: 'success' });
                    cargado();

                    Seguridad.bitacora("EliminarComprobante", 5, idproyecto + "," + idinforme + "," + idgasto, "Se elimino comprobante.", 1);

                    if (comprobante === "XML") {
                        //selectInforme(result[0]['idinforme'], result[0]['idproyecto'], 0);
                        consultaInfoGastos(idinforme, estatus, 0);
                        ObtenerInformes();
                    } else {
                        //consultaInfoGastos($("#idinforme").val(), $("#idproyecto").val(), 2, 1);
                        obtenerGastosInforme(idinforme, estatus, 0);
                    }

                },
                error: function (result) {
                    cargado();
                    $.notify("Error al Eliminar", { globalPosition: 'top center', className: 'error' });
                    Seguridad.bitacora("EliminarComprobante", 5, 0 + "," + idinforme + "," + idgasto, "Error: Al borrar comprobante de gasto. " + JSON.stringify(result), 0);
                }
            });
            $(this).dialog("close");
            $("#verComprobante").modal('hide');

            //actualiza lista de informes
            ObtenerInformes();
        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Eliminar Comprobante?", botones, "Eliminar Comprobante.", "#verComprobante");

}
function verComprobante(comprobante, idproyecto, idinforme, idgasto, dircomp, estatus, confrontacionBan) {
    $("#verComprobante").modal('show');
    opacityModalVerInfoG();

    var btnEliCom = "";
    if (estatus <= 2 && confrontacionBan === 0) {
        btnEliCom = "<button type='button' class='btn btn-danger btn-xs' onclick='eliminarGastoCom(\"" + comprobante + "\", " + 0 + ", " + idinforme + ", " + idgasto + ", \"" + dircomp + "\", " + estatus + ")'><i class='zmdi zmdi-delete zmdi-hc-2x'></i> Eliminar</button>";
    };

    $("#verComprobante").css({ 'z-index': 2000 });
    $("#ModalLabelComprobante").empty();
    $("#ModalLabelComprobante").append("Comprobante: " + comprobante);
    $("#compPDF").removeAttr("data");
    $("#print_compXML").hide();
    //$("#print_compXML").empty();
    $("#compPDF").hide();
    $("#divPDF").hide();
    $("#compOTRO").removeAttr("src");
    $("#compOTRO").hide();
    $("#controles_compOTRO").empty();
    $("#controles_compXML").empty();
    $("#qrxml").removeAttr("src");
    if (comprobante === "XML") {
        $("#print_compXML").show();
        $.ajax({
            async: false,
            type: "GET",
            contentType: false,
            url: "/" + dircomp,
            dataType: "xml",
            success: function (response) {
                //console.log(response);
                var controles = "";
                //controles = "<button type='button' class='btn btn-warning btn-xs' onclick='imprimirXML()'><i class='zmdi zmdi-print zmdi-hc-2x'></i> Imprimir</button> ";
                controles += btnEliCom;
                $("#controles_compXML").append(controles);

                var xmljson = xmlToJson(response);
                //console.log(xmljson);
                verFacturaJSONenHTML(xmljson, 0, idinforme, idgasto);
            },
            error: function (result) {
                console.log("error", result);
            }
        });
        if (dircomp === "") {
            $("#print_compXML").empty();
        }
    }
    if (comprobante === "PDF") {
        $("#divPDF").show();
        "#controles_compPDF".AsHTML(btnEliCom);
        $("#compPDF").show();
        $("#compPDF").attr("data", "/" + dircomp);
    }
    if (comprobante === "OTRO") {
        //var ruta = url.absolutePath().split("/includes/");

        dircomp = "/" + dircomp;
        var controles = "";
        //controles += "<button type='button' class='btn btn-warning btn-xs' onclick='imprimirImg(\"" + dircomp + "\")'><i class='zmdi zmdi-print zmdi-hc-2x'></i> Imprimir</button> ";
        controles += btnEliCom;
        var f = new Date();
        var fh = f.getDate() + '' + f.getMonth() + '' + f.getFullYear() + '' + f.getHours() + '' + f.getMinutes() + '' + f.getSeconds();
        $("#controles_compOTRO").append(controles);
        $("#compOTRO").show();
        $("#compOTRO").attr("src", dircomp + "?" + fh);
    }

}
function tdtotales(total, totalmonto) {
    var newtd = "";
    newtd += "<tr><td id='tdcolsdatgasto' colspan='4'></td>";
    newtd += "<td align='right'>Total:</td>";
    newtd += "<td align='right'><span id='lbltotalg'>" + formatNumber.new((total * 1).toFixed(2), "$ ") + "</span></td>";
    //newtd += "<td></td>";
    newtd += "<td id='tdcolstotales' colspan='3'></td>";//colspan = 4 reduce a 3 para ocultar columna de forma de pago
    newtd += "<td align='right'><span id='lblmontog'>" + formatNumber.new((totalmonto * 1).toFixed(2), "$ ") + "</span></td>";
    newtd += "<td colspan='5'></td></tr>";
    setTimeout(function () {
        $('#tblGastos tfoot').remove();
        $('#tblGastos').append("<tfoot></tfoot>");
        $('#tblGastos tfoot').empty();
        $('#tblGastos tfoot').append(newtd);

    }, 1000);

    //font-size: 18px;
}
function actualizarGasto(id, idinforme, idproyecto, xml, pdf, otros, estatusinf, origen) {

    var error = 0;
    var total = $("#total" + id).val() * 1;
    if (!validarNumero($("#total" + id).val())) {//($.trim($("#filexml" + id).val()) != "" || xml != "") &&
        $("#total" + id).notify("Ingresa un Importe valido.", { position: "top" }, "error");
        error = 1;
    }
    var totalmaximo = parseFloat($("#total" + id).attr('max'));
    if (total > totalmaximo) {
        $("#total" + id).notify("El importe *Capturado* (gastado) no puede ser mayor al importe *Por Comprobar* (disponible).", { position: "top" }, "error");
        var valorOld = parseFloat($("#total" + id).attr('valueOld'));
        $("#total" + id).val(valorOld);
        $("#total" + id).focus().select();
        error = 1;
    }
    if ($("#categoria" + id).val() === "0") {//($.trim($("#filexml" + id).val()) != "" || xml != "") &&
        $("#categoria" + id).notify("Selecciona una categoria.", { position: "top" }, "error");
        error = 1;
    }
    if ($("#formapago" + id).val() === "0") {
        $("#formapago" + id).notify("Selecciona la Forma de Pago.", { position: "top" }, "error");
        error = 1;
    }
    if ($.trim($("#concepto" + id).val()) === "") {
        $("#concepto" + id).notify("Indica el Concepto del gasto.", { position: "top" }, "error");
        error = 1;
    }
    if (total === "" && total <= 0) {
        $("#total" + id).notify("Indica Importe gastado.", { position: "top" }, "error");
        error = 1;
    }
    if (error === 1) {
        return false;
    }

    var hgasto = $("#hgasto" + id).val();
    if (origen === "txtHgasto") {
        var hgastoold = $("#hgasto" + id).attr("hgasto");
        if (hgasto === hgastoold)
            return false;
        else
            $("#hgasto" + id).attr("hgasto", hgasto);
    }


    var FormaPagoSelect = "", formapago = "";

    try {
        FormaPagoSelect = document.getElementById("formapago" + id);
        formapago = FormaPagoSelect.options[FormaPagoSelect.selectedIndex].text;
    } catch (e) {
        FormaPagoSelect = "";
        formapago = $("#inputCabeceraFormaPago").val();
    }

    var c = $("#categoria" + id).val();
    var categoria = c;

    var CategoriaSelect = document.getElementById("categoria" + id);


    var elementoCat = $("#categoria" + id)[0];
    var opIndex = elementoCat.selectedIndex;
    var NombreCategoria = "";
    var datosCat = [];
    var ivacategoria = 0;
    try {
        NombreCategoria = CategoriaSelect.options[opIndex].text;
        datosCat = elementoCat.options[opIndex].dataset;
        ivacategoria = datosCat.grmativa;
    } catch (err) {
        NombreCategoria = "S/Categoria";
        datosCat = [];
        ivacategoria = 0;
    }

    //inicio comensales
    var listComensales = [];
    var comensales = "";
    var ncomensales = 0;
    try {
        listComensales = JSON.parse(localStorage.getItem('comensales'));
        ncomensales = listComensales.length;
        if (ncomensales > 0) {
            for (var i = 0; i < ncomensales; i++) {
                comensales += valorVacio(comensales) ? listComensales[i] : (", " + listComensales[i]);
            }
        }
    } catch (err) {
        ncomensales = 0;
    }
    //fin comensales

    if ((NombreCategoria.toLowerCase()).indexOf("alimenta") > -1) {
        if (ncomensales === 0 || valorVacio(comensales)) {
            $.notify("Los comensales son obligatorios cuando la categoria es alimentaciÃ³n.", { position: "top center" }, "error");
        }
    } else if ((NombreCategoria.toLowerCase()).indexOf("sesion") > -1) {
        if (ncomensales === 0 || valorVacio(comensales)) {
            $.notify("Los comensales son obligatorios cuando la categoria es sesiones de trabajo.", { position: "top center" }, "error");
        }
        if (valorVacio(observaciones)) {
            $.notify("El Objetivo es obligatorio para sesiones de trabajo.", { position: "top center" }, "error");
        }
    }


    var datos = {
        "id": id,
        "idinforme": idinforme,
        "idproyecto": idproyecto,
        "fgasto": $("#fgasto" + id).val(),
        "hgasto": hgasto,
        "concepto": $("#concepto" + id).val(),
        "negocio": $("#negocio" + id).val(),
        "total": total,
        "formapago": formapago,
        "categoria": categoria,
        "observaciones": $("#observaciones" + id).val(),
        "nombreCategoria": NombreCategoria,
        "ivaCategoria": ivacategoria
    };

    $.ajax({
        async: true,
        type: "POST",
        url: "/api/UpdateGasto",
        data: datos, //data,
        dataType: "json",
        cache: false,
        beforeSend: function () {
            //cargando();
        },
        success: function (result) {
            $.notify("Gasto Actualizado.", { globalPosition: 'top center', className: 'success', autoHideDelay: 6000 });
            //cargado();//origen: txtFecha, text, txtTotal, selectfp, selectc
            if (origen === "txtFecha" || origen === "txtTotal" || origen === "selectfp") {
                selectInforme(result, 0);

            }
            obtenerGastosInforme(result, 0, estatusinf);
            /*
            var gorigen = $("#gorigen" + id).val() * 1;
            if (origen === "txtTotal" && valorVacio(xml) && gorigen === 0) {
                $("#monto" + id).empty();
                $("#monto" + id).append(formatNumber.new(total.toFixed(2), "$ "));
                obtenerGastosInforme(result, 0, estatusinf);
            }
            */

        },
        complete: function () {
            //cargado();
            // $.notify("Se actualizo gasto.", { globalPosition: 'top center', className: 'success' });
            //Seguridad.bitacora("UpdateGasto", 5, idproyecto + "," + idinforme + "," + id, "Se actualizo gasto.", 1);
            //actualiza lista de informes
            if (origen === "txtTotal" || origen === "txtFecha") {
                ObtenerInformes();
            }

            if (origen === "selectfp") {
                // validaFPdeGastoyFactura(id, formapago);
            }
        },
        error: function (result) {
            //cargado();
            $.notify("Error al Guardar", { globalPosition: 'top center', className: 'error' });
            Seguridad.bitacora("UpdateGasto", 5, idproyecto + "," + idinforme + "," + id, "Error: actualizar gasto. " + JSON.stringify(result), 0);
        }
    });
}
function validaFPdeGastoyFactura(idgasto, fpgasto) {
    var dirxml = $("#verxml" + idgasto).attr("dirxml");
    if (!valorVacio(dirxml)) {
        var IdFPSATGasto = selectIdFPSATGasto(fpgasto);
        if (!valorVacio(IdFPSATGasto)) {
            var IdFPSATFactura = selectIdFPSATFactura(dirxml);
            if (IdFPSATGasto !== IdFPSATFactura) {
                $.notify("El MÃƒÂ©todo o Forma de Pago de la Factura no es igual al del Gasto.", { globalPosition: 'top center', className: 'error', autoHideDelay: 8000 });
            }
        }
    }
}
function selectIdFPSATGasto(idfpgasto) {
    var id = "";
    $.ajax({
        async: false,
        type: "POST",
        url: "includes/Script.php?modulo=gastos&accion=SelectIdFPSAT&Content-Type=text/json",
        data: { 'idfpgasto': idfpgasto },
        dataType: "json",
        success: function (result) {
            id = result.tiposat;
        },
        error: function (result) {
            id = "";
            console.log(result);
        }
    });
    return id;
}
function recuperagastoxml(id, idinforme, idproyecto, xml, pdf, otros, estatusinf, origen, bitxml) {

    var datos = {
        "id": id,
        "idinforme": idinforme,
        "idproyecto": 0,
        "dir": bitxml
    };

    //console.log(fileXML);
    $.ajax({
        async: false,
        type: "POST",
        url: "/api/UpdateGastoXML",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        processData: false,
        success: function (result) {
            //console.log(result, "fpgasto = ", result[0].fpgasto, "fpfactura = ", result[0].fpfactura[0]);
            //var fpgasto = (result[0].fpgasto).substring(0, 2);
            //var fpfactura = (result[0].fpfactura[0]).substring(0, 2);
            //console.log("fpgasto = ", fpgasto, "fpfactura = ", fpfactura);
            var error = 0;


            if (result === 'El UUID ingresado ya existe') {
                error = 1;
                $.notify('El Xml cargado ya existe en la base de datos,favor de verificar', { globalPosition: 'top center', className: 'error', autoHideDelay: 6000 });
                consultaInfoGastos(idinforme, estatusinf, 0);
                //ObtenerInformes();
            };

            if (result === 'No se puede guardar el comprobante, el importe es igual o mayor a $ 2000.00 y la forma de pago es efectivo.') {
                $.notify(result, { globalPosition: 'top center', className: 'error', autoHideDelay: 6000 });

                error = 1;
                consultaInfoGastos(idinforme, estatusinf, 0);
                ObtenerInformes();
            };

            if (result === 'Gasto Actualizado,la forma de pago capturada no coincide con la del XML') {
                $.notify(result, { globalPosition: 'top center', className: 'error', autoHideDelay: 6000 });

                error = 1;
                consultaInfoGastos(idinforme, estatusinf, 0);
                ObtenerInformes();
            };


            $("#filexml" + id).filestyle('clear');

            if (error === 0) {
                //if (!valorVacio(fpgasto)) {
                //    if (fpgasto !== fpfactura) {
                //        $.notify("El MÃƒÂ©todo o Forma de Pago de la Factura no es igual al del Gasto.", { globalPosition: 'top center', className: 'error', autoHideDelay: 8000 });
                //    }
                //}
                $.notify("Comprobante XML cargado y gasto Actualizado.", { globalPosition: 'top center', className: 'success', autoHideDelay: 6000 });
                //if (result[0]["xmlvigente"] !== "") {
                consultaInfoGastos(idinforme, estatusinf, 0);
                ObtenerInformes();
                //}
            }

        },
        error: function (result) {
            console.log("error", result);
            $("#filexml" + id).filestyle('clear');
            $.notify("Error al Guardar", { globalPosition: 'top center', className: 'error' });
        }
    });

}
function actualizarGastoComXML(id, idinforme, idproyecto, xml, pdf, otros, estatusinf, origen) {
    var inputFileXML = document.getElementById("filexml" + id);
    var fileXML = inputFileXML.files[0];

    var formapagogasto = ""; //$("#formapago" + id).val() * 1;

    try {
        formapagogasto = $("#formapago" + id).val() * 1;
    } catch (e) {
        formapagogasto = $("#inputCabeceraFormaPago").val() * 1;
    }

    var error = 0;

    var extFileXML = (($("#filexml" + id).val()).substring(($("#filexml" + id).val()).lastIndexOf(".") + 1)).toLowerCase();
    if ($.trim($("#filexml" + id).val()) !== "") {
        if (extFileXML != "xml") {
            $("#filexml" + id).notify("Solo se permiten archivos XML.", { position: "top" }, "error");
            $("#filexml" + id).filestyle('clear');
            error = 1;
        }
    }
    if (formapagogasto === 0) {
        $("#formapago" + id).notify("Seleciona una forma de pago.", { position: "top" }, "error");
        $("#filexml" + id).filestyle('clear');
        error = 1;
    }

    var c = $("#categoria" + id).val();
    var categoria = c * 1;

    if (categoria === 0) {
        $("#categoria" + id).notify("Seleciona una categoria.", { position: "top" }, "error");
        $("#filexml" + id).filestyle('clear');
        error = 1;
    }

    if (error === 1) {
        return false;
    }

    var bitxml;

    var file = $("#filexml" + id).get(0).files[0];
    var r = new FileReader();
    r.onload = function () {
        bitxml = r.result;
        recuperagastoxml(id, idinforme, idproyecto, xml, pdf, otros, estatusinf, origen, bitxml)
    };
    r.readAsDataURL(file);

}
function RecuperaDatos(id, idinforme, idproyecto, xml, pdf, otros, estatusinf, origen, binimagePDF) {
    var datos = {
        "id": id,
        "idinforme": idinforme,
        "dir": binimagePDF,
        "Valida": "1"
    };

    $.ajax({
        type: "POST",
        url: "/api/UpdateGastoPDFOtros",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        success: function (result) {
            console.log("success", result);
            $.notify("Gasto Actualizado [Comprobante PDF Cargado].", { globalPosition: 'top center', className: 'success' });
        },
        complete: function () {
            obtenerGastosInforme(idinforme, 0, estatusinf);

        },
        error: function (result) {
            console.log(result);
            $.notify("Error al Guardar", { globalPosition: 'top center', className: 'error' });

        }
    });

}
function actualizarGastoComPDF(id, idinforme, idproyecto, xml, pdf, otros, estatusinf, origen) {

    var inputFilePDF = document.getElementById("filepdf" + id);
    var filePDF = inputFilePDF.files[0];

    var sizeByte = inputFilePDF.files[0].size;
    var siezekiloByte = parseInt(sizeByte / 1024);

    var error = 0;

    var extFilePDF = (($("#filepdf" + id).val()).substring(($("#filepdf" + id).val()).lastIndexOf(".") + 1)).toLowerCase();
    if ($.trim($("#filepdf" + id).val()) !== "") {
        if (extFilePDF !== "pdf") {
            $("#filepdf" + id).notify("Solo se permiten archivos PDF.", { position: "top" }, "error");
            $("#filepdf" + id).filestyle('clear');
            error = 1;
        }
    }

    if (siezekiloByte > 2000) {
        $("#filepdf" + id).notify("No se permiten archivos mayores a 2MB.", { position: "top" }, "error");
        $("#filepdf" + id).filestyle('clear');
        error = 1;
    }


    if (error === 1) {
        return false;
    }



    var binimagePDF;

    var file = $('#filepdf' + id).get(0).files[0];
    var r = new FileReader();
    r.onload = function () {
        binimagePDF = r.result;
        RecuperaDatos(id, idinforme, idproyecto, xml, pdf, otros, estatusinf, origen, binimagePDF)

    };
    r.readAsDataURL(file);


}
function recuperadatosotros(id, idinforme, idproyecto, xml, pdf, otros, estatusinf, origen, bitPDF) {

    var datos = {
        "id": id,
        "idinforme": idinforme,
        "dir": bitPDF,
        "Valida": "0"
    };

    $.ajax({
        type: "POST",
        url: "/api/UpdateGastoPDFOtros",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        success: function () {
            $.notify("Gasto Actualizado [Comprobante Cargado].", { globalPosition: 'top center', className: 'success' });
        },
        complete: function () {
            obtenerGastosInforme(idinforme, idproyecto, estatusinf);
            //Seguridad.bitacora("", 5, idproyecto + "," + idinforme + "," + id, "Se cargo comprobante para el gasto (imagen).", 1);
        },
        error: function (result) {
            console.log(result);
            $.notify("Error al Guardar Comprobante.", { globalPosition: 'top center', className: 'error' });
            //Seguridad.bitacora("", 5, idproyecto + "," + idinforme + "," + id, "Error: Al cargar comprobante para el gasto (imagen). " + JSON.stringify(result), 0);
        }
    });

}
function actualizarGastoComOTRO(id, idinforme, idproyecto, xml, pdf, otros, estatusinf, origen) {

    var inputFileOTRO = document.getElementById("fileotro" + id);
    var fileOTRO = inputFileOTRO.files[0];

    var sizeByte = inputFileOTRO.files[0].size;
    var siezekiloByte = parseInt(sizeByte / 1024);

    var error = 0;

    var extFileOTRO = (($("#fileotro" + id).val()).substring(($("#fileotro" + id).val()).lastIndexOf(".") + 1)).toLowerCase();
    if ($.trim($("#fileotro" + id).val()) !== "") {
        var extensiones_permitidas = new Array("gif", "jpg", "png", "jpeg");
        var permitida = false;
        for (var i = 0; i < extensiones_permitidas.length; i++) {
            if (extensiones_permitidas[i] === extFileOTRO) {
                permitida = true;
                break;
            }
        }
        if (permitida === false) {
            $("#fileotro" + id).notify("Solo se permiten archivos de tipo Imagen.", { position: "top" }, "error");
            $("#fileotro" + id).filestyle('clear');
            error = 1;
        }
    }

    if (siezekiloByte > 2000) {
        $("#fileotro" + id).notify("No se permiten archivos mayores a 2MB.", { position: "top" }, "error");
        $("#fileotro" + id).filestyle('clear');
        error = 1;
    }

    if (error === 1) {
        return false;
    }

    var bitPDF;

    var file = $('#fileotro' + id).get(0).files[0];
    var r = new FileReader();
    r.onload = function () {
        bitPDF = r.result;
        recuperadatosotros(id, idinforme, idproyecto, xml, pdf, otros, estatusinf, origen, bitPDF)

    };
    r.readAsDataURL(file);

}
function eliminarGasto(id, idinforme, idproyecto, estatusinf, dirxml, dirpdf, dirotros, nombre) {
    //alert(id, idinforme, idproyecto);
    var ConfBanco = $("#ConfBanco").val() * 1;
    if (ConfBanco === 1) {
        $.notify("No puede eliminar el gasto el informe ya esta confrontado", { globalPosition: 'top center', className: 'error' });
        return false;
    }

    var datos = {
        "id": id,
        "idinforme": idinforme
    };


    var botones = [];
    botones[0] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            tablaGastos.row('.selected').remove().draw(false);
            $.ajax({
                async: true,
                type: "POST",
                url: "/api/DeleteGasto",
                data: JSON.stringify(datos), //checar con hector{'dirxml': dirxml, 'dirpdf': dirpdf, 'dirotros': dirotros},
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (result) {
                    //console.log(result);
                    //tablaxmlinforme.row('.selected').remove().draw(false);
                    $.notify("Gasto [" + nombre + "] Borrado.", { globalPosition: 'top center', className: 'success' });
                    consultaInfoGastos($("#idinforme").val(), 2, 1);
                    //Seguridad.bitacora("DeleteGasto", 5, idproyecto + "," + idinforme + "," + id, "Se Borro el Gasto.", 1);
                    selectInforme(result[0]['idinforme'], result[0]['idproyecto'], 1);
                    //actualiza lista de informes
                    ObtenerInformes();

                },
                error: function (result) {
                    $.notify("Error al Eliminar", { globalPosition: 'top center', className: 'error' });
                }
            });
        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
            tablaGastos.$('tr.selected').removeClass('selected');
        }
    };
    Seguridad.confirmar("Eliminar Gasto:<br /><b>" + nombre + "</b>?", botones, "Eliminar Gasto.", "#verInformeGastos");
}
$('#tblGastos tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    } else {
        tablaGastos.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});
function inputNuevoGasto(tablaGastos, id, idproyecto) {
    var txtfgasto = "<input id='fgasto' placeholder='Fecha Gasto' readonly='readonly' class='form-control' name='fgasto' type='text' style='width: 100px;' />";
    var txtconcepto = "<input type='text' placeholder='Concepto' id='concepto' name='concepto' class='form-control' style='width: 100px;'  />";
    var txtnegocio = "<input type='text' placeholder='Negocio' id='negocio' name='negocio' class='form-control' style='width: 100px;'  />";
    var txttotal = "<input type='text' placeholder='$ Importe' id='totalng' name='totalng' class='form-control' onkeypress='return justNumbers(event);' style='width: 100px;'  />";

    var menuFpago = menuFormaPago();
    var menucategoria = menucategorias2();
    /*var filexml = "<input id='filexml' accept='.xml' name='filexml' type='file'/>";
     var filepdf = "<input id='filepdf' accept='.pdf' name='filepdf' type='file'/>";*/
    var fileotro = "<input id='fileotro' accept='image/*' name='fileotro' type='file'/>";
    var txtobservaciones = "<input type='text' placeholder='Observaciones' id='observaciones' name='observaciones' class='form-control' style='width: 180px;' />";

    var txtrfc = "<input type='text' id='irfc' name='irfc' placeholder='R.F.C.' class='form-control' maxlength='14' style='width: 180px;' />";
    var txtcontacto = "<input type='text' id='icontacto' name='icontacto' placeholder='Contacto' class='form-control' maxlength='100' style='width: 180px;' />";
    var txttelefono = "<input type='text' id='itelefono' name='itelefono' placeholder='Telefono' class='form-control' maxlength='12' style='width: 130px;'/>";
    var txtcorreo = "<input type='text' id='icorreo' name='icorreo' placeholder='correo@dominio.com' class='form-control' style='width: 180px;' maxlength='100' />";

    var btnGuardar = "<button type='button' class='btn btn-success' onclick='guardarGasto()'><span class='glyphicon glyphicon-floppy-saved'></span> Guardar</button>";

    var tbl = "";
    tbl = "<input id='idinforme' name='idinforme' type='hidden' value='" + id + "' /><input id='idproyecto' name='idproyecto' type='hidden' value='" + idproyecto + "' />";
    tbl += "<table id='filtro' class='display browse' cellspacing='0' widtd='100%'>";
    //tbl += "<tr><td colspan='9'><span class='badge'>Agrega Nuevo Gasto</span></td></tr>";
    tbl += "<tr>";
    tbl += "<td widtd='100px'>" + txtfgasto + "</td>";
    tbl += "<td widtd='100px'>" + txtconcepto + "</td>";
    tbl += "<td widtd='200px'>" + txtnegocio + "</td>";
    tbl += "<td widtd='80px'>" + txttotal + "</td>";
    tbl += "<td widtd='80px'>" + menuFpago + "</td>";
    tbl += "<td>" + menucategoria + "</td>";
    //tbl += "<td widtd='50px'>" + filepdf + "</td>";
    //tbl += "<td widtd='50px'>" + filexml + "</td>";
    tbl += "</tr>";

    tbl += "<tr><td colspan='5'><span class='badge'>Datos Adicionales1</span></td>";
    tbl += "<td widtd='50px' rowspan='4'>" + btnGuardar + "</td>";
    tbl += "</tr>";

    tbl += "<tr>";
    tbl += "<td widtd='100px' colspan='2'>" + txtrfc + "</td>";
    tbl += "<td widtd='100px' colspan='2'>" + txtcontacto + "</td>";
    tbl += "<td widtd='200px'>" + txttelefono + "</td>";
    tbl += "</tr>";

    tbl += "<tr>";
    tbl += "<td widtd='100px' colspan='2'>" + txtcorreo + "</td>";
    tbl += "<td widtd='100px' colspan='2'>" + txtobservaciones + "</td>";
    tbl += "<td widtd='200px'>" + fileotro + "</td>";
    tbl += "</tr>";

    tbl += "</table>";
    //$("#inputnuevogasto").empty();
    //$("#inputnuevogasto").append(tbl);

    return tbl;


    //$("#categoria").val("9999999");//no aplica por default
}
$('#tblGastos tbody').on('click', 'td.details-control', function () {
    var tr = $(this).closest('tr');
    var row = tablaGastos.row(tr);
    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
        tr.removeClass('shown');
    } else {
        // Open this row
        row.child(format(row.data())).show();
        tr.addClass('shown');
    }
});
var binimage;
$('#fileotro').change(function () {
    var file = $("#fileotro").get(0).files[0];
    var r = new FileReader();
    r.onload = function () {
        binimage = r.result;

    };
    r.readAsDataURL(file);
});
function almacenaComensal() {
    var nmbComensales = [];
    $("input[type=text][name=nmbComensales]").each(function () {
        var nombre = $(this).val();
        if (!valorVacio(nombre))
            nmbComensales.push(nombre);
    });
    localStorage.setItem('comensales', JSON.stringify(nmbComensales));
}
$("#ncomensales").change(function () {
    var comensales = []
    if (!localStorage.getItem('comensales')) {
        localStorage.setItem('comensales', []);
    } else {
        comensales = JSON.parse(localStorage.getItem('comensales'));
    }
    var ncomensales = $("#ncomensales").val() * 1;
    if (ncomensales < 0)
        ncomensales = 0;
    if (ncomensales > 50)
        ncomensales = 50;

    $("#ncomensales").val(ncomensales);
    creaInputsComensales(comensales);
});
function creaInputsComensales(comensales) {
    var ncomensales = $("#ncomensales").val() * 1;
    if (comensales.length === 0 && ncomensales > 0) {
        comensales = JSON.parse(localStorage.getItem('comensales'));
    }
    $("#inpComensales").empty();
    if (ncomensales > 0) {
        var tablaGastos = "<table style='width: 100%; border:hidden;'>";
        for (var i = 0; i < ncomensales; i++) {
            var nombre = comensales[i];
            nombre = valorVacio(nombre) ? "" : nombre;
            var inputs = "<div class='row input-group'>" +
                "<div class='input-group-append' style='padding: 0px;'> " +
                "<span class='input-group-text label label-info' style='margin:0px; padding: 10px 20px;'><b>" + (i + 1) + "</b></span>" +
                "</div>" +
                "<input type='hidden' id='nComensal" + i + "' name='nComensal' class='form-control' value='" + i + "' />" +
                "<input type='text' id='nmbComensales" + i + "' name='nmbComensales' onchange='almacenaComensal()' class='form-control' value='" + nombre + "' placeholder='Nombre Comensal " + (i + 1) + "' />" +
                //"<div class='input-group-append'> " +
                //"<button class='btn btn-danger' type='button' style='padding: 10px 20px;'><i class='zmdi zmdi-minus zmdi-hc-lg'></i></button>" +
                //"</div>" +
                "</div>";
            tablaGastos += "<tr><td>" + inputs + "</td></tr>";
        }
        tablaGastos += "</table>";
        $("#inpComensales").append(tablaGastos);
        $("#inpComensales").show();
    } else if (ncomensales === 0) {
        $("#inpComensales").empty();
    }
    almacenaComensal();
}
function masmenoscomensal(valor) {
    var comensales = []
    if (!localStorage.getItem('comensales')) {
        localStorage.setItem('comensales', []);
    } else {
        comensales = JSON.parse(localStorage.getItem('comensales'));
    }
    var ncomensales = $("#ncomensales").val() * 1;
    ncomensales += valor;

    if (ncomensales < 0)
        ncomensales = 0;
    if (ncomensales > 50)
        ncomensales = 50;

    $("#ncomensales").val(ncomensales);
    creaInputsComensales(comensales);
}
/*
inicio funcionalidad comensales - datos adicionales
*/
$("#ncomensalesda").change(function () {
    var comensales = []
    if (!localStorage.getItem('comensales')) {
        localStorage.setItem('comensales', []);
    } else {
        comensales = JSON.parse(localStorage.getItem('comensales'));
    }
    var ncomensales = $("#ncomensalesda").val() * 1;
    if (ncomensales < 0)
        ncomensales = 0;
    if (ncomensales > 50)
        ncomensales = 50;

    $("#ncomensalesda").val(ncomensales);
    creaInputsComensalesda(comensales);
});
function masmenoscomensalda(valor) {
    var comensales = []
    if (!localStorage.getItem('comensales')) {
        localStorage.setItem('comensales', []);
    } else {
        comensales = JSON.parse(localStorage.getItem('comensales'));
    }

    var ncomensales = $("#ncomensalesda").val() * 1;
    ncomensales += valor;
    console.log(comensales, ncomensales)
    if (ncomensales < 0)
        ncomensales = 0;
    if (ncomensales > 50)
        ncomensales = 50;

    $("#ncomensalesda").val(ncomensales);
    creaInputsComensalesda(comensales);
}
function creaInputsComensalesda(comensales) {
    var estatus = $("#estatusInformeDA").val() * 1;
    var ncomensales = $("#ncomensalesda").val() * 1;
    $("#inpComensalesda").empty();
    if (ncomensales > 0) {
        var tablaGastos = "<table style='width: 100%; border:hidden;'>";
        for (var i = 0; i < ncomensales; i++) {
            var nombre = comensales[i];
            nombre = valorVacio(nombre) ? "" : nombre;
            var inputs = "<td style='width: 20px'><label class='label label-primary' style='padding: 10px 20px;'><b>" + (i + 1) + "</b></label></td>";
            inputs += "<td>" +
                "<input type='hidden' id='nComensalda" + i + "' name='nComensalda' class='form-control' value='" + i + "' />" +
                "<input type='text' id='nmbComensalesda" + i + "' name='nmbComensalesda' onchange='almacenaComensalda()' class='form-control " + (estatus > 2 ? "disabled" : "") + "' value='" + nombre + "' placeholder='Nombre Comensal " + (i + 1) + "' style='width: 250px'/>" +
                "</td>";
            tablaGastos += "<tr>" + inputs + "</tr>";
        }
        tablaGastos += "</table>";
        $("#inpComensalesda").append(tablaGastos);
        $("#inpComensalesda").show();
    } else if (ncomensales === 0) {
        $("#inpComensalesda").empty();
    }
    almacenaComensalda();
}
function almacenaComensalda() {
    var nmbComensales = [];
    $("input[type=text][name=nmbComensalesda]").each(function () {
        var nombre = $(this).val();
        if (!valorVacio(nombre))
            nmbComensales.push(nombre);
    });
    localStorage.setItem('comensales', JSON.stringify(nmbComensales));
}
/*
fin funcionalidad comensales - datos adicionales
*/
function guardarGasto() {
    //var fcg = "01-" + $("#fgasto").val().substr(3, 7);
    //if (fcg !== $("#verinforme #mesInfVer").val()) {
    //    $("#inpustGasto").notify("La fecha de gasto NO corresponde al mes contable del informe.", {position: "top"}, "error");
    //}
    $("#inpustGasto").css({
        opacity: 0.5,
        "background-color": "#ffffff"
    });
    $("#preInputGasto").show();

    var inputFileOTRO = document.getElementById("fileotro");
    var fileOTRO = inputFileOTRO.files[0];

    var sizeByte = 0;
    if (fileOTRO) {
        sizeByte = (inputFileOTRO.files[0].size * 1);
    }
    var siezekiloByte = 0;
    if (sizeByte > 0) {
        siezekiloByte = parseInt(sizeByte / 1024);
    }

    var total = $("#totalng").val() * 1;
    var error = 0;

    var extFileOTRO = (($("#fileotro").val()).substring(($("#fileotro").val()).lastIndexOf(".") + 1)).toLowerCase();

    if ($.trim($("#fileotro").val()) !== "") {
        var extensiones_permitidas = new Array("gif", "jpg", "png", "jpeg");
        var permitida = false;
        for (var i = 0; i < extensiones_permitidas.length; i++) {
            if (extensiones_permitidas[i] === extFileOTRO) {
                permitida = true;
                break;
            }
        }
        if (permitida === false) {
            $("#fileotro").notify("Solo se permiten archivos de tipo Imagen.", { position: "top" }, "error");
            $("#fileotro").filestyle('clear');
            error = 1;
        }
    }

    var fgasto = $("#fgasto").val();

    if (fgasto === "") {
        $("#fgasto").notify("Selecciona una fecha.", { position: "bottom" }, "error");
        error = 1;
    }

    var hrg = $("#hgasto").val();
    if (valorVacio(hrg))
        hrg = horaActual("hh:mm");

    if (hrg.length < 5) {
        var hhmm = hrg.split(":");
        hrg = padi(hhmm[0], 2) + ":" + hhmm[1];
    }

    if ($("#concepto").val() === "") {
        $("#concepto").notify("Indica el Concepto.", { position: "bottom left" }, "error");
        error = 1;
    }
    if ($("#negocio").val() === "") {
        $("#negocio").notify("Indica el Negocio.", { position: "bottom" }, "error");
        error = 1;
    }
    if (total === "" || total <= 0) {
        $("#totalng").notify("Indica Importe.", { position: "bottom" }, "error");
        error = 1;
    }

    var rfc = $("#irfc").val();
    if (!valorVacio(rfc)) {
        if (ValidaRFC(rfc) === false) {
            $("#irfc").notify("El formato del R.F.C. es incorrecto.", { position: "top" }, "error");
            error = 1;
        }
    }

    var correo = $("#icorreo").val();
    if (!valorVacio(correo)) {
        if (ValidaEMail(correo) === false) {
            $("#icorreo").notify("El correo no es valido.", { position: "top" }, "error");
            error = 1;
        }
    }

    var telefono = $("#itelefono").val();
    if (!valorVacio(telefono)) {
        if (ValidaTelefono(telefono) === false) {
            $("#itelefono").notify("El telefono no es valido.", { position: "top" }, "error");
            error = 1;
        }
    }

    if (siezekiloByte > 2000) {
        $("#fileotro").notify("No se permiten archivos mayores a 2MB.", { position: "top" }, "error");
        $("#fileotro").filestyle('clear');
        error = 1;
    }


    if (error === 1) {
        $("#inpustGasto").css({
            opacity: 1,
            "background-color": "transparent"
        });
        $("#preInputGasto").hide('slow');
        return false;
    }


    //data.append("filexml", fileXML);
    //data.append("filepdf", filePDF);
    var ugasto = localStorage.getItem("cosa");
    var idinforme = $("#idinforme").val();
    var idproyecto = $("#idproyecto").val();
    var concepto = $("#concepto").val();
    var negocio = $("#negocio").val();
    var observaciones = $("#observaciones").val();
    var rfc = rfc.replace(/-/gi, "");
    var contacto = $("#icontacto").val()
    var telefono = telefono.replace(/-/gi, "");

    var FormaPagoSelect = "", formapago = "";
    try {
        FormaPagoSelect = document.getElementById("formapago");
        formapago = FormaPagoSelect.options[FormaPagoSelect.selectedIndex].text;
    } catch (e) {
        FormaPagoSelect = "";
        formapago = $("#inputCabeceraFormaPago").val();
    }

    var Categoria = $("#categoria").val();
    var CategoriaSelect = document.getElementById("categoria");
    var NombreCategoria = CategoriaSelect.options[CategoriaSelect.selectedIndex].text;
    //inicio comensales
    var listComensales = [];
    var comensales = "";
    var ncomensales = 0;
    try {
        listComensales = JSON.parse(localStorage.getItem('comensales'));
        ncomensales = listComensales.length;
        if (ncomensales > 0) {
            for (var i = 0; i < ncomensales; i++) {
                comensales += valorVacio(comensales) ? listComensales[i] : (", " + listComensales[i]);
            }
        }
    } catch (err) {
        ncomensales = 0;
    }

    comensales = $("#comensalesInsert").val();
    if (!valorVacio(comensales)) {
        var cComensales = comensales.split(",");
        ncomensales = cComensales.length;
    }

    //fin comensales

    if ((NombreCategoria.toLowerCase()).indexOf("alimenta") > -1) {
        if (ncomensales === 0 || valorVacio(comensales)) {
            $("#comensalesInsert").notify("Los comensales son obligatorios.", { position: "top" }, "error");
            error = 1;
        }
    } else if ((NombreCategoria.toLowerCase()).indexOf("sesion") > -1) {
        if (ncomensales === 0 || valorVacio(comensales)) {
            $("#comensalesInsert").notify("Los comensales son obligatorios.", { position: "top" }, "error");
            error = 1;
        }
        if (valorVacio(observaciones)) {
            $("#observaciones").notify("El Objetivo es obligatorio para sesiones de trabajo.", { position: "top" }, "error");
            error = 1;
        }
    }
    var disAnticipo = $("#disAnticipo").val() * 1;
    if (disAnticipo < total) {
        $("#totalng").notify("El importe del gasto no puede ser mayor al importe Por Comprobar (disponible).", { position: "top" }, "error");
        error = 1;
    }

    if (error === 1) {
        $("#inpustGasto").css({
            opacity: 1,
            "background-color": "transparent"
        });
        $("#preInputGasto").hide('slow');
        return false;
    }

    var elementoCat = $("#categoria")[0];
    //var opIndex = elementoCat.selectedIndex;
    var datosCat = elementoCat.options[CategoriaSelect.selectedIndex].dataset;
    var ivacategoria = datosCat.grmativa;

    var datos = {
        "idinforme": idinforme,
        "idproyecto": idproyecto,
        "ugasto": ugasto,
        "concepto": concepto,
        "negocio": negocio,
        "observaciones": observaciones,
        "rfc": rfc,
        "contacto": contacto,
        "telefono": telefono,
        "fgasto": fgasto,
        "hgasto": hrg,
        "total": total,
        "formapago": formapago,
        "categoria": Categoria,
        "correo": correo,
        "fileotros": binimage,
        "nombreCategoria": NombreCategoria,
        "ivaCategoria": ivacategoria,
        "ncomensales": ncomensales,
        "nmbcomensales": comensales
    };

    $.ajax({
        type: "POST",
        url: "/api/InsertGasto",
        data: JSON.stringify(datos), //checar con hector{'dirxml': dirxml, 'dirpdf': dirpdf, 'dirotros': dirotros},
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        beforeSend: function () {
            $("#concepto").val("");
            $("#negocio").val("");
            $("#formapago").val("0");
            $("#categoria").val("0");
            $("#totalng").val("0");
            $("#observaciones").val("");
            $("#icontacto").val("");
            $("#itelefono").val("");
            $("#icorreo").val("");
            $("#irfc").val("");
            var fecha = fechaActual();
            $("#fgasto").val(fecha);
            $("#hgasto").timepicker('setTime', horaActual("hh:mm"));
            $("#ncomensales").val("");
            localStorage.removeItem('comensales');

            creaInputsComensales([]);
            //$("#fileotro").filestyle('clear');
        },
        success: function (result) {
            //console.log(result);
            $.notify("Gasto Guardado.", { globalPosition: 'top center', className: 'success' });
            //obtenerGastosInforme($("#idinforme").val(), $("#idproyecto").val(), 2);
            //selectInforme($("#idinforme").val(), $("#idproyecto").val());
            //console.log($("#idinforme").val(), $("#idproyecto").val(), 2, 1, result);
            consultaInfoGastos($("#idinforme").val(), 2, 1);
        },
        complete: function () {
            //$("#fgasto").val(fgasto);
            $("#inpustGasto").css({
                opacity: 1,
                "background-color": "transparent"
            });
            $("#formapago, #categoria").select2({
                dropdownParent: $("#mAgregarGastoInf")
            });
            //actualiza lista de informes
            ObtenerInformes();
            $("#preInputGasto").hide('slow');
            $('#fileotro').filestyle('clear');
            cargado();
        },
        error: function (result) {
            //
            $("#inpustGasto").css({
                opacity: 1,
                "background-color": "transparent"
            });
            $("#preInputGasto").hide('slow');

            $("#preInputGasto").hide('slow');
            $.notify("Error al Guardar", { globalPosition: 'top center', className: 'error' });
        }
    });

    /*$('.modal-body').stop().animate({
     scrollTop: $("#inputnuevogasto").offset().top
     }, 1000);*/
    //return false;


    //return false;
}
function menuFormaPago() {

    var catDefault = JSON.parse(localStorage.getItem("default"));

    var menu = "";
    menu = "<select id='formapago' name='formapago' data-width='auto' onchange style='height:32px; width:100px'>";
    //menu += "<option value='0'>- Forma de Pago -</option>";

    $("#formapago").empty();
    //$("#formapago").append("<option value='0'>- Forma de Pago -</option>");

    var req = DatosRequisicion();
    //console.log(req);
    var tarjetaTokaReq = datoEle(req.RmReqTarjetaToka);
    var tarjeta = "";
    if (!valorVacio(tarjetaTokaReq)) {
        tarjeta = tarjetaTokaReq;
    } else {
        tarjeta = catDefault.GrEmpTarjetaToka
    }

    var option = "";
    if (!valorVacio(datoEle(tarjeta))) {
        menu += "<option value='2' selected>" + + tarjeta + "</option>";
        option += "<option value='2' selected>" + tarjeta + "</option>";
        $("#formapago").append(option);
    }

    //$("#formapago").append("<option value='1'>Efectivo</option>");
    //menu += "<option value='1'>Efectivo</option>";

    menu += "</select>";
    return menu;

    /*var i = 0;
    $.ajax({
        async: false,
        type: "POST",
        url: "/api/FormaPago",
        data: datos,
        dataType: "json",
        beforeSend: function () {
            $("#formapago").append("<option value='0'>- Forma de Pago -</option>");
           
        },
        success: function (result) {

            $("#formapago").append("<option value='1'>Efectivo</option>");
            menu += "<option value='1'>Efectivo</option>";

            $.each(result, function (key, value) {
                var option = "";

                menu += "<option value='2'>" + + value.GrEmpTarjetaToka + "</option>";
                option += "<option value='2'>" + value.GrEmpTarjetaToka + "</option>";
                
                $("#formapago").append(option);
                i++;
            });
        },
        error: function (result) {
            console.log(result);
        }
    });*/
}
function menucategorias() {

    var menu = "";
    menu = "<select id='categoria' name='categoria' data-width='auto' onchange style='height:32px; width:120px'>";
    menu += "<option value='0' data-informacion=''>- Categoria -</option>";

    var IdRequisicion = document.getElementById("HFRmRdeRequisicion").value * 1;

    var datos = {
        "RmRdeRequisicion": IdRequisicion,
        "TipoRequisicion": 99,
        "valida": 0
    }

    $("#categoria").empty();
    var i = 0;
    $.ajax({
        async: false,
        type: "POST",
        url: "/api/ConsultaMaterial",
        data: datos,
        dataType: "json",
        beforeSend: function () {
            $("#categoria").append("<option value='0' data-informacion=''>- Categoria -</option>");
        },
        success: function (result) {

            $.each(result, function (key, value) {
                var option = "";
                menu += "<option value='" + value.GrMatId + "' data-GrMatIva ='" + value.GrMatIva + "'>" + value.GrMatNombre + "</option>";
                option += "<option value='" + value.GrMatId + "' data-GrMatIva ='" + value.GrMatIva + "'>" + value.GrMatNombre + "</option>";

                $("#categoria").append(option);
                i++;
            });
        },
        error: function (result) {
            console.log(result);
        }
    });
    menu += "</select>";

    return menu;

}
function menucategorias2() {

    var menu = "";
    menu = "<select id='categoria' name='categoria' class='menu-categoria' data-width='auto' onchange style='height:32px; width:120px'>";
    //menu += "<option value='0' data-informacion=''>- Categoria -</option>";

    var IdRequisicion = document.getElementById("HFRmRdeRequisicion").value * 1;
    var datos = {
        "Requisicion": IdRequisicion,
        "Valida": 0,
        'Usuario': UsuarioActivo,
        'Empleado': EmpeladoActivo
    }
    $("#categoria").empty();
    var i = 0;
    $.ajax({
        async: false,
        type: "POST",
        url: "/api/ConsultaMaterial2",
        data: datos,
        dataType: "json",
        beforeSend: function () {
            //$("#categoria").append("<option value='0' data-informacion=''>- Categoria -</option>");
        },
        success: function (result) {
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
                        var option = "";
                        menu += "<option value='" + GrMatId + "' data-GrMatIva ='" + GrMatIva + "'>" + GrMatNombre + "</option>";
                        option += "<option value='" + GrMatId + "' data-GrMatIva ='" + GrMatIva + "'>" + GrMatNombre + "</option>";

                        $("#categoria").append(option);
                    });
                } else {
                    if (!valorVacio(resultado)) {
                        var GrMatId = datoEle(resultado.GrMatId);
                        var GrMatNombre = datoEle(resultado.GrMatNombre);
                        var GrMatPrecio = datoEle(resultado.GrMatPrecio) * 1;
                        var GrMatIva = datoEle(resultado.GrMatIva) * 1;
                        var GrMatGrupo = datoEle(resultado.GrMatGrupo);
                        var GrMatUnidadMedida = datoEle(resultado.GrMatUnidadMedida);
                        var option = "";
                        menu += "<option value='" + GrMatId + "' data-GrMatIva ='" + GrMatIva + "'>" + GrMatNombre + "</option>";
                        option += "<option value='" + GrMatId + "' data-GrMatIva ='" + GrMatIva + "'>" + GrMatNombre + "</option>";

                        $("#categoria").append(option);
                    }
                }

            }
        },
        error: function (result) {
            console.log(result);
        }
    });
    menu += "</select>";
    return menu;
}
/*
funciones adicional por gasto - propina - facturas adicionales
*/
function menuOpcionesAdicionales(datos) {
    //dropdown-toggle
    var menu = "<div class='dropdown'>";
    menu += "<button type='button' class='btn btn-info' data-toggle='dropdown'><i class='zmdi zmdi-more-vert'></i></button>";
    menu += "<div class='dropdown-menu' style='padding: 5px;'>";
    menu += "<a href='#' onclick='gastoAjuste(1, " + JSON.stringify(datos) + ")' class='dropdown-item' style='margin:0px; padding: 10px 0px;'>Agregar otros gastos y/o propina al Gasto</a>";

    if ($.trim(datos.DirXML) !== "") {
        menu += "<label for='fileXmlAdicional" + datos.IdGasto + "' class='dropdown-item' style='margin:0px; padding: 10px 0px;'>";
        menu += "Cargar factura adicional";
        menu += "<input id='fileXmlAdicional" + datos.IdGasto + "' accept='.xml' onchange='gastoAjuste(2," + JSON.stringify(datos) + ")' name='fileXmlAdicional" + datos.IdGasto + "' type='file' class='hidden' />";
        menu += "</label>";
    }

    menu += "</div>";
    menu += "</div>";

    return menu;
}
function gastoAjuste(tipo, datos) {
    var ucrea = localStorage.getItem("cosa");
    var ImportePropina = 0;
    datos['Tipo'] = tipo;
    datos['UCrea'] = ucrea;
    datos['BinXML'] = "";
    datos['NombreArc'] = "";
    datos['ExtFile'] = "";
    if (tipo === 1) {
        var botones = [];
        botones[0] = {
            text: "Si", click: function () {
                var error = 0;
                ImportePropina = $("#importePropina").val() * 1;
                var importePropinaMax = $("#importePropina").attr("max") * 1;
                var afecta = ($("#ImpGastadoPro").is(':checked') || $("#ImpComprobadoPro").is(':checked')) ? true : false;
                if (ImportePropina <= 0) {
                    error = 1;
                    $.notify("El importe debe ser mayor a cero (0).", { position: "top center", className: "error" });
                }
                if (ImportePropina > importePropinaMax) {
                    error = 1;
                    $.notify("El importe no puede ser mayor a lo gastado.", { position: "top center", className: "error" });
                }
                if (afecta === false) {
                    error = 1;
                    $.notify("El importe debe afectar al importe gastado y/o comprobado.", { position: "top center", className: "error" });
                }
                if (error === 0) {
                    $(this).dialog("close");
                    datos['Observaciones'] = "Gasto: " + datos['Concepto'];
                    datos['Concepto'] = "Otro Gasto / Propina";
                    if ($("#ImpGastadoPro").is(':checked')) {
                        datos['TGastado'] = ImportePropina;
                        datos['AfectaImpGastado'] = 1;
                    }
                    else {
                        datos['TGastado'] = 0;
                        datos['AfectaImpGastado'] = 0;
                    }

                    if ($("#ImpComprobadoPro").is(':checked')) {
                        datos['TComprobar'] = ImportePropina;
                        datos['AfectaImpComprobado'] = 1;
                    }
                    else {
                        datos['TComprobar'] = 0;
                        datos['AfectaImpComprobado'] = 0;
                    }

                    agregaAjusteGasato(datos);
                } else {
                    return false;
                }
            }
        };
        botones[1] = {
            text: "No", click: function () {
                $(this).dialog("close");
            }
        };
        var gasto = datos.Concepto + " / " + datos.Negocio + " / " + formatNumber.new((datos.TGastado * 1).toFixed(2), "$ ");
        var txtImporte = "<div class='input-group'>";
        txtImporte += "<span class='input-group-addon' style='width: 140px;'>Por un importe de $</span>";
        txtImporte += "<div class='form-group'>";
        txtImporte += "<input type='number' id='importePropina' name='importePropina' placeholder='0.00' min='0' max='" + datos.ValMaxPropina + "' class='form-control'>";
        txtImporte += "<i class='form-group__bar'></i> ";
        txtImporte += "</div>";
        txtImporte += "</div>";

        var chkAfectaa = "<div id='chkAfectaImpPro' class='hidden'>";
        chkAfectaa += "<b>Afecta al importe:</b><br /><label class='custom-control custom-checkbox'>";
        chkAfectaa += "<input type='checkbox' checked id='ImpGastadoPro' class='custom-control-input'>";
        chkAfectaa += "<span class='custom-control-indicator'></span>";
        chkAfectaa += "<span class='custom-control-description'>Gastado</span>";
        chkAfectaa += "</label>";
        chkAfectaa += "<label class='custom-control custom-checkbox'>";
        chkAfectaa += "<input type='checkbox' checked id='ImpComprobadoPro' class='custom-control-input'>";
        chkAfectaa += "<span class='custom-control-indicator'></span>";
        chkAfectaa += "<span class='custom-control-description'>Comprobado</span>";
        chkAfectaa += "</label>";
        chkAfectaa += "</div>";

        Seguridad.confirmar("Agregar otro gasto / propina al gasto: <b>" + gasto + "</b>" + txtImporte + chkAfectaa, botones, "Agregar Gasto Adicional.", "#verInformeGastos");
    } else if (tipo === 2) {
        console.log("xml adicional => ", datos);

        var file = $("#fileXmlAdicional" + datos.IdGasto).get(0).files[0];
        var r = new FileReader();
        var nombre = file.name;
        var extFile = (nombre.substring(nombre.lastIndexOf(".") + 1)).toLowerCase();
        if (extFile === "xml") {

            var botones = [];
            botones[0] = {
                text: "Si", click: function () {
                    $(this).dialog("close");

                    r.onload = function () {
                        var binXML = r.result;
                        var afecta = ($("#ImpGastadoFac").is(':checked') || $("#ImpComprobadoFac").is(':checked')) ? true : false;
                        datos['Observaciones'] = "Gasto: " + datos['Concepto'];
                        datos['Concepto'] = "Factura Adicional";

                        if ($("#ImpGastadoFac").is(':checked')) {
                            datos['AfectaImpGastado'] = 1;
                        }
                        else {
                            datos['AfectaImpGastado'] = 0;
                        }

                        if ($("#ImpComprobadoFac").is(':checked')) {
                            datos['AfectaImpComprobado'] = 1;
                        }
                        else {
                            datos['AfectaImpComprobado'] = 0;
                        }

                        datos['BinXML'] = binXML;
                        datos['NombreArc'] = nombre;
                        datos['ExtFile'] = extFile;
                        nombre = nombre.replace("." + extFile, "");
                        agregaAjusteGasato(datos);
                        $("#fileXmlAdicional" + datos.IdGasto).clearInputs();
                    };
                    r.readAsDataURL(file);

                }
            };
            botones[1] = {
                text: "No", click: function () {
                    $(this).dialog("close");
                    $("#fileXmlAdicional" + datos.IdGasto).clearInputs();
                }
            };
            var gasto = datos.Concepto + " / " + datos.Negocio + " / " + formatNumber.new((datos.TGastado * 1).toFixed(2), "$ ");

            var chkAfectaa = "<div id='chkAfectaImpFac' class='hidden'>";
            chkAfectaa += "<b>Afecta al importe:</b><br /><label class='custom-control custom-checkbox'>";
            chkAfectaa += "<input type='checkbox' checked id='ImpGastadoFac' class='custom-control-input'>";
            chkAfectaa += "<span class='custom-control-indicator'></span>";
            chkAfectaa += "<span class='custom-control-description'>Gastado</span>";
            chkAfectaa += "</label>";
            chkAfectaa += "<label class='custom-control custom-checkbox'>";
            chkAfectaa += "<input type='checkbox' checked id='ImpComprobadoFac' class='custom-control-input'>";
            chkAfectaa += "<span class='custom-control-indicator'></span>";
            chkAfectaa += "<span class='custom-control-description'>Comprobado</span>";
            chkAfectaa += "</label>";
            chkAfectaa += "</div>";

            Seguridad.confirmar("Agregar factura adcional:<br /><b>" + nombre + "</b><br />al gasto: <b>" + gasto + "</b>" + chkAfectaa, botones, "Agregar Factura.", "#verInformeGastos");

        } else {
            $.notify("Archivo invalido, prueba con otro.", { globalPosition: 'top center', className: 'error', autoHideDelay: 4000 });
            $("#fileXmlAdicional" + datos.IdGasto).clearInputs();
        }
    }
}
function agregaAjusteGasato(datos) {
    $.ajax({
        async: true,
        type: "POST",
        url: "/api/AgregarAjusteGasto",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            if (result.AgregadoOk === true) {
                $.notify(result.Descripcion, { globalPosition: 'top center', className: 'success' });
                consultaInfoGastos(result.IdInforme, 2, 1);
            } else {
                console.log(result);
                $.notify(result.Descripcion, { globalPosition: 'top center', className: 'error', autoHideDelay: 6000 });
            }
        },
        complete: function () {
        },
        error: function (result) {
            console.log(result);
            $.notify("Error al agregar.", { globalPosition: 'top center', className: 'error' });
        }
    });
}
function actualizarCategoriaGastoAjuste(datos) {
    var IdCategoria = $("#categoria" + datos.IdGasto).val();
    var elementoCat = $("#categoria" + datos.IdGasto)[0];
    var opIndex = elementoCat.selectedIndex;
    var Categoria = "";
    var datosCat = [];
    var IvaCategoria = 0;
    try {
        Categoria = elementoCat.options[opIndex].text;
        datosCat = elementoCat.options[opIndex].dataset;
        IvaCategoria = datosCat.grmativa;
    } catch (err) {
        Categoria = "S/Categoria";
        datosCat = [];
        IvaCategoria = 0;
    }

    datos['IdCategoria'] = IdCategoria;
    datos['Categoria'] = Categoria;
    datos['IvaCategoria'] = IvaCategoria;

    $.ajax({
        async: true,
        type: "POST",
        url: "/api/UpdateCategoriaGastoAjuste",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            console.log(result);
            if (result.ActualizadaOk === true) {
                $.notify(result.Descripcion, { globalPosition: 'top center', className: 'success' });
            } else {
                $.notify(result.Descripcion, { globalPosition: 'top center', className: 'error' });
            }
        },
        complete: function () {
        },
        error: function (result) {
            console.log(result);
            $.notify("Error al actualizar categoria.", { globalPosition: 'top center', className: 'error' });
        }
    });
}
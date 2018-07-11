var tablaGastos = "";
var disabled = "";
var trueFalse = false;
var fecUltgastos = "";

//var PGastos = Seguridad.permisos(5);

function formatoInputFile() {
    $(":file[id!='fileotro']").filestyle({
        input: false,
        buttonText: "",
        size: "xs"
    });
}

function obtenerGastosInforme(id, idproyecto, estatusinf) {
    estatusinf = estatusinf * 1;
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
    var total = 0, totalmonto = 0, totalaceptable = 0,
        totalnoaceptable = 0, totalnodeducible = 0;
    $.ajax({
        async: true,
        type: "POST",
        url: "/api/browseGastosInforme",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        success: function (result) {

            var menuFpago = menuFormaPago();
            var menucategoria = menucategorias();
            var numGastos = result.length;
            tablaGastos
                .clear()
                .draw();
            $.each(result, function (key, value) {
                var menuFpago2 = menuFpago;
                var menucategoria2 = menucategoria;

                i++;
                var txtfgasto = "", txtconcepto = "", txtnegocio = "", txttotal = ""
                    , filexml = "", filepdf = "", fileotro = "", chkAplica = ""
                    , tblBtn = "", txtgorigen = "", txtmonto = "", txthgasto = "";

                var aplica = value.g_aplica * 1;

                var latitud = valorVacio(value.g_latitud) ? "" : value.g_latitud;
                var logitud = valorVacio(value.g_longitud) ? "" : value.g_longitud;

                //variables de tipo de gasto
                var tipoajuste = value.tipoajuste * 1;
                var najustes = value.najustes * 1;

                var ngasto_orden = value.orden;
                var valmaxpropina = value.valmaxpropina * 1;

                uresponsable = value.i_uresponsable * 1;

                if (tipoajuste !== 1) {
                    filexml = "<span id='inputfilexml" + value.g_id + "'>";
                    filexml += inputfilexml(id, idproyecto, value.g_id, aplica, value.g_dirxml, value.g_dirpdf, value.g_dirotros, 2, value.g_idgorigen, tipoajuste);//estatusinf
                    filexml += "</span>";
                }

                filepdf = "<span id='inputfilepdf" + value.g_id + "'>";
                filepdf += inputfilepdf(id, idproyecto, value.g_id, aplica, value.g_dirxml, value.g_dirpdf, value.g_dirotros, 2);//estatusinf
                filepdf += "</span>";

                fileotro = "<span id='inputfileotro" + value.g_id + "'>";
                fileotro += inputfileotro(id, idproyecto, value.g_id, aplica, value.g_dirxml, value.g_dirpdf, value.g_dirotros, 2);//estatusinf
                fileotro += "</span>";

                txtmonto = "<div id='monto" + value.g_id + "' style='display: table-cell; text-align: right; width: 80px; word-wrap: break-word'>" + formatNumber.new((value.MONTO * 1).toFixed(2), "$ ") + "</div>";

                var hrmm = (valorVacio(value.g_hgasto) ? "0:00" : value.g_hgasto).split(":");
                var hgasto = "0:00";
                if ((hrmm[0] * 1) === 0 && (hrmm[1] * 1) === 0) {
                    hgasto = "0:00";
                } else {
                    hgasto = (hrmm[0] * 1) + ":" + hrmm[1];
                }

                var fg = (value.g_fgasto).split('-');
                txtfgasto = formatFecha(fg[2] + '-' + fg[1] + '-' + fg[0], 'dd mmm'); //
                txtconcepto = value.g_concepto;
                txtnegocio = value.g_negocio;
                txttotal = formatNumber.new((value.g_total * 1).toFixed(2), "$");
                txthgasto = hgasto;
                menuFpago2 = value.g_formapago;
                //menucategoria2 = value.g_nombreCategoria;
                var functionActCta = "actualizarCtaGasto(" + value.g_id + ", " + id + ", " + estatusinf + ")";
                menucategoria2 = menucategoria.replace("categoria", "categoria" + value.g_id).replace("onchange", "onchange='" + functionActCta + "' ");
                var functionActObs = "actualizarObsGasto(" + value.g_id + ", " + id + ", " + estatusinf + ")";
                var txtObservaciones = inputTextGasto("observaciones" + value.g_id, "observaciones" + value.g_id, value.g_observaciones, functionActObs, "", "", "", "", "");
                //txtobservaciones = "<div style='display: table-cell; width: 200px; word-wrap: break-word'>" + value.g_observaciones + "</div>";
                txtgorigen = "";

                var txttype = "text";
                if (value.g_id === value.g_idgorigen) {
                    txttype = "hidden";
                }

                txtgorigen = "<span id='tooltip" + value.g_id + "' data-toggle='tooltip' data-placement='top' data-html='true' " +
                    " title=\"<div style='width: 170px;'>El gasto esta relacionado</div>\" aria-hidden='true'><input id='gorigen" + value.g_id + "' disabled class='txtgorigen' value='' name='gorigen" + value.g_id + "' type='" + txttype + "' style='width: 20px;' /></span>";

                //}

                fecUltgastos = value.g_fgasto;

                var aAnt = (value.c_afectaant === "1" ? "" : "");//"*" asterico de si afecta anticipo o no
                var id_gasto = lblNGasto(value.g_id, value.g_total, value.MONTO, i, value.g_idgorigen, ngasto_orden, tipoajuste)

                // if ((estatusinf <= 2) || (estatusinf > 2 && (aplica === 1))) {

                tblBtn = tblBtnGasto(0, id, value.g_id, SelectDatosAdicional, DeleteGasto, estatusinf, uresponsable, uidlogin, value.g_dirxml, value.g_dirpdf, value.g_dirotros, (value.g_concepto + " " + value.g_negocio));

                total += value.g_total * 1;
                totalmonto += value.MONTO * 1;

                totalaceptable += value.importeaceptable * 1;
                totalnoaceptable += value.importenoaceptable * 1;
                totalnodeducible += value.importenodeducible * 1;

                var ccomprobante = valorVacio(value.g_dirxml) ? 0 : 1;
                var fCambioNoAceptable = "CambioNoAceptable(" + id + ", " + value.g_id + "," + value.importeaceptable + ", " + value.MONTO + ", " + ccomprobante + ")";
                var fCambioNoDeducible = "CambioNoDeducible(" + id + ", " + value.g_id + "," + value.importeaceptable + ", " + value.MONTO + ", " + ccomprobante + ")";
                var txtAceptable = formatNumber.new(value.importeaceptable.toFixed(2), "$ ");
                var txtNoAceptable = "<input type='number' id='noAceptable" + value.g_id + "' onChange='" + fCambioNoAceptable + "' value='" + (value.importenoaceptable.toFixed(2)) + "' class='form-control' style='width: 100%' min='0' max='" + value.MONTO + "'>";
                var txtNoDeducible = "";
                //if (ccomprobante === 0) {
                //    txtNoDeducible = "<label id='NoDeducible" + value.g_id + "'style='width: 100%'>" + formatNumber.new(value.importenodeducible.toFixed(2), "$ ") + "</label>";
                //} else {
                txtNoDeducible = "<input type='number' id='noDeducible" + value.g_id + "' onChange='" + fCambioNoDeducible + "' value='" + (value.importenodeducible.toFixed(2)) + "' class='form-control' style='width: 100%' min='0' max='" + value.MONTO + "'>";
                //}

                var linkComensales = "";
                var ncomensales = value.ncomensales * 1;
                var nmbcomensales = ncomensales > 0 ? value.nmbcomensales : "";
                //console.log(ncomensales, nmbcomensales);
                if (ncomensales === 0 && tipoajuste === 0 && estatusinf === 2) {
                    linkComensales = nmbcomensales; //inputTextGasto("comensal_" + value.g_id, "comensal_" + value.g_id, "", "actualizaComensales(this.value," + value.g_id + ", " + id + ")", "width: 130px;", "", "", "", "");
                } else if (ncomensales === 1) {
                    if (tipoajuste === 0 && estatusinf === 2 && (value.g_conciliacionbancos * 1) === 0) {
                        linkComensales = nmbcomensales; //inputTextGasto("comensal_" + value.g_id, "comensal_" + value.g_id, nmbcomensales, "actualizaComensales(this.value," + value.g_id + ", " + id + ")", "width: 130px;", "", "", "", "");
                    } else {
                        linkComensales = nmbcomensales !== "" ? nmbcomensales : "";
                    }
                } else if (ncomensales > 1) {
                    //linkComensales = "<button type='button' class='btn btn-link' onclick='verComensale(" + value.g_id + ", " + id + "," + ncomensales + ", \"" + nmbcomensales + "\", " + estatusinf + ",  " + (value.g_conciliacionbancos * 1) + ")'>Ver " + ncomensales +" Comensales</button>";
                    linkComensales = "<button type='button' class='btn btn-link' onclick='verDatosAdicionales(" + value.g_id + ", " + id + ", " + 0 + ", " + estatusinf + ")'>Click para ver <b>" + ncomensales + "</b> Comensales</button>";
                }

                tablaGastos.row.add([
                    id_gasto,
                    txtfgasto,
                    txthgasto,
                    txtconcepto,
                    menucategoria2,
                    txtnegocio,
                    txttotal,
                    //menuFpago2,
                    filexml, //XML
                    filepdf, //PDF
                    fileotro, //OTRO
                    txtmonto,
                    txtAceptable,
                    txtNoAceptable,
                    txtNoDeducible,
                    txtObservaciones + "<br />" + linkComensales,
                ]).draw(false);
                // }
                //
                var selCue = value.g_categoria;
                $("#categoria" + value.g_id).val(selCue);
                $("#totalg").val(total);
                $("#montog").val(totalmonto);

                tdtotales(total, totalmonto, totalaceptable, totalnoaceptable, totalnodeducible);
            });
        },
        complete: function () {
            formatoInputFile();

            obtenNumGasto();

            $('#tblGastos tbody tr input').css({ height: '15px' });

            //ocultar forma de pago
            //var column = tablaGastos.column(6);
            //column.visible(false);

        },
        error: function (result) {
            console.log(result);
        }
    });
    $('#nuevoGasto').css('display', 'block');
    hideColHoraGasto('inicio');
    setTimeout(function () {
        tablaGastos
            .order([1, 'asc'])
            .draw();
    }, 1000);

    tablaGastos.columns.adjust().draw();

    setTimeout(function () {
        $("#tblGastos tbody tr").each(function () {
            $(this)[0].cells[5].className = "text-right";
            $(this)[0].cells[9].className = "text-right";
            $(this)[0].cells[10].className = "text-right";
            $(this)[0].cells[11].className = "text-right";
        });
    }, 1000);
}

function actualizarCtaGasto(IdGasto, IdInforme, estatus) {

    var Categoria = $("#categoria" + IdGasto).val();
    var CategoriaSelect = document.getElementById("categoria" + IdGasto);
    var NombreCategoria = CategoriaSelect.options[CategoriaSelect.selectedIndex].text;

    var datos = {
        'IdInforme': IdInforme,
        'IdGasto': IdGasto,
        'Categoria': Categoria,
        'NombreCategoria': NombreCategoria
    };
    $.ajax({
        type: "POST",
        url: "/api/ActualizarCtaGasto",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result === "OK") {
                $.notify("Cuenta Actualizada.", { globalPosition: 'top center', className: 'success' });
            } else {
                $.notify("Error al Guardar.", { globalPosition: 'top center', className: 'error' });
            }
        },
        complete: function () {
            obtenerGastosInforme(IdInforme, 0, 0);
        },
        error: function (result) {
            console.log(result);
            $.notify("Error al Guardar.", { globalPosition: 'top center', className: 'error' });
        }
    });
}

function actualizarObsGasto(IdGasto, IdInforme, estatus) {

    var Observaciones = $("#observaciones" + IdGasto).val();

    var datos = {
        'IdInforme': IdInforme,
        'IdGasto': IdGasto,
        'Observaciones': Observaciones
    };
    $.ajax({
        type: "POST",
        url: "/api/ActualizarObsGasto",
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result === "OK") {
                $.notify("Observación Guardada.", { globalPosition: 'top center', className: 'success' });
            } else {
                $.notify("Error al Guardar.", { globalPosition: 'top center', className: 'error' });
            }
        },
        complete: function () {
            obtenerGastosInforme(IdInforme, 0, 0);
        },
        error: function (result) {
            console.log(result);
            $.notify("Error al Guardar.", { globalPosition: 'top center', className: 'error' });
        }
    });
}

function CambioNoAceptable(idinforme, idgasto, importeaceptable, monto, ccomprobante) {

    var importeNoAceptable = $("#noAceptable" + idgasto).val() * 1;
    var importeNoDeducible = $("#noDeducible" + idgasto).val() * 1;

    if ((importeNoAceptable <= monto && importeNoAceptable >= 0) &&
        (importeNoDeducible <= monto && importeNoDeducible >= 0)) {
        var datos = {
            'IdInforme': idinforme,
            'IdGasto': idgasto,
            'ImporteAceptable': importeaceptable,
            'Monto': monto,
            'ImporteNoAceptable': importeNoAceptable,
            'ImporteNoDeducible': importeNoDeducible
        };
        $.ajax({
            type: "POST",
            url: "/api/GastoImporteNoAceptable",
            data: JSON.stringify(datos),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            cache: false,
            success: function () {
                //$.notify("Gasto Actualizado [Comprobante Cargado].", { globalPosition: 'top center', className: 'success' });
            },
            complete: function () {
                obtenerGastosInforme(idinforme, 0, 0);
            },
            error: function (result) {
                console.log(result);
                $.notify("Error al Guardar.", { globalPosition: 'top center', className: 'error' });
            }
        });
    } else {
        $.notify("El importe no aceptado debe ser mayor o igual a 0 y menor o igual al importe a comprobar.", { globalPosition: 'top center', className: 'success' });
    }
}
function CambioNoDeducible(idinforme, idgasto, importeaceptable, monto, ccomprobante) {
    var importeNoAceptable = $("#noAceptable" + idgasto).val() * 1;
    var importeNoDeducible = $("#noDeducible" + idgasto).val() * 1;


    if ((importeNoAceptable <= monto && importeNoAceptable >= 0) &&
        (importeNoDeducible <= monto && importeNoDeducible >= 0)) {
        var datos = {
            'IdInforme': idinforme,
            'IdGasto': idgasto,
            'ImporteAceptable': importeaceptable,
            'Monto': monto,
            'ImporteNoAceptable': importeNoAceptable,
            'ImporteNoDeducible': importeNoDeducible
        };
        $.ajax({
            type: "POST",
            url: "/api/GastoImporteNoAceptable",
            data: JSON.stringify(datos),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            cache: false,
            success: function () {
                //$.notify("Gasto Actualizado [Comprobante Cargado].", { globalPosition: 'top center', className: 'success' });
            },
            complete: function () {
                obtenerGastosInforme(idinforme, 0, 0);
            },
            error: function (result) {
                console.log(result);
                $.notify("Error al Guardar.", { globalPosition: 'top center', className: 'error' });
            }
        });
    } else {
        $.notify("El importe no deducible debe ser mayor o igual a 0 y menor o igual al importe a comprobar.", { globalPosition: 'top center', className: 'success' });
    }
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

function tblBtnGasto(idproyecto, idinforme, id, SelectDatosAdicional, DeleteGasto, estatusinf, uresponsable, uidlogin, dirxml, dirpdf, dirotros, nombregasto) {

    var tbl = "<table class='display' cellspacing='0' width='100%'><tr>";

    tbl += "<td><button type='button' class='btn btn-success btn-xs glyphicon glyphicon-plus' onclick='verDatosAdicionales(" + id + ", " + idinforme + ", " + 0 + ", " + estatusinf + ")'></button></td>";
    //if (estatusinf <= 2)
    //  tbl += "<td><button type='button' class='btn btn-danger btn-xs glyphicon glyphicon-trash DelGas' onclick='eliminarGasto(" + id + ", " + idinforme + ", " + 0 + ", " + estatusinf + ", \"" + dirxml + "\", \"" + dirpdf + "\", \"" + dirotros + "\", \"" + nombregasto + "\")'></button></td>";

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

function inputfileotro(idinforme, idproyecto, id, aplica, dirxml, dirpdf, dirotros, estatusinf) {
    var fileotro = "";
    var colorver = "";
    if (valorVacio(dirotros) && estatusinf <= 2) {
        if (aplica === 0) {
            fileotro = "<a data-toggle='tooltip' class='btn btn-default btn-xs disabled glyphicon glyphicon-folder-open' aria-disabled='false' role='button'></a>";
        } else {
            fileotro = "<input id='fileotro" + id + "' accept='image/*' onchange='actualizarGastoComOTRO(" + id + ", " + idinforme + ", " + 0 + ", \"" + dirxml + "\", \"" + dirpdf + "\", \"" + dirotros + "\", " + estatusinf + ", \"file\")' name='fileotro" + id + "' type='file'/>";
        }
    } else {
        habilitarLinkComprobante(dirotros);
        colorver = (disabled === "disabled") ? "default" : "success";
        if (aplica === 0) {
            disabled = "disabled";
            trueFalse = true;
        }
        fileotro = "<a href='#' data-toggle='tooltip' title='Ver Otros Comprobantes' class='btn btn-" + colorver + " btn-xs " + disabled + " glyphicon glyphicon-eye-open' aria-disabled='" + trueFalse + "' onclick='verComprobante(\"OTRO\", " + 0 + ", " + idinforme + ", " + id + ", \"" + dirotros + "\", " + estatusinf + ")' role='button'></a>";

    }
    return fileotro;
}

function inputfilepdf(idinforme, idproyecto, id, aplica, dirxml, dirpdf, dirotros, estatusinf) {
    var filepdf = "";
    var colorver = "";
    if (valorVacio(dirpdf) && estatusinf <= 2) {
        if (aplica === 0) {
            filepdf = "<a data-toggle='tooltip' class='btn btn-default btn-xs disabled glyphicon glyphicon-folder-open' aria-disabled='false' role='button'></a>";
        } else {
            // filepdf = "<input id='filepdf" + id + "' accept='.pdf'  name='filepdf" + id + "' type='file'/>";
            filepdf = "<input id='filepdf" + id + "' accept='.pdf' onchange='actualizarGastoComPDF(" + id + ", " + idinforme + ", " + 0 + ", \"" + dirxml + "\", \"" + dirpdf + "\", \"" + dirotros + "\", " + estatusinf + ", \"file\")' name='filepdf" + id + "' type='file'/>";
        }
    } else {
        habilitarLinkComprobante(dirpdf);
        colorver = (disabled === "disabled") ? "default" : "success";
        if (aplica === 0) {
            disabled = "disabled";
            trueFalse = true;
        }
        filepdf = "<a href='#' data-toggle='tooltip' title='Ver PDF' class='btn btn-" + colorver + " btn-xs " + disabled + " glyphicon glyphicon-eye-open' aria-disabled='" + trueFalse + "' onclick='verComprobante(\"PDF\", " + 0 + ", " + idinforme + ", " + id + ", \"" + dirpdf + "\", " + estatusinf + ")' role='button'></a>";
    }
    return filepdf;
}

function inputfilexml(idinforme, idproyecto, id, aplica, dirxml, dirpdf, dirotros, estatusinf, idgorigen, tipoajuste) {
    id = id * 1;
    idgorigen = idgorigen * 1;
    var filexml = "";
    var colorver = "";
    if (valorVacio(dirxml) && estatusinf <= 2) {
        if (aplica === 0 || id !== idgorigen) {
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
        filexml = "<a id='verxml" + id + "' data-xml='si' dirxml='" + dirxml + "' href='#' data-toggle='tooltip' title='Ver XML' class='btn btn-" + colorver + " btn-xs " + disabled + " glyphicon glyphicon-eye-open' aria-disabled='" + trueFalse + "' onclick='verComprobante(\"XML\", " + 0 + ", " + idinforme + ", " + id + ", \"" + dirxml + "\", " + estatusinf + ")' role='button'></a>";
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

    $("#frmDatosAdicionales #rfc").val("");
    $("#frmDatosAdicionales #contacto").val("");
    $("#frmDatosAdicionales #telefono").val("");
    $("#frmDatosAdicionales #correo").val("");

    localStorage.removeItem('comensales');
    $("#ncomensales, #ncomensalesda").val("");

    if (estatusinf === 2) {//Seguridad.permiso(5, "SaveDatosAdicionales") === 1)
        $("#btnDA").show();
        $("#frmDatosAdicionales #rfc").attr("disabled", false);
        $("#frmDatosAdicionales #contacto").attr("disabled", false);
        $("#frmDatosAdicionales #telefono").attr("disabled", false);
        $("#frmDatosAdicionales #correo").attr("disabled", false);
        $("#ncomensalesda").attr("disabled", false);
    } else {
        $("#btnDA").hide();
        $("#frmDatosAdicionales #rfc").attr("disabled", true);
        $("#frmDatosAdicionales #contacto").attr("disabled", true);
        $("#frmDatosAdicionales #telefono").attr("disabled", true);
        $("#frmDatosAdicionales #correo").attr("disabled", true);
        $("#ncomensalesda").attr("disabled", true);
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
                //localStorage.setItem('comensales', JSON.stringify(['dos', 'tres']));
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
    $("#frmDatosAdicionales #idproyecto").val(idproyecto);
}
/*
inicio funcionalidad comensales - datos adicionales
*/
function creaInputsComensalesda(comensales) {
    var estatus = 3;//$("#estatusInformeDA").val() * 1;
    var ncomensales = $("#ncomensalesda").val() * 1;
    $("#inpComensalesda").empty();
    if (ncomensales > 0) {
        var tablaGastos = "<table style='width: 100%'>";
        for (var i = 0; i < ncomensales; i++) {
            var nombre = comensales[i];
            nombre = valorVacio(nombre) ? "" : nombre;
            var inputs = "<td style='width: 20px'><button class='btn btn-default disabled' type='button' style='padding: 10px 20px;'><b>" + (i + 1) + "</b></button></td>";
            inputs += "<td>" +
                "<input type='hidden' id='nComensalda" + i + "' name='nComensalda' class='form-control' value='" + i + "' />" +
                "<input type='text' id='nmbComensalesda" + i + "' name='nmbComensalesda' onchange='almacenaComensalda()' class='form-control disabled' disabled value='" + nombre + "' placeholder='Nombre Comensal " + (i + 1) + "' style='width: 250px'/>" +
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
function GuardarDatosAdicionales() {
    var error = 0;
    var contacto = $("#frmDatosAdicionales #contacto").val();
    var idgasto = $("#frmDatosAdicionales #idgasto").val();
    var idinforme = $("#frmDatosAdicionales #idinforme").val();
    var idproyecto = $("#frmDatosAdicionales #idproyecto").val();
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

    var datos = { 'idgasto': idgasto, 'idinforme': idinforme, 'idproyecto': 0, 'contacto': contacto, 'rfc': rfc.replace(/-/gi, ""), 'correo': correo, 'telefono': telefono.replace(/-/gi, "") };

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
            $.notify("Datos Guardados.", { globalPosition: 'top center', className: 'success' });
            Seguridad.bitacora("SaveDatosAdicionales", 5, 0 + "," + idinforme + "," + idgasto, "Se guardaron datos adicionales.", 1);
        },
        error: function (result) {
            $.notify("Error al Guardar", { globalPosition: 'top center', className: 'error' });
            Seguridad.bitacora("SaveDatosAdicionales", 5, 0 + "," + idinforme + "," + idgasto, "Error: Al guardar datos adicionales. " + JSON.stringify(datos) + " " + JSON.stringify(result), 0);
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

                    if (comprobante === "XML") {
                        //selectInforme(result[0]['idinforme'], result[0]['idproyecto'], 0);
                        consultaInfoGastos(idinforme, 3, 0);
                        ObtenerInformes();
                    } else {
                        //consultaInfoGastos($("#idinforme").val(), $("#idproyecto").val(), 2, 1);
                        obtenerGastosInforme(idinforme, 3, 0);
                    }

                },
                error: function (result) {
                    cargado();
                    $.notify("Error al Eliminar", { globalPosition: 'top center', className: 'error' });
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

function verComprobante(comprobante, idproyecto, idinforme, idgasto, dircomp, estatus) {
    $("#verComprobante").modal('show');
    opacityModalVerInfoG();

    var btnEliCom = "";
    btnEliCom = "<button type='button' class='btn btn-danger btn-xs' onclick='eliminarGastoCom(\"" + comprobante + "\", " + 0 + ", " + idinforme + ", " + idgasto + ", \"" + dircomp + "\", " + estatus + ")'><i class='zmdi zmdi-delete zmdi-hc-2x'></i> Eliminar</button>";

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
                controles = "<button type='button' class='btn btn-warning btn-xs' onclick='imprimirXML()'><i class='zmdi zmdi-print zmdi-hc-2x'></i> Imprimir</button> ";
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
        //controles += "<button type='button' class='btn btn-danger btn-xs' onclick='rotarImg(\"" + dircomp + "\", 90, " + 0 + ", " + idinforme + ", " + idgasto + ")'><i class='zmdi zmdi-rotate-left zmdi-hc-2x'></i></button> ";
        //controles += "<button type='button' class='btn btn-success btn-xs' onclick='rotarImg(\"" + dircomp + "\", -90, " + 0 + ", " + idinforme + ", " + idgasto + ")'><i class='zmdi zmdi-rotate-right zmdi-hc-2x'></i></button> ";
        controles += "<button type='button' class='btn btn-warning btn-xs' onclick='imprimirImg(\"" + dircomp + "\")'><i class='zmdi zmdi-print zmdi-hc-2x'></i> Imprimir</button> ";
        controles += btnEliCom;
        var f = new Date();
        var fh = f.getDate() + '' + f.getMonth() + '' + f.getFullYear() + '' + f.getHours() + '' + f.getMinutes() + '' + f.getSeconds();
        $("#controles_compOTRO").append(controles);
        $("#compOTRO").show();
        $("#compOTRO").attr("src", dircomp + "?" + fh);
    }

}

function imprimirImg(imagen) {
    $("#print_compOTRO").printArea();
}

function imprimirXML() {
    $("#print_compXML").printArea({
        mode: "iframe",
        standard: "html5",
        popTitle: 'relatorio',
        popClose: false,
        extraCss: 'css/app.css',
        extraHead: '',
        retainAttr: ["id", "class", "style"],
        printDelay: 500, // tempo de atraso na impressao
        printAlert: true,
        printMsg: 'Aguarde'
    });
}

function qrxml(datos) {
    $.ajax({
        async: true,
        type: "POST",
        url: "includes/Script.php?modulo=gastos&accion=crearQR&Content-Type=text/json",
        data: datos,
        dataType: "json",
        success: function (result) {
            $("#qrxml").attr("src", result.url + "?" + Math.random());
        },
        error: function (result) {
            Seguridad.bitacora("", 5, datos.idproyecto + "," + datos.idinforme + "," + datos.idgasto, "Error: Crear QR del XML. " + JSON.stringify(result), 0);
        }
    });
}

function tdtotales(total, totalmonto, totalaceptable, totalnoaceptable, totalnodeducible) {
    var newtd = "";
    newtd += "<tr><td id='tdcolsdatgasto' colspan='4'></td>";
    newtd += "<td align='right'>Total:</td>";
    newtd += "<td align='right'><span id='lbltotalg'>" + formatNumber.new((total * 1).toFixed(2), "$ ") + "</span></td>";
    //newtd += "<td></td>";
    newtd += "<td id='tdcolstotales' colspan='3'></td>";// se reduce de 4 a 3 para ocultar forma de pago
    newtd += "<td align='right'><span id='lblmontog'>" + formatNumber.new((totalmonto * 1).toFixed(2), "$ ") + "</span></td>";
    newtd += "<td align='right'>" + formatNumber.new(totalaceptable.toFixed(2), "$ ") + "</td>";
    newtd += "<td align='right'>" + formatNumber.new(totalnoaceptable.toFixed(2), "$ ") + "</td>";
    newtd += "<td align='right'>" + formatNumber.new(totalnodeducible.toFixed(2), "$ ") + "</td>";
    newtd += "<td></td>";
    newtd += "</tr>";

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


    var FormaPagoSelect = document.getElementById("formapago" + id);
    var formapago = FormaPagoSelect.options[FormaPagoSelect.selectedIndex].text;



    var c = $("#categoria" + id).val();
    var categoria = c;

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
        "observaciones": $("#observaciones" + id).val()
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
                selectInforme(result[0]['idinforme'], result[0]['idproyecto'], 0);
            }

            var gorigen = $("#gorigen" + id).val() * 1;
            if (origen === "txtTotal" && valorVacio(xml) && gorigen === 0) {
                $("#monto" + id).empty();
                $("#monto" + id).append(formatNumber.new(total.toFixed(2), "$ "));
                //obtenerGastosInforme(result[0]['idinforme'], result[0]['idproyecto'], estatusinf);
            }

        },
        complete: function () {
            //cargado();
            Seguridad.bitacora("UpdateGasto", 5, idproyecto + "," + idinforme + "," + id, "Se actualizo gasto.", 1);
            //actualiza lista de informes
            if (origen === "txtTotal" || origen === "txtFecha") {
                ObtenerInformes();
            }

            if (origen === "selectfp") {
                validaFPdeGastoyFactura(id, formapago);
            }
        },
        error: function (result) {
            //cargado();
            $.notify("Error al Guardar", { globalPosition: 'top center', className: 'error' });
            Seguridad.bitacora("UpdateGasto", 5, idproyecto + "," + idinforme + "," + id, "Error: actualizar gasto. " + JSON.stringify(result), 0);
        }
    });
}

function recuperagastoxml(id, idinforme, idproyecto, xml, pdf, otros, estatusinf, origen, bitxml) {

    var datos = {
        "id": id,
        "idinforme": idinforme,
        "idproyecto": 0,
        "dir": bitxml
    };

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

            ////if (result[0]["formatovalido"] === 1) {
            ////    if (result[0]["xml"] === 1 && result[0]["vxml"] !== 0) {

            ////        error = 1;
            ////    }
            ////    if (result[0]["vxml"] === 0 || result[0]["xmlvigente"] === "Cancelado") {

            ////        error = 1;
            ////    }

            ////    if (result[0]["xmlvigente"] === "NoValidado") {

            ////        error = 1;
            ////    }

            ////    if (result[0]["xmlvigente"] === "rfcNoValido") {

            ////        error = 1;
            ////    }
            ////} else {

            ////    error = 1;
            ////}
            $("#filexml" + id).filestyle('clear');

            if (error === 0) {
                //if (!valorVacio(fpgasto)) {
                //    if (fpgasto !== fpfactura) {
                //        $.notify("El Método o Forma de Pago de la Factura no es igual al del Gasto.", { globalPosition: 'top center', className: 'error', autoHideDelay: 8000 });
                //    }
                //}
                $.notify("Comprobante XML cargado y gasto Actualizado.", { globalPosition: 'top center', className: 'success', autoHideDelay: 6000 });
                //if (result[0]["xmlvigente"] !== "") {
                consultaInfoGastos(idinforme, 3, estatusinf, 0);

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
    var formapagogasto = $("#formapago" + id).val() * 1;

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
            //console.log("success", result);
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
            obtenerGastosInforme(idinforme, idproyecto, 3);
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
    var menucategoria = menucategorias();
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

function menuFormaPago() {
    var catDefault = JSON.parse(localStorage.getItem("default"));

    var menu = "";
    menu = "<select id='formapago' name='formapago' data-width='auto' onchange style='height:32px; width:100px'>";
    menu += "<option value='0'>- Forma de Pago -</option>";

    $("#formapago").empty();
    $("#formapago").append("<option value='0'>- Forma de Pago -</option>");
    $("#formapago").append("<option value='1'>Efectivo</option>");
    menu += "<option value='1'>Efectivo</option>";
    var tarjeta = catDefault.GrEmpTarjetaToka;
    var option = "";
    if (!valorVacio(datoEle(tarjeta))) {
        menu += "<option value='2'>" + + tarjeta + "</option>";
        option += "<option value='2'>" + tarjeta + "</option>";
    }

    $("#formapago").append(option);

    menu += "</select>";
    return menu;
    /*var menu = "";
    menu = "<select id='formapago' name='formapago' data-width='auto' onchange style='height:32px; width:100px'>";
    menu += "<option value='0'>- Forma de Pago -</option>";

    var Empleado = localStorage.getItem("cosa2");

    var datos = {
        "GrEmpID": Empleado
    };

    $("#formapago").empty();
    var i = 0;
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
    });
    menu += "</optgroup>";
    menu += "</select>";
    return menu;*/
}

function menucategorias() {

    var menu = "";
    menu = "<select id='categoria' name='categoria' class='menu-categoria' data-width='auto' onchange style='height:32px; width:120px'>";
    menu += "<option value='0' data-informacion=''>- Categoria -</option>";

    var IdRequisicion = document.getElementById("HFRmRdeRequisicion").value;
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
            $("#categoria").append("<option value='0' data-informacion=''>- Categoria -</option>");
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
        },
        error: function (result) {
            console.log(result);
        }
    });
    menu += "</select>";
    return menu;
}
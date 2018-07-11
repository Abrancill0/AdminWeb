var tabla = "", tabla2 = "";
var PCentros = [];// Seguridad.permisos(3);
var UsuarioActivo = localStorage.getItem("cosa");
$(function () {
    try {
        cargaInicialConfrontacion();
    } catch (err) {
        console.log("Input is ", err);
        setTimeout(cargaInicialConfrontacion, 100);
    }
});

function cargaInicialConfrontacion() {
    if (pagina === "carga") {
        $("#filebanco:file").filestyle({
            input: false,
            buttonName: "btn-success",
            buttonText: "&nbsp; Cargar Movimientos*"
        });
    } else if (pagina === "depuracion") {
        //rango de fechas
        rangoFechas();

        var fechafin = fechaActual();
        var fechaini = fechafin.split("-")
        fechaini = FechaMasMenos("01-" + fechaini[1] + "-" + fechaini[2], 2, "m", "-");

        $("input#repde").val(fechaini);
        $("input#repa").val(fechafin);

        tabla = crearTablaDetalleRow("#tblDepuracionMovBanco", 0, "desc", 11, false);

        ObtenerMovBancoDepuracion();
    } else if (pagina === "confrontacion") {
        ObtenerUsuarios();
        ObtenerInformesSinConfrontar();
        tabla = crearTablaDetalleRow("#tblGastosInforme", 0, "asc", 10, false);
        tabla2 = crearTablaReportes("#tblMovSinGasto", 0, "desc", false, '');
    }
}
/*
 * carga
 */
function cargaBanco() {
    //var data = new FormData($("#filebanco").parents('form')[0]);
    cargando();
    var banco = $("#banco").val();
    if (!valorVacio(banco)) {
        var file = $("#filebanco").get(0).files[0];
        var r = new FileReader();
        var nombre = file.name;
        var extFile = (nombre.substring(nombre.lastIndexOf(".") + 1)).toLowerCase();
        if (extFile === "xlsx") {
            r.onload = function () {
                var binimage = r.result;
                nombre = nombre.replace("." + extFile, "");
                guardarExcelBanco(banco, nombre, extFile, binimage);
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
function guardarExcelBanco(banco, nombre, extFile, binimage) {
    var datos = {
        'Usuario': encriptaDesencriptaEle(UsuarioActivo, 0),
        'ArchivoNmb': nombre,
        'ArchivoExt': extFile,
        'Archivo': binimage
    };
    var tablaMov = ""
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
                                Tarjeta = value.Tarjeta;
                                Descripcion = value.Descripcion;
                                Fecha = ((value.Fecha).split(" "))[0];
                                Importe = value.Importe;
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
                                    spanDuplicado,
                                    spanFechaDuplicado,
                                    spanChk
                                ];
                                tablaMov.row.add(newRow).draw(false);
                                i++;
                            }                            
                        });
                        "#tdTarjeta".AsHTML(Tarjeta);
                    }
                    $("#tblMovimientos").append("</tbody>");

                    $("#MovBanco").modal({
                        show: true,
                        keyboard: false,
                        backdrop: "static"
                    });



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
    if (elementosSel === false)
        $.notify("Seleccionar movimiento(s).", { position: "top center", className: 'error' });
    else
        $.notify("Información guardada.", { position: "top center", className: 'success' });
});
function guardarMBanco(datos) {
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
/*
 * depuracion
 */
$("#btnBuscar").click(function () {
    ObtenerMovBancoDepuracion();
});
function ObtenerMovBancoDepuracion() {
    var datos = {
        'FechaIni': $("#repde").val(),
        'FechaFin': $("#repa").val()
    };
    var TImporte = 0, TTotal = 0;
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaDepuracionMovBanco',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cargando();
            tabla
                .clear()
                .draw();
            $('#tblDepuracionMovBanco tfoot').empty();
        },
        success: function (result) {
            $.each(result, function (key, value) {
                var Ninforme = (value.Ninforme === "0") ? "" : value.Ninforme;
                var btnEli = "";
                var nmbmovimiento = value.Fmovimiento + " / " + value.Observaciones;
                btnEli = "<button type='button' class='btn btn-danger btn-sm' onclick='confEliminarMovBanco(" + value.Id + ", " + value.Idinforme + ", " + value.Idgasto + ", \"" + nmbmovimiento + "\")'><span class='glyphicon glyphicon-trash'></span> Eliminar</button>";
                var Importe = value.Importe;
                var Total = value.Total;
                TImporte += Importe;
                TTotal += Total;
                tabla.row.add([
                    "",
                    value.Banco,
                    value.Tarjeta,
                    value.Tipomovimiento,
                    value.Fmovimiento,
                    value.Observaciones,
                    formatNumber.new(Importe.toFixed(2), "$ "),
                    Ninforme + " - " + value.NmbInf,
                    value.Concepto + " / " + value.Negocio,
                    formatNumber.new(Total.toFixed(2), "$ "),
                    btnEli,
                    value
                ]).draw(false);
            });
        },
        complete: function () {
            cargado();
            $("#tblDepuracionMovBanco tbody tr").each(function () {
                $(this)[0].cells[6].className = "text-right";
                $(this)[0].cells[9].className = "text-right";
            });
            var newtd = "<tr><td colspan='6' align='right'>Total:</td>";
            newtd += "<td align='right'>" + formatNumber.new(TImporte.toFixed(2), "$ ") + "</td>";
            newtd += "<td colspan='2'></td>";
            newtd += "<td align='right'>" + formatNumber.new(TTotal.toFixed(2), "$ ") + "</td>";
            newtd += "<td></td></tr>";
            $('#tblDepuracionMovBanco tfoot').append(newtd);
        },
        error: function (result) {
            cargado();
            console.log("error", result);
        }
    });
}
$('#tblDepuracionMovBanco tbody').on('click', 'td.details-control', function () {
    var tr = $(this).closest('tr');
    var row = tabla.row(tr);
    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
        tr.removeClass('shown');
    } else {
        // Open this row
        var datos = row.data()[11];
        row.child(verPanelDetalleMovBanco(datos)).show();
        tr.addClass('shown');
    }
});
function verPanelDetalleMovBanco(datos) {
    var panel = "<div id='PDMB" + datos.Id + "' class='card-demo'>";
    panel += "<div class='card'>";
    panel += "<div class='card-header'>";
    panel += "<h2 class='card-title'>Movimiento Bancario</h2>";
    panel += "<div class='actions'>";
    panel += "<a href='#' onclick='imprimirDetalleMovBanco(\"#PDMB" + datos.Id + "\")' class='actions__item zmdi zmdi-print'></a>";
    panel += "</div>";
    panel += "</div>";

    panel += "<div class='card-block'>";

    panel += "<blockquote class='blockquote' style='width: 100%; font-size:12px;'>";

    panel += "<div class='pmbb-view mb-0'>";
    panel += conceptoDetalleMovBanco("Banco:", datos.Banco);
    panel += conceptoDetalleMovBanco("No. Tarjeta:", datos.Tarjeta);
    panel += conceptoDetalleMovBanco("Embosado:", datos.Embosado);
    panel += conceptoDetalleMovBanco("Nombre:", datos.Nombre);
    panel += conceptoDetalleMovBanco("Nomina:", datos.Nomina);
    panel += conceptoDetalleMovBanco("Fecha:", datos.Fmovimiento);
    panel += conceptoDetalleMovBanco("Observaciones:", datos.Observaciones);
    panel += conceptoDetalleMovBanco("Importe:", formatNumber.new(datos.Importe.toFixed(2), "$ "));
    panel += "<hr />"
    var Total = datos.Total;
    if (Total > 0) {
        var Ninforme = (datos.Ninforme === "0") ? "" : datos.Ninforme;
        panel += conceptoDetalleMovBanco("Informe:", Ninforme + " / " + datos.NmbInf);
        panel += conceptoDetalleMovBanco("Gasto:", datos.Concepto + " - " + datos.Negocio);
        panel += conceptoDetalleMovBanco("Categoria:", datos.NombreCategoria);
        panel += conceptoDetalleMovBanco("Fecha Gasto:", datos.Fgasto);
        panel += conceptoDetalleMovBanco("Importe:", formatNumber.new(Total.toFixed(2), "$ "));
    }
    panel += "</div>";

    panel += "</blockquote>";

    panel += "</div>";

    panel += "</div>";
    panel += "</div>";

    return panel;
}
function conceptoDetalleMovBanco(concepto, descripcion) {
    var panel = "<dl class='dl-horizontal'>";
    panel += "<dt>" + concepto + "</dt><dd>" + descripcion + "</dd>";
    panel += "</dl>";
    return panel;
}
function imprimirDetalleMovBanco(area) {
    $(area).printArea({
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
function confEliminarMovBanco(id, idinforme, idgasto, nombre) {
    var datos = { 'IdMovBanco': id, 'IdInforme': idinforme, 'IdGasto': idgasto };
    var botones = [];
    botones[0] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            var resultado = eliminarMovBanco(datos);
            var estatus = "error";
            if (resultado.EliminadoOk === true) {
                ObtenerMovBancoDepuracion();
                estatus = "success";
            }
            $.notify(resultado.Descripcion, { position: "top center", className: estatus });//, autoHideDelay: 2000 
        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Eliminar Movimiento:<br /><b>" + nombre + "</b>?", botones, " Eliminar Movimiento Bancario.");

}
function eliminarMovBanco(datos) {
    var resultado = {};
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/EliminaMovBanco',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        success: function (result) {
            resultado = { 'EliminadoOk': result.EliminadoOk, 'Descripcion': result.Descripcion };
        },
        error: function (result) {
            console.log(result);
            resultado = { 'EliminadoOk': false, 'Descripcion': "Error al eliminar movimiento." };
        }
    });
    return resultado;
}
/*
 * confrontacion
 */
function ObtenerUsuarios() {
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaUsuariosConInforme',
        //data: JSON.stringify({ 'Usuario': usuario }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargando();
            $("#usuario").empty();
            $("#usuario").append("<option value=''> - Usuario - </option>");
        },
        success: function (result) {
            $.each(result, function (key, value) {
                var Usuario = value.Usuario;
                var Nombre = value.Nombre;
                var option = "<option value='" + Usuario + "'>" + Nombre + " (" + Usuario + ")</option>";
                $("#usuario").append(option);
            });
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
        }
    });
}
$("#usuario").change(function () {
    ObtenerInformesSinConfrontar();
});
function ObtenerInformesSinConfrontar() {
    var usuario = $("#usuario").val();
    usuario = valorVacio(usuario) ? "" : usuario;
    var datos = { 'Usuario': usuario };
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaInformesSinConfrontar',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargando();
            $("#informe").empty();
        },
        success: function (result) {
            $.each(result, function (key, value) {
                var IdInforme = value.IdInforme;
                var NmbInforme = value.NmbInforme;
                var NoInforme = value.NoInforme;
                var option = "<option value='" + IdInforme + "'>" + NoInforme + " - " + NmbInforme + "</option>";
                $("#informe").append(option);
            });
        },
        complete: function () {
            $("#informe").select2();
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
        }
    });
}
$("#informe").change(function () {
    SelectInformeParaConfrontar();
});
function SelectInformeParaConfrontar() {
    tabla
        .clear()
        .draw();
    cargando();
    var informes = $("#informe").val();
    if (informes.length > 0) {
        var ninformes = informes.length;
        for (var i = 0; i < ninformes; i++) {
            BrowseInformeParaConfrontar(informes[i]);
        }
    }
    cargado();
}
function BrowseInformeParaConfrontar(idinforme) {
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaInformeParaConfrontar',
        data: JSON.stringify({ 'IdInforme': idinforme }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargando();
        },
        success: function (result) {
            if (!valorVacio(result)) {
                $.each(result, function (key, value) {
                    var NoInforme = value.NoInforme;
                    var IdInforme = value.IdInforme;
                    var IdGasto = value.IdGasto;
                    var IdMovBanco = value.IdMovBanco;
                    var Concepto = value.Concepto;
                    var Negocio = value.Negocio;
                    var FormaPago = "T.Toka: " + value.FormaPago;
                    var FechaGasto = value.FechaGasto;
                    var Monto = value.Monto;
                    var EnBanco = value.EnBanco;
                    var LblEnBanco = SiNo(EnBanco);
                    var BtnAsignaMB = "";
                    var ID = IdInforme + "_" + IdGasto;
                    value['ID'] = ID;

                    if (EnBanco === 0)
                        BtnAsignaMB = "<button type='button' class='btn btn-success btn-sm' onclick='MovSinGasto(" + JSON.stringify(value) + ")'><i class='zmdi zmdi-file-plus zmdi-hc-2x'></i> Mov´s Banco</button>";

                    var inpChk = chk("ConGasto" + ID, "ConGasto", "checked", "", JSON.stringify({ 'IdInforme': IdInforme, 'IdGasto': IdGasto, 'IdMovBanco': IdMovBanco }), "18", "success", "danger", "");
                    var newRow = ["",
                        NoInforme,
                        Concepto,
                        Negocio,
                        FormaPago,
                        FechaGasto,
                        formatNumber.new(Monto.toFixed(2), "$ "),
                        LblEnBanco,
                        BtnAsignaMB,
                        inpChk,
                        value
                    ];
                    tabla.row.add(newRow).draw(false);
                });
            }
        },
        complete: function () {
            $("#tblGastosInforme tbody tr").each(function () {
                $(this)[0].cells[6].className = "text-right";
            });
        },
        error: function (result) {
            //cargado();
            console.log("error", result);
        }
    });
}
$('#tblGastosInforme tbody').on('click', 'td.details-control', function () {
    var tr = $(this).closest('tr');
    var row = tabla.row(tr);
    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
        tr.removeClass('shown');
    } else {
        // Open this row
        var datos = row.data()[10];
        row.child(verPanelDetalleGastoMovBanco(datos)).show();
        tr.addClass('shown');
    }
});
$("#tblGastosInforme").on("click, mouseenter", "tbody tr", function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    } else {
        $('#tblGastosInforme tbody tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});
function verPanelDetalleGastoMovBanco(datos) {
    //console.log(datos);
    var panel = "<div id='PDGMB" + datos.ID + "' class='card-demo'>";
    panel += "<div class='card'>";
    panel += "<div class='card-header'>";
    panel += "<h2 class='card-title'>Detalle Movimiento Gasto-Banco</h2>";
    panel += "<div class='actions'>";
    panel += "<a href='#' onclick='imprimirDetalleMovBanco(\"#PDGMB" + datos.ID + "\")' class='actions__item zmdi zmdi-print'></a>";
    panel += "</div>";
    panel += "</div>";

    panel += "<div class='card-block'>";
    panel += "<h6>Gasto</h6>";
    panel += "<table class='table' style='margin: 0px; padding: 0px'>";
    panel += "<thead>";
    panel += "<tr><th>Informe</th><th>Concepto</th><th>Negocio</th><th>Fecha</th><th>Importe</th></tr>";
    panel += "</thead>";
    panel += "<tbody>";
    panel += "<tr>";
    panel += "<td>" + datos.NoInforme + " - " + datos.NmbInforme + "</td>";
    panel += "<td>" + datos.Concepto + "</td>";
    panel += "<td>" + datos.Negocio + "</td>";
    panel += "<td>" + datos.FechaGasto + "</td>";
    panel += "<td>" + formatNumber.new((datos.Monto).toFixed(2), "$ ") + "</td>";
    panel += "</tr>";
    panel += "</tbody>";
    panel += "</table>";

    if (datos.IdMovBanco > 0) {
        panel += "<hr />";
        panel += "<h6>Movmiento Bancario</h6>";
        panel += "<table class='table' style='margin: 0px; padding: 0px'>";
        panel += "<thead>";
        panel += "<tr><th>Banco</th><th>Tarjeta</th><th>Observaciones</th><th>Fecha</th><th>Importe</th></tr>";
        panel += "</thead>";
        panel += "<tbody>";
        panel += "<tr>";
        panel += "<td>" + datos.Banco + "</td>";
        panel += "<td>" + datos.Tarjeta + "</td>";
        panel += "<td>" + datos.ObservacionesMovimiento + "</td>";
        panel += "<td>" + datos.FechaMovimiento + "</td>";
        panel += "<td>" + formatNumber.new((datos.ImporteMovimiento).toFixed(2), "$ ") + "</td>";
        panel += "</tr>";
        panel += "</tbody>";
        panel += "</table>";
    }

    panel += "</div>";

    panel += "</div>";
    panel += "</div>";

    return panel;
}
$("#confrontarInforme").click(function () {
    var informes = $("#informe").val();
    //console.log($("#informe"), $("#informe").text());
    if (informes.length > 0) {
        /*var ninformes = informes.length;
        for (var i = 0; i < ninformes; i++) {
            
        }*/
        confConfrontarInforme();
    } else {
        $.notify("Seleccionar uno o más informes.", { position: "top center", className: 'error' });
    }
});
function confConfrontarInforme() {
    var botones = [];
    botones[0] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            cargando();
            $("input:checkbox[name=ConGasto]").each(function () {
                var chkOk = 0;
                if ($(this).is(':checked'))
                    chkOk = 1;

                var datos = JSON.parse($(this).val());
                datos['ChkOk'] = chkOk;
                confrontarInforme(datos);
            });
            cargado();
            ObtenerUsuarios();
            ObtenerInformesSinConfrontar();
            tabla
                .clear()
                .draw();
        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Confrontar Informe(s)?", botones, " Confrontar Informe.");
}
function confrontarInforme(datos) {
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConfrontarGastoInforme',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            //console.log(result)
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
function MovSinGasto(datos) {

    var importe = datos.Monto;
    var fecha1 = datos.FechaGasto;
    fecha1 = fecha1.split("/");
    var fecha = fecha1[2] + "-" + fecha1[1] + "-" + fecha1[0];

    $("#msnmb").empty();
    $("#msnmb").append("<table class='table table-bordered'>" +
        "<thead><tr><th># Informe</th><th>Concepto</th><th>Negocio</th><th>Fecha</th><th>Importe</th></tr></thead>" +
        "<tbody><tr><td>" + datos.NoInforme + "</td><td>" + datos.Concepto + "</td><td>" + datos.Negocio + "</td>" +
        "<td>" + formatFecha(fecha, 'dd mmm yyyy') + "</td><td align='right'>" + formatNumber.new(importe.toFixed(2), "$ ") + "</td></tr></tbody>" +
        "</table>");

    rangoFechas("repde2", "repa2", "reporte2", "BuscarMovSinGasto(" + importe + ", '" + fecha + "', '" + datos.FormaPago + "', " + datos.IdInforme + ", " + datos.IdGasto + ", 'fecha')");

    var fechafin = FechaMasMenos(formatFecha(fecha, 'dd-mm-yyyy'), 1, "m", "+");
    var fechaini = formatFecha(fecha, 'dd-mm-yyyy').split("-");
    fechaini = FechaMasMenos("01-" + fechaini[1] + "-" + fechaini[2], 1, "m", "-");
    $("input#repde2").val(fechaini);
    $("input#repa2").val(fechafin);

    var importemin = 0;
    var importemax = 0;
    var importe2 = 0;
    if (importe < 0) {
        importe2 = Math.abs(importe);
        importemin = importe * 2;
        importemax = importe2 * 2;
    } else if (importe === 0) {
        importemin = 0;
        importemax = 100;
    } else {
        importemin = 0;
        importemax = importe * 2;
    }

    var valmin = importe < 0 ? importe : 0;
    
    $("#importede").val(Math.ceil(importemin));
    $("#importea").val(Math.ceil(importemax));

    BuscarMovSinGasto(importe, fecha, datos.FormaPago, datos.IdInforme, datos.IdGasto, 'automatica');

    $("#MovBanSinGasto").modal({
        show: true,
        keyboard: false,
        backdrop: "static"
    });
}
function BuscarMovSinGasto(importe, fecha, tarjeta, IdInforme, IdGasto, origen) {
    var repde = $("#repde2").val();
    var repa = $("#repa2").val();
    var importede = $("#importede").val() * 1;
    var importea = $("#importea").val() * 1;

    if (valorVacio(tarjeta) || valorVacio(importe) || valorVacio(fecha) ||
        valorVacio(IdInforme) || valorVacio(IdGasto)) {
        var vdefault = JSON.parse($("#importede").attr("valoresDefault"));
        tarjeta = vdefault.Tarjeta;
        importe = vdefault.Importe;
        fecha = vdefault.FGasto;
        IdInforme = vdefault.IdInforme;
        IdGasto = vdefault.IdGasto;
    }

    if (importede > importea) {
        if (origen === "importede") {
            $("#importea").val(importede);
            importea = $("#importea").val() * 1;
        }
        else {
            $("#importede").val(importea);
            importede = $("#importede").val() * 1;
        }            
    }

    repde = repde.split("-");
    repa = repa.split("-");

    var repde1 = repde[2] + "-" + repde[1] + "-" + repde[0];
    var repa1 = repa[2] + "-" + repa[1] + "-" + repa[0];

    var datos = {
        'RepDe': repde1,
        'RepA': repa1,
        'ImporteDe': importede,
        'ImporteA': importea,
        'Importe': importe,
        'FGasto': fecha,
        'Tarjeta': tarjeta,
        'IdInforme': IdInforme,
        'IdGasto': IdGasto
    };
    $("#importede, #importea").attr("valoresDefault", JSON.stringify(datos));
    tabla2
        .clear()
        .draw();
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultarMovSinGasto',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargando();
        },
        success: function (result) {
            var i = 1;
            $.each(result, function (key, value) {
                var checked = i === 1 ? "checked" : "";
                var inpRadio = "<input type='radio' id='movBanco" + value.IdMovBanco + "' name='movimiento-banco' onchange='selectInputRadio(this)' value='" + JSON.stringify(value) + "' autocomplete='off' " + checked + ">";
                var newRow = [inpRadio.TdLabelMovBan("movBanco" + value.IdMovBanco),
                    value.Banco.TdLabelMovBan("movBanco" + value.IdMovBanco),
                    value.Tarjeta.TdLabelMovBan("movBanco" + value.IdMovBanco),
                    value.Descripcion.TdLabelMovBan("movBanco" + value.IdMovBanco),
                    value.Fecha.TdLabelMovBan("movBanco" + value.IdMovBanco),
                    formatNumber.new((value.Importe).toFixed(2), "$ ").TdLabelMovBan("movBanco" + value.IdMovBanco)
                ];
                tabla2.row.add(newRow).draw(false);
                if (i === 1) {
                    $("#tblMovSinGasto tbody tr:has(#movBanco" + value.IdMovBanco + ")").addClass("selected");
                }
                
                i++;
            });
        },
        complete: function () {
            $("#tblMovSinGasto tbody tr").each(function () {
                try {
                    $(this)[0].cells[5].className = "text-right";
                } catch (err) {
                }
            });
        },
        error: function (result) {
            console.log(result);
        }
    });
}
function selectInputRadio(elemento) {
    var datos = JSON.parse($(elemento).val());

    $("#tblMovSinGasto tbody tr:odd").css("background-color", "#e0e0e0"); 
    $("#tblMovSinGasto tbody tr:even").css("background-color", "#ffffff"); 

    $('#tblMovSinGasto tbody tr.selected').removeClass('selected');
    $("#tblMovSinGasto tbody tr:has(#movBanco" + datos.IdMovBanco + ")").addClass("selected");

    if (datos.Fecha !== datos.FechaGasto &&
        (datos.Importe).toFixed(2) !== (datos.ImporteGasto).toFixed(2)) {
        $("#tblMovSinGasto tbody tr:has(#movBanco" + datos.IdMovBanco + ")").notify("El importe y fecha del gasto son diferentes al importe y fecha del movmiento.", { position: "top", className: 'error' });
    } else if (datos.Fecha !== datos.FechaGasto &&
        (datos.Importe).toFixed(2) === (datos.ImporteGasto).toFixed(2)) {
        $("#tblMovSinGasto tbody tr:has(#movBanco" + datos.IdMovBanco + ")").notify("La fecha del gasto es diferente a la fecha del movmiento.", { position: "top", className: 'error' });
    } else if (datos.Fecha === datos.FechaGasto &&
        (datos.Importe).toFixed(2) !== (datos.ImporteGasto).toFixed(2)) {
        $("#tblMovSinGasto tbody tr:has(#movBanco" + datos.IdMovBanco + ")").notify("El importe del gasto es diferente al importe del movmiento.", { position: "top", className: 'error' });
    }
}
String.prototype.TdLabelMovBan = function (radio) {
    var dato = this.toString();
    return "<label for='" + radio + "' style='width:100%; height:20px; margin:0px; padding: 5px 0px; font-weight:bold'>" + dato + "</label>";

};
$("#btnAsignarMovBanGasto").click(function () {
    var datos = [];
    try {
        datos = JSON.parse($("input[type=radio][name='movimiento-banco']:checked").val());
    } catch (err) {
        datos = [];
        $.notify("Seleccionar un movmiento", { position: "top center", className: "error" });
        return false;
    }
    
    var botones = [];
    botones[0] = {
        text: "Si", click: function () {
            $(this).dialog("close");
            if (valorVacio(datoEle(datos.IdMovBanco))) {
                $.notify("Seleccionar un movmiento", { position: "top center", className: "error" });
            } else {
                relacionaGastoMovBanco(datos);
            }
        }
    };
    botones[1] = {
        text: "No", click: function () {
            $(this).dialog("close");
        }
    };
    Seguridad.confirmar("Relacionar el gasto con el movimiento seleccionado?", botones, " Rrelacionar.", "#tblMovSinGasto");
});
function relacionaGastoMovBanco(datos) {
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/RelacionaGastoMovBanco',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargando();
            $("#MovBanSinGasto").modal('hide');
        },
        success: function (result) {
            console.log(result);
            var tipomsn = result.RelacionOk === true ? "success" : "error";
            $.notify(result.Descripcion, { position: "top center", className: tipomsn });
        },
        complete: function () {
            SelectInformeParaConfrontar();
        },
        error: function (result) {
        }
    });
}



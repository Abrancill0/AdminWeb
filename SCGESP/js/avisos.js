var tabla = "";
var PCentros = [];// Seguridad.permisos(3);
var UsuarioActivo = localStorage.getItem("cosa");
var EmpeladoActivo = localStorage.getItem("cosa2");
$(".ui-dialog-titlebar-close").addClass("ui-icon-close");

$(function () {
    try {
        cargaInicialAvisos();
    } catch (err) {
        console.log("Input is ", err);
        setTimeout(cargaInicialAvisos, 100);
    }
});

function cargaInicialAvisos() {
    browseAvisos();
    avisosToolBar();
    //avisosNotificaciones();
}

function browseAvisos() {
    var resultado = ObtenerAvisos();
    $("#msnAvisos").empty();
    $("#verMsn").attr("msnnuevos", "").removeClass("top-nav__notify");
    var listAvisos = [];
    var i = 0;
    if (resultado.nrow > 1) {
        $.each(resultado.lista, function (key, value) {
            if (i < 20) {
                var row = newRowCentro(value);
                listAvisos[i] = value.ProcesoNombre;
                i++;
                $("#msnAvisos").append(row);
                $("#verMsn").addClass("top-nav__notify").attr("msnnuevos", i);
            }
        });
    } else if (resultado.nrow === 1) {
        var row = newRowCentro(resultado.lista);
        listAvisos[i] = resultado.lista.ProcesoNombre;
        $("#msnAvisos").append(row);
        $("#verMsn").addClass("top-nav__notify").attr("msnnuevos", 1);
    } else {
        //sin avisos
        $("#verMsn").attr("msnnuevos", "").removeClass("top-nav__notify");
    }

    localStorage.setItem("misavisos", JSON.stringify(listAvisos));
}

function avisosToolBar() {
    if (!valorVacio(localStorage.getItem("misavisos"))) {
        var listAvisos = JSON.parse(localStorage.getItem("misavisos"));
        var nAvisos = listAvisos.length;
        var i = 0;
        var intervaloNot = setInterval(function () {
            if (nAvisos > 0) {
                if (i < nAvisos) {
                    if (nAvisos === 1) {
                        $("#notifToolBar").empty().append(listAvisos[0]);
                        clearTimeout(intervaloNot);
                    } else {
                        $("#notifToolBar").empty().append(listAvisos[i]);
                        i++;
                    }
                } else {
                    $("#notifToolBar").empty().append(listAvisos[0]);
                    i = 0;
                }
            } else {
                $("#notifToolBar").empty().append("<b>Notificaciones</b>");
                clearTimeout(intervaloNot);
            }
        }, 5000);
    }
}

function avisosNotificaciones() {
    setTimeout(function () {
        if (!valorVacio(localStorage.getItem("misavisos"))) {
            var listAvisos = JSON.parse(localStorage.getItem("misavisos"));
            var nAvisos = listAvisos.length;
            var listAvisos = JSON.parse(localStorage.getItem("misavisos"));
            var nAvisos = listAvisos.length;
            for (var i = 0; i < nAvisos; i++) {
                $.notify(listAvisos[i], { position: "bottom right", className: 'info' });
            }
        }
    }, 300);
}

function newRowCentro(datos) {
    var row = "";
    row += "<div class='listview__content aviso-noleido' style='margin: 5px 5px; padding:5px;'>";
    row += "<div class='listview__heading'><b>" + datos.ProcesoNombre + "</b></div>";
    row += "<p>Registros: <b>" + datos.Registros + "</b></p>";
    row += "</div>";
    return row;
}

function ObtenerAvisos() {
    var resultado = {};
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaPendientesUsuario',
        data: JSON.stringify({ 'Usuario': UsuarioActivo, 'Empleado': EmpeladoActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            try {
                var exito = result.Salida.Resultado * 1;
                if (exito === 1) {
                    resultado["lista"] = result.Salida.Tablas.Pendientes.NewDataSet.Pendientes;
                    var nrow = datoEle(resultado.lista.length) * 1;
                    nrow = nrow === 0 ? 1 : nrow;
                    resultado["nrow"] = nrow;
                } else {
                    $.notify("Error: Al consultar Pendientes.", { globalPosition: 'top center', className: 'error' });
                }
            } catch (err) {
                resultado["nrow"] = 0;
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
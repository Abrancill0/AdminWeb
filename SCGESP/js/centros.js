var tabla = "";
var PCentros = [];// Seguridad.permisos(3);
var UsuarioActivo = localStorage.getItem("cosa");
$(function () {
    try {
        cargaInicialCentros();
    } catch (err) {
        console.log("Input is ", err);
        setTimeout(cargaInicialCentros, 100);
    }
});

function cargaInicialCentros() {
    tabla = crearTabla("#tblCentros", 0, "desc");
    ObtenerCatalogoCentros();
}

$("#refreshTbl").click(function () {
    ObtenerCatalogoCentros();
});

function ObtenerCatalogoCentros() {
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaCatalogoCentros',
        data: JSON.stringify({ 'Usuario': UsuarioActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cargado();
        },
        success: function (result) {
            //console.log("success", result);
            var exito = result.Salida.Resultado * 1;
            if (exito === 1) {
                tabla
                    .clear()
                    .draw();
                var resultado = result.Salida.Tablas.Catalogo.NewDataSet.Catalogo;
                var nsubramos = 0;
                try {
                    nsubramos = resultado.length;
                } catch (err) {
                    nsubramos = 0;
                }
                if (nsubramos > 0) {
                    $.each(resultado, function (key, value) {
                        tabla.row.add(newRowCentro(value)).draw(false);
                    });
                } else {
                    tabla.row.add(newRowCentro(resultado)).draw(false);
                }
            } else {
                $.notify("Error: Al consultar Centros.", { globalPosition: 'top center', className: 'error' });
            }

        },
        complete: function () {
            cargado();
        },
        error: function (result) {
            cargado();
            console.log("error", result);
        }
    });
}

$("#tblCentros").on("click, mouseenter", "tbody tr", function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    } else {
        $('#tblCentros tbody tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});

function newRowCentro(datos) {
    var FiCenId = datoEle(datos.FiCenId);
    var FiCenNombre = datoEle(datos.FiCenNombre);
    var FiCenNivel = datoEle(datos.FiCenNivel);
    var FiCenNivelNombre = datoEle(datos.FiCenNivelNombre);
    var btnVer = "<button type='button' onclick='verCentro(" + JSON.stringify(datos) + ")' class='btn btn-success btn-sm'><span class='glyphicon glyphicon-eye-open'></span> Ver</button>";
    var newrow = [
        FiCenId,
        FiCenNombre,
        FiCenNivel,
        FiCenNivelNombre,
        btnVer
    ];
    return newrow;
}

function verCentro(datos) {
    cargado();
    $("#FiCenId, #FiCenNombre, #FiCenMontoMinimo, #FiCenNivel, #FiCenNivelNombre, " +
        " #FiCenResponsableNombre, #FiCenCentroSuperior, #FiCenCentroSuperiorNombre").empty();
    
    var FiCenMontoMinimo = datoEle(datos.FiCenMontoMinimo) * 1;
    var MontoMinimo = "";
    if (FiCenMontoMinimo > 0)
        MontoMinimo = formatNumber.new(FiCenMontoMinimo.toFixed(2), "$ ");

    "#FiCenId".AsHTML(datoEle(datos.FiCenId));
    "#FiCenNombre".AsHTML(datoEle(datos.FiCenNombre));
    "#FiCenMontoMinimo".AsHTML(MontoMinimo);
    "#FiCenNivel".AsHTML(datoEle(datos.FiCenNivel));
    "#FiCenNivelNombre".AsHTML(datoEle(datos.FiCenNivelNombre));
    "#FiCenResponsableNombre".AsHTML(datoEle(datos.FiCenResponsableNombre));
    "#FiCenCentroSuperior".AsHTML(datoEle(datos.FiCenCentroSuperior));
    "#FiCenCentroSuperiorNombre".AsHTML(datoEle(datos.FiCenCentroSuperiorNombre));
    
    $("#FiCenId, #FiCenNombre, #FiCenMontoMinimo, #FiCenNivel, #FiCenNivelNombre, " +
        " #FiCenResponsableNombre, #FiCenCentroSuperior, #FiCenCentroSuperiorNombre").removeClass("valuetd")
        .addClass("valuetd");

    cargado();

    $("#verCentro").modal({
        show: true,
        keyboard: false,
        backdrop: "static"
    });
    
}

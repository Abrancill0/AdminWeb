var tabla = "";
var PSubramos = [];// Seguridad.permisos(3);
var UsuarioActivo = localStorage.getItem("cosa");
$(function () {
    try {
        cargaInicialSubramos();
    } catch (err) {
        console.log("Input is ", err);
        setTimeout(cargaInicialSubramos, 100);
    }
});

function cargaInicialSubramos() {
    tabla = crearTabla("#tblSubRamo", 0, "desc");
    ObtenerCatalogoSubramos();
}

$("#refreshTbl").click(function () {
    ObtenerCatalogoSubramos();
});

function ObtenerCatalogoSubramos() {
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaCatalogoSubramos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ 'Usuario': UsuarioActivo }),
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cargado();
        },
        success: function (result) {
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
                        tabla.row.add(newRowSubRamo(value)).draw(false);
                    });
                } else {
                    tabla.row.add(newRowSubRamo(resultado)).draw(false);
                }
            } else {
                $.notify("Error: Al consultar Subramos.", { globalPosition: 'top center', className: 'error' });
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

$("#tblSubRamo").on("click, mouseenter", "tbody tr", function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    } else {
        $('#tblSubRamo tbody tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});

function newRowSubRamo(datos) {
    var FiSraId = datoEle(datos.FiSraId);
    var FiSraNombre = datoEle(datos.FiSraNombre);
    var FiSraRamo = datoEle(datos.FiSraRamo);
    var FiSraRamoNombre = datoEle(datos.FiSraRamoNombre);
    var ramo = FiSraRamo.toString() + " - " + FiSraRamoNombre;
    var btnVer = "<button type='button' onclick='verSubRamo(" + FiSraId + ")' class='btn btn-success btn-sm'><span class='glyphicon glyphicon-eye-open'></span> Ver</button>";
    var newrow = [
        FiSraId,
        FiSraNombre,
        ramo,
        btnVer
    ];
    return newrow;
}

function verSubRamo(idSubRamo) {
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaSubramoID',
        data: JSON.stringify({ 'FiSraId': idSubRamo, 'Usuario': UsuarioActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cargado();
            $("#FiSraId, #FiSraNombre, #FiSraRamo, #FiSraRamoNombre").empty();
        },
        success: function (result) {
            var exito = result.Salida.Resultado * 1;
            if (exito === 1) {
                var resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
                $("#verSubRamo").modal({
                    show: true,
                    keyboard: false,
                    backdrop: "static"
                });

                "#FiSraId".AsHTML(datoEle(resultado.FiSraId));
                "#FiSraNombre".AsHTML(datoEle(resultado.FiSraNombre));
                "#FiSraRamo".AsHTML(datoEle(resultado.FiSraRamo));
                "#FiSraRamoNombre".AsHTML(datoEle(resultado.FiSraRamoNombre));

            } else {
                $.notify("Error: Al consultar Subramo.", { globalPosition: 'top center', className: 'error' });
            }

        },
        complete: function () {
            cargado();
            $("#FiSraId, #FiSraNombre, #FiSraRamo, #FiSraRamoNombre").removeClass("valuetd")
                .addClass("valuetd");
        },
        error: function (result) {
            cargado();
            console.log("error", result);
        }
    });
}

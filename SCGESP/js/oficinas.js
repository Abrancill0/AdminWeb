var tabla = "";
var POficinas = [];// Seguridad.permisos(3);
var UsuarioActivo = localStorage.getItem("cosa");
$(function () {
    try {
        cargaInicialOficinas();
    } catch (err) {
        console.log("Input is ", err);
        setTimeout(cargaInicialOficinas, 100);
    }
});

function cargaInicialOficinas() {
    tabla = crearTabla("#tblOfinas", 0, "desc");
    ObtenerCatalogoOficinas();
}

$("#refreshTbl").click(function () {
    ObtenerCatalogoOficinas();
});

function ObtenerCatalogoOficinas() {
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaCatalogoOficinas',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ 'Usuario': UsuarioActivo }),
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
                        tabla.row.add(newRowOficina(value)).draw(false);
                    });
                } else {
                    tabla.row.add(newRowOficina(resultado)).draw(false);
                }
            } else {
                $.notify("Error: Al consultar Oficinas.", { globalPosition: 'top center', className: 'error' });
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

$("#tblOfinas").on("click, mouseenter", "tbody tr", function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    } else {
        $('#tblOfinas tbody tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});

function newRowOficina(datos) {
    var GrOfiId = datoEle(datos.GrOfiId);
    var GrOfiNombre = datoEle(datos.GrOfiNombre);
    var btnVer = "<button type='button' onclick='verOficina(" + GrOfiId + ")' class='btn btn-success btn-sm'><span class='glyphicon glyphicon-eye-open'></span> Ver</button>";
    var newrow = [
        GrOfiId,
        GrOfiNombre,
        btnVer
    ];
    return newrow;
}

function verOficina(GrOfiId) {
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaOficinaID',
        data: JSON.stringify({ 'GrOfiId': GrOfiId, 'Usuario': UsuarioActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cargado();
            $("#GrOfiId, #GrOfiNombre").empty();
        },
        success: function (result) {
            //console.log("success", result);
            var exito = result.Salida.Resultado * 1;
            if (exito === 1) {
                var resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
                $("#verOficina").modal({
                    show: true,
                    keyboard: false,
                    backdrop: "static"
                });

                "#GrOfiId".AsHTML(datoEle(resultado.GrOfiId));
                "#GrOfiNombre".AsHTML(datoEle(resultado.GrOfiNombre));

            } else {
                $.notify("Error: Al consultar Oficina.", { globalPosition: 'top center', className: 'error' });
            }

        },
        complete: function () {
            cargado();
            $("#GrOfiId, #GrOfiNombre").removeClass("valuetd")
                .addClass("valuetd");
        },
        error: function (result) {
            cargado();
            console.log("error", result);
        }
    });
}

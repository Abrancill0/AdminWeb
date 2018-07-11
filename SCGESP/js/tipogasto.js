var tabla = "";
var PTipoGasto = [];// Seguridad.permisos(3);
var UsuarioActivo = localStorage.getItem("cosa");
$(function () {
    try {
        cargaInicialTipoGastos();
    } catch (err) {
        console.log("Input is ", err);
        setTimeout(cargaInicialTipoGastos, 100);
    }
});

function cargaInicialTipoGastos() {
    tabla = crearTabla("#tblTipoGasto", 0, "desc");
    ObtenerCatalogoTipoGastos();
}

$("#refreshTbl").click(function () {
    ObtenerCatalogoTipoGastos();
});

function ObtenerCatalogoTipoGastos() {
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaCatalogoTipoGasto',
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
                        tabla.row.add(newRowTipoGasto(value)).draw(false);
                    });
                } else {
                    tabla.row.add(newRowTipoGasto(resultado)).draw(false);
                }
            } else {
                $.notify("Error: Al consultar Tipos de Gasto.", { globalPosition: 'top center', className: 'error' });
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

$("#tblTipoGasto").on("click, mouseenter", "tbody tr", function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    } else {
        $('#tblTipoGasto tbody tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});

function newRowTipoGasto(datos) {
    var FiTgaId = datoEle(datos.FiTgaId);
    var FiTgaNombre = datoEle(datos.FiTgaNombre);
    var btnVer = "<button type='button' onclick='verTipoGasto(" + JSON.stringify(datos) + ")' class='btn btn-success btn-sm'><span class='glyphicon glyphicon-eye-open'></span> Ver</button>";
    var newrow = [
        FiTgaId,
        FiTgaNombre,
        btnVer
    ];
    return newrow;
}

function verTipoGasto(datos) {
    cargado();
    $("#FiTgaId, #FiTgaNombre").empty();

    //var dTipoGasto = SelectTipoGasto(datos.FiTgaId);
    
    var FiTgaId = datoEle(datos.FiTgaId);
    var FiTgaNombre = datoEle(datos.FiTgaNombre);

    "#FiTgaId".AsHTML(FiTgaId);
    "#FiTgaNombre".AsHTML(FiTgaNombre);

    $("#FiTgaId, #FiTgaNombre").removeClass("valuetd")
        .addClass("valuetd");

    cargado();

    $("#verTipoGasto").modal({
        show: true,
        keyboard: false,
        backdrop: "static"
    });

}

function SelectTipoGasto(FiTgaId) {
    var resultado = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaTipoGastoID',
        data: JSON.stringify({ 'FiTgaId': FiTgaId, 'Usuario': UsuarioActivo }),
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
                $.notify("Error: Al consultar Tipo de Gasto.", { globalPosition: 'top center', className: 'error' });
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
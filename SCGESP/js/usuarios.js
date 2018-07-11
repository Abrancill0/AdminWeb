var tabla = "";
var PUsuarios = [];// Seguridad.permisos(3);
var UsuarioActivo = localStorage.getItem("cosa");
$(function () {
    try {
        cargaInicialUsuarios();
    } catch (err) {
        console.log("Input is ", err);
        setTimeout(cargaInicialUsuarios, 100);
    }
});

function cargaInicialUsuarios() {
    tabla = crearTabla("#tblUsuarios", 0, "desc");
    ObtenerCatalogoUsuarios();
}

$("#refreshTbl").click(function () {
    ObtenerCatalogoUsuarios();
});

function ObtenerCatalogoUsuarios() {
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ConsultaCatalogoUsuarios',
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
                        tabla.row.add(newRowUsuario(value)).draw(false);
                    });
                } else {
                    tabla.row.add(newRowUsuario(resultado)).draw(false);
                }
            } else {
                $.notify("Error: Al consultar Usuarios.", { globalPosition: 'top center', className: 'error' });
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

$("#tblUsuarios").on("click, mouseenter", "tbody tr", function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    } else {
        $('#tblUsuarios tbody tr.selected').removeClass('selected');
        $(this).addClass('selected');
    }
});

function newRowUsuario(datos) {
    var SgUsuId = datoEle(datos.SgUsuId);
    var SgUsuNombre = datoEle(datos.SgUsuNombre);
    var SgUsuEmpleado = datoEle(datos.SgUsuEmpleado);
    var SgUsuEmpleadoNombre = datoEle(datos.SgUsuEmpleadoNombre);
    var SgUsuActivo = datoEle(datos.SgUsuActivo);
    SgUsuActivo = SgUsuActivo.toLowerCase() === "true" ? 1 : 0;
    SgUsuActivo = SiNo(SgUsuActivo);
    var btnVer = "<button type='button' onclick='verUsuario(" + JSON.stringify(datos) + ")' class='btn btn-success btn-sm'><span class='glyphicon glyphicon-eye-open'></span> Ver</button>";
    var newrow = [
        SgUsuId,
        SgUsuNombre,
        SgUsuEmpleado,
        SgUsuEmpleadoNombre,
        SgUsuActivo, 
        btnVer
    ];
    return newrow;
}

function verUsuario(datos) {
    cargado();
    $("#SgUsuId, #SgUsuNombre, #SgUsuActivo, #SgUsuEmpleado, #SgUsuEmpleadoNombre, " +
        " #SgUsuGrupoUsuario, #SgUsuGrupoUsuarioNombre, #SgUsuFechaVencimiento").empty();

    var dUsuario = SelectUsuario(datos.SgUsuId);
    console.log(dUsuario);
    
    var SgUsuId = datoEle(dUsuario.SgUsuId);
    var SgUsuNombre = datoEle(dUsuario.SgUsuNombre);
    SgUsuNombre = valorVacio(SgUsuNombre) ? datoEle(datos.SgUsuNombre) : SgUsuNombre;

    var SgUsuActivo = datoEle(dUsuario.SgUsuActivo);
    SgUsuActivo = SgUsuActivo.toLowerCase() === "true" ? 1 : 0;
    SgUsuActivo = SiNo(SgUsuActivo);

    var SgUsuEmpleado = datoEle(dUsuario.SgUsuEmpleado);
    SgUsuEmpleado = valorVacio(SgUsuEmpleado) ? datoEle(datos.SgUsuEmpleado) : SgUsuEmpleado;
    var SgUsuEmpleadoNombre = datoEle(dUsuario.SgUsuEmpleadoNombre);
    SgUsuEmpleadoNombre = valorVacio(SgUsuEmpleadoNombre) ? datoEle(datos.SgUsuEmpleadoNombre) : SgUsuEmpleadoNombre;

    var SgUsuGrupoUsuario = datoEle(dUsuario.SgUsuGrupoUsuario);
    SgUsuGrupoUsuario = valorVacio(SgUsuGrupoUsuario) ? datoEle(datos.SgUsuGrupoUsuario) : SgUsuGrupoUsuario;
    var SgUsuGrupoUsuarioNombre = datoEle(dUsuario.SgUsuGrupoUsuarioNombre);
    SgUsuGrupoUsuarioNombre = valorVacio(SgUsuGrupoUsuarioNombre) ? datoEle(datos.SgUsuGrupoUsuarioNombre) : SgUsuGrupoUsuarioNombre;

    var SgUsuFechaVencimiento = datoEle(dUsuario.SgUsuFechaVencimiento);
    var FechaVencimiento = "No Definida";
    if (!valorVacio(SgUsuFechaVencimiento)) {
        SgUsuFechaVencimiento = SgUsuFechaVencimiento.split("T");
        FechaVencimiento = formatFecha(SgUsuFechaVencimiento[0], "dd De mmmm yyyy");
    }

    "#SgUsuId".AsHTML(SgUsuId);
    "#SgUsuNombre".AsHTML(SgUsuNombre);
    "#SgUsuActivo".AsHTML(SgUsuActivo);
    "#SgUsuEmpleado".AsHTML(SgUsuEmpleado);
    "#SgUsuEmpleadoNombre".AsHTML(SgUsuEmpleadoNombre);
    "#SgUsuGrupoUsuario".AsHTML(SgUsuGrupoUsuario);
    "#SgUsuGrupoUsuarioNombre".AsHTML(SgUsuGrupoUsuarioNombre);
    "#SgUsuFechaVencimiento".AsHTML(FechaVencimiento);


    $("#SgUsuId, #SgUsuNombre, #SgUsuActivo, #SgUsuEmpleado, #SgUsuEmpleadoNombre, " +
        " #SgUsuGrupoUsuario, #SgUsuGrupoUsuarioNombre, #SgUsuFechaVencimiento").removeClass("valuetd")
        .addClass("valuetd");

    cargado();

    $("#verUsuario").modal({
        show: true,
        keyboard: false,
        backdrop: "static"
    });

}

function SelectUsuario(SgUsuId) {
    var resultado = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaUsuarioID',
        data: JSON.stringify({ 'SgUsuId': SgUsuId, 'Usuario': UsuarioActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            var exito = result.Salida.Resultado * 1;
            if (exito === 1) {
                resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
            } else {
                $.notify("Error: Al consultar Usuario.", { globalPosition: 'top center', className: 'error' });
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
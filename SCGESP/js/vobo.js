var tabla = "";
var UsuarioActivo = localStorage.getItem("cosa");
var EmpeladoActivo = localStorage.getItem("cosa2");
$(function () {
    try {
        cargaInicialVoBo();
    } catch (err) {
        console.log("Input is ", err);
        setTimeout(cargaInicialVoBo, 100);
    }
});

function cargaInicialVoBo() {
    SelectUsuario();
    SelectVoBo();
}

function SelectVoBo() {
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaConfVoBo',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ 'Usuario': UsuarioActivo }),
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cargado();
        },
        success: function (result) {
            console.log("success", result);
            var resultado = result[0];

            if (resultado.ValorDefault === 1) {
                $("#ChkVoBoActivo").attr("checked", true);
            } else {
                $("#ChkVoBoActivo").removeAttr("checked");
            }
            if (resultado.ChkBloqueado === 1) {
                $("#ChkVoBoBloqueado").attr("checked", true);
            } else {
                $("#ChkVoBoBloqueado").removeAttr("checked");
            }
            if (resultado.ValidarImporte === 1) {
                $("#ChkVoBoImporte").attr("checked", true);
            } else {
                $("#ChkVoBoImporte").removeAttr("checked");
            }
            $("#importemayor").val(resultado.ImporteMayorQue);
            $("#SgUsuId").val($.trim(resultado.Usuario));

            if (encriptaDesencriptaEle($("#SgUsuId").val(), 1) !== localStorage.getItem("cosa")) {
                history.back();
            }
            
        },
        complete: function () {
            cargado();
            actualizaImgChk();
        },
        error: function (result) {
            cargado();
            console.log("error", result);
        }
    });
}

function SelectUsuario() {
    $.ajax({
        async: false,
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
            $("#SgUsuId").empty();
            var exito = result.Salida.Resultado * 1;
            if (exito === 1) {
                var resultado = result.Salida.Tablas.Catalogo.NewDataSet.Catalogo;
                var nsubramos = 0;
                try {
                    nsubramos = resultado.length;
                } catch (err) {
                    nsubramos = 0;
                }
                if (nsubramos > 0) {
                    $.each(resultado, function (key, value) {
                        var option = "<option value = '" + $.trim(value.SgUsuId) + "' data-SgUsuEmpleado='" + $.trim(value.SgUsuEmpleado) + "'>" + value.SgUsuNombre + "</option>";
                        $("#SgUsuId").append(option);
                    });
                } else {
                    var option = "<option value = '" + $.trim(resultado.SgUsuId) + "'>" + resultado.SgUsuNombre + "</option>";
                    $("#SgUsuId").append(option);
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

$("#ChkVoBoActivo, #ChkVoBoBloqueado").change(function () {
    actualizaImgChk();
});

$("#ChkVoBoImporte").change(function () {
    if ($(this).is(':checked')) {
        if (($("#importemayor").val() * 1) <= 0)
            $("#importemayor").notify("Se requiere un valor mayor a cero.", { globalPosition: 'top center', className: 'error' });
    }
});

function actualizaImgChk() {

    var dir = "img/chkVoBo/chk";
    var nombreChk = "";
    var ChkVoBoActivo = "Desactivado", ChkVoBoBloqueado = "Desbloqueado";

    if ($("#ChkVoBoActivo").is(':checked')) {
        ChkVoBoActivo = "Activo";
    }
    if ($("#ChkVoBoBloqueado").is(':checked')) {
        ChkVoBoBloqueado = "Bloqueado";
    }

    nombreChk = ChkVoBoActivo + ChkVoBoBloqueado + ".PNG";
    dir = dir + nombreChk + "?" + Math.random();

    $("#chkVoBo").removeAttr("src");
    $("#chkVoBo").attr("src", dir);
}
$("#btnGuardaVoBo").click(function () {
    actualizarConfiVoBo();
});

function actualizarConfiVoBo() {
    var ChkVoBoActivo = 0, ChkVoBoBloqueado = 0, ChkVoBoImporte = 0, importemayor = 0;
    if ($("#ChkVoBoActivo").is(':checked')) {
        ChkVoBoActivo = 1;
    }
    if ($("#ChkVoBoBloqueado").is(':checked')) {
        ChkVoBoBloqueado = 1;
    }
    if ($("#ChkVoBoImporte").is(':checked')) {
        ChkVoBoImporte = 1;
    }

    importemayor = $("#importemayor").val() * 1;

    if (importemayor <= 0 && ChkVoBoImporte === 1) {
        $.notify("El importe debe ser mayor a cero (0).", { globalPosition: 'top center', className: 'error' });
        return false;
    }

    importemayor = ChkVoBoImporte === 1 ? importemayor : 0;

    var Usuario = $("#SgUsuId").val();
    var SelectUsuario = $("#SgUsuId");
    SelectUsuario = SelectUsuario[0];
    var nombreUsuario = SelectUsuario.options[SelectUsuario.selectedIndex].text;
    var SgUsuEmpleado = $("#SgUsuId option:selected").attr("data-SgUsuEmpleado")
        //$("#SgUsuId").children('option:selected').data("SgUsuEmpleado"); //$("#SgUsuId option:selected").data("SgUsuEmpleado");
    
    var datos = {
        'Usuario': Usuario,
        'NombreUsuario': nombreUsuario,
        'ValorDefault': ChkVoBoActivo,
        'ChkBloqueado': ChkVoBoBloqueado,
        'ValidarImporte': ChkVoBoImporte,
        'ImporteMayorQue': importemayor,
        'SgUsuEmpleado': SgUsuEmpleado
    };

    $.ajax({
        async: true,
        type: "POST",
        url: '/api/ActualizaConfVoBo',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(datos),
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            cargado();
        },
        success: function (result) {
            if (result.ActualizadoOk === true) {
                $.notify(result.Descripcion, { globalPosition: 'top center', className: 'success' });
            } else {
                $.notify(result.Descripcion, { globalPosition: 'top center', className: 'error' });
            }

        },
        complete: function () {
            cargado();
            SelectVoBo();
        },
        error: function (result) {
            cargado();
            console.log("error", result);
        }
    });
}

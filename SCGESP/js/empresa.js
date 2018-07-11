var PEmpresa = [];// Seguridad.permisos(3);
var UsuarioActivo = localStorage.getItem("cosa");
$(function () {
    try {
        cargaInicialPerfil();
    } catch (err) {
        console.log("Input is ", err);
        setTimeout(cargaInicialPerfil, 100);
    }
});

function cargaInicialPerfil() {
    SelectPerfil();
    //SelectEmpresas();
}

function SelectPerfil() {
    //SelectEmpleados();
    //return false;
    var SgUsuId = UsuarioActivo;
    var datos = [];
    datos['usuario'] = SelectUsuario(SgUsuId);
    $(".SgUsuId, .SgUsuNombre").empty();
    $(".GrConCalle, .GrConCiudad, .GrConCodigoPostal, .GrConColonia, .GrConEstado, .GrConNumExt, .GrConRazonSocial, .GrConRfc").empty();
    //GrConLogotipo
    $(".SgUsuId").append(datoEle(datos.usuario.SgUsuId));
    $(".SgUsuNombre").append(datoEle(datos.usuario.SgUsuNombre));

    datos['empresa'] = SelectEmpresa();
    if (!valorVacio(datos.empresa.GrConId)) {
        $(".GrConCalle").append(datos.empresa.GrConCalle);
        $(".GrConCiudad").append(datos.empresa.GrConCiudad);
        $(".GrConCodigoPostal").append(datos.empresa.GrConCodigoPostal);
        $(".GrConColonia").append(datos.empresa.GrConColonia);
        $(".GrConEstado").append(datos.empresa.GrConEstado);
        $(".GrConNumExt").append(datos.empresa.GrConNumExt);
        $(".GrConRazonSocial").append(datos.empresa.GrConRazonSocial);
        $(".GrConRfc").append(datos.empresa.GrConRfc);

    } else {
        //datos['empresa'] = SelectEmpresas();
    }

    //console.log(datos);
    
}

function SelectUsuario(SgUsuId) {
    var resultado = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaUsuarioID',
        data: JSON.stringify({ 'SgUsuId': SgUsuId, 'Usuario': SgUsuId }),
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

function SelectEmpresa() {
    var resultado = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaEmpresaID',
        data: JSON.stringify({ 'Usuario': UsuarioActivo, 'GrConId': "1" }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            console.log(result);
            var exito = result.Salida.Resultado * 1;
            if (exito === 1) {
                resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
            } else {
                $.notify("Error: Al consultar Empresa.", { globalPosition: 'top center', className: 'error' });
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

function SelectEmpresas() {
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaCatalogoEmpresa',
        data: JSON.stringify({ 'Usuario': UsuarioActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            console.log(result);
            var exito = result.Salida.Resultado * 1;
            if (exito === 1) {
                resultado = result.Salida.Tablas.Catalogo.NewDataSet.Catalogo;
            } else {
                $.notify("Error: Al consultar Empresas.", { globalPosition: 'top center', className: 'error' });
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
}

$("#fileFotoUsuario").change(function () {
    cargado();
    var file = $(this).get(0).files[0];
    var r = new FileReader();
    r.onload = function () {
        var binimage = r.result;
        guardarFotoUsuario(binimage);
    };
    r.readAsDataURL(file);
});

function guardarFotoUsuario(binimage) {
    var fotoold = $(".img-perfil.img-responsive").attr("src");
    var fotoold2 = fotoold.split("?");
    fotoold = fotoold2[0];
    if (fotoold.indexOf("default") > 0)
        fotoold = "";
    var datos = { 'Usuario': encriptaDesencriptaEle(UsuarioActivo, 0), 'Foto': binimage, 'FotoOld': fotoold };
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/CargarFotoUsuario',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            var resultado = result[0];
            var img = resultado.Img + "?" + Math.random();
            $(".img-perfil").attr("src", img);
            localStorage.setItem("cosa4", encriptaDesencriptaEle(img, 1));
            if (resultado.FotoOK === false)
                Seguridad.alerta(resultado.Descripcion);
            else
                $.notify(resultado.Descripcion, { globalPosition: 'top center', className: 'success' });
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
var PPerfil = [];// Seguridad.permisos(3);
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
}

$("#refreshTbl").click(function () {
    SelectPerfil();
});

function SelectPerfil() {
    //SelectEmpleados();
    //return false;
    var SgUsuId = UsuarioActivo;
    var datos = [];
    datos['usuario'] = SelectUsuario(SgUsuId);
    var SgUsuEmpleado = datoEle(datos.usuario.SgUsuEmpleado);
    if (!valorVacio(SgUsuEmpleado)) {
        datos['empleado'] = SelectEmpleado(datos.usuario.SgUsuEmpleado, UsuarioActivo);
    }
    //console.log(datos);

    $(".SgUsuId, .SgUsuNombre .GrEmpId, .GrEmpNombre, .GrEmpApellidoPaterno " +
        ", .GrEmpApellidoMaterno, .GrEmpFechaAlta, .GrEmpCentroNombre " + 
        ", .GrEmpOficinaNombre, .GrEmpTipoGastoNombre, .GrEmpCorreoElectronico " +
        ", .GrEmpTelefonoOficina, .GrEmpTarjetaToka").empty();

    $(".SgUsuId").append(datoEle(datos.usuario.SgUsuId));
    $(".SgUsuNombre").append(datoEle(datos.usuario.SgUsuNombre));

    if (!valorVacio(SgUsuEmpleado)) {
        $(".GrEmpId").append(datoEle(datos.empleado.GrEmpId));
        $(".GrEmpNombre").append(datoEle(datos.empleado.GrEmpNombre));
        $(".GrEmpApellidoPaterno").append(datoEle(datos.empleado.GrEmpApellidoPaterno));
        $(".GrEmpApellidoMaterno").append(datoEle(datos.empleado.GrEmpApellidoMaterno));

        var GrEmpFechaAlta = datoEle(datos.empleado.GrEmpFechaAlta);
        if (!valorVacio(GrEmpFechaAlta)) {
            GrEmpFechaAlta = GrEmpFechaAlta.split("T");
            var FechaAlta = formatFecha(GrEmpFechaAlta[0], "dd De mmmm yyyy");
            $(".GrEmpFechaAlta").append(FechaAlta);
        }
        $(".GrEmpCentroNombre").append(datoEle(datos.empleado.GrEmpCentroNombre));
        $(".GrEmpOficinaNombre").append(datoEle(datos.empleado.GrEmpOficinaNombre));
        $(".GrEmpTipoGastoNombre").append(datoEle(datos.empleado.GrEmpTipoGastoNombre));

        $(".GrEmpCorreoElectronico").append(datoEle(datos.empleado.GrEmpCorreoElectronico));
        $(".GrEmpTelefonoOficina").append(datoEle(datos.empleado.GrEmpTelefonoOficina));
        $(".GrEmpTarjetaToka").append(datoEle(datos.empleado.GrEmpTarjetaToka));
    }



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

function SelectEmpleado(GrEmpID, UsuarioActivo) {
    var resultado = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaEmpleadoID',
        data: JSON.stringify({ 'GrEmpID': GrEmpID, 'Usuario': UsuarioActivo }),
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
                $.notify("Error: Al consultar Empleado.", { globalPosition: 'top center', className: 'error' });
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

function SelectEmpleados() {
    var resultado = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaCatalogoEmpleados',
        data: JSON.stringify({ 'Usuario': UsuarioActivo }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            var exito = result.Salida.Resultado * 1;
            if (exito === 1) {
                //resultado = result.Salida.Tablas.Llave.NewDataSet.Llave;
            } else {
                $.notify("Error: Al consultar empleados.", { globalPosition: 'top center', className: 'error' });
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

$("#fileFotoUsuario").change(function () {
    cargado();
    var file = $(this).get(0).files[0];
    var r = new FileReader();
    r.onload = function () {
        var binimage = r.result;
        guardarFotoUsuario(binimage, file);
    };
    r.readAsDataURL(file);
});

function guardarFotoUsuario(binimage, fileFoto) {
    var fotoold = $(".img-perfil.img-responsive").attr("src");
    var fotoold2 = fotoold.split("?");

    fotoold = fotoold2[0];
    if (fotoold.indexOf("default") > 0)
        fotoold = "";

    //var datos = { 'Usuario': encriptaDesencriptaEle(UsuarioActivo, 0), 'Foto': binimage, 'FotoOld': fotoold };
    /*
    $.ajax({
        async: true,
        type: "POST",
        url: '/api/CargarFotoUsuario',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,*/

    var aDatos = new FormData();
    aDatos.append("Usuario", encriptaDesencriptaEle(UsuarioActivo, 0));
    aDatos.append("Foto", binimage);
    aDatos.append("FotoOld", fotoold);
    aDatos.append("FileFoto", fileFoto);

    $.ajax({
        async: false,
        type: "POST",
        url: "/api/CargarFotoUsuario",
        data: aDatos,
        //dataType: "json",
        contentType: false,
        processData: false,
        cache: false,
        dataType: "json",
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            var resultado = result;
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
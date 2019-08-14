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
    tabla = crearTabla("#tblUsuarios", 4, "asc");
    ObtenerCatalogoUsuarios();
}

$("#refreshTbl").click(function () {
    ObtenerCatalogoUsuarios();
});

function ObtenerCatalogoUsuarios() {
    var usuAdmin = buscaTipoAdmin();
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
                var tAdmin = 0, tSup = 0;
                if (nsubramos > 0) {
                    $.each(resultado, function (key, value) {
                        var datosRow = newRowUsuario(value, usuAdmin);
                        if (datosRow.row !== null) {
                            tabla.row.add(datosRow.row).draw(false);
                            tAdmin += datosRow.administrador * 1;
                            tSup += datosRow.suplente * 1;
                        }
                            
                    });
                } else {
                    var datosRow = newRowUsuario(resultado, usuAdmin);
                    if (datosRow.row !== null) {
                        tabla.row.add(datosRow.row).draw(false);
                        tAdmin += datosRow.administrador * 1;
                        tSup += datosRow.suplente * 1;
                    }
                }
                "#tdNoAutPrincipal".AsHTML("Aut. Principal: " + tAdmin);
                "#tdNoAutSuplente".AsHTML("Aut. Suplente: " + tSup);
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

function newRowUsuario(datos, usuAdmin) {
    var SgUsuId = datoEle(datos.SgUsuId);
    var SgUsuNombre = datoEle(datos.SgUsuNombre);
    var SgUsuEmpleado = datoEle(datos.SgUsuEmpleado);
    var SgUsuEmpleadoNombre = datoEle(datos.SgUsuEmpleadoNombre);
    var datosAdm = usuAdmin.usuarios[SgUsuEmpleado];
    var administrador = "", suplente = "";
    var stAdministrador = 0, stSuplente = 0;
    if (valorVacio(datosAdm)) {
        administrador = "";
        suplente = "";
        stAdministrador = 0;
        stSuplente = 0;
    } else {
        stAdministrador = datosAdm.Administrador;
        stSuplente = datosAdm.Suplente;
        administrador = stAdministrador === 1 ? "checked" : "";
        suplente = stSuplente === 1 ? "checked" : "";

    }
    var datos = {
        'SgUsuId': SgUsuId,
        'SgUsuEmpleado': SgUsuEmpleado,
        'SgUsuEmpleadoNombre': SgUsuEmpleadoNombre,
        'StAdministrador': stAdministrador,
        'StSuplente': stSuplente,
        'SgUsuIdAdm': datoEle(usuAdmin.admin.UAutoriza),
        'SgUsuEmpleadoAdm': datoEle(usuAdmin.admin.IdEmpleado),
        'SgUsuEmpleadoNombreAdm': datoEle(usuAdmin.admin.Nombre)

    };
    var inpChkAdm = chk2("ChkAdm" + SgUsuId, "ChkAdm", administrador, "confActualizaUsuAdmin(this)", JSON.stringify(datos), "18", "default", "default", "");
    var inpChkSup = chk2("ChkSup" + SgUsuId, "ChkSup", suplente, "confActualizaUsuSub(this)", JSON.stringify(datos), "18", "default", "default", "");
    if (valorVacio(datos.SgUsuIdAdm) || (datos.SgUsuId === datos.SgUsuIdAdm))
        inpChkSup = "";
    var renglon = [];
    if (!valorVacio(SgUsuEmpleado)) {
        var newrow = [
            SgUsuId,
            SgUsuNombre,
            SgUsuEmpleado,
            SgUsuEmpleadoNombre,
            inpChkAdm,
            inpChkSup
        ];
        renglon = {
            'row': newrow,
            'administrador': stAdministrador,
            'suplente': stSuplente
        };

        return renglon;
    } else {
        renglon = {
            'row': null,
            'administrador': 0,
            'suplente': 0
        };

        return renglon;
    }
}

function confActualizaUsuAdmin(elemento) {
    var datos = JSON.parse($(elemento).val());
    datos['Origen'] = "Administrador";
    if ($(elemento).is(':checked')) {
        if (datos.StAdministrador === 0 && datos.SgUsuIdAdm === "") {
            var botones = [];
            
            botones[0] = {
                text: "No", click: function () {
                    $(this).dialog("close");
                    ObtenerCatalogoUsuarios();
                }
            };
            botones[1] = {
                text: "Si", click: function () {
                    $(this).dialog("close");
                    datos['StAdministrador'] = 1;
                    datos['StSuplente'] = 0;
                    datos['ExistiaUnAdm'] = 0;
                    actualizaUsuAdmin(datos);
                    $.notify("Se agrego nuevo usuario autorizador principal.", { globalPosition: 'top center', className: 'success' });
                }
            };
            Seguridad.confirmar("Agregar al usuario <b>" + datos.SgUsuEmpleadoNombre + "</b> como administrador?", botones, " Marcar como Administrador.");
        } else if (datos.StAdministrador === 0 && datos.SgUsuIdAdm !== "") {
            var botones = [];
            botones[0] = {
                text: "No", click: function () {
                    $(this).dialog("close");
                    ObtenerCatalogoUsuarios();
                }
            };
            botones[1] = {
                text: "Si", click: function () {
                    $(this).dialog("close");
                    datos['StAdministrador'] = 1;
                    datos['StSuplente'] = 0;
                    datos['ExistiaUnAdm'] = 1;
                    actualizaUsuAdmin(datos);
                    $.notify("Se agrego nuevo usuario autorizador principal.", { globalPosition: 'top center', className: 'success' });
                }
            };
            Seguridad.confirmar("Agregar al usuario <b>" + datos.SgUsuEmpleadoNombre + "</b> como administrador y quitar a <b>" + datos.SgUsuEmpleadoNombreAdm + "</b>?", botones, " Marcar como Administrador.");
        }
    } else {
        if (datos.StAdministrador === 1) {
            var botones = [];
            botones[0] = {
                text: "No", click: function () {
                    $(this).dialog("close");
                    ObtenerCatalogoUsuarios();
                }
            };
            botones[1] = {
                text: "Si", click: function () {
                    $(this).dialog("close");
                    datos['StAdministrador'] = 0;
                    datos['StSuplente'] = 0;
                    datos['ExistiaUnAdm'] = 1;
                    actualizaUsuAdmin(datos);
                    $.notify("Se quito usuario autorizador principal.", { globalPosition: 'top center', className: 'success' });
                }
            };
            Seguridad.confirmar("Quitar el usuario <b>" + datos.SgUsuEmpleadoNombre + "</b> como administrador?", botones, " Desmarcar como Administrador.");
        }
    }
}
function actualizaUsuAdmin(datos) {
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ActualizaUsuAdmin',
        data: JSON.stringify(datos),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        success: function (result) {
            ObtenerCatalogoUsuarios();
        },
        error: function (result) {
            console.log(result);
        }
    });
}
function confActualizaUsuSub(elemento) {
    var datos = JSON.parse($(elemento).val());
    datos['Origen'] = "Suplente";
    if ($(elemento).is(':checked')) {
        if (datos.StAdministrador === 1 && datos.StSuplente === 0) {
            var botones = [];
            botones[0] = {
                text: "No", click: function () {
                    $(this).dialog("close");
                    ObtenerCatalogoUsuarios();
                }
            };
            botones[1] = {
                text: "Si", click: function () {
                    $(this).dialog("close");
                    datos['StAdministrador'] = 0;
                    datos['StSuplente'] = 1;
                    datos['ExistiaUnAdm'] = 1;
                    actualizaUsuSub(datos);
                    $.notify("Se quito usuario autorizador suplente.", { globalPosition: 'top center', className: 'success' });
                }
            };
            Seguridad.confirmar("Quitar al usuario <b>" + datos.SgUsuEmpleadoNombre + "</b> como administrador y agregarlo como suplente?", botones, " Marcar como Suplente.");
        } else if (datos.StAdministrador === 0 && datos.StSuplente === 0) {
            var botones = [];
            botones[0] = {
                text: "No", click: function () {
                    $(this).dialog("close");
                    ObtenerCatalogoUsuarios();
                }
            };
            botones[1] = {
                text: "Si", click: function () {
                    $(this).dialog("close");
                    datos['StAdministrador'] = 0;
                    datos['StSuplente'] = 1;
                    datos['ExistiaUnAdm'] = 1;
                    actualizaUsuSub(datos);
                    $.notify("Se agrego nuevo usuario autorizador suplente.", { globalPosition: 'top center', className: 'success' });
                }
            };

            Seguridad.confirmar("Agregar al usuario <b>" + datos.SgUsuEmpleadoNombre + "</b> como suplente?", botones, " Marcar como Suplente.");
        }
    } else {
        var botones = [];
        botones[0] = {
            text: "No", click: function () {
                $(this).dialog("close");
                ObtenerCatalogoUsuarios();
            }
        };
        botones[1] = {
            text: "Si", click: function () {
                $(this).dialog("close");
                datos['StAdministrador'] = 0;
                datos['StSuplente'] = 0;
                datos['ExistiaUnAdm'] = 1;
                actualizaUsuSub(datos);
                $.notify("Se quito usuario autorizador suplente.", { globalPosition: 'top center', className: 'success' });
            }
        };
        Seguridad.confirmar("Quitar al usuario <b>" + datos.SgUsuEmpleadoNombre + "</b> como suplente?", botones, " Desmarcar como Suplente.");
    }
}
function actualizaUsuSub(datos) {
    actualizaUsuAdmin(datos);
}
function buscaTipoAdmin(SgUsuEmpleado) {
    var resultado = [];
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/ConsultaTipoAdminAutorizador',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ 'SgUsuEmpleado': UsuarioActivo }),
        dataType: 'json',
        cache: false,
        beforeSend: function () {
            //cargado();
        },
        success: function (result) {
            resultado['usuarios'] = [];
            var exiAdmin = false;
            $.each(result, function (key, value) {
                if (value.Administrador === 1) {
                    exiAdmin = true;
                    resultado['admin'] = {
                        'IdEmpleado': value.IdEmpleado,
                        'UAutoriza': value.UAutoriza,
                        'Nombre': value.Nombre
                    }
                }
                resultado['usuarios'][value.IdEmpleado] = {
                    'UAutoriza': value.UAutoriza,
                    'Nombre': value.Nombre,
                    'Administrador': value.Administrador,
                    'Suplente': value.Suplente
                }

            });
            if (exiAdmin === false) {
                resultado['admin'] = {
                    'IdEmpleado': '',
                    'UAutoriza': '',
                    'Nombre': 'No existe un usuario administrador'
                }
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
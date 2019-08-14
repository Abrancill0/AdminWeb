/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

var urlcge = urlcge ? urlcge : "http://gapptest.elpotosi.com.mx";
$(function () {
    urlcge = "http://gapptest.elpotosi.com.mx";
    $("#lblNmbEmp").empty().append(localStorage.getItem("nmbemp"));
    var colortb = localStorage.getItem("colortb") ? localStorage.getItem("colortb") : "blue";
    $("body").attr("data-ma-theme", colortb);
    if (valorVacio(localStorage.getItem("cosa4"))) {
        var foto = encriptaDesencriptaEle(localStorage.getItem("cosa"), 0);
        var fotook = consultaFoto(foto, "jpg");
        if (fotook === false) {
            fotook = consultaFoto(foto, "jpeg");
            if (fotook === false) {
                fotook = consultaFoto(foto, "png");
                if (fotook === false) {
                    fotook = consultaFoto(foto, "gif");
                }
            }
        }
    } else {
        var cosa4 = encriptaDesencriptaEle(localStorage.getItem("cosa4"), 0);
        $(".img-perfil").attr("src", cosa4);
    }
});
$("#cambiapass").click(function () {
    cambiapass();
});

function cambiapass() {
    var botones = [];
    botones[0] = {
        text: "Cambiar", click: function () {
            var passa = $("#passa").val();
            var passn = $("#passn").val();
            var passnc = $("#passnc").val();
            var datos = {
                'passn': passn,
                'passa': passa
            };
            if (valorVacio(passa)) {
                $("#alerta").notify("Necesitas ingresar la contraseña anterior.", { position: "top center", autoHideDelay: 3000 }, "error");
            } else if (valorVacio(passn)) {
                $("#alerta").notify("Necesitas ingresar la nueva contraseña.", { position: "top center", autoHideDelay: 3000 }, "error");
            } else if (valorVacio(passnc)) {
                $("#alerta").notify("Necesitas confirmar la nueva contraseña.", { position: "top center", autoHideDelay: 3000 }, "error");
            } else if (passn !== passnc) {
                $("#alerta").notify("La contraseña Nueva NO coincide con la Confirmación.", { position: "top center", autoHideDelay: 3000 }, "error");
            } else if (Seguridad.UpdatePassUsu(datos) === 1) {
                $("#alerta").notify("La contraseña Anterior NO es correcta.", { position: "top center", autoHideDelay: 3000 }, "error");
            } else {
                $(this).dialog("close");
                Seguridad.alerta("<span class='glyphicon glyphicon-ok'></span> Se cambio la Contraseña.");
            }
        }
    };
    botones[1] = {
        text: "Cancelar", click: function () {
            $(this).dialog("close");
        }
    };
    var inputspass = "<input type='password' id='passa' name='passa' class='form-control' placeholder='Contraseña Anterior' value='' required /><br />";
    inputspass += "<input type='password' id='passn' name='passn' class='form-control' placeholder='Contraseña Nueva' value='' required /><br />";
    inputspass += "<input type='password' id='passnc' name='passnc' class='form-control' placeholder='Confirma Nueva Contraseña' value='' required />";
    Seguridad.confirmar("Cambiar Contraseña?" + inputspass, botones, "Cambiar Contraseña");
}
var Seguridad = {
    permiso: function (mod, acc) {
        var permiso = 0;
        /*$.ajax({
            async: false,
            type: "POST",
            url: "includes/Script.php?modulo=seguridad&accion=permiso&smodulo=" + mod + "&saccion=" + acc + "&Content-Type=text/json",
            dataType: "json",
            cache: false,
            success: function (result) {
                permiso = result.PERMISO * 1;
            },
            error: function (result) {
                console.log(result);
            }
        });*/
        return permiso;
    },
    permisos: function (mod) {
        var permiso = [];
        /*$.ajax({
            async: false,
            type: "POST",
            url: "includes/Script.php?modulo=seguridad&accion=permisos&smodulo=" + mod + "&Content-Type=text/json",
            dataType: "json",
            cache: false,
            success: function (result) {
                permiso = result;
            },
            error: function (result) {
                console.log(result);
            }
        });*/
        return permiso;
    },
    bitacora: function (accion, idmodulo, iddocum, descripcion, musuario) {
        /*
         * iddocum = id's de documento separado por comas(,)
         * descripcion = descripcion de la accion a ejecutar
         * musuario = bandera 1: muestra a usuario, 0: no muestra a usuarios (reporte)
         */
        var datos = { 'accion': accion, 'idmodulo': idmodulo, 'iddocum': iddocum, 'descripcion': descripcion, 'musuario': musuario };
        /*$.ajax({
            async: true,
            type: "POST",
            url: "includes/Script.php?modulo=seguridad&accion=InsertBitacora&Content-Type=text/json",
            data: datos,
            dataType: "json",
            error: function (result) {
                console.log("Error: ", result);
            }
        });*/
    },
    alerta: function (msn, abriren, titulo) {
        //event.preventDefault();
        $("#alerta").dialog({
            position: { my: "center", at: "center", of: window },
            appendTo: abriren,
            autoOpen: true,
            modal: true,
            dialogClass: "alert",
            title: titulo ? titulo : "Alerta!",
            buttons: [
                {
                    text: "Aceptar",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            ]
        });

        var titlebar = '<table width="100%" >' +
            '<tr>' +
            '<td style="text-align:left;color:#FFF">' +
            (titulo ? titulo : "Alerta!") +
            '</td> <td style="text-align:right" > ' +
            '<a  onclick="Seguridad.cerrarpanel()" style="color:#FFF;border-left:1px solid #FFF ">&nbsp;<i class="zmdi zmdi-close"></i>&nbsp;Cerrar</a>' +
            '</td></tr></table>';

        $("#alerta").empty();
        $("#alerta").append(msn);
        $(".ui-dialog-titlebar.ui-corner-all.ui-widget-header.ui-helper-clearfix.ui-draggable-handle").empty().append(titlebar);
    },
    confirmar: function (msn, botones, titulo, abriren) {
        //event.preventDefault();
        $("#alerta").dialog({
            //dialogClass: "no-close",
            position: { my: "center", at: "center", of: window },
            classes: {
                "ui-dialog": "highlight"
            },
            appendTo: abriren,
            autoOpen: true,
            modal: true,
            title: titulo ? titulo : "Alerta!",
            buttons: botones

        });
        $("#alerta").empty();

        $("#alerta").append(msn);

        var titlebar = '<table width="100%" >' +
            '<tr>' +
            '<td style="text-align:left;color:#FFF">' +
            (titulo ? titulo : "Alerta!") +
            '</td> <td style="text-align:right" > ' +
            '<a  onclick="Seguridad.cerrarpanel()" style="color:#FFF;border-left:1px solid #FFF ">&nbsp;<i class="zmdi zmdi-close"></i>&nbsp;Cerrar</a>' +
            '</td></tr></table>';

        $(".ui-dialog-titlebar.ui-corner-all.ui-widget-header.ui-helper-clearfix.ui-draggable-handle").empty().append(titlebar);


    },
    cerrarpanel: function(){
        $("#alerta").dialog("close");
    },

    login: function (dato) {
        var valor = "";
        /*$.ajax({
            async: false,
            type: "POST",
            url: "includes/Script.php?modulo=login&accion=infoLogIn&Content-Type=text/json",
            dataType: "json",
            success: function (result) {
                valor = result[dato];
            },
            error: function (result) {
                console.log(result);
            }
        });*/
        return valor;
    },
    UpdatePassUsu: function (datos) {
        var ok = 0;
        /*$.ajax({
            async: false,
            type: "POST",
            url: "includes/Script.php?modulo=usuarios&accion=UpdatePassUsu&id&Content-Type=text/json",
            data: datos,
            dataType: "json",
            success: function (result) {
                ok = result["ERROR"] * 1;
            },
            error: function (result) {
                console.log(result);
            }
        });*/
        return ok;
    },
    sessionActiva: function () {
        var usu = this.login("usu");
        if (usu === "") {
            window.location.href = "../";
            return false;
        }
    },
    logo: function () {
        var logo = "";
        /*$.ajax({
            async: false,
            type: "POST",
            url: "includes/Script.php?modulo=login&accion=logo&Content-Type=text/json",
            dataType: "json",
            success: function (result) {
                logo = result;
            },
            error: function (result) {
                console.log(result);
            }
        });*/
        return logo;
    },
    rutajs: function (mod) {
        var ruta = "";
        var datos = { 'idmodulo': mod };
        /*$.ajax({
            async: false,
            type: "POST",
            url: "includes/Script.php?modulo=seguridad&accion=rutajs&Content-Type=text/json",
            data: datos,
            dataType: "json",
            success: function (result) {
                ruta = result;
            },
            error: function (result) {
                console.log("Error: ", result);
            }
        });*/
        return ruta;
    },
    menu: function () {
        var menuhtml = "";
        /*$.ajax({
            async: true,
            type: "POST",
            url: "../menu/" + this.login("emp") + "/menu" + this.login("id") + ".html?Content-Type=text/html",
            success: function (result) {
                menuhtml = result;
                $("#menup").empty();
                $("#menup").append(menuhtml);
                if (menuhtml === "") {
                    Seguridad.logout();
                }
            },
            error: function (result) {
                console.log("Error: ", result);
                Seguridad.logout();
            }
        });*/
    },
    logout: function () {
        /*$.ajax({
            async: false,
            type: "POST",
            url: "includes/Script.php?modulo=login&accion=LogOut&Content-Type=text/json",
            dataType: "json",
            error: function (result) {
                console.log("Error: ", result);
            }
        });*/
    },
    colortoolbar: function (color) {
        /*$.ajax({
            async: true,
            type: "POST",
            url: "includes/Script.php?modulo=usuarios&accion=ColorToolbar&id&Content-Type=text/json",
            data: { 'color': color },
            dataType: "json",
            success: function () {
                //success
            },
            error: function (result) {
                console.log(result);
            }
        });*/
    },
    visiblemenu: function (visible) {
        /*$.ajax({
            async: true,
            type: "POST",
            url: "includes/Script.php?modulo=usuarios&accion=VisibleMenu&id&Content-Type=text/json",
            data: { 'visible': visible },
            dataType: "json",
            complete: function () {
                location.reload();
            },
            error: function (result) {
                console.log(result);
            }
        });*/
    },
    error404: function () {
        $("#principal").empty();
        /*$.ajax({
            async: false,
            type: "POST",
            url: "includes/404.html",
            success: function (result) {
                $("#principal").append(result);
            },
            error: function (result) {
                console.log(result);
            }
        });*/

    }
};
function logout() {
    /*Seguridad.logout();
    var ruta = "login.php";
    window.location.href = ruta;*/
}

function miperfil() {
    //var ruta = "?modulo=verDatos";
    //window.location.href = ruta;
}
function consultaFoto(de, ext) {
    var imgDefault = "img/usuarios/default.png";
    try {
        var respuesta = {};
        $.ajax({
            async: false,
            type: "POST",
            url: "/api/ValidaImagen",
            data: JSON.stringify({ 'Ruta': "img\\usuarios\\", "Archivo": de + "." + ext, 'Ext': ext, 'ImgDefault': imgDefault }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            cache: false,
            success: function (result) {
                respuesta = result[0];
                var img = respuesta.Img + "?" + Math.random();
                $(".img-perfil").attr("src", img);
                localStorage.setItem("cosa4", encriptaDesencriptaEle(img, 1));
                return respuesta.Existe;
            },
            error: function (error) {
                respuesta = {
                    'Existe': false,
                    'Img': imgDefault + "?" + Math.random()
                };
                $(".img-perfil").attr("src", respuesta.Img);
                localStorage.setItem("cosa4", encriptaDesencriptaEle(respuesta.Img, 1));
                return respuesta.Existe;
            }
        });
        return respuesta.Existe;
    } catch (err) {
        var respuesta = {};
        respuesta = {
            'Existe': false,
            'Img': imgDefault + "?" + Math.random()
        };
        $(".img-perfil").attr("src", respuesta.Img);
        localStorage.setItem("cosa4", encriptaDesencriptaEle(respuesta.Img, 1));
        return respuesta.Existe;
    }
}
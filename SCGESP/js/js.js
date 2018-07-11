/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
function clearCache() {
    $.ajax({
        url: "",
        context: document.body,
        success: function(s, x) {
            $('html[manifest=saveappoffline.appcache]').attr('content', '');
            $(this).html(s);
        }
    });
}
function cerrarPanel(elemento) {
    //console.log(elemento);
    $(elemento).hide();
    //window.history.back();
}
function crearTablaReportes(tabla, colOrden, ascDesc, paginar, botones) {
    var controles = {
        'copy': { extend: 'copy', text: 'Copiar' },
        'csv': { extend: 'csv', text: 'Excel CSV' },
        'excel': { extend: 'excel', text: 'Excel XLSX' },
        'pdf': { extend: 'pdf', text: 'PDF' },
        'print': { extend: 'print', text: 'Imprimir' }
    };
    var control = [];
    for (var i = 0; i < botones.length; i++) {
        var boton = botones[i];
        control[i] = controles[boton];

    }
    return $(tabla).DataTable({
        dom: 'Bfrtip',
        fixedHeader: true,
        buttons: control,
        "paginate": paginar,
        "lengthMenu": [[-1, 10, 25, 50], ["Todos", "10", "25", "50"]],
        "order": [[colOrden, ascDesc]],
        "processing": true,
        "language": {
            "lengthMenu": "_MENU_ Registros Por Página",
            "zeroRecords": "No se encontraron Registros",
            "info": "Página _PAGE_ de _PAGES_ (_MAX_ Registros)",
            "infoEmpty": "No hay registros para mostrar.",
            "infoFiltered": "(_TOTAL_ Registros de _MAX_)",
            "search": "Buscar:",
            "processing": "Cargando Información",
            "decimal": ".",
            "thousands": ",",
            "paginate": {
                "first": "Primera Página",
                "last": "Ultima Página",
                "next": "Siguiente",
                "sPrevious": "Anterior"
            }
        }
    });
}
function crearTabla(tabla, colOrden, ascDesc) {
    return $(tabla).DataTable({
        "lengthMenu": [[-1, 10, 25, 50], ["Todos", "10", "25", "50"]],
        "order": [[colOrden, ascDesc]],
        "autoWidth": !1,
        "responsive": !0,
        "processing": true,
        //"scrollY": "500px",
        //"scrollCollapse": true,
        "language": {
            "lengthMenu": "_MENU_ Registros Por Página",
            "zeroRecords": "No se encontraron Registros",
            "info": "Página _PAGE_ de _PAGES_ (_MAX_ Registros)",
            "infoEmpty": "No hay registros para mostrar.",
            "infoFiltered": "(_TOTAL_ Registros de _MAX_)",
            "search": "Buscar:",
            "processing": "Cargando Información",
            "decimal": ".",
            "thousands": ",",
            "paginate": {
                "first": "Primera Página",
                "last": "Ultima Página",
                "next": "Siguiente",
                "sPrevious": "Anterior"
            }
        }

    });
}
function crearTablaDetalleRow(tabla, colOrden, ascDesc, ncolumnas, paginar) {
    var columnas = [];
    var i = 1;
    columnas[0] = {
        "className": 'details-control',
        "orderable": false,
        "data": null,
        "defaultContent": ''
    };
    for (i = 1; i <= ncolumnas; i++) {
        columnas[i] = { "data": i };
    }

    return $(tabla).DataTable({
        "paginate": paginar,
        "columns": columnas,
        "columnDefs": [
            {
                "targets": [ncolumnas],
                "visible": false,
                "searchable": false
            }
        ],
        "lengthMenu": [[-1, 25, 50], ["Todos", 25, 50]],
        "processing": true,
        "order": [[colOrden, ascDesc]],
        "language": {
            "lengthMenu": "_MENU_ Registros Por Página",
            "zeroRecords": "No se encontraron Registros",
            "info": "Página _PAGE_ de _PAGES_ (_MAX_ Registros)",
            "infoEmpty": "No hay registros para mostrar.",
            "infoFiltered": "(_TOTAL_ Registros de _MAX_)",
            "search": "Buscar:",
            "processing": "Cargando Información",
            "decimal": ".",
            "thousands": ",",
            "paginate": {
                "first": "Primera Página",
                "last": "Ultima Página",
                "next": "Siguiente",
                "sPrevious": "Anterior"
            }
        }
    });
}
function rangoFechas(repde, repa, reporte, fc) {
    repde = (repde) ? repde : "repde";
    repa = (repa) ? repa : "repa";
    reporte = (reporte) ? reporte : "reporte";
    $("input." + reporte).datepicker({
        dateFormat: "dd-mm-yy",
        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
        dayNamesShort: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"],
        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
        changeMonth: true,
        changeYear: true,
        showButtonPanel: false,
        //showWeek: true,
        //firstDay: 0,
        onSelect: function(selectedDate) {
            var elemento = this;
            var idinput = elemento.id;
            var idinput2 = (idinput === repde) ? repa : repde;
            var maxmin = (idinput2 === repde) ? "maxDate" : "minDate";
            $("#" + idinput2).datepicker("option", maxmin, selectedDate);
            if (fc !== "")
                setTimeout(fc, 100);
        },
        beforeShow: function(input, inst) {
            var calendar = inst.dpDiv;
            setTimeout(function() {
                calendar.css({
                    'z-index': 2000
                });
            }, 0);
        }
    });

    $("#" + repde + " + i + span span").removeAttr("onclick");
    $("#" + repa + " + i + span span").removeAttr("onclick");
    $("#" + repde + " + i + span span").attr("onclick", "$('#" + repde + "').datepicker('show')");
    $("#" + repa + " + i + span span").attr("onclick", "$('#" + repa + "').datepicker('show')");
}
function destroyRangoFechas(repde, repa) {
    repde = (repde) ? repde : "repde";
    repa = (repa) ? repa : "repa";

    $("#" + repde + ", #" + repde + " + span span").datepicker("destroy");
    $("#" + repa + ", #" + repa + " + span span").datepicker("destroy");

    $("#" + repde + ", #" + repde + " + span span").removeAttr("onclick");
    $("#" + repa + ", #" + repa + " + span span").removeAttr("onclick");
}
function fechaActual() {
    var f = new Date();
    var nmes = ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"];
    var ndia = ["", "01", "02", "03", "04", "05", "06", "07", "08", "09"];
    var dia = (f.getDate() < 10) ? ndia[f.getDate()] : f.getDate();
    return dia + "-" + nmes[f.getMonth()] + "-" + f.getFullYear();//dd-mm-yyyy
}
function FechaMasMenos(fecha, cant, dma, MasMenos) {//fecha: dia-mm-yyyy, dma: d = dia, m = mes, a = año, MasMenos = +-
    dma = (valorVacio(dma)) ? "d" : dma;
    MasMenos = (valorVacio(MasMenos)) ? "+" : MasMenos;
    var f = fecha.split("-");
    var now = new Date(f[2], f[1] - 1, f[0]);
    var nfecha = "";
    if (MasMenos === '+') {
        switch (dma) {
            case 'a':
                nfecha = new Date(now.getFullYear() + cant, now.getMonth(), now.getDate());
                var anio = nfecha.getFullYear();
                var mes = nfecha.getMonth() + 1;
                var dia = nfecha.getDate();
                mes = (mes < 10) ? ("0" + mes) : mes;
                dia = (dia < 10) ? ("0" + dia) : dia;
                nfecha = dia + "-" + mes + "-" + anio;
                break;
            case 'm':
                if (now.getMonth() === 11) {
                    nfecha = new Date(now.getFullYear() + cant, 0, now.getDate());
                } else {
                    nfecha = new Date(now.getFullYear(), now.getMonth() + cant, now.getDate());
                }
                var anio = nfecha.getFullYear();
                var mes = nfecha.getMonth() + 1;
                var dia = nfecha.getDate();
                mes = (mes < 10) ? ("0" + mes) : mes;
                dia = (dia < 10) ? ("0" + dia) : dia;
                nfecha = dia + "-" + mes + "-" + anio;
                break;
            case 'd':
                nfecha = sumaDias(fecha, cant);
                break;
        }
    } else {
        switch (dma) {
            case 'a':
                nfecha = new Date(now.getFullYear() - cant, now.getMonth(), now.getDate());
                var anio = nfecha.getFullYear();
                var mes = nfecha.getMonth() + 1;
                var dia = nfecha.getDate();
                mes = (mes < 10) ? ("0" + mes) : mes;
                dia = (dia < 10) ? ("0" + dia) : dia;
                nfecha = dia + "-" + mes + "-" + anio;
                break;
            case 'm':
                if (now.getMonth() === 0) {
                    nfecha = new Date(now.getFullYear() - 1, 11, now.getDate());
                } else {
                    nfecha = new Date(now.getFullYear(), now.getMonth() - cant, now.getDate());
                }
                var anio = nfecha.getFullYear();
                var mes = nfecha.getMonth() + 1;
                var dia = nfecha.getDate();
                mes = (mes < 10) ? ("0" + mes) : mes;
                dia = (dia < 10) ? ("0" + dia) : dia;
                nfecha = dia + "-" + mes + "-" + anio;
                break;
            case 'd':
                nfecha = sumaDias(fecha, (-1 * cant));
                break;
        }
    }
    return nfecha;
}
function sumaDias(fecha, d) {//fecha: dia-mm-yyyy
    var Fecha = new Date();
    var sFecha = fecha || (Fecha.getDate() + "/" + (Fecha.getMonth() + 1) + "/" + Fecha.getFullYear());
    var sep = sFecha.indexOf('/') != -1 ? '/' : '-';
    var aFecha = sFecha.split(sep);
    fecha = aFecha[2] + '/' + aFecha[1] + '/' + aFecha[0];
    fecha = new Date(fecha);
    fecha.setDate(fecha.getDate() + parseInt(d));
    var anno = fecha.getFullYear();
    var mes = fecha.getMonth() + 1;
    var dia = fecha.getDate();
    mes = (mes < 10) ? ("0" + mes) : mes;
    dia = (dia < 10) ? ("0" + dia) : dia;
    var fechaFinal = dia + sep + mes + sep + anno;
    return (fechaFinal);
}
function difDiaFecha(f1, f2) {//f1, f2: dia-mm-yyyy
    var fec1 = f1.split("-");
    var fec2 = f2.split("-");

    var fecha1 = fec1[2] + "-" + fec1[1] + "-" + fec1[0];
    var fecha2 = fec2[2] + "-" + fec2[1] + "-" + fec2[0];

    var fechaInicio = new Date(fecha1).getTime();
    var fechaFin = new Date(fecha2).getTime();

    var diff = fechaFin - fechaInicio;

    return (diff / (1000 * 60 * 60 * 24));
}
function cargando() {
    $.blockUI({
        message: "<img src='img/loader.gif' alt='' style='width:10%; height:10%' /><b> Cargando. Espere por favor.</b>",
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        },
        baseZ: 1000
    });
}
function cargado() {
    $.unblockUI();
}
function justNumbers(e) {
    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum == 8) || (keynum == 46))
        return true;

    return /\d/.test(String.fromCharCode(keynum));
}
function formatFecha(fecha, formato) {
    /* formato fecha entrada -> yyy-mm-dd hh:m:s*/
    formato = (formato === "") ? "dd-mm-yyyy" : formato;
    formato = "UTC:" + formato;
    var dateFormat = function() {
        var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
            timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
            timezoneClip = /[^-+\dA-Z]/g,
            pad = function(val, len) {
                val = String(val);
                len = len || 2;
                while (val.length < len)
                    val = "0" + val;
                return val;
            };

        // Regexes and supporting functions are cached through closure
        return function(date, mask, utc) {
            var dF = dateFormat;

            // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
            if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
                mask = date;
                date = undefined;
            }

            // Passing date through Date applies Date.parse, if necessary
            date = date ? new Date(date) : new Date;
            if (isNaN(date))
                throw SyntaxError("invalid date");

            mask = String(dF.masks[mask] || mask || dF.masks["default"]);

            // Allow setting the utc argument via the mask
            if (mask.slice(0, 4) == "UTC:") {
                mask = mask.slice(4);
                utc = true;
            }

            var _ = utc ? "getUTC" : "get",
                d = date[_ + "Date"](),
                D = date[_ + "Day"](),
                m = date[_ + "Month"](),
                y = date[_ + "FullYear"](),
                H = date[_ + "Hours"](),
                M = date[_ + "Minutes"](),
                s = date[_ + "Seconds"](),
                L = date[_ + "Milliseconds"](),
                o = utc ? 0 : date.getTimezoneOffset(),
                flags = {
                    d: d,
                    dd: pad(d),
                    ddd: dF.i18n.dayNames[D],
                    dddd: dF.i18n.dayNames[D + 7],
                    m: m + 1,
                    mm: pad(m + 1),
                    mmm: dF.i18n.monthNames[m],
                    mmmm: dF.i18n.monthNames[m + 12],
                    yy: String(y).slice(2),
                    yyyy: y,
                    h: H % 12 || 12,
                    hh: pad(H % 12 || 12),
                    H: H,
                    HH: pad(H),
                    M: M,
                    MM: pad(M),
                    s: s,
                    ss: pad(s),
                    l: pad(L, 3),
                    L: pad(L > 99 ? Math.round(L / 10) : L),
                    t: H < 12 ? "a" : "p",
                    tt: H < 12 ? "am" : "pm",
                    T: H < 12 ? "A" : "P",
                    TT: H < 12 ? "AM" : "PM",
                    Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                    o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                    S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
                };

            return mask.replace(token, function($0) {
                return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
            });
        };
    }();

    // Some common format strings
    dateFormat.masks = {
        "default": "ddd mmm dd yyyy HH:MM:ss",
        shortDate: "m/d/yy",
        mediumDate: "mmm d, yyyy",
        longDate: "mmmm d, yyyy",
        fullDate: "dddd, mmmm d, yyyy",
        shortTime: "h:MM TT",
        mediumTime: "h:MM:ss TT",
        longTime: "h:MM:ss TT Z",
        isoDate: "yyyy-mm-dd",
        isoTime: "HH:MM:ss",
        isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
        isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
    };

    // Internationalization strings
    dateFormat.i18n = {
        dayNames: [
            "Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab",
            "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado"
        ],
        monthNames: [
            "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic",
            "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
        ]
    };

    // For convenience...
    Date.prototype.format = function(mask, utc) {
        return dateFormat(this, mask, utc);
    };

    fecha = new Date(fecha);
    var nfecha = fecha.format(formato);
    return nfecha;
}
var formatNumber = {
    separador: ",", // separador para los miles
    sepDecimal: '.', // separador para los decimales
    formatear: function(num) {
        num += '';
        var splitStr = num.split('.');
        var splitLeft = splitStr[0];
        var splitRight = splitStr.length > 1 ? this.sepDecimal + splitStr[1] : '';
        var regx = /(\d+)(\d{3})/;
        while (regx.test(splitLeft)) {
            splitLeft = splitLeft.replace(regx, '$1' + this.separador + '$2');
        }
        return this.simbol + splitLeft + splitRight;
    },
    new: function(num, simbol) {
        this.simbol = simbol || '';
        return this.formatear(num);
    }
};
var url = {
    urlActual: function() {
        var URLactual = $(location).attr('href');
        return URLactual; //url actual = protocolo://dominio/directorio/pagina.html?key=value#fragmento
    },
    pathName: function() {
        var pathname = window.location.pathname;
        return pathname; //pathname = protocolo://dominio/directorio/pagina.html
    },
    absolutePath: function() {
        var loc = window.location;
        var pathName = loc.pathname.substring(0, loc.pathname.lastIndexOf('/') + 1);
        var absolutePath = loc.href.substring(0, loc.href.length - ((loc.pathname + loc.search + loc.hash).length - pathName.length));
        return absolutePath; //absolutePath = protocolo://dominio/directorio/
    },
    domain: function() {
        var domain = window.location.host;
        return domain; //domain = dominio.com
    },
    get: function(variable) {
        var results = { 0: "", 1: "" };
        var ruta = this.urlActual();
        if (ruta.lastIndexOf('?') > 0 || ruta.lastIndexOf('&') > 0) {
            results = new RegExp('[\\?&]' + variable + '=([^&#]*)').exec(ruta);
        }
        return results[1];
    },
    hash: function() {
        var hash = window.location.hash;
        return hash; //hash = #fragmento
    },
    pagName: function() {
        var ruta = this.urlActual();
        var pag = ruta.substring(ruta.lastIndexOf('/') + 1);
        if (pag.lastIndexOf('?') > 0)
            pag = pag.substring(0, pag.lastIndexOf('?'));
        if (pag.lastIndexOf('#') > 0)
            pag = pag.substring(0, pag.lastIndexOf('#'));
        return pag;
    },
    extPagName: function() {
        var ext = this.pagName();
        ext = ext.split(".");
        ext = ext[1];
        return ext;
    },
    pagNameSinExt: function() {
        var pag = this.pagName().replace("." + this.extPagName(), "");
        return pag;
    },
    cge: function() {
        var cge = "http://sceg.gapp.mx";
        return cge;
    }
};
function valorVacio(valor) {
    valor = (valor + "").toString();
    var ok = true;
    if (valor === null || valor === 'null' || valor === '' || valor === 'undefined' || valor.length === 0 || /^\s+$/.test(valor)) {
        ok = true;
    } else {
        ok = false;
    }
    return ok;
}
function ValidaRFC(rfc) {
    var frfc = /^([A-ZÑ\x26]{3,4}(-)?([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))((-)?([A-Z\d]{3}))?$/;
    var ok = false;
    if (frfc.test(rfc)) {
        ok = true;
    }
    return ok;
}
function ValidaEMail(email) {
    var ok = false;
    var femail = /[\w-\.]{2,}@([\w-]{2,}\.)*([\w-]{2,}\.)[\w-]{2,4}/;
    if (femail.test(email)) {
        ok = true;
    }
    return ok;
}
function ValidaTelefono(telefono) {
    var ok = false;
    var ftelefono = /^\d{3}(-)?\d{3}(-)?\d{4}$/;
    if (ftelefono.test(telefono)) {
        ok = true;
    }
    return ok;
}
function validarNumero(numero) {
    var ok = false;
    var fnumero = /^(\d|-)?(\d|,)*\.?\d*$/;
    // /^([0-9]+([.][0-9]+)?)*$/;
    if (fnumero.test(numero)) {
        ok = true;
    }
    return ok;
}
function SiNo(sn) {
    sn = (sn === 1) ? "<span style='font-size: 11px' class='label label-success'><span class='glyphicon glyphicon-ok'></span> SI</span>" :
        "<span style='font-size: 11px' class='label label-danger'><span class='glyphicon glyphicon-remove'></span> NO</span>";
    return sn;

}
function SiNo2(sn) {
    sn = (sn === 1) ? "<span style='font-size: 11px' class='label label-success'><span class='glyphicon glyphicon-ok'></span> </span>" :
        "<span style='font-size: 11px' class='label label-danger'><span class='glyphicon glyphicon-remove'></span> </span>";
    return sn;

}
function chk(id, nombre, checked, onchange, valor, size, tTrue, tFalse, otros) {
    var checkbox = "";//width
    checkbox += "<div class='form-group' style='height: 10px'>";
    checkbox += "<input type='checkbox' id='" + id + "' name='" + nombre + "' " + checked + " onchange='" + onchange + "' value='" + valor + "' autocomplete='off' />";
    checkbox += "<div class='btn-group btn-chk'>";
    checkbox += "<label id='lbl" + id + "' for='" + id + "' " + otros + " style='cursor: pointer;'>";
    checkbox += "<span style='height: 10px'><i class='alert-" + tTrue + " glyphicon glyphicon-ok' style='font-size:" + size + "px'></i></span>";
    checkbox += "<span style='height: 10px'><i class='alert-" + tFalse + " glyphicon glyphicon-remove' style='font-size:" + size + "px'></i></span>";
    checkbox += "</label>";
    checkbox += "</div>";
    checkbox += "</div>";
    return checkbox;
}
function chk2(id, nombre, checked, onchange, valor, size, tTrue, tFalse, otros) {
    tFalse = tFalse ? tFalse : "dafault";
    tTrue = tTrue ? tTrue : "dafault";
    var checkbox = "";//width
    checkbox += "<div class='form-group' style='height: 10px'>";
    checkbox += "<input type='checkbox' id='" + id + "' name='" + nombre + "' " + checked + " onchange='" + onchange + "' value='" + valor + "' autocomplete='off' />";
    checkbox += "<div class='btn-group btn-chk'>";
    checkbox += "<label id='lbl" + id + "' for='" + id + "' " + otros + " style='cursor: pointer;'>";
    checkbox += "<span style='height: 10px'><i class='alert-" + tTrue + " glyphicon  glyphicon-check' style='font-size:" + size + "px'></i></span>";
    checkbox += "<span style='height: 10px'><i class='alert-" + tFalse + " glyphicon glyphicon-unchecked' style='font-size:" + size + "px'></i></span>";
    checkbox += "</label>";
    checkbox += "</div>";
    checkbox += "</div>";
    return checkbox;
}
Array.prototype.unique = function(a) {
    return function() {
        return this.filter(a)
    }
}(function(a, b, c) {
    return c.indexOf(a, b + 1) < 0
});
String.prototype.AsHTML = function(val) {
    var elemento = this.toString();
    $(elemento).empty();
    $(elemento).append(val);
};
function xmlToJson(xml) {

    // Create the return object
    var obj = {};

    if (xml.nodeType === 1) { // element
        // do attributes
        if (xml.attributes.length > 0) {
            obj["@attributes"] = {};
            for (var j = 0; j < xml.attributes.length; j++) {
                var attribute = xml.attributes.item(j);
                obj["@attributes"][attribute.nodeName] = attribute.nodeValue;
            }
        }
    } else if (xml.nodeType === 3) { // text
        obj = xml.nodeValue;
    }

    // do children
    if (xml.hasChildNodes()) {
        for (var i = 0; i < xml.childNodes.length; i++) {
            var item = xml.childNodes.item(i);
            var nodeName = item.nodeName;
            if (typeof (obj[nodeName]) === "undefined") {
                obj[nodeName] = xmlToJson(item);
            } else {
                if (typeof (obj[nodeName].push) === "undefined") {
                    var old = obj[nodeName];
                    obj[nodeName] = [];
                    obj[nodeName].push(old);
                }
                obj[nodeName].push(xmlToJson(item));
            }
        }
    }
    return obj;
}
function padi(n, length) {
    n = n.toString();
    while (n.length < length)
        n = "0" + n;
    return n;
}
function padr(n, length) {
    n = n.toString();
    while (n.length < length)
        n = n + "0";
    return n;
}
function datoFiscal(varArray, varKey1, varKey2) {
    var variable = "";

    try {
        if (!valorVacio(varArray[varKey1])) {
            variable = varArray[varKey1];
        } else if (!valorVacio(varArray[varKey2])) {
            variable = varArray[varKey2];
        }
    } catch (err) {
        variable = "";
    }

    return variable;
}
function getInfPc() {
    window.RTCPeerConnection = window.RTCPeerConnection || window.mozRTCPeerConnection || window.webkitRTCPeerConnection; //compatibility for firefox and chrome
    var pc = new RTCPeerConnection({
        iceServers: []
    }),
        noop = function() { };
    pc.createDataChannel(""); //create a bogus data channel
    pc.createOffer(pc.setLocalDescription.bind(pc), noop); // create offer and set local description
    pc.onicecandidate = function(ice) { //listen for candidate events
        if (!ice || !ice.candidate || !ice.candidate.candidate)
            return;
        var myIP = /([0-9]{1,3}(\.[0-9]{1,3}){3}|[a-f0-9]{1,4}(:[a-f0-9]{1,4}){7})/.exec(ice.candidate.candidate)[1];
        pc.onicecandidate = noop;
    };
    console.log(navigator, pc);
}
function bloqueaTeclado(e) {
    var tecla = (document.all) ? e.keyCode : e.which;
    var patron = /1/; //ver nota
    var te = String.fromCharCode(tecla);
    return patron.test(te);
}
function verFacturaJSONenHTML(xmljson, idproyecto, idinforme, idgasto) {
    var version = 0;
    var emisor = xmljson['cfdi:Comprobante']['cfdi:Emisor'];
    var domEmisor = {};
    try {
        domEmisor = emisor['cfdi:DomicilioFiscal']['@attributes'];
    } catch (err) {
        domEmisor = {};
    }

    var regimenFiscal = {};
    try {
        regimenFiscal = emisor['cfdi:RegimenFiscal']['@attributes'].Regimen;
    } catch (err) {
        regimenFiscal = {};
    }
    var receptor = xmljson['cfdi:Comprobante']['cfdi:Receptor'];

    var domreceptor = {};
    try {
        domreceptor = receptor['cfdi:Domicilio']['@attributes'];
    } catch (err) {
        domreceptor = {};
    }

    var complemento = {};
    try {
        complemento = xmljson['cfdi:Comprobante']['cfdi:Complemento']['tfd:TimbreFiscalDigital']['@attributes'];
    } catch (err) {
        complemento = {};
    }

    var comprobante = {};
    try {
        comprobante = xmljson['cfdi:Comprobante']['@attributes'];
    } catch (err) {
        comprobante = {};
    }

    var impTraslados = {};
    try {
        impTraslados = xmljson['cfdi:Comprobante']['cfdi:Impuestos']['cfdi:Traslados']['cfdi:Traslado'];
    } catch (err) {
        impTraslados = {};
    }

    var impTrasladosLoc = {};
    try {
        impTrasladosLoc = xmljson['cfdi:Comprobante']['cfdi:Complemento']['implocal:ImpuestosLocales']['implocal:TrasladosLocales'];
    } catch (err) {
        impTrasladosLoc = {};
    }

    var conceptos = {};
    try {
        conceptos = xmljson['cfdi:Comprobante']['cfdi:Conceptos']['cfdi:Concepto'];
    } catch (err) {
        conceptos = {};
    }


    try {
        version = datoFiscal(comprobante, 'version', 'Version') * 1;
        if (version > 0) {
            "#version".AsHTML("Versi&oacute;n: " + version + " &nbsp;");
        } else {
            $("#version").empty();
        }
    } catch (err) {
        $("#version").empty();
    }

    if (version >= 3.3) {
        "#lblformaPago".AsHTML("Método de Pago:");
        "#lblmetodoPago".AsHTML("Forma de Pago:");
    } else {
        "#lblformaPago".AsHTML("Forma de Pago:");
        "#lblmetodoPago".AsHTML("Método de Pago:");
    }

    var uuid = complemento.UUID.toUpperCase();
    var re = datoFiscal(emisor['@attributes'], 'rfc', 'Rfc');
    var rr = datoFiscal(receptor['@attributes'], 'rfc', 'Rfc');
    var serie = datoFiscal(comprobante, 'serie', 'Serie');
    var folio = datoFiscal(comprobante, 'folio', 'Folio');
    var TipoCambio = datoFiscal(comprobante, 'tipoCambio', 'TipoCambio');
    if (!valorVacio(TipoCambio)) {
        TipoCambio = (TipoCambio * 1).toFixed(2);
    }
    var enoCertificado = datoFiscal(comprobante, 'noCertificado', 'NoCertificado');
    var eCalle = datoFiscal(domEmisor, 'calle', 'Calle');
    var eNoExterior = datoFiscal(domEmisor, 'noExterior', 'NoExterior');
    var eColonia = datoFiscal(domEmisor, 'colonia', 'Colonia');
    var eMunicipio = datoFiscal(domEmisor, 'municipio', 'Municipio');
    var eEstado = datoFiscal(domEmisor, 'estado', 'Estado');
    var ePais = datoFiscal(domEmisor, 'pais', 'Pais');
    var eCodigoPostal = datoFiscal(domEmisor, 'codigoPostal', 'CodigoPostal');

    var fecha = datoFiscal(comprobante, 'fecha', 'Fecha');
    var tipoDeComprobante = datoFiscal(comprobante, 'tipoDeComprobante', 'TipoDeComprobante');

    if (tipoDeComprobante === "I") {
        tipoDeComprobante = "Ingreso";
    } else if (tipoDeComprobante === "E") {
        tipoDeComprobante = "Egreso";
    } else if (tipoDeComprobante === "N") {
        tipoDeComprobante = "Nómina";
    }

    var rCalle = datoFiscal(domreceptor, 'calle', 'Calle');
    var rNoExterior = datoFiscal(domreceptor, 'noExterior', 'NoExterior');
    var rColonia = datoFiscal(domreceptor, 'colonia', 'Colonia');
    var rMunicipio = datoFiscal(domreceptor, 'municipio', 'Municipio');
    var rEstado = datoFiscal(domreceptor, 'estado', 'Estado');
    var rPais = datoFiscal(domreceptor, 'pais', 'Pais');
    var rCodigoPostal = datoFiscal(domreceptor, 'codigoPostal', 'CodigoPostal');

    "#nmbEmisor".AsHTML(datoFiscal(emisor['@attributes'], 'nombre', 'Nombre'));
    "#rfcEmisor".AsHTML(re);
    "#folioFiscal".AsHTML(uuid);
    "#noSerie".AsHTML(enoCertificado);
    "#domFiscalEmisor".AsHTML("Calle " + eCalle +
        " No. Exterior " + eNoExterior +
        " Colonia " + eColonia +
        " Municipio " + eMunicipio +
        " Estado " + eEstado +
        " " + ePais +
        " CP. " + eCodigoPostal);
    "#lugarHora".AsHTML(comprobante.LugarExpedicion + "<br />" + fecha);
    "#efectoComprobante".AsHTML(tipoDeComprobante);
    "#rfcReceptor".AsHTML(rr);
    "#folioSerie".AsHTML(serie + " " + folio);
    "#nmbRemisor".AsHTML(datoFiscal(receptor['@attributes'], 'nombre', 'Nombre'));
    "#regimenFiscal".AsHTML(regimenFiscal);
    "#domFiscalReceptor".AsHTML("Calle " + rCalle +
        " No. Exterior " + rNoExterior +
        " Colonia " + rColonia +
        " Municipio " + rMunicipio +
        " Estado " + rEstado +
        " " + rPais +
        " CP. " + rCodigoPostal);

    var formaDePago = datoFiscal(comprobante, 'formaDePago', 'FormaDePago');
    if (valorVacio(formaDePago)) {
        formaDePago = datoFiscal(comprobante, 'formaDePago', 'FormaPago');
    }
    var metodoDePago = datoFiscal(comprobante, 'metodoDePago', 'MetodoDePago');
    if (valorVacio(metodoDePago)) {
        metodoDePago = datoFiscal(comprobante, 'metodoDePago', 'MetodoPago');
    }
    var textMetodoDePago = "";
    var TextFPSAT = {
        '01': 'Efectivo',
        '02': 'Cheque nominativo',
        '03': 'Transferencia electrónica de fondos',
        '04': 'Tarjeta de crédito',
        '05': 'Monedero electrónico',
        '06': 'Dinero electrónico',
        '08': 'Vales de despensa',
        '12': 'Dación de pago',
        '13': 'Pago por subrogación',
        '14': 'Pago por consignación',
        '15': 'Condonación',
        '17': 'Compensación',
        '23': 'Novación',
        '24': 'Confusión',
        '25': 'Remisión de deuda',
        '26': 'Prescripción o caducidad',
        '27': 'A satisfacción del acreedor',
        '28': 'Tarjeta de débito',
        '29': 'Tarjeta de servicios',
        '30': 'Aplicación de anticipos',
        '99': 'Por definir'
    };
    if (version >= 3.3) {
        textMetodoDePago = "Efectivo"; //selectTextFPSAT(formaDePago);hacer adecuacion para tomar las fos formas de pago ABR
        textMetodoDePago = TextFPSAT[formaDePago];
    } else {
        textMetodoDePago = "Efectivo"; //selectTextFPSAT(metodoDePago);
    }
    textMetodoDePago = valorVacio(textMetodoDePago) ? "" : (" - " + textMetodoDePago);
    var condicionesDePago = datoFiscal(comprobante, 'condicionesDePago', 'CondicionesDePago');
    var selloCFD = datoFiscal(complemento, 'selloCFD', 'SelloCFD');
    var selloSAT = datoFiscal(complemento, 'selloSAT', 'SelloSAT');
    var sello = datoFiscal(comprobante, 'sello', 'Sello');
    var noCertificadoSAT = datoFiscal(complemento, 'noCertificadoSAT', 'NoCertificadoSAT');

    "#moneda".AsHTML(comprobante.Moneda);
    "#formaPago".AsHTML((version < 3.3) ? formaDePago : metodoDePago);
    "#metodoPago".AsHTML(((version >= 3.3) ? formaDePago : metodoDePago) + textMetodoDePago);
    "#nCuentaPago".AsHTML(comprobante.NumCtaPago);
    "#tipoCambio".AsHTML(TipoCambio);
    "#condPago".AsHTML(condicionesDePago);
    "#SD_CFDI".AsHTML(selloCFD);
    "#S_SAT".AsHTML(selloSAT);
    "#COCCD_SAT".AsHTML(sello);
    "#noSC_SAT".AsHTML(noCertificadoSAT);
    "#FH_certificacion".AsHTML(complemento.FechaTimbrado);

    $("#tblConceptosXML tbody").empty();
    var row = "";
    if (conceptos.length > 0) {
        for (var i = 0; i < (conceptos.length); i++) {
            var concepto = conceptos[i]['@attributes'];
            var cantidad = datoFiscal(concepto, 'cantidad', 'Cantidad') * 1;
            var unidad = datoFiscal(concepto, 'unidad', 'Unidad');
            var descripcion = datoFiscal(concepto, 'descripcion', 'Descripcion');
            var valorUnitario = datoFiscal(concepto, 'valorUnitario', 'ValorUnitario') * 1;
            var importe = datoFiscal(concepto, 'importe', 'Importe') * 1;

            cantidad = cantidad.toFixed(2);

            row = "<tr>";
            row += "<td>" + cantidad + "</td>";
            row += "<td>" + unidad + "</td>";
            row += "<td>" + descripcion + "</td>";
            row += "<td align='right'>" + formatNumber.new(valorUnitario.toFixed(2), "$") + "</td>";
            row += "<td align='right'>" + formatNumber.new(importe.toFixed(2), "$") + "</td>";
            row += "</tr>";
            $("#tblConceptosXML tbody").append(row);
        }
    } else {
        concepto = xmljson['cfdi:Comprobante']['cfdi:Conceptos']['cfdi:Concepto']['@attributes'];
        var cantidad = datoFiscal(concepto, 'cantidad', 'Cantidad') * 1;
        var unidad = datoFiscal(concepto, 'unidad', 'Unidad');
        var descripcion = datoFiscal(concepto, 'descripcion', 'Descripcion');
        var valorUnitario = datoFiscal(concepto, 'valorUnitario', 'ValorUnitario') * 1;
        var importe = datoFiscal(concepto, 'importe', 'Importe') * 1;

        row = "<tr>";
        row += "<td>" + cantidad.toFixed(2) + "</td>";
        row += "<td>" + unidad + "</td>";
        row += "<td>" + descripcion + "</td>";
        row += "<td align='right'>" + formatNumber.new(valorUnitario.toFixed(2), "$") + "</td>";
        row += "<td align='right'>" + formatNumber.new(importe.toFixed(2), "$") + "</td>";
        row += "</tr>";
        $("#tblConceptosXML tbody").append(row);
    }
    var subtotal = datoFiscal(comprobante, 'subTotal', 'SubTotal') * 1;

    "#subtotal".AsHTML(formatNumber.new(subtotal.toFixed(2), "$ "));
    for (var i = 0; i <= 6; i++) {
        var tdimp = "#imp" + i;
        var tdLblImp = "#lblImp" + i;
        $(tdimp).empty();
        $(tdLblImp).empty();
    }
    var nimp = 1;
    if (impTraslados.length > 0) {
        var aImpuesto = [];
        var aImporte = [];
        var pos = 0;
        for (var i = 0; i < (impTraslados.length); i++) {
            var importe = datoFiscal(impTraslados[i]['@attributes'], 'importe', 'Importe') * 1;
            var impuesto = datoFiscal(impTraslados[i]['@attributes'], 'impuesto', 'Impuesto');
            if (impuesto === "001") {
                impuesto = "ISR";
            } else if (impuesto === "002") {
                impuesto = "IVA";
            } else if (impuesto === "003") {
                impuesto = "IEPS";
            }

            var tasa = (datoFiscal(impTraslados[i]['@attributes'], 'tasa', 'TasaOCuota') * 1).toFixed(2);

            if (importe > 0) {
                var nmbImpuesto = (impuesto + " " + tasa + "%");
                aImporte[nmbImpuesto] = ((aImporte[nmbImpuesto]) ? aImporte[nmbImpuesto] : 0) + importe;
                aImpuesto[pos] = nmbImpuesto;
                pos += 1;
            }
        }
        var nmbImpuestos = aImpuesto.unique();
        var nImpuestos = nmbImpuestos.length;

        for (var i = 0; i < (nImpuestos); i++) {
            var tdimp = "#imp" + nimp;
            var tdLblImp = "#lblImp" + nimp;
            var impuesto = nmbImpuestos[i];
            var importe = aImporte[impuesto];
            tdimp.AsHTML(formatNumber.new(importe.toFixed(2), "$ "));
            tdLblImp.AsHTML(impuesto);
            nimp += 1;
        }
    } else {
        var nimp = 1;
        var importe = datoFiscal(impTraslados['@attributes'], 'importe', 'Importe') * 1;
        var impuesto = datoFiscal(impTraslados['@attributes'], 'impuesto', 'Impuesto').toUpperCase();

        if (impuesto === "001") {
            impuesto = "ISR";
        } else if (impuesto === "002") {
            impuesto = "IVA";
        } else if (impuesto === "003") {
            impuesto = "IEPS";
        }

        var tasa = (datoFiscal(impTraslados['@attributes'], 'tasa', 'TasaOCuota') * 1).toFixed(2);
        /*if (impuesto === "IVA" && importe > 0) {
         tasa = tasa * ((tasa > 0 && tasa < 1) ? 100 : 1);
         "#iva".AsHTML(formatNumber.new(importe.toFixed(2), "$ "));
         "#lblIVA".AsHTML(impuesto + " " + tasa + "%");
         }*/
        if (importe > 0) {
            var tdimp = "#imp" + nimp;
            var tdLblImp = "#lblImp" + nimp;
            tasa = tasa * ((tasa > 0 && tasa < 1) ? 100 : 1);
            tdimp.AsHTML(formatNumber.new(importe.toFixed(2), "$ "));
            tdLblImp.AsHTML(impuesto + " " + tasa + "%");
            nimp += 1;
        }
    }
    //impuestos locales
    if (impTrasladosLoc.length > 0) {
        var aImpuesto = [];
        var aImporte = [];
        var pos = 0;
        for (var i = 0; i < (impTrasladosLoc.length); i++) {
            var importe = datoFiscal(impTraslados[i]['@attributes'], 'importe', 'Importe') * 1;
            var impuesto = datoFiscal(impTraslados[i]['@attributes'], 'imploctrasladado', 'ImpLocTrasladado');

            var tasa = (datoFiscal(impTraslados[i]['@attributes'], 'tasadetraslado', 'TasadeTraslado') * 1).toFixed(2);

            if (importe > 0) {
                var nmbImpuesto = (impuesto + " " + tasa + "%");
                aImporte[nmbImpuesto] = ((aImporte[nmbImpuesto]) ? aImporte[nmbImpuesto] : 0) + importe;
                aImpuesto[pos] = nmbImpuesto;
                pos += 1;
            }
        }
        var nmbImpuestos = aImpuesto.unique();
        var nImpuestos = nmbImpuestos.length;
        for (var i = 0; i < (nImpuestos); i++) {
            var tdimp = "#imp" + nimp;
            var tdLblImp = "#lblImp" + nimp;
            var impuesto = nmbImpuestos[i];
            var importe = aImporte[impuesto];
            tdimp.AsHTML(formatNumber.new(importe.toFixed(2), "$ "));
            tdLblImp.AsHTML(impuesto);
            nimp += 1;
        }
    } else {
        if (!valorVacio(impTrasladosLoc)) {
            var importe = datoFiscal(impTrasladosLoc['@attributes'], 'importe', 'Importe') * 1;
            var impuesto = datoFiscal(impTrasladosLoc['@attributes'], 'imploctrasladado', 'ImpLocTrasladado').toUpperCase();
            var tasa = (datoFiscal(impTrasladosLoc['@attributes'], 'tasadetraslado', 'TasadeTraslado') * 1).toFixed(2);
            if (importe > 0) {
                var tdimp = "#imp" + nimp;
                var tdLblImp = "#lblImp" + nimp;
                tasa = tasa * ((tasa > 0 && tasa < 1) ? 100 : 1);
                tdimp.AsHTML(formatNumber.new(importe.toFixed(2), "$ "));
                tdLblImp.AsHTML(impuesto + " " + tasa + "%");
                nimp += 1;
            }
        }
    }

    var total = datoFiscal(comprobante, 'total', 'Total') * 1;
    "#total".AsHTML(formatNumber.new(total.toFixed(2), "$ "));
    /*$("#qrxml").qrcode({
     render:'canvas',
     width: 150,
     height: 150,
     color: '#3A3',
     text: 'hrc'
     });*/
    var t1c = 0, t2c = 0;
    if (total.toString().indexOf(".") >= 0) {
        var t = total.toString().split(".");
        var t1 = t[0], t2 = t[1];
        t1c = padi(t1, 10);
        t2c = padr(t2, 6);
    } else {
        t1c = padi(total, 10);
        t2c = padr(0, 6);
    }
    var tt = t1c + "." + t2c;

    ////var datos = {'idproyecto': idproyecto, 'idinforme': idinforme, 'idgasto': idgasto, 'id': uuid, 're': re, 'rr': rr, 'tt': tt};
    //////////qrxml(datos); abr Hacer codigo QR del lado del servidor
    //  "#totalLetra".AsHTML(totalLetra(total));
}
var arr = {
    inarray: function(valor, valores) {
        var existe = true;
        if ($.inArray(valor, valores) < 0)
            existe = false;
        return existe;
    },
    countElement: function(valor, valores) {
        var count = 0;
        $.each(valores, function(i, v) {
            if (v === valor)
                count++;
        });
        return count;
    }
};
function inputHora(opcion) {

    var input = "";
    input += "<div class='form-group'>";
    input += "<div class='input-group date'>";
    input += "<input type='text' id='" + opcion.id + "' ";
    input += " class='form-control time-picker ";
    input += valorVacio(opcion.clases) ? "" : opcion.clases;
    input += "' value='" + (valorVacio(opcion.valor) ? "" : opcion.valor) + "' ";

    input += valorVacio(opcion.onchange) ? "" : " onchange='" + opcion.onchange + "'";

    input += valorVacio(opcion.onclick) ? "" : " onclick='" + opcion.onclick + "'";

    input += valorVacio(opcion.otros) ? "" : " " + opcion.otros;

    input += " />";

    input += "<label class='input-group-addon' for='" + opcion.id + "'>";
    input += "<i class='zmdi zmdi-time zmdi-hc-2x'></i>";
    input += "</label>";
    input += "<i class='form-group__bar'></i>";
    input += "</div>";
    input += "</div>";
    return input;
}
function horaActual(formato) {//siempre en 24hr
    if (!valorVacio(formato)) {
        var form = formato.split(":");
        var tiempo = new Date();
        var hora = tiempo.getHours();
        var minuto = tiempo.getMinutes();
        var segundo = tiempo.getSeconds();

        var tiempo = { "hh": padi(hora, 2), "mm": padi(minuto, 2), "ss": padi(segundo, 2) };
        var largo = { 1: 2, 2: 5, 3: 8 };
        var valor = "";
        for (var i = 0; i < form.length; i++) {
            valor += valorVacio(tiempo[form[i]]) ? "" : tiempo[form[i]] + ":";
        }
        valor = valor.substr(0, largo[form.length]);
    } else {
        valor = "error";
    }
    return valor;
}
function datoEle(dato) {
    var valor = "";
    try {
        valor = valorVacio(dato) ? "" : dato;
    } catch (err) {
        valor = "";
    }
    return valor;
}
function encriptaDesencriptaEle(valor, ed) {
    //valor = cadena, ed = 0 para desencriptar y 1 para encripatar
    var resultado = "";
    $.ajax({
        async: false,
        type: "POST",
        url: '/api/EncriptaDesencripta',
        data: JSON.stringify({ 'valor': valor, 'variable': ed }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        cache: false,
        beforeSend: function() {
            //cargado();
        },
        success: function(result) {
            //console.log("success", result);
            resultado = result;
        },
        complete: function() {
            //cargado();
        },
        error: function(result) {
            //cargado();
            resultado = "";
            console.log("error", result);
        }
    });
    return resultado;
}
$.fn.clearFields = $.fn.clearInputs = function (includeHidden) {
    var re = /^(?:color|date|datetime|email|month|number|password|range|search|tel|text|time|url|week)$/i; // 'hidden' is not in this list
    return this.each(function () {
        var t = this.type, tag = this.tagName.toLowerCase();
        if (re.test(t) || tag == 'textarea') {
            this.value = '';
        }
        else if (t == 'checkbox' || t == 'radio') {
            this.checked = false;
        }
        else if (tag == 'select') {
            this.selectedIndex = -1;
        }
        else if (t == "file") {
            if (/MSIE/.test(navigator.userAgent)) {
                $(this).replaceWith($(this).clone(true));
            } else {
                $(this).val('');
            }
        }
        else if (includeHidden) {
            // includeHidden can be the value true, or it can be a selector string
            // indicating a special test; for example:
            //  $('#myForm').clearForm('.special:hidden')
            // the above would clean hidden inputs that have the class of 'special'
            if ((includeHidden === true && /hidden/.test(t)) ||
                (typeof includeHidden == 'string' && $(this).is(includeHidden)))
                this.value = '';
        }
    });
};
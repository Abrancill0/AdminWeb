﻿using Ele.Generales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Xml;

namespace SCGESP.Clases
{
	public class EnvioCorreosELE
	{

		public class AutorizaRequisicionResult
		{
			public int Resultado { get; set; }
		}

		public static XmlDocument Envio(string Usuario, string correo, string EmpleadoID, string UsuarioID, string correoCopia, string Asunto, string Mensaje, int usuEncriptado)
		{
			try
			{
				string UsuarioDesencripta = "";
				if (usuEncriptado == 1)
				{
					UsuarioDesencripta = Clases.Seguridad.DesEncriptar(Usuario);
				}
				else
				{
					UsuarioDesencripta = Usuario;
				}

				if (correo == "" && EmpleadoID == "" && UsuarioID == "")
				{
					XmlDocument xml = new XmlDocument();
					xml.LoadXml("<Error>No se cuenta con un correo o con una forma para recuperarlo.</Error>");
					return xml;
				}

				DocumentoEntrada entrada = new DocumentoEntrada
				{
					Usuario = UsuarioDesencripta,
					Origen = "AdminWEB",
					Transaccion = 3
				};

				string Empleado = "";
				string Correo = "";

				DataTable DTCorreo = new DataTable();

				if (correo == "")
				{
					if (EmpleadoID == "")
					{
						Empleado = ObtieneEmpelado(UsuarioID, UsuarioDesencripta);

						Correo = ObtieneCorreo(Empleado, UsuarioDesencripta);
					}
					else
					{
						Correo = ObtieneCorreo(EmpleadoID, UsuarioDesencripta);
					}

				}
				else
				{
					Correo = correo;
				}
				//Correo = "hector.ramos@trascenti.com";
				try
				{
					bool correoOK = Enviar_correo_html(Correo, Asunto, Mensaje);
					XmlDocument xml = new XmlDocument();
					if (correoOK)
					{
						xml.LoadXml("<resultado>Correo Enviado</resultado>");
					}
					else
					{
						correoOK = Enviar_correo_html_comprogapp(Correo, Asunto, Mensaje);
						if (correoOK)
						{
							xml.LoadXml("<resultado>Correo Enviado (comprogapp)</resultado>");
						}
						else
						{
							entrada.agregaElemento("Para", Correo);
							entrada.agregaElemento("Copia", correoCopia);
							entrada.agregaElemento("Asunto", Asunto);
							entrada.agregaElemento("Mensaje", Mensaje);
							DocumentoSalida respuesta = PeticionGeneral(entrada.Documento);
							if (respuesta.Resultado == "1")
							{
								return respuesta.Documento;
							}
							else
							{
								return respuesta.Documento;
							}
						}
					}

					return xml;
				}
				catch (Exception)
				{
					entrada.agregaElemento("Para", Correo);
					entrada.agregaElemento("Copia", correoCopia);
					entrada.agregaElemento("Asunto", Asunto);
					entrada.agregaElemento("Mensaje", Mensaje);
					DocumentoSalida respuesta = PeticionGeneral(entrada.Documento);
					if (respuesta.Resultado == "1")
					{
						return respuesta.Documento;
					}
					else
					{
						return respuesta.Documento;
					}
				}
			}
			catch (System.Exception ex)
			{
				XmlDocument xml = new XmlDocument();
				xml.LoadXml("<Error>" + ex.ToString() + "</Error>");
				return xml;
			}
		}

		public static DocumentoSalida PeticionGeneral(XmlDocument doc)
		{
			Localhost.Elegrp ws = new Localhost.Elegrp();
			ws.Timeout = -1;
			string respuesta = ws.PeticionGeneral(doc);
			return new DocumentoSalida(respuesta);
		}

		public static string ObtieneEmpelado(string UsuarioID, string UsuarioDesencriptado)
		{

			DocumentoEntrada entrada = new DocumentoEntrada
			{
				Usuario = UsuarioDesencriptado,
				Origen = "Programa CGE",  //Datos.Origen; 
				Transaccion = 100004,
				Operacion = 6//verifica si existe una llave y regresa una tabla de un renglon con todos los campos de la tabla
			};
			entrada.agregaElemento("SgUsuId", UsuarioID);

			DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

			DataTable DTEmpleado = new DataTable();

			string EmpleadoResult = "";

			if (respuesta.Resultado == "1")
			{

				DTEmpleado = respuesta.obtieneTabla("Llave");
				for (int i = 0; i < DTEmpleado.Rows.Count; i++)
				{
					EmpleadoResult = Convert.ToString(DTEmpleado.Rows[i]["SgUsuEmpleado"]);  // Convert.ToString(DTEmpleado.Rows[i]["GrEmpId"]);
				}
				string nEmp = respuesta.obtieneValor("SgUsuEmpleado");
				if (EmpleadoResult.Trim() == "") {
					EmpleadoResult = nEmp.Trim();
				}
				return EmpleadoResult;

			}
			else
			{
				var errores = respuesta.Errores;

				return "";
			}
		}

		public static string ObtieneCorreo(string Empelado, string UsuarioDesencriptado)
		{
			string UsuarioDesencripta = Seguridad.DesEncriptar(UsuarioDesencriptado);

			DocumentoEntrada entrada = new DocumentoEntrada
			{
				Usuario = UsuarioDesencripta,
				Origen = "Programa CGE",  //Datos.Origen; 
				Transaccion = 120037,
				Operacion = 6//verifica si existe una llave y regresa una tabla de un renglon con todos los campos de la tabla
			};
			entrada.agregaElemento("GrEmpId", Empelado);

			DocumentoSalida respuesta = PeticionCatalogo(entrada.Documento);

			DataTable DTCorreo = new DataTable();
			string CorreoResult = "";

			if (respuesta.Resultado == "1")
			{
				DTCorreo = respuesta.obtieneTabla("Llave");

				for (int i = 0; i < DTCorreo.Rows.Count; i++)
				{
					CorreoResult = Convert.ToString(DTCorreo.Rows[i]["GrEmpCorreoElectronico"]);
				}

				return CorreoResult;
			}
			else
			{
				var errores = respuesta.Errores;

				return null;
			}
		}

		public static DocumentoSalida PeticionCatalogo(XmlDocument doc)
		{
			Localhost.Elegrp ws = new Localhost.Elegrp
			{
				Timeout = -1
			};
			string respuesta = ws.PeticionCatalogo(doc);
			return new DocumentoSalida(respuesta);
		}

		public static bool Enviar_correo_html(string CorreoCliente, string Asunto, string Contenido)
		{
			try
			{
				string htmlBody = Template_html(0);

				htmlBody = htmlBody.Replace("saludo_nombre", "");
				htmlBody = htmlBody.Replace("texto_contenido", Contenido);
				//saludo_nombre, texto_contenido
				var objSmtp = new SmtpClient();
				var objMail = new MailMessage("adminerp-notificaciones@elpotosi.com.mx", CorreoCliente);

				AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

				objMail.Subject = Asunto;
				objMail.AlternateViews.Add(avHtml);
				objMail.From = new System.Net.Mail.MailAddress("adminerp-notificaciones@elpotosi.com.mx");


				objSmtp.Host = "outlook.office365.com"; //"smtp.live.com"

				objSmtp.EnableSsl = true;

				objSmtp.Port = 587;

				objSmtp.Credentials = new System.Net.NetworkCredential("adminerp-notificaciones@elpotosi.com.mx", "Mur76017");// "Ham61041"

                try
				{
					objSmtp.Send(objMail);
					return true;
				}
				catch (Exception ex)
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static bool Enviar_correo_html_comprogapp(string CorreoCliente, string Asunto, string Contenido)
		{
			try
			{
				string htmlBody = Template_html(1);

				htmlBody = htmlBody.Replace("saludo_nombre", "");
				htmlBody = htmlBody.Replace("texto_contenido", Contenido);
				//saludo_nombre, texto_contenido
				var objSmtp = new SmtpClient();
				var objMail = new MailMessage("notificaciones@comprogapp.com", CorreoCliente);

				AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

				objMail.Subject = Asunto;
				objMail.AlternateViews.Add(avHtml);
				objMail.From = new System.Net.Mail.MailAddress("notificaciones@comprogapp.com");

				/*
Secure SSL/TLS Settings (recomendado)
Nombre de usuario:	notificaciones@comprogapp.com
Contraseña:	Usa la contraseña de la cuenta de correo electrónico.
Servidor entrante:	p3plcpnl0889.prod.phx3.secureserver.net
IMAP Port: 993 POP3 Port: 995
Outgoing Server:	p3plcpnl0889.prod.phx3.secureserver.net
SMTP Port: 465

Non-SSL Settings (NO recomendado)
Nombre de usuario:	notificaciones@comprogapp.com
Contraseña:	Usa la contraseña de la cuenta de correo electrónico.
Servidor entrante:	mail.comprogapp.com
IMAP Port: 143 POP3 Port: 110
Outgoing Server:	mail.comprogapp.com
SMTP Port: 587
				 */
				objSmtp.Host = "mail.comprogapp.com"; //"smtp.live.com"

				objSmtp.EnableSsl = false;

				objSmtp.Port = 587;

				objSmtp.Credentials = new System.Net.NetworkCredential("notificaciones@comprogapp.com", "notificaciones_123");

				try
				{
					objSmtp.Send(objMail);
					return true;
				}
				catch (Exception ex)
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static string Template_html(int esRespaldo)
		{
			/*
			 adminerp-notificaciones@elpotosi.com.mx
			 Ham61041
			 outlook.office365.com
			 rConPuertoSMTP: 587
			 */
			string htmlBody = "<!DOCTYPE html>" +
"<html lang='en' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'>" +
"<head><meta http-equiv='Content-Type' content='text/html; charset=gb18030'>" +
"<meta http-equiv='x-ua-compatible' content='ie=edge'>" +
"<meta name='viewport' content='width=device-width, initial-scale=1'>" +
"<meta name='x-apple-disable-message-reformatting'>" +
"<title>Notificaciones</title>" +
"<!--[if mso]>" +
"<xml>" +
"<o:OfficeDocumentSettings>" +
"<o:AllowPNG/>" +
"<o:PixelsPerInch>96</o:PixelsPerInch>" +
"</o:OfficeDocumentSettings>" +
"</xml>" +
"<style>" +
"table {border-collapse: collapse;}" +
".spacer,.divider {mso-line-height-rule:exactly;}" +
"td,th,div,p,a {font-size: 13px; line-height: 22px;}" +
"td,th,div,p,a,h1,h2,h3,h4,h5,h6 {font-family:'Segoe" +
"UI',Helvetica,Arial,sans-serif;}" +
"</style>" +
"<![endif]-->" +
"" +
"<style type='text/css'>" +
"" +
"@import url('https://fonts.googleapis.com/css?family=Lato:300,400,700|Open+Sans');" +
"@media only screen {" +
".col, td, th, div, p {font-family: 'Open Sans',-apple-system,system-ui,BlinkMacSystemFont,'Segoe" +
"UI','Roboto','Helvetica Neue',Arial,sans-serif;}" +
".webfont {font-family: 'Lato',-apple-system,system-ui,BlinkMacSystemFont,'Segoe UI','Roboto','Helvetica Neue',Arial,sans-serif;}" +
"}" +
"" +
"img {border: 0; line-height: 100%; vertical-align: middle;}" +
"#outlook a, .links-inherit-color a {padding: 0; color: inherit;}" +
".col {font-size: 13px; line-height: 22px; vertical-align: top;}" +
"" +
".hover-scale:hover {transform: scale(1.2);}" +
".star:hover a, .star:hover ~ .star a {color: #FFCF0F!important;}" +
"" +
"@media only screen and (max-width: 600px) {" +
"u ~ div .wrapper {min-width: 100vw;}" +
".wrapper img {height: auto!important;}" +
".container {width: 100%!important; -webkit-text-size-adjust: 100%;}" +
"}" +
"" +
"@media only screen and (max-width: 480px) {" +
".col {" +
"box-sizing: border-box;" +
"display: inline-block!important;" +
"line-height: 20px;" +
"width: 100%!important;" +
"}" +
".col-sm-1 {max-width: 25%;}" +
".col-sm-2 {max-width: 50%;}" +
".col-sm-3 {max-width: 75%;}" +
".col-sm-third {max-width: 33.33333%;}" +
".col-sm-auto {width: auto!important;}" +
".col-sm-push-1 {margin-left: 25%;}" +
".col-sm-push-2 {margin-left: 50%;}" +
".col-sm-push-3 {margin-left: 75%;}" +
".col-sm-push-third {margin-left: 33.33333%;}" +
"" +
".full-width-sm {display: table!important; width: 100%!important;}" +
".stack-sm-first {display: table-header-group!important;}" +
".stack-sm-last {display: table-footer-group!important;}" +
".stack-sm-top {display: table-caption!important; max-width: 100%;" +
"padding-left: 0!important;}" +
"" +
".toggle-content {" +
"max-height: 0;" +
"overflow: auto;" +
"transition: max-height .4s linear;" +
"-webkit-transition: max-height .4s linear;" +
"}" +
".toggle-trigger:hover + .toggle-content," +
".toggle-content:hover {max-height: 999px!important;}" +
"" +
".show-sm {" +
"display: inherit!important;" +
"font-size: inherit!important;" +
"line-height: inherit!important;" +
"max-height: none!important;" +
"}" +
".hide-sm {display: none!important;}" +
"" +
".align-sm-center {" +
"display: table!important;" +
"float: none;" +
"margin-left: auto!important;" +
"margin-right: auto!important;" +
"}" +
".align-sm-left {float: left;}" +
".align-sm-right {float: right;}" +
"" +
".text-sm-center {text-align: center!important;}" +
".text-sm-left {text-align: left!important;}" +
".text-sm-right {text-align: right!important;}" +
"" +
".nav-sm-vertical .nav-item {display: block!important;}" +
".nav-sm-vertical .nav-item a {display: inline-block; padding: 5px 0!important;}" +
"" +
".h1 {font-size: 32px !important;}" +
".h2 {font-size: 24px !important;}" +
".h3 {font-size: 16px !important;}" +
"" +
".borderless-sm {border: none!important;}" +
".height-sm-auto {height: auto!important;}" +
".line-height-sm-0 {line-height: 0!important;}" +
".overlay-sm-bg {background: #232323; background: rgba(0,0,0,0.4);}" +
"" +
".p-sm-0 {padding: 0!important;}" +
".p-sm-8 {padding: 8px!important;}" +
".p-sm-16 {padding: 16px!important;}" +
".p-sm-24 {padding: 24px!important;}" +
".pt-sm-0 {padding-top: 0!important;}" +
".pt-sm-8 {padding-top: 8px!important;}" +
".pt-sm-16 {padding-top: 16px!important;}" +
".pt-sm-24 {padding-top: 24px!important;}" +
".pr-sm-0 {padding-right: 0!important;}" +
".pr-sm-8 {padding-right: 8px!important;}" +
".pr-sm-16 {padding-right: 16px!important;}" +
".pr-sm-24 {padding-right: 24px!important;}" +
".pb-sm-0 {padding-bottom: 0!important;}" +
".pb-sm-8 {padding-bottom: 8px!important;}" +
".pb-sm-16 {padding-bottom: 16px!important;}" +
".pb-sm-24 {padding-bottom: 24px!important;}" +
".pl-sm-0 {padding-left: 0!important;}" +
".pl-sm-8 {padding-left: 8px!important;}" +
".pl-sm-16 {padding-left: 16px!important;}" +
".pl-sm-24 {padding-left: 24px!important;}" +
".px-sm-0 {padding-right: 0!important; padding-left: 0!important;}" +
".px-sm-8 {padding-right: 8px!important; padding-left: 8px!important;}" +
".px-sm-16 {padding-right: 16px!important; padding-left: 16px!important;}" +
".px-sm-24 {padding-right: 24px!important; padding-left: 24px!important;}" +
".py-sm-0 {padding-top: 0!important; padding-bottom: 0!important;}" +
".py-sm-8 {padding-top: 8px!important; padding-bottom: 8px!important;}" +
".py-sm-16 {padding-top: 16px!important; padding-bottom: 16px!important;}" +
".py-sm-24 {padding-top: 24px!important; padding-bottom: 24px!important;}" +
"}" +
"</style>" +
"</head>" +
"<body style='box-sizing:border-box;margin:0;padding:0;width:100%;word-break:break-word;-webkit-font-smoothing:antialiased;'>" +
"" +
"<div style='display:none;font-size:0;line-height:0;'><!-- Add your inbox preview text here --></div>" +
"" +
"<table class='wrapper' cellpadding='0' cellspacing='0' role='presentation' width='100%'>" +
"<tr>" +
"<td class='px-sm-16' align='center' bgcolor='#EEEEEE'>" +
"<table class='container' cellpadding='0' cellspacing='0' role='presentation' width='600'>" +
"<tr>" +
"<td class='px-sm-8' align='left' bgcolor='#EEEEEE'>" +
"<div class='spacer line-height-sm-0 py-sm-8' style='line-height: 24px;'>&zwnj;</div>" +
"<table cellpadding='0' cellspacing='0' role='presentation' style='margin: 0 auto;'>" +
"<tr>" +
"<td class='col' align='center'>" +
"</td>" +
"</tr>" +
"</table>" +
"<div class='spacer line-height-sm-0 py-sm-8' style='line-height: 24px;'>&zwnj;</div>" +
"</td>" +
"</tr>" +
"</table>" +
"</td>" +
"</tr>" +
"</table>" +
"" +
"<table class='wrapper' cellpadding='0' cellspacing='0' role='presentation' width='100%'>" +
"<tr>" +
"<td align='center' bgcolor='#EEEEEE' class='px-sm-16'>" +
"<table class='container' cellpadding='0' cellspacing='0' role='presentation' width='600'>" +
"<tr>" +
"<td bgcolor='#f2f2f2' style='background: linear-gradient(to right, #f2f2f2, #f2f2f2);'>" +
"<!--[if gte mso 9]>" +
"<v:rect xmlns:v='urn:schemas-microsoft-com:vml' fill='true'" +
"stroke='false' style='width:600px;'>" +
"<v:fill type='gradient' color='#f2f2f2' color2='#f2f2f2' angle='90' />" +
"<v:textbox style='mso-fit-shape-to-text:true' inset='0,0,0,0'>" +
"<div><![endif]-->" +
"<div class='spacer line-height-sm-0 py-sm-16' style='line-height: 32px;'>&zwnj;</div>" +
"<table cellpadding='0' cellspacing='0' role='presentation' width='100%'>" +
"<tr>" +
"<td align='center' class='px-sm-16' style='padding: 0 96px;'>" +
"<h1 class='webfont h1' style='color: #FFFFFF; font-size: 36px;" +
"font-weight: 300; line-height: 100%; margin: 0;'>" +
"<a href='link_webpage'>" +
"<img src='https://gapp.elpotosi.com.mx/img/logo170x70.png' alt='Seguros El Potosi' width='170px' height='70px' style='width:170px; height: 70px;'>" +
"</a>" +
"</h1>" +
"</td>" +
"</tr>" +
"</table>" +
"<div class='spacer line-height-sm-0 py-sm-16' style='line-height: 40px;'>&zwnj;</div>" +
"<!--[if gte mso 9]></div></v:textbox></v:rect><![endif]-->" +
"</td>" +
"</tr>" +
"</table>" +
"</td>" +
"</tr>" +
"</table>" +
"" +
"<table class='wrapper' cellpadding='0' cellspacing='0'" +
"role='presentation' width='100%'>" +
"<tr>" +
"<td class='px-sm-16' align='center' bgcolor='#EEEEEE'>" +
"<table class='container' cellpadding='0' cellspacing='0'" +
"role='presentation' width='600'>" +
"<tr>" +
"<td class='px-sm-8' align='left' bgcolor='#FFFFFF' style='padding: 0 24px;'>" +
"<div class='spacer line-height-sm-0 py-sm-8' style='line-height: 24px;'>&zwnj;</div>" +
"<table cellpadding='0' cellspacing='0' role='presentation' width='100%'>" +
"<tr>" +
"<td class='col px-sm-16' align='center' width='100%' style='padding: 0 64px;'>" +
"<h2 style='color: #000; font-size: 20px; font-weight: 300; line-height: 28px; margin: 0 0 24px;'>" +
"saludo_nombre" +
"</h2>" +
"<p style='color: #888888; font-size: 18px; line-height: 24px; margin: 0;'>" +
"texto_contenido" +
"</p>" +
"<div class='spacer' style='line-height: 32px;'>&zwnj;</div>" +
"</td>" +
"</tr>" +
"</table>" +
"<div class='spacer line-height-sm-0 py-sm-8' style='line-height: 24px;'>&zwnj;</div>" +
"</td>" +
"</tr>" +
"</table>" +
"</td>" +
"</tr>" +
"</table>" +
"" +
"<table class='wrapper' cellpadding='0' cellspacing='0' role='presentation' width='100%'>" +
"<tr>" +
"<td class='px-sm-16' align='center' bgcolor='#EEEEEE'>" +
"<table class='container' cellpadding='0' cellspacing='0' role='presentation' width='600'>" +
"<tr>" +
"<td class='divider py-sm-16 px-sm-16' bgcolor='#FFFFFF' style='padding: 24px 32px;'>" +
"<div style='background: #EEEEEE; height: 1px; line-height: 1px;'>&zwnj;</div>" +
"</td>" +
"</tr>" +
"</table>" +
"</td>" +
"</tr>" +
"</table>" +
"" +
"<table class='wrapper' cellpadding='0' cellspacing='0' role='presentation' width='100%'>" +
"<tr>" +
"<td class='px-sm-16' align='center' bgcolor='#EEEEEE'>" +
"<table class='container' cellpadding='0' cellspacing='0' role='presentation' width='600'>" +
"<tr>" +
"<td class='px-sm-8' bgcolor='#FFFFFF' style='padding: 0 24px;'>" +
"<div class='spacer line-height-sm-0 py-sm-8' style='line-height: 24px;'>&zwnj;</div>" +
"<div class='spacer line-height-sm-0 py-sm-8' style='line-height: 24px;'>&zwnj;</div>" +
"</td>" +
"</tr>" +
"</table>" +
"</td>" +
"</tr>" +
"</table>" +
"" +
"<table class='wrapper' cellpadding='0' cellspacing='0' role='presentation' width='100%'>" +
"<tr>" +
"<td class='px-sm-16' align='center' bgcolor='#EEEEEE'>" +
"<table class='container' cellpadding='0' cellspacing='0' role='presentation' width='600'>" +
"<tr>" +
"<td class='spacer height-sm-auto py-sm-8' bgcolor='#EEEEEE' height='24'></td>" +
"</tr>" +
"</table>" +
"</td>" +
"</tr>" +
"</table>";
			if (esRespaldo == 1)
			{
				htmlBody += "<table class='wrapper' cellpadding='0' cellspacing='0' role='presentation' width='100%'>" +
			"<tr>" +
			"<td class='px-sm-8' bgcolor='#EEEEEE' style='padding: 0 24px;'>" +
			"<div class='spacer line-height-sm-0 py-sm-8' style='line-height: 24px;'>&zwnj;</div>" +
			"<table cellpadding = '0' cellspacing='0' role='presentation' width='100%'>" +
			"<tr>" +
			"<td class='col' align='center' width='100%' style='padding: 0 8px;'>" +
			"<p style = 'color: #888888; margin: 0; font-weight: bold;' > " +
			"Correo de respaldo." +
			"</p>" +
			"</td>" +
			"</tr>" +
			"</table>" +
			"<div class='spacer line-height-sm-0 py-sm-8' style='line-height: 24px;'>&zwnj;</div>" +
			"</td>" +
			"</tr>" +
			"</table>";
			}

			htmlBody += "<table class='wrapper' cellpadding='0' cellspacing='0' role='presentation' width='100%'>" +
				"<tr>" +
				"<td class='px-sm-8' bgcolor='#EEEEEE' style='padding: 0 24px;'>" +
				"<div class='spacer line-height-sm-0 py-sm-8' style='line-height: 24px;'>&zwnj;</div>" +
				"<table cellpadding = '0' cellspacing='0' role='presentation' width='100%'>" +
				"<tr>" +
				"<td class='col' align='center' width='100%' style='padding: 0 8px;'>" +
				"<p style = 'color: #888888; margin: 0; font-weight: bold;' > " +
				"No responder este e-mail, es un envío automático" +
				"</p>" +
				"</td>" +
				"</tr>" +
				"</table>" +
				"<div class='spacer line-height-sm-0 py-sm-8' style='line-height: 24px;'>&zwnj;</div>" +
				"</td>" +
				"</tr>" +
				"</table>";


			htmlBody += "</body>" +
"</html>";

			return htmlBody;


		}
	}
}
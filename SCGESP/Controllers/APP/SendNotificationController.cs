﻿using System.Web.Http;
using System.Collections.Generic;
using Firebase.NET.Messages;
using Firebase.NET.Notifications;
using Firebase.NET;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using System.Data;
using SCGESP.Clases;

namespace SCGESP.Controllers.APP
{
    public class SendNotificationController : ApiController
    {
        public class datos
        {
            public string usuario { get; set; }
            public string Titulo { get; set; }
            public string Mensaje { get; set; }

        }

        public async Task<string> Post(datos Datos)
        {
            try
            {
                string TokenID = "";
                int Pendientes = 0;

                SqlCommand comando = new SqlCommand("ObtieneUsuariosToken");
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@Usuario", SqlDbType.VarChar);

                comando.Parameters["@Usuario"].Value = Datos.usuario;

                comando.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando.CommandTimeout = 0;
                comando.Connection.Open();

                DataTable DT = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(comando);
                comando.Connection.Close();
                DA.Fill(DT);

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow row in DT.Rows)
                    {
                        TokenID = Convert.ToString(row["Token"]);
                        Pendientes = Convert.ToInt32(row["Pendientes"]);
                    }
                }
                else
                {
                    TokenID = "";
                    Pendientes = 0;
                }

                if (TokenID != "")
                {
                    try
                    {
                        string[] ids = { TokenID };

                        var requestMessage = new RequestMessage
                        {
                            Body =
                { RegistrationIds = ids,
                  Notification = new CrossPlatformNotification
                { Title = Datos.Titulo,
                  Body = Datos.Mensaje,
                  Badge = Convert.ToString(Pendientes),
                  Sound = "true",
                 }
                }
                        };

                        var pushService = new PushNotificationService("AAAAhj1oXWM:APA91bFKxoagtjhXM8o1alJCPlQsAvDtaEKM6TyfN7HaLwqqq95KapxHc73Dgq5tVSdJU7fbbSAxaxQJ56OFEK6ft6mOAoh01ZBJxhe108WX5CT4EimDYz_nAslLFcqiVJ4jxJAErCJ3");
                        //AAAAhj1oXWM:APA91bFKxoagtjhXM8o1alJCPlQsAvDtaEKM6TyfN7HaLwqqq95KapxHc73Dgq5tVSdJU7fbbSAxaxQJ56OFEK6ft6mOAoh01ZBJxhe108WX5CT4EimDYz_nAslLFcqiVJ4jxJAErCJ3
                        // clave anterior AAAASvHYA78:APA91bEYxMsqdhV-7h-DdDGORTinWDc8G_JEYmOBhMA7FVNfNNpbrsviDW0BNK0etgD2l6QEMiiQHz3Of_Kv2YMEwQHVl6kvoStC0SBucb9nSGP6XGWG-6IAN48WBtb4Te2QPG1jWzM
                        var responseMessage = await pushService.PushMessage(requestMessage);

                        return "Notificacion Enviada Exitosamente";

                    }
                    catch (Exception ex)
                    {

                        return ex.ToString();
                    }
                }
                else
                {
                    return "Usuario no cuenta con token asignado";
                }
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }

            
            //    try
            //    {
            //        using (var firebase = new FireBase.Notification.Firebase())
            //        {
            //            firebase.ServerKey = "AAAASvHYA78:APA91bEYxMsqdhV-7h-DdDGORTinWDc8G_JEYmOBhMA7FVNfNNpbrsviDW0BNK0etgD2l6QEMiiQHz3Of_Kv2YMEwQHVl6kvoStC0SBucb9nSGP6XGWG-6IAN48WBtb4Te2QPG1jWzMx";

            //            var id = Datos.TokenID;

            //            firebase.PushNotifyAsync(id, Datos.Titulo, Datos.Mensaje).Wait();

            //            Console.ReadLine();
            //        }

            //        return "OK";
            //    }
            //    catch (Exception ex)
            //    {

            //        return ex.ToString();
            //    }

        }


    }
}


using System.Web.Http;
using System.Collections.Generic;
using Firebase.NET.Messages;
using Firebase.NET.Notifications;
using Firebase.NET;
using System.Threading.Tasks;
using System;

namespace SCGESP.Controllers.APP
{
    public class SendNotificationController : ApiController
    {
        public class datos
        {
            public string usuario { get; set; }
            public string TokenID { get; set; }
            public string Titulo { get; set; }
            public string Mensaje { get; set; }

        }

        public async Task<string> Post(datos Datos)
        {
            try
            {
                string[] ids = {
                Datos.TokenID
                };

                var requestMessage = new RequestMessage
                {
                    Body =
                {
                RegistrationIds = ids,
                Notification = new CrossPlatformNotification
                {
                Title = Datos.Titulo,
                Body = Datos.Mensaje
                }

                }
                };

                var pushService = new PushNotificationService("AAAASvHYA78:APA91bEYxMsqdhV-7h-DdDGORTinWDc8G_JEYmOBhMA7FVNfNNpbrsviDW0BNK0etgD2l6QEMiiQHz3Of_Kv2YMEwQHVl6kvoStC0SBucb9nSGP6XGWG-6IAN48WBtb4Te2QPG1jWzMx");
                var responseMessage = await pushService.PushMessage(requestMessage);

            }
            catch (Exception ex)
            {

                return ex.ToString();
            }

            return "OK";

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


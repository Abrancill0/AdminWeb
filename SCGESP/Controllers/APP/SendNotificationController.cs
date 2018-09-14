using System.Web.Http;
using System;

namespace SCGESP.Controllers.APP
{
    public class SendNotificationController : ApiController
    {
        public class datos
        {
            public string usuario { get; set; }
            public string contrasena { get; set; }
        }

        public class ObtieneEmpleadoResult
        {
            public int SgUsuEmpleado { get; set; }
        }

        public static String SendNotificationFromFirebaseCloud()
        {
            using (var firebase = new FireBase.Notification.Firebase())
            {
                firebase.ServerKey = "AAAASvHYA78:APA91bEYxMsqdhV-7h-DdDGORTinWDc8G_JEYmOBhMA7FVNfNNpbrsviDW0BNK0etgD2l6QEMiiQHz3Of_Kv2YMEwQHVl6kvoStC0SBucb9nSGP6XGWG-6IAN48WBtb4Te2QPG1jWzMx";
                var id = "{Your Device Id}";
                firebase.PushNotifyAsync(id, "Hello", "World").Wait();
                Console.ReadLine();
            }

            return "OK";
        }

    }
}


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SCGESP.Controllers.APP
{
    public class EnvioNotificacionesController : ApiController
    {
        public class Message
        {
            public string[] registration_ids { get; set; }
            public Notification notification { get; set; }
            public object data { get; set; }
        }
        public class Notification
        {
            public string title { get; set; }
            public string body { get; set; }
        }
        
        public async Task<bool> Post(Notification Datos)
        {
             Uri FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");
             string ServerKey = "AAAAhj1oXWM:APA91bFKxoagtjhXM8o1alJCPlQsAvDtaEKM6TyfN7HaLwqqq95KapxHc73Dgq5tVSdJU7fbbSAxaxQJ56OFEK6ft6mOAoh01ZBJxhe108WX5CT4EimDYz_nAslLFcqiVJ4jxJAErCJ3";
             string[] divec = { "c6sWsjugeCE:APA91bHeDBAJsZcMmS55NaGnnb8aqKCyOeePOo6CTm_7kgWYoiONipMHpzx1nLZWOZ4jVXoFpOB-fW5rFVkZrkeqaIK5yAXjFw6awIz5sxnoGrQOGZDgfuHWBPFxHxY5Iij1PeZAYJKP" };


        bool sent = false;
            var data = new { action = "Play", userId = 5 };
            var messageInformation = new Message()
            {
                notification = new Notification()
                {
                    title = Datos.title,
                    body = Datos.body,
                    
                },
                data = data,
                registration_ids = divec
            };

            //Object to JSON STRUCTURE => using Newtonsoft.Json;
            string jsonMessage = JsonConvert.SerializeObject(messageInformation);


            //Create request to Firebase API
            var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);

            request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
            request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

            HttpResponseMessage result;
            using (var client = new HttpClient())
            {
                result = await client.SendAsync(request);
                sent = sent && result.IsSuccessStatusCode;
            }
            return sent;
        }


    }

}


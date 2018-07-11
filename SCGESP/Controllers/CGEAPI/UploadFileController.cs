using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SCGESP.Controllers.CGEAPI
{
    public class UploadFileController : ApiController
    {
        public string PostSaveImage(string Based64BinaryString)
        {
            string result = "";
            try
            {
                string format = "";
                string path = HttpContext.Current.Server.MapPath("imageupload/");
                string name = DateTime.Now.ToString("yyyyMMddhhmmss");

                if (Based64BinaryString.Contains("data:application/zip;base64,"))
                {
                    format = "zip";
                }
                if (Based64BinaryString.Contains("data:;base64,"))
                {
                    format = "zip";
                }
                if (Based64BinaryString.Contains("data:image/jpeg;base64,"))
                {
                    format = "jpg";
                }
                if (Based64BinaryString.Contains("data:image/png;base64,"))
                {
                    format = "png";
                }
                if (Based64BinaryString.Contains("data:text/plain;base64,"))
                {
                    format = "txt";
                }

                string str = Based64BinaryString.Replace("data:image/jpeg;base64,", " ");//jpg check
                str = str.Replace("data:image/png;base64,", " ");//png check
                str = str.Replace("data:text/plain;base64,", " ");//text file check
                str = str.Replace("data:;base64,", " ");//zip file check
                str = str.Replace("data:application/zip;base64,", " ");//zip file check

                byte[] data = Convert.FromBase64String(str);

                if (format == "zip")
                {
                    using (MemoryStream stream = new MemoryStream(data))
                    {
                        //using (ZipFile zip = new ZipFile())
                        //{
                        //    zip.AddEntry("mainContent.zip", stream);
                        //    zip.Save(path + "/file" + name + ".zip");
                        //    result = "file uploaded succesfully";
                        //}
                    }
                }
                else
                {
                    MemoryStream ms = new MemoryStream(data, 0, data.Length);
                    ms.Write(data, 0, data.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    image.Save(path + "/Image" + name + ".jpg");
                    result = "image uploaded successfully";
                }
            }
            catch (Exception ex)
            {
                result = "Error : " + ex;
            }
            return result;
        }


    }
}

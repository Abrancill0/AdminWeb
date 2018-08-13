using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SCGESP.Controllers.APP
{
    public class CargaImagenController : ApiController
    {
       // [Route("api/Files/Upload")]
        public async Task<string> Post()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                string filename = "";
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        filename = postedFile.FileName.Split('\\').LastOrDefault().Split('\\').LastOrDefault();

                        var filepath = HttpContext.Current.Server.MapPath("~/Comprobantes/" + filename);

                        postedFile.SaveAs(filepath);

                    }
                    return "/upload/ " + filename;
                }
                else
                {
                    return "No se encontro archivo";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString() + ' ' + "Error no se subio";

            }



        }

    }

}
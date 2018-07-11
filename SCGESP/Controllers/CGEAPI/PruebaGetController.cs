using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCGESP.Controllers
{
    public class PruebaGetController : ApiController
    {

        public string[] Get()
        {
            return new string[]
            {
        "Hello",
        "World"
            };
        }

    }
}

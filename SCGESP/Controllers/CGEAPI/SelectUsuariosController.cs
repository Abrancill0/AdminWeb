using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SCGESP.Controllers
{
	public class SelectUsuariosController : ApiController
	{
		public List<GetUsuarios.Resultado> Post()
		{
			try
			{
				List<GetUsuarios.Resultado> Res = GetUsuarios.Post();
				return Res;
			}
			catch (Exception)
			{

				return null;
			}
		}
	}
}
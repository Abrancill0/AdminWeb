using SCGESP.Clases;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SCGESP.Controllers
{
	public class SelectUsuariosController : ApiController
	{
		public class ParmetrosEntrada
		{
			public int VerSoloActivos { get; set; }
		}
		public List<GetUsuarios.Resultado> Post(ParmetrosEntrada Datos)
		{
			try
			{
				Datos = Datos is null ? new ParmetrosEntrada
				{
					VerSoloActivos = 0
				} : Datos;
			}
			catch (Exception)
			{
				Datos = new ParmetrosEntrada
				{
					VerSoloActivos = 0
				};
			}
			try
			{
				List<GetUsuarios.Resultado> Res = GetUsuarios.Post(Datos.VerSoloActivos);
				return Res;
			}
			catch (Exception ex)
			{

				return null;
			}
		}
	}
}
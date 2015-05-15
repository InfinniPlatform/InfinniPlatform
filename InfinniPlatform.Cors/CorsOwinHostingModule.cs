using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Modules;

using Microsoft.Owin.Cors;

using Owin;

namespace InfinniPlatform.Cors
{
	/// <summary>
	/// Модуль хостинга ASP.NET CORS (Cross-origin resource sharing) на базе OWIN.
	/// </summary>
	public sealed class CorsOwinHostingModule : OwinHostingModule
	{
		public override void Configure(IAppBuilder builder, IHostingContext context)
		{
			// Todo: Добавить правила CORS проверки из конфигурации

			builder.UseCors(CorsOptions.AllowAll);
		}
	}
}
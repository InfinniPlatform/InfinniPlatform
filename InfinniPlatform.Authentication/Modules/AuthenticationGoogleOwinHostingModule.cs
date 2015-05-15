using System;

using Microsoft.Owin.Security.Google;

using Owin;

using InfinniPlatform.Authentication.Properties;
using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Modules;

namespace InfinniPlatform.Authentication.Modules
{
	/// <summary>
	/// Модуль хостинга подсистемы аутентификации Google на базе OWIN.
	/// </summary>
	/// <remarks>
	/// Информация о реализации Google OAuth:
	/// https://developers.google.com/+/api/oauth
	/// Информация о создании проекта Google:
	/// https://developers.google.com/console/help/#creatingdeletingprojects
	/// После создания проекта нужно не забыть включить "Google+ API".
	/// </remarks>
	public sealed class AuthenticationGoogleOwinHostingModule : OwinHostingModule
	{
		public AuthenticationGoogleOwinHostingModule(string clientId, string clientSecret)
		{
			if (string.IsNullOrWhiteSpace(clientId))
			{
				throw new ArgumentNullException("clientId", Resources.AuthenticationGoogleClientIdCannotBeNullOrWhiteSpace);
			}

			if (string.IsNullOrWhiteSpace(clientSecret))
			{
				throw new ArgumentNullException("clientSecret", Resources.AuthenticationGoogleClientSecretCannotBeNullOrWhiteSpace);
			}

			_options = new GoogleOAuth2AuthenticationOptions
					   {
						   ClientId = clientId,
						   ClientSecret = clientSecret
					   };
		}


		private readonly GoogleOAuth2AuthenticationOptions _options;


		public override void Configure(IAppBuilder builder, IHostingContext context)
		{
			builder.UseGoogleAuthentication(_options);
		}
	}
}
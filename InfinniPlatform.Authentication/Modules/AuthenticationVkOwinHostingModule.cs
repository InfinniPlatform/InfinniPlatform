using System;
using Duke.Owin.VkontakteMiddleware;
using InfinniPlatform.Authentication.Properties;
using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Modules;
using Owin;

namespace InfinniPlatform.Authentication.Modules
{
	public class AuthenticationVkOwinHostingModule : OwinHostingModule
	{
		readonly VkAuthenticationOptions _options;

		public AuthenticationVkOwinHostingModule(string clientId, string clientSecret)
		{
			if (string.IsNullOrWhiteSpace(clientId))
				throw new ArgumentNullException("clientId", Resources.AuthenticationVkClientIdCannotBeNullOrWhiteSpace);

			if (string.IsNullOrWhiteSpace(clientSecret))
				throw new ArgumentNullException("clientSecret", Resources.AuthenticationVkClientSecretCannotBeNullOrWhiteSpace);

			_options = new VkAuthenticationOptions
						{
							AppId = clientId,
							AppSecret = clientSecret
						};
		}

		public override void Configure(IAppBuilder builder, IHostingContext context)
		{
			builder.UseVkontakteAuthentication(_options);
		}
	}
}
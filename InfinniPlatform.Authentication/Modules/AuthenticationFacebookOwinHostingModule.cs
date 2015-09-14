using System;
using InfinniPlatform.Authentication.Properties;
using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Modules;
using Microsoft.Owin.Security.Facebook;
using Owin;

namespace InfinniPlatform.Authentication.Modules
{
	public class AuthenticationFacebookOwinHostingModule : OwinHostingModule
	{
		readonly FacebookAuthenticationOptions _options;

		public AuthenticationFacebookOwinHostingModule(string clientId, string clientSecret)
		{
			if (string.IsNullOrWhiteSpace(clientId))
				throw new ArgumentNullException("clientId", Resources.AuthenticationFacebookClientIdCannotBeNullOrWhiteSpace);

			if (string.IsNullOrWhiteSpace(clientSecret))
				throw new ArgumentNullException("clientSecret", Resources.AuthenticationFacebookClientSecretCannotBeNullOrWhiteSpace);

			_options = new FacebookAuthenticationOptions
						{
							AppId = clientId,
							AppSecret = clientSecret
						};
		}

		public override void Configure(IAppBuilder builder, IHostingContext context)
		{
			builder.UseFacebookAuthentication(_options);
		}
	}
}
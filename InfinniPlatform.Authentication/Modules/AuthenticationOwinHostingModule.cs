using System;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;

using Owin;

using InfinniPlatform.Api.Security;
using InfinniPlatform.Authentication.DataProtectors;
using InfinniPlatform.Authentication.Middleware;
using InfinniPlatform.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Security;
using Microsoft.Owin.Security.DataProtection;

namespace InfinniPlatform.Authentication.Modules
{
	/// <summary>
	/// Модуль хостинга подсистемы аутентификации на базе OWIN.
	/// </summary>
	public sealed class AuthenticationOwinHostingModule : OwinHostingModule
	{
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="externalProviders">Список модулей хостинга OWIN для внешних провайдеров аутентификации.</param>
		public AuthenticationOwinHostingModule(IEnumerable<OwinHostingModule> externalProviders = null)
		{
			_externalProviders = externalProviders;

			OnStart += OnStartHandler;
			OnStop += OnStopHandler;
		}


		private readonly IEnumerable<OwinHostingModule> _externalProviders;


		public override void Configure(IAppBuilder builder, IHostingContext context)
		{
			if (IsRunningOnMono())
			{
				builder.SetDataProtectionProvider(new AesDataProtectionProvider());
			}

			// Разрешение использования cookie для входа в систему через внутренний провайдер

			var cookieAuthOptions = new CookieAuthenticationOptions
									{
										AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
										LoginPath = AuthenticationOwinMiddleware.SignInInternalPath,
										LogoutPath = AuthenticationOwinMiddleware.SignOutPath,
										ExpireTimeSpan = TimeSpan.FromDays(1),
										SlidingExpiration = true
									};

			if (Uri.UriSchemeHttps.Equals(context.Configuration.ServerScheme, StringComparison.OrdinalIgnoreCase))
			{
				cookieAuthOptions.CookieSecure = CookieSecureOption.Always;
			}

			builder.UseCookieAuthentication(cookieAuthOptions);

			// Разрешение использования cookie для входа в систему через внешние провайдеры
			builder.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			// Регистрация метода для создания менеджера управления пользователями
			builder.CreatePerOwinContext(() => CreateUserManager(context));

			// Внешние провайдеры представлены в виде модулей хостинга OWIN
			ConfigureExternalProviders(builder, context);

			// Регистрация обработчика запросов к подсистеме аутентификации
			builder.Use(typeof(AuthenticationOwinMiddleware));

			// Регистрация IApplicationUserManager в контексте
			context.Set<IApplicationUserManager>(new IdentityApplicationUserManager(() => CreateUserManager(context)));
		}

		private static UserManager<IdentityApplicationUser> CreateUserManager(IHostingContext context)
		{
			// Делается предположение, что IApplicationUserStore попадает в контекст извне
			var applicationUserStore = context.Get<IApplicationUserStore>();

			if (applicationUserStore != null)
			{
				var userStore = new IdentityApplicationUserStore(applicationUserStore);
				var userValidator = new IdentityApplicationUserValidator(userStore);
				var userManager = new UserManager<IdentityApplicationUser>(userStore) { UserValidator = userValidator };

				// Делается предположение, что IApplicationUserPasswordHasher попадает в контекст извне
				var applicationUserPasswordHasher = context.Get<IApplicationUserPasswordHasher>() ?? new DefaultApplicationUserPasswordHasher();
				var identityApplicationUserPasswordHasher = new IdentityApplicationUserPasswordHasher(applicationUserPasswordHasher);
				userManager.PasswordHasher = identityApplicationUserPasswordHasher;

				return userManager;
			}

			return null;
		}

		private void ConfigureExternalProviders(IAppBuilder builder, IHostingContext context)
		{
			if (_externalProviders != null)
			{
				foreach (var externalProvider in _externalProviders)
				{
					externalProvider.Configure(builder, context);
				}
			}
		}

		private void OnStartHandler(HostingContextBuilder contextBuilder, IHostingContext context)
		{
			if (_externalProviders != null)
			{
				foreach (var externalProvider in _externalProviders)
				{
					if (externalProvider.OnStart != null)
					{
						externalProvider.OnStart(contextBuilder, context);
					}
				}
			}
		}

		private void OnStopHandler(IHostingContext context)
		{
			if (_externalProviders != null)
			{
				foreach (var externalProvider in _externalProviders)
				{
					if (externalProvider.OnStop != null)
					{
						externalProvider.OnStop(context);
					}
				}
			}
		}

		private static bool IsRunningOnMono()
		{
			return Type.GetType("Mono.Runtime") != null;
		}
	}
}
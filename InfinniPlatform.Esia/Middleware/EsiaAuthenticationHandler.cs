using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;

using InfinniPlatform.Esia.Implementation.Protocol;

namespace InfinniPlatform.Esia.Middleware
{
	/// <summary>
	/// Обработчик OWIN для аутентификации пользователей с использованием Единой системы идентификации и аутентификации (ЕСИА).
	/// </summary>
	sealed class EsiaAuthenticationHandler : AuthenticationHandler<EsiaAuthenticationOptions>
	{
		public EsiaAuthenticationHandler(AuthRequestBuilder authRequestBuilder, AuthResponseParser authResponseParser, ILogger logger)
		{
			_authRequestBuilder = authRequestBuilder;
			_authResponseParser = authResponseParser;
			_logger = logger;
		}


		private readonly AuthRequestBuilder _authRequestBuilder;
		private readonly AuthResponseParser _authResponseParser;
		private readonly ILogger _logger;


		/// <summary>
		/// Обработчик перенаправления пользователя на страницу аутентификации.
		/// </summary>
		protected override Task ApplyResponseChallengeAsync()
		{
			if (Response.StatusCode == 401)
			{
				var challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

				if (challenge != null)
				{
					var baseUri = Request.Scheme + Uri.SchemeDelimiter + Request.Host + Request.PathBase;
					var currentUri = baseUri + Request.Path + Request.QueryString;
					var callbackUri = baseUri + Options.CallbackPath;

					// Адрес для перенаправления ответа от сервера аутентификации

					var properties = challenge.Properties;

					if (string.IsNullOrEmpty(properties.RedirectUri))
					{
						properties.RedirectUri = currentUri;
					}

					// Защита от межсайтовой подделки запроса (Сross Site Request Forgery, CSRF)
					GenerateCorrelationId(properties);

					// Состояние запроса аутентификации, придет без изменений в ответе от сервера
					var state = Options.StateDataFormat.Protect(properties);

					// Адрес страницы аутентификации для перенаправления на нее пользователя
					var authEndpoint = _authRequestBuilder.BuildAuthEndpoint(state, callbackUri);

					// Перенаправление пользователя на страницу аутентификации
					var redirectContext = new EsiaApplyRedirectContext(Context, Options, properties, authEndpoint);
					Options.Provider.ApplyRedirect(redirectContext);
				}
			}

			return Constants.NullTask;
		}


		/// <summary>
		/// Обработчик получения ответа на запрос аутентификации.
		/// </summary>
		protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
		{
			ClaimsIdentity userIdentity = null;
			AuthenticationProperties properties = null;

			try
			{
				string state;

				// Чтение информации об аутентифицированном пользователе
				var userInfo = _authResponseParser.GetAuthenticatedUserInfo(Context.Request, out state);

				// Чтение состояние запроса аутентификации пользователя
				properties = Options.StateDataFormat.Unprotect(state);

				if (properties == null)
				{
					return null;
				}

				// Защита от межсайтовой подделки запроса (Сross Site Request Forgery, CSRF)
				if (ValidateCorrelationId(properties, _logger))
				{
					userIdentity = new ClaimsIdentity(Options.AuthenticationType, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

					// Создание контекста аутентификации пользователя
					var authContext = new EsiaAuthenticatedContext(Context, userInfo) { Identity = userIdentity, Properties = properties };

					// Заполнение утверждений об аутентифицированном пользователе

					AddClaim(userIdentity, ClaimTypes.NameIdentifier, authContext.Id);
					AddClaim(userIdentity, ClaimTypes.Name, CreateUserName(authContext.Snils));
					AddClaim(userIdentity, ClaimTypes.Email, authContext.Email);
					AddClaim(userIdentity, EsiaClaimTypes.Snils, authContext.Snils);

					AddClaim(userIdentity, ClaimTypes.GivenName, authContext.FirstName);
					AddClaim(userIdentity, EsiaClaimTypes.FirstName, authContext.FirstName);

					AddClaim(userIdentity, EsiaClaimTypes.MiddleName, authContext.MiddleName);

					AddClaim(userIdentity, ClaimTypes.Surname, authContext.LastName);
					AddClaim(userIdentity, EsiaClaimTypes.LastName, authContext.LastName);

					AddClaim(userIdentity, EsiaClaimTypes.RegionCode, authContext.RegionCode);
					AddClaim(userIdentity, EsiaClaimTypes.RegionName, authContext.RegionName);

					AddClaims(userIdentity, ClaimTypes.Role, authContext.RoleCodes);
					AddClaims(userIdentity, EsiaClaimTypes.RoleCode, authContext.RoleCodes);
					AddClaims(userIdentity, EsiaClaimTypes.RoleName, authContext.RoleNames);

					AddClaims(userIdentity, ClaimTypes.System, authContext.SystemCodes);
					AddClaims(userIdentity, EsiaClaimTypes.SystemCode, authContext.SystemCodes);
					AddClaims(userIdentity, EsiaClaimTypes.SystemName, authContext.SystemNames);

					AddClaims(userIdentity, EsiaClaimTypes.OrganizationCode, authContext.OrganizationCodes);
					AddClaims(userIdentity, EsiaClaimTypes.OrganizationName, authContext.OrganizationNames);

					// Вызов точки расширения обработки успешной аутентификации
					await Options.Provider.Authenticated(authContext);

					userIdentity = authContext.Identity;
					properties = authContext.Properties;
				}
			}
			catch (Exception error)
			{
				_logger.WriteError(Properties.Resources.AuthenticationFailed, error);
			}

			return new AuthenticationTicket(userIdentity, properties);
		}


		/// <summary>
		/// Обработчик для автоматического входа пользователя в систему.
		/// </summary>
		public override async Task<bool> InvokeAsync()
		{
			var isRequestCompleted = false;

			// Если пришел ответ от сервера аутентификации
			if (Options.CallbackPath.HasValue && Options.CallbackPath == Request.Path)
			{
				var ticket = await AuthenticateAsync();

				if (ticket != null)
				{
					var context = new EsiaReturnEndpointContext(Context, ticket)
								  {
									  SignInAsAuthenticationType = Options.SignInAsAuthenticationType,
									  RedirectUri = ticket.Properties.RedirectUri
								  };

					await Options.Provider.ReturnEndpoint(context);

					// Если удостоверение пользователя установлено
					if (context.SignInAsAuthenticationType != null && context.Identity != null)
					{
						var userIdentity = context.Identity;

						if (!string.Equals(userIdentity.AuthenticationType, context.SignInAsAuthenticationType, StringComparison.Ordinal))
						{
							userIdentity = new ClaimsIdentity(userIdentity.Claims, context.SignInAsAuthenticationType, userIdentity.NameClaimType, userIdentity.RoleClaimType);
						}

						// Автоматический вход пользователя в систему
						Context.Authentication.SignIn(context.Properties, userIdentity);
					}

					if (!context.IsRequestCompleted && !string.IsNullOrWhiteSpace(context.RedirectUri))
					{
						var redirectUri = context.RedirectUri;

						if (context.Identity == null)
						{
							// Добавление в адрес подсказки, что вход не удался по какой-то причине
							redirectUri = WebUtilities.AddQueryString(redirectUri, "error", "access_denied");
						}

						Response.Redirect(redirectUri);
						context.RequestCompleted();
					}

					isRequestCompleted = context.IsRequestCompleted;
				}
				else
				{
					_logger.WriteWarning(Properties.Resources.UnableToRedirect);

					Response.StatusCode = 500;
					isRequestCompleted = true;
				}
			}

			return isRequestCompleted;
		}


		private void AddClaim(ClaimsIdentity identity, string claimType, string claimValue)
		{
			if (!string.IsNullOrEmpty(claimValue))
			{
				identity.AddClaim(new Claim(claimType, claimValue, ClaimValueTypes.String, Options.AuthenticationType));
			}
		}

		private void AddClaims(ClaimsIdentity identity, string claimType, IEnumerable<string> claimValues)
		{
			if (claimValues != null)
			{
				foreach (var claimValue in claimValues)
				{
					AddClaim(identity, claimType, claimValue);
				}
			}
		}

		private static string CreateUserName(string snils)
		{
			if (!string.IsNullOrEmpty(snils))
			{
				return "User" + snils.Replace(" ", "").Replace("-", "");
			}

			return null;
		}
	}
}
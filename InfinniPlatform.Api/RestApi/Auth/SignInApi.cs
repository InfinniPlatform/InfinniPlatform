using System;
using System.Collections.Generic;
using System.Net;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.Dynamic;
using RestSharp;

namespace InfinniPlatform.Api.RestApi.Auth
{
	/* Это вообще бред сивой кобылы. Пользователь вызывает серверный метод входа в систему,
	 * сервер сам логинится от имени этого пользователя. (То есть в итоге вход делает не
	 * пользователь, а сам сервер.) В итоге сервер получает куки от самого себя и отправляет
	 * их пользователю. (Наверное, глупо говорить, что делиться куками с кем-то, как минимум,
	 * не безопасно.) Для пользователя это звучит так: "Слышишь, мужик, я тут от твоего имя
	 * залогинился. Тебе мои куки нужны?" Пользователь, как ненормальный берет чужие куки,
	 * пытается их использовать при последующих обращениях к серверу и у него, естественно,
	 * ничего не получается. (Получится, если пользователь и сервер будут работать на одной
	 * машине - localhost. Но это, скорей, баг. В случае, когда пользователь и сервер на
	 * разных машинах, конечно, ничего не получится. И уж тем более не получится, если
	 * будет использоваться HTTPS, а он будет использоваться.) Собственно все ниже указанные
	 * авторские комментарии с непониманием того, что происходит, говорят о том, что все
	 * сказанное выше - верно!
	 */

	/// <summary>
	///     API для входа и выхода из системы
	///     Вызывается только с клиента
	/// </summary>
	[Obsolete]
	public sealed class SignInApi
	{
		/* Вообще, если даже не брать во внимание всю абсурдность этого функционала, которая
		 * описана выше, статический куки-контейнер (читай: глобальная переменная на весь
		 * процесс, на всех пользователей) это полный бред! В многопользовательской системе
		 * это работать не будет! Никогда! Был добавлен атрибут [ThreadStatic], чтобы хоть
		 * как-то обезопасить себя. Но это все равно не спасет ситуацию, т.к. потоки берутся
		 * из пула. Тем не менее, вероятность риска залезть на "чужую территорию" немного
		 * снижают. После небольшого исследования выяснилось, что этот контейнер по факту
		 * никем не используется. Можно сказать, что повезло. Однозначно, нужно избавляться
		 * от этого кода.
		 */

		[ThreadStatic]
		private static CookieContainer _cookieContainer;

		public static CookieContainer CookieContainer
		{
			get { return _cookieContainer; }
		}

		/// <summary>
		///     Зарегистрироваться в системе
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <param name="password">Пароль пользователя</param>
		/// <param name="remember">Запомнить пароль</param>
		public dynamic SignInInternal(string userName, string password, bool remember)
		{
			//TODO необходима доработка регистрации пользователя: должен возвращать Cookie с результатом
			var signInInternalUrl = ControllerRoutingFactory.Instance.GetCustomRouting("Auth/SignInInternal");


			var restClient = new RestClient(signInInternalUrl);

			var request = new RestRequest
			{
				RequestFormat = DataFormat.Json
			};
			request.AddParameter("UserName", userName)
				.AddParameter("Password", password);

			var restResponse = restClient.Post(request);
			if (restResponse.StatusCode != HttpStatusCode.OK)
			{
				throw new ArgumentException(string.Format("Fail in process of sign in user: {0} : {1} : ({2})",
					restResponse.ErrorException, restResponse.ErrorMessage, restResponse.Content));
			}

			var cookieContainer = new CookieContainer();

			var responseCookies = new List<dynamic>();

			foreach (var cookie in restResponse.Cookies)
			{
				var responseCookie = new Cookie
				{
					Name = cookie.Name,
					Value = cookie.Value,
					Domain = cookie.Domain,
					Expires = cookie.Expires,
					Path = cookie.Path,
					Discard = cookie.Discard
				};

				cookieContainer.Add(responseCookie);
				responseCookies.Add(responseCookie);
			}

			dynamic result = new DynamicWrapper();

			//гребаный JsonObjectSerializer добавляет служебный символ в начале при сериализации
			result.UserInfo = restResponse.Content.Substring(1).ToDynamic();

			result.ResponseCookies = responseCookies.ToEnumerable();

			result.IsValid = true;

			_cookieContainer = cookieContainer;

			return result;
		}

		/// <summary>
		///     Выйти из системы
		/// </summary>
		public dynamic SignOutInternal()
		{
			//----------------------------------------------------
			var signOutInternalUrl = ControllerRoutingFactory.Instance.GetCustomRouting("Auth/SignOut");

			var restClient = new RestClient(signOutInternalUrl);

			var request = new RestRequest
			{
				RequestFormat = DataFormat.Json
			}.AddBody("");


			var restResponse = restClient.Post(request);
			if (restResponse.StatusCode != HttpStatusCode.OK)
			{
				throw new ArgumentException(string.Format("Fail in process of sign out user: {0}", restResponse.Content));
			}

			//----------------------------------------------------
			//необходима доработка разлогинивания пользователей (при текущей реализации не разлогинивает пользователя на клиенте)

			dynamic result = new DynamicWrapper();
			result.IsValid = true;
			result.ValidationMessage = "User sign out successfully";

			_cookieContainer = null;

			return result;
		}

		/// <summary>
		///     Добавить пользователя во внутреннее хранилище
		/// </summary>
		/// <param name="userName">Пользователь</param>
		/// <param name="oldPassword">старый пароль</param>
		/// <param name="newPassword">старый пароль</param>
		public dynamic ChangePassword(string userName, string oldPassword, string newPassword)
		{
			return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "changepassword", null, new
				{
					UserName = userName,
					OldPassword = oldPassword,
					NewPassword = newPassword
				}).ToDynamic();
		}
	}
}
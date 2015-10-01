using System;
using System.Net;
using System.Net.Http;
using System.Text;

using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Properties;

using Newtonsoft.Json;

namespace InfinniPlatform.Sdk.Api
{
	/// <summary>
	/// API для аутентификации пользователей.
	/// </summary>
	public sealed class InfinniSignInApi : BaseApi, ISignInApi
	{
		public InfinniSignInApi(string server, string port, string route = "")
			: base(server, port, route)
		{
			var cookieContainer = new CookieContainer();
			var clientHandler = new HttpClientHandler { CookieContainer = cookieContainer };
			var clientBaseAddress = new Uri(string.Format("http://{0}:{1}/Auth/", server, port));
			var client = new HttpClient(clientHandler) { BaseAddress = clientBaseAddress };

			CookieContainer = cookieContainer;

			_client = client;
		}


		private readonly HttpClient _client;


		/// <summary>
		/// Осуществляет вход пользователя в систему через внутренний провайдер.
		/// </summary>
		/// <param name="userName">Имя пользователя.</param>
		/// <param name="password">Пароль пользователя.</param>
		/// <param name="remember">Запомнить пользователя.</param>
		public object SignInInternal(string userName, string password, bool remember = true)
		{
			return ExecutePostRequest("SignInInternal", new { UserName = userName, Password = password, Remember = remember });
		}

		/// <summary>
		/// Изменяет пароль текущего пользователя.
		/// </summary>
		/// <param name="oldPassword">Старый пароль пользователя.</param>
		/// <param name="newPassword">Новый пароль пользователя.</param>
		public object ChangePassword(string oldPassword, string newPassword)
		{
			return ExecutePostRequest("ChangePassword", new { OldPassword = oldPassword, NewPassword = newPassword });
		}

		/// <summary>
		/// Осуществляет выход пользователя из системы.
		/// </summary>
		public object SignOut()
		{
			return ExecutePostRequest("SignOut");
		}


		private string ExecutePostRequest(string requestUrl, object requestBody = null)
		{
			var jsonRequestBody = (requestBody != null) ? SerializeObjectToJson(requestBody) : string.Empty;
			var jsonRequestContent = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

			var response = _client.PostAsync(new Uri(requestUrl, UriKind.Relative), jsonRequestContent).Result;
			var content = response.Content.ReadAsStringAsync().Result;

			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidOperationException(string.Format(Resources.CannotProcessPostRequest, response.StatusCode, content));
			}

			return content;
		}

		private static string SerializeObjectToJson(object target)
		{
			if (target != null)
			{
				return JsonConvert.SerializeObject(target);
			}

			return null;
		}
	}
}
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Sdk.RestApi.Auth
{
    /// <summary>
    /// Реализует REST-клиент для AuthApi.
    /// </summary>
    public sealed class AuthApiClient : BaseRestClient
    {
        public AuthApiClient(string server, int port, IJsonObjectSerializer serializer = null) : base(server, port, serializer)
        {
        }

        /// <summary>
        /// Осуществляет вход пользователя в систему через внутренний провайдер.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <param name="remember">Запомнить пользователя.</param>
        public async Task SignInAsync(string userName, string password, bool remember = true)
        {
            var requestUri = BuildRequestUri("/Auth/SignInInternal");

            var requestData = new DynamicWrapper
                              {
                                  { "UserName", userName },
                                  { "Password", password },
                                  { "Remember", remember }
                              };

            var requestContent = new StringContent(Serializer.ConvertToString(requestData), Encoding.UTF8, HttpConstants.JsonContentType);

            var result = await RequestExecutor.PostAsync<DynamicWrapper>(requestUri, requestContent);

            var error = result["Error"] as string;

            if (error != null)
            {
                throw new InvalidOperationException(error);
            }
        }

        /// <summary>
        /// Осуществляет выход пользователя из системы.
        /// </summary>
        public async void SignOutAsync()
        {
            var requestUri = BuildRequestUri("/Auth/SignOut");

            await RequestExecutor.PostAsync<DynamicWrapper>(requestUri);
        }
    }
}
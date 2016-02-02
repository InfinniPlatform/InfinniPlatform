using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.RestApi
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
        public void SignInInternal(string userName, string password, bool remember = true)
        {
            var requestUri = BuildRequestUri("/Auth/SignInInternal");

            var requestData = new DynamicWrapper
                              {
                                  ["UserName"] = userName,
                                  ["Password"] = password,
                                  ["Remember"] = remember
                              };

            RequestExecutor.PostObject(requestUri, requestData);
        }

        /// <summary>
        /// Изменяет пароль текущего пользователя.
        /// </summary>
        /// <param name="oldPassword">Старый пароль пользователя.</param>
        /// <param name="newPassword">Новый пароль пользователя.</param>
        public void ChangePassword(string oldPassword, string newPassword)
        {
            var requestUri = BuildRequestUri("/Auth/ChangePassword");

            var requestData = new DynamicWrapper
                              {
                                  ["OldPassword"] = oldPassword,
                                  ["NewPassword"] = newPassword
                              };

            RequestExecutor.PostObject(requestUri, requestData);
        }

        /// <summary>
        /// Осуществляет выход пользователя из системы.
        /// </summary>
        public void SignOut()
        {
            var requestUri = BuildRequestUri("/Auth/SignOut");

            RequestExecutor.PostObject(requestUri);
        }
    }
}
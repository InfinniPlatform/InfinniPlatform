using System.Linq;
using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    ///   API для работы с ролями, пользователями и правами
    /// </summary>
    public sealed class InfinniAuthApi : BaseApi
    {
        public InfinniAuthApi(string server, string port, string version) : base(server, port, version)
        {
        }

        /// <summary>
        ///   Добавить нового пользователя системы
        /// </summary>
        /// <param name="userName">Пользователь системы</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Результат добавления</returns>
        public dynamic AddUser(string userName, string password)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlAddUser(Version), new
            {
                UserName = userName,
                Password = password
            });

            return ProcessAsObjectResult(response,
                string.Format(Resources.UnableToAddUser, response.GetErrorContent()));
        }

        /// <summary>
        ///  Удалить пользователя
        /// </summary>
        /// <param name="user">Пользователь системы</param>
        /// <returns>Результат удаления пользователя</returns>
        public dynamic DeleteUser(dynamic user)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDeleteUser(Version), user);

            return ProcessAsObjectResult(response, 
                string.Format(Resources.UnableToDeleteUser, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить пользователя системы
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Пользователь системы</returns>
        public dynamic GetUser(string userName)
        {
            var infinniDocumentApi = new InfinniDocumentApi(Server, Port, Version);

            return infinniDocumentApi.GetDocument("Administration", "User",
                f => f.AddCriteria(cr => cr.Property("UserName").IsEquals(userName)), 0, 1).FirstOrDefault();            
        }

    }
}

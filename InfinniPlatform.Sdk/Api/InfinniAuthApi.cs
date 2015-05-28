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

        /// <summary>
        ///   Предоставить доступ пользователю к указанному ресурсу
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="service">Наименование сервиса</param>
        /// <param name="instanceId">Идентификатор экземпляра сущности</param>
        /// <returns>Признак успешной установки прав</returns>
        public dynamic GrantAccess(string userName, string application, string documentType = null, string service = null, string instanceId = null)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            dynamic body = new
            {
                UserName = userName,
                Application = application,
                DocumentType = documentType,
                Service = service,
                InstanceId = instanceId
            };

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlGrantAccess(Version), body);

            return ProcessAsObjectResult(response,
                string.Format((string)Resources.UnableToGrantAccessToUser, response.GetErrorContent()));
        }

        /// <summary>
        ///  Запретить доступ пользователю к указанному ресурсу 
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="service">Наименование се</param>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public dynamic DenyAccess(string userName, string application, string documentType = null, string service = null,
            string instanceId = null)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            dynamic body = new
            {
                UserName = userName,
                Application = application,
                DocumentType = documentType,
                Service = service,
                InstanceId = instanceId
            };

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDenyAccess(Version), body);

            return ProcessAsObjectResult(response,
                string.Format((string)Resources.UnableToGrantAccessToUser, response.GetErrorContent()));
        }


        /// <summary>
        /// Установить пользователю указанное значение для claim указанного типа
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="claimType">Тип claim</param>
        /// <param name="claimValue">Значение claim для указанного типа claim</param>
        /// <returns>Результат запроса добавления утверждения</returns>
        public dynamic AddUserClaim(string userName, string claimType, string claimValue)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            dynamic body = new
            {
                UserName = userName,
                ClaimType = claimType,
                ClaimValue = claimValue
            };

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingAddUserClaim(Version), body);

            return ProcessAsObjectResult(response,
                string.Format(Resources.UnableToAddUserClaim, response.GetErrorContent()));
        }

        /// <summary>
        ///Получить значение утверждение относительно пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="claimType">Тип утверждениия относительно пользователя</param>
        /// <returns>Значение утверждения относительно пользователя</returns>
        public dynamic GetUserClaim(string userName, string claimType)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingGetUserClaim(Version, userName, claimType));

            return ProcessAsObjectResult(response,
                string.Format(Resources.UnableToGetUserClaim, response.GetErrorContent()));
        }

        /// <summary>
        /// Удалить у пользователя утверждение указанного типа
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="claimType">Тип утверждения относительно пользователя</param>
        /// <returns>Результат запроса удаления утверждения</returns>
        public dynamic RemoveUserClaim(string userName, string claimType)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            dynamic body = new
            {
                UserName = userName,
                ClaimType = claimType,
            };

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingRemoveUserClaim(Version), body);

            return ProcessAsObjectResult(response,
                string.Format(Resources.UnableToRemoveUserClaim, response.GetErrorContent()));
        }

    }
}

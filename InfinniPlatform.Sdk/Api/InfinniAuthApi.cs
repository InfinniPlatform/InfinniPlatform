using System;
using System.Collections.Generic;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.Api
{
    public enum AclType { User, Role }

    /// <summary>
    ///   API для работы с ролями, пользователями и правами
    /// </summary>
    public sealed class InfinniAuthApi : BaseApi, IAuthApi
    {
        public InfinniAuthApi(string server, string port, string route) : base(server, port, route)
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

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlAddUser(), new
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
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Результат удаления пользователя</returns>
        public dynamic DeleteUser(string userName)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingToSpecifiedUser(userName));

            return ProcessAsObjectResult(response, 
                string.Format(Resources.UnableToDeleteUser, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить пользователя системы
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Пользователь системы</returns>
        public dynamic GetUser(string userName) {

            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingToSpecifiedUser(userName));

            return ProcessAsObjectResult(response, 
                string.Format(Resources.UnableToGetUser, response.GetErrorContent()));        
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

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlGrantAccess(), body);

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

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDenyAccess(), body);

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
                ClaimValue = claimValue
            };

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingToSpecifiedUserClaim(userName, claimType), body);

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

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingToSpecifiedUserClaim(userName, claimType));

            return ProcessAsObjectResult(response,
                string.Format(Resources.UnableToGetUserClaim, response.GetErrorContent()));
        }

        /// <summary>
        /// Удалить у пользователя утверждение указанного типа
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="claimType">Тип утверждения относительно пользователя</param>
        /// <returns>Результат запроса удаления утверждения</returns>
        public dynamic DeleteUserClaim(string userName, string claimType)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingToSpecifiedUserClaim(userName,claimType));

            return ProcessAsObjectResult(response,
                string.Format(Resources.UnableToRemoveUserClaim, response.GetErrorContent()));
        }

        /// <summary>
        ///   Добавить роль пользователя
        /// </summary>
        /// <param name="roleName">Наименование роли</param>
        /// <returns>Результат добавления роли пользователя</returns>
        public dynamic AddRole(string roleName)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingToSpecifiedRole(roleName));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToAddRole,response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить роль пользователя
        /// </summary>
        /// <param name="roleName">Наименование роли</param>
        /// <returns>Результат удаления роли пользователя</returns>
        public dynamic DeleteRole(string roleName)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingToSpecifiedRole(roleName));

            return ProcessAsObjectResult(response,
                string.Format(Resources.UnableToDeleteRole, response.GetErrorContent()));
        }

        /// <summary>
        ///  Добавить роль пользователю
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="roleName">Роль</param>
        /// <returns>Результат добавления роли пользователю</returns>
        public dynamic AddUserRole(string userName, string roleName)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingToSpecifiedUserRole(userName, roleName));

            return ProcessAsObjectResult(response,
                string.Format(Resources.UnableToAddUserRole, response.GetErrorContent()));
        }

        /// <summary>
        /// Удалить роль пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="roleName">Роль пользователя</param>
        /// <returns>Результат удаления роли пользователя</returns>
        public dynamic DeleteUserRole(string userName, string roleName)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingToSpecifiedUserRole(userName, roleName));

            return ProcessAsObjectResult(response,
                string.Format(Resources.UnableToDeleteUserRole, response.GetErrorContent()));
        }

        /// <summary>
        ///Получить список объектов ACL указанного типа
        /// </summary>
        /// <param name="aclType">Тип объекта ACL</param>
        /// <param name="filter">Фильтр получаемых объектов</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sorting">Порядок сортировки</param>
        /// <returns>Список объектов ACL</returns>
        public IEnumerable<dynamic> GetAclList(
            AclType aclType,
            Action<FilterBuilder> filter,
            int pageNumber,
            int pageSize,
            Action<SortingBuilder> sorting = null)
        {
            var docApi = new InfinniDocumentApi(Server, Port, Route);

            docApi.CookieContainer = CookieContainer;

            if (aclType == AclType.User)
            {
                return docApi.GetDocument("Administration", "User", filter, pageNumber, pageSize, sorting);
            }
            if (aclType == AclType.Role)
            {
                return docApi.GetDocument("Administration", "Role", filter, pageNumber, pageSize, sorting);
            }
            throw new ArgumentException(string.Format(Resources.CantGetAclListForSpecifiedType,aclType));
        }
    }
}

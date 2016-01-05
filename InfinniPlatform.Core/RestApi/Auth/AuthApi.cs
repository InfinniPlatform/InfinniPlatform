using System;
using System.Collections.Generic;

using InfinniPlatform.Core.RestApi.CommonApi;

namespace InfinniPlatform.Core.RestApi.Auth
{
    /// <summary>
    /// API для работы с Access control list
    /// </summary>
    [Obsolete("Без проверки прав пользователя, от имени которого выполняется данный запрос, выполнять эти действия нельзя!")]
    public sealed class AuthApi
    {
        public AuthApi(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        /// <summary>
        /// Предоставить доступ пользователю
        /// </summary>
        /// <param name="configuration">Конфигурация, к которой предоставляется доступ</param>
        /// <param name="metadata">Метаданные, к которым предоставляется доступ</param>
        /// <param name="action">Действие, для которого предоставляется доступ</param>
        /// <param name="recordId">Экземпляр метаданных, к которым предоставлен доступ</param>
        /// <param name="userName">Пользователь, которому предоставляется доступ</param>
        public dynamic GrantAccess(string userName, string configuration, string metadata = null, string action = null,
                                   string recordId = null)
        {
            return GrantAccessTo(userName, configuration, metadata, action, recordId);
        }

        /// <summary>
        /// Предоставить доступ пользователю
        /// </summary>
        /// <param name="configuration">Конфигурация, к которой предоставляется доступ</param>
        /// <param name="metadata">Метаданные, к которым предоставляется доступ</param>
        /// <param name="action">Действие, для которого предоставляется доступ</param>
        /// <param name="recordId">Экземпляр метаданных, к которым предоставлен доступ</param>
        public dynamic GrantAccessAll(string configuration, string metadata = null, string action = null,
                                      string recordId = null)
        {
            return GrantAccessTo(AuthorizationStorageExtensions.Default, configuration, metadata, action, recordId);
        }

        private dynamic GrantAccessTo(string accessObject, string configuration, string metadata, string action,
                                      string recordId)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "applyaccess", null, new
                                                                                                      {
                                                                                                          Configuration = configuration,
                                                                                                          Metadata = metadata,
                                                                                                          Action = action,
                                                                                                          RecordId = recordId,
                                                                                                          UserName = accessObject,
                                                                                                          Result = true
                                                                                                      }).ToDynamic();
        }

        /// <summary>
        /// Предоставить доступ пользователю
        /// </summary>
        /// <param name="configuration">Конфигурация, к которой предоставляется доступ</param>
        /// <param name="metadata">Метаданные, к которым предоставляется доступ</param>
        /// <param name="action">Действие, для которого предоставляется доступ</param>
        /// <param name="recordId">Экземпляр метаданных, к которым предоставлен доступ</param>
        /// <param name="roleName">Роль, которой предоставляется доступ</param>
        public dynamic GrantAccessRole(string roleName, string configuration, string metadata = null,
                                       string action = null, string recordId = null)
        {
            return GrantAccessTo(roleName, configuration, metadata, action, recordId);
        }

        /// <summary>
        /// Заблокировать доступ пользователю
        /// </summary>
        /// <param name="configuration">Конфигурация, к которой заблокирован доступ</param>
        /// <param name="metadata">Метаданные, к которым заблокирован доступ</param>
        /// <param name="action">Действие, к которому заблокирован доступ</param>
        /// <param name="recordId">Запись, к которой заблокирован доступ</param>
        /// <param name="userName">Пользователь, которому заблокирован доступ</param>
        public dynamic DenyAccess(string userName, string configuration, string metadata = null, string action = null,
                                  string recordId = null)
        {
            return DenyAccessTo(userName, configuration, metadata, action, recordId);
        }

        private dynamic DenyAccessTo(string accessObject, string configuration, string metadata, string action,
                                     string recordId)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "applyaccess", null, new
                                                                                                      {
                                                                                                          Configuration = configuration,
                                                                                                          Metadata = metadata,
                                                                                                          Action = action,
                                                                                                          RecordId = recordId,
                                                                                                          UserName = accessObject,
                                                                                                          Result = false
                                                                                                      }).ToDynamic();
        }

        /// <summary>
        /// Заблокировать доступ пользователю
        /// </summary>
        /// <param name="configuration">Конфигурация, к которой заблокирован доступ</param>
        /// <param name="metadata">Метаданные, к которым заблокирован доступ</param>
        /// <param name="action">Действие, к которому заблокирован доступ</param>
        /// <param name="recordId">Запись, к которой заблокирован доступ</param>
        /// <param name="roleName">Роль, которой заблокирован доступ</param>
        public dynamic DenyAccessRole(string roleName, string configuration, string metadata = null,
                                      string action = null, string recordId = null)
        {
            return DenyAccessTo(roleName, configuration, metadata, action, recordId);
        }

        /// <summary>
        /// Заблокировать доступ всем пользователям
        /// </summary>
        /// <param name="configuration">Конфигурация, к которой заблокирован доступ</param>
        /// <param name="metadata">Метаданные, к которым заблокирован доступ</param>
        /// <param name="action">Действие, к которому заблокирован доступ</param>
        /// <param name="recordId">Запись, к которой заблокирован доступ</param>
        public dynamic DenyAccessAll(string configuration, string metadata = null, string action = null,
                                     string recordId = null)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "applyaccess", null, new
                                                                                                      {
                                                                                                          Configuration = configuration,
                                                                                                          Metadata = metadata,
                                                                                                          Action = action,
                                                                                                          RecordId = recordId,
                                                                                                          UserName = AuthorizationStorageExtensions.Default,
                                                                                                          Result = false
                                                                                                      }).ToDynamic();
        }

        /// <summary>
        /// Установить значение утверждения для пользователя
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="claimType">Тип утверждения</param>
        /// <param name="claimValue">Значение утверждения</param>
        public dynamic SetSessionData(string userName, string claimType, string claimValue)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "setsessiondata", null, new
                                                                                                         {
                                                                                                             UserName = userName,
                                                                                                             ClaimType = claimType,
                                                                                                             ClaimValue = claimValue
                                                                                                         }).ToDynamic();
        }

        /// <summary>
        /// Удалить значение утверждения для пользователя
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="claimType">Тип утверждения</param>
        /// <returns>Результат удаления</returns>
        public dynamic RemoveSessionData(string userName, string claimType)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "removesessiondata", null, new
                                                                                                            {
                                                                                                                UserName = userName,
                                                                                                                ClaimType = claimType
                                                                                                            }).ToDynamic();
        }

        /// <summary>
        /// Получить значение утверждения относительно пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="claimType">Тип утверждения</param>
        /// <param name="fromCache">Получить информацию из кэша</param>
        /// <returns>Значение утверждения относительно пользователя</returns>
        public dynamic GetSessionData(string userName, string claimType, bool fromCache = true)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "getsessiondata", null, new
                                                                                                         {
                                                                                                             UserName = userName,
                                                                                                             ClaimType = claimType,
                                                                                                             FromCache = fromCache
                                                                                                         }).ToDynamic();
        }

        /// <summary>
        /// Добавить пользователю роль
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="roleName">Роль</param>
        public dynamic AddUserToRole(string userName, string roleName)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "adduserrole", null, new
                                                                                                      {
                                                                                                          UserName = userName,
                                                                                                          RoleName = roleName
                                                                                                      }).ToDynamic();
        }

        /// <summary>
        /// Удалить роль пользователя
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="roleName">Роль</param>
        public dynamic RemoveUserRole(string userName, string roleName)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "removeuserrole", null, new
                                                                                                         {
                                                                                                             UserName = userName,
                                                                                                             RoleName = roleName
                                                                                                         }).ToDynamic();
        }

        /// <summary>
        /// Удалить ACL правило
        /// </summary>
        /// <param name="id">Идентификатор ACL</param>
        public dynamic RemoveAcl(string id)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "removeacl", null, new
                                                                                                    {
                                                                                                        AclId = id
                                                                                                    }).ToDynamic();
        }

        /// <summary>
        /// Получить список доступных ролей пользователей
        /// </summary>
        /// <param name="fromCache">Получить информацию из кэша</param>
        /// <returns>Список ролей пользователей</returns>
        public IEnumerable<dynamic> GetRoles(bool fromCache = true)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "getroles", null, new
                                                                                                   {
                                                                                                       FromCache = fromCache
                                                                                                   }).ToDynamicList();
        }

        /// <summary>
        /// Получить список версий конфигураций. установленных пользователям
        /// </summary>
        /// <param name="fromCache">Получить информацию из кэша</param>
        /// <returns>Список версий</returns>
        public IEnumerable<dynamic> GetVersions(bool fromCache = true)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "getversions", null, new
                                                                                                      {
                                                                                                          FromCache = fromCache
                                                                                                      }).ToDynamicList();
        }

        /// <summary>
        /// Получить список связанных с пользователями ролей
        /// </summary>
        /// <param name="fromCache">Получить информацию из кэша</param>
        /// <returns>Список связок</returns>
        public IEnumerable<dynamic> GetUserRoles(bool fromCache = true)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "getuserroles", null, new
                                                                                                       {
                                                                                                           FromCache = fromCache
                                                                                                       }).ToDynamicList();
        }

        /// <summary>
        /// Получить список прав доступа для всех пользователей
        /// </summary>
        /// <param name="fromCache">Получить информацию из кэша</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetAcl(bool fromCache = true)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "getacl", null, new
                                                                                                 {
                                                                                                     FromCache = fromCache
                                                                                                 }).ToDynamicList();
        }

        /// <summary>
        /// Получить список зарегистрированных пользователей
        /// </summary>
        /// <param name="fromCache">Получить информацию из кэша</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetUsers(bool fromCache = true)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "getusers", null, new
                                                                                                   {
                                                                                                       FromCache = fromCache
                                                                                                   }).ToDynamicList();
        }

        /// <summary>
        /// Получить указанного пользователя системы
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="fromCache">Получить информацию из кэша</param>
        /// <returns>Пользователь системы</returns>
        public dynamic GetUser(string userName, bool fromCache = true)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "getuser", null, new
                                                                                                  {
                                                                                                      UserName = userName,
                                                                                                      FromCache = fromCache
                                                                                                  }).ToDynamic();
        }

        /// <summary>
        /// Добавить пользователя во внутреннее хранилище
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="password">пароль</param>
        public dynamic AddUser(string userName, string password)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "Authorization", "adduser", null, new
                                                                                                  {
                                                                                                      UserName = userName,
                                                                                                      Password = password
                                                                                                  }).ToDynamic();
        }

        /// <summary>
        /// Удалить пользователя из внутреннего хранилища
        /// </summary>
        /// <param name="userName">Пользователь</param>
        public dynamic RemoveUser(string userName)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "Authorization", "removeuser", null, new
                                                                                                     {
                                                                                                         UserName = userName
                                                                                                     }).ToDynamic();
        }

        /// <summary>
        /// Добавить роль
        /// </summary>
        /// <param name="roleName">Роль</param>
        /// <param name="roleCaption"></param>
        /// <param name="roleDescription"></param>
        public dynamic AddRole(string roleName, string roleCaption, string roleDescription)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "Authorization", "addrole", null, new
                                                                                                  {
                                                                                                      Name = roleName,
                                                                                                      Caption = roleCaption,
                                                                                                      Description = roleDescription
                                                                                                  }).ToDynamic();
        }

        /// <summary>
        /// Удалить роль
        /// </summary>
        /// <param name="roleName">Роль</param>
        public dynamic RemoveRole(string roleName)
        {
            return _restQueryApi.QueryPostJsonRaw("RestfulApi", "Authorization", "removerole", null, new
                                                                                                     {
                                                                                                         RoleName = roleName
                                                                                                     }).ToDynamic();
        }

        /// <summary>
        /// Обновить список доступных ролей пользователей на сервере
        /// </summary>
        /// <returns>Список ролей пользователей</returns>
        public void InvalidateRoles()
        {
            _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "updateroles", null, null);
        }

        /// <summary>
        /// Обновить ACL на сервере
        /// </summary>
        /// <returns></returns>
        public void InvalidateAcl()
        {
            _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "updateacl", null, null);
        }

        /// <summary>
        /// Обновить утверждения относительно пользователей на сервере
        /// </summary>
        public void InvalidateUserClaims()
        {
            _restQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "updateuserclaims", null, null);
        }
    }
}
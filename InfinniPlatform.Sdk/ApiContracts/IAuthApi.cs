using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.Sdk.ApiContracts
{
	[Obsolete("Must be replaced with IApplicationUserManager")]
	public interface IAuthApi
    {
        /// <summary>
        ///   Добавить нового пользователя системы
        /// </summary>
        /// <param name="userName">Пользователь системы</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Результат добавления</returns>
        dynamic AddUser(string userName, string password);

        /// <summary>
        ///  Удалить пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Результат удаления пользователя</returns>
        dynamic DeleteUser(string userName);

        /// <summary>
        ///   Получить пользователя системы
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Пользователь системы</returns>
        dynamic GetUser(string userName);

        /// <summary>
        ///   Предоставить доступ пользователю к указанному ресурсу
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="service">Наименование сервиса</param>
        /// <param name="instanceId">Идентификатор экземпляра сущности</param>
        /// <returns>Признак успешной установки прав</returns>
        dynamic GrantAccess(string userName, string application, string documentType = null, string service = null, string instanceId = null);

        /// <summary>
        ///  Запретить доступ пользователю к указанному ресурсу 
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="service">Наименование се</param>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        dynamic DenyAccess(string userName, string application, string documentType = null, string service = null,
            string instanceId = null);

        /// <summary>
        /// Установить пользователю указанное значение для claim указанного типа
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="claimType">Тип claim</param>
        /// <param name="claimValue">Значение claim для указанного типа claim</param>
        /// <returns>Результат запроса добавления утверждения</returns>
        dynamic SetSessionData(string userName, string claimType, string claimValue);

        /// <summary>
        ///Получить значение утверждение относительно пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="claimType">Тип утверждениия относительно пользователя</param>
        /// <returns>Значение утверждения относительно пользователя</returns>
        dynamic GetSessionData(string userName, string claimType);

        /// <summary>
        /// Удалить у пользователя утверждение указанного типа
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="claimType">Тип утверждения относительно пользователя</param>
        /// <returns>Результат запроса удаления утверждения</returns>
        dynamic RemoveSessionData(string userName, string claimType);

        /// <summary>
        ///   Добавить роль пользователя
        /// </summary>
        /// <param name="roleName">Наименование роли</param>
        /// <returns>Результат добавления роли пользователя</returns>
        dynamic AddRole(string roleName);

        /// <summary>
        ///   Удалить роль пользователя
        /// </summary>
        /// <param name="roleName">Наименование роли</param>
        /// <returns>Результат удаления роли пользователя</returns>
        dynamic DeleteRole(string roleName);

        /// <summary>
        ///  Добавить роль пользователю
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="roleName">Роль</param>
        /// <returns>Результат добавления роли пользователю</returns>
        dynamic AddUserRole(string userName, string roleName);

        /// <summary>
        /// Удалить роль пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="roleName">Роль пользователя</param>
        /// <returns>Результат удаления роли пользователя</returns>
        dynamic DeleteUserRole(string userName, string roleName);
    }
}

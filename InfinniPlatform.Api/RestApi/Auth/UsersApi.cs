using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.RestApi.Auth
{
    /// <summary>
    ///     Публичный API для управления учетными записями пользователей
    /// </summary>
    public sealed class UsersApi
    {
        /// <summary>
        ///     Добавить пользователя во внутреннее хранилище
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="password">пароль</param>
        public dynamic AddUser(string userName, string password)
        {
            return RestQueryApi.QueryPostJsonRaw("Administration", "User", "adduser", null, new
                {
                    UserName = userName,
                    Password = password
                }).ToDynamic();
        }

        /// <summary>
        ///     Удалить пользователя из внутреннего хранилища
        /// </summary>
        /// <param name="userName">Пользователь</param>
        public dynamic RemoveUser(string userName)
        {
            return RestQueryApi.QueryPostJsonRaw("Administration", "User", "DeleteUser", null, new
                {
                    UserName = userName
                }).ToDynamic();
        }

        /// <summary>
        ///     Добавить роль
        /// </summary>
        /// <param name="roleName">Роль</param>
        /// <param name="roleCaption"></param>
        /// <param name="roleDescription"></param>
        public dynamic AddRole(string roleName, string roleCaption, string roleDescription)
        {
            return RestQueryApi.QueryPostJsonRaw("Administration", "Role", "addrole", null, new
                {
                    Name = roleName,
                    Caption = roleCaption,
                    Description = roleDescription
                }).ToDynamic();
        }

        /// <summary>
        ///     Удалить роль
        /// </summary>
        /// <param name="roleName">Роль</param>
        public dynamic DeleteRole(string roleName)
        {
            return RestQueryApi.QueryPostJsonRaw("Administration", "Role", "deleterole", null, new
                {
                    RoleName = roleName
                }).ToDynamic();
        }

        /// <summary>
        ///     Получить указанного пользователя системы
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Пользователь системы</returns>
        public dynamic GetUser(string userName)
        {
            return RestQueryApi.QueryPostJsonRaw("Administration", "User", "getuser", null, new
                {
                    UserName = userName
                }).ToDynamic();
        }

        /// <summary>
        ///     Добавить пользователю указанную роль
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="roleName">Роль пользователя</param>
        /// <returns>Результат добавления пользователя</returns>
        public dynamic AddUserRole(string userName, string roleName)
        {
            return RestQueryApi.QueryPostJsonRaw("Administration", "User", "adduserrole", null, new
                {
                    UserName = userName,
                    RoleName = roleName
                }).ToDynamic();
        }

        /// <summary>
        ///     Удалить у пользователя указанную роль
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="roleName">Роль пользователя</param>
        /// <returns>Результат удаления роли пользователя</returns>
        public dynamic DeleteUserRole(string userName, string roleName)
        {
            return RestQueryApi.QueryPostJsonRaw("Administration", "User", "deleteuserrole", null, new
                {
                    UserName = userName,
                    RoleName = roleName
                }).ToDynamic();
        }
    }
}
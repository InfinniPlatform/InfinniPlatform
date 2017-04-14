using System.Security.Principal;

namespace InfinniPlatform.Sdk.Security
{
    /// <summary>
    /// Предоставляет метод для получения идентификационных данных текущего пользователя.
    /// </summary>
    public interface IUserIdentityProvider
    {
        /// <summary>
        /// Возвращает идентификационные данные текущего пользователя.
        /// </summary>
        IIdentity GetUserIdentity();


        /// <summary>
        /// Устанавливает идентификационные данные текущего пользователя.
        /// </summary>
        void SetUserIdentity(IPrincipal identity);
    }
}
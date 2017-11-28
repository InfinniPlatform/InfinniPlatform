using System.Security.Principal;

namespace InfinniPlatform.Security
{
    /// <summary>
    /// Предоставляет метод для получения идентификационных данных текущего пользователя.
    /// </summary>
    public interface IUserIdentityProvider
    {
        /// <summary>
        /// Возвращает идентификационные данные текущего пользователя.
        /// </summary>
        IIdentity Get();
    }
}
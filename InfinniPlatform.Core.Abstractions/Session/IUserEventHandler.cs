using System.Security.Principal;
using System.Threading.Tasks;

namespace InfinniPlatform.Core.Session
{
    /// <summary>
    /// Обработчик событий пользователя.
    /// </summary>
    public interface IUserEventHandler
    {
        /// <summary>
        /// Вызывается после входа пользователя в систему.
        /// </summary>
        /// <param name="identity">Идентификационные данные пользователя.</param>
        Task OnAfterSignIn(IIdentity identity);

        /// <summary>
        /// Вызывается перед выходом пользователя из системы.
        /// </summary>
        /// <param name="identity">Идентификационные данные пользователя.</param>
        Task OnBeforeSignOut(IIdentity identity);
    }
}
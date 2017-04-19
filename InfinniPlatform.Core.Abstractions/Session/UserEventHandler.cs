using System.Security.Principal;
using System.Threading.Tasks;

namespace InfinniPlatform.Core.Session
{
    /// <summary>
    /// Базовый класс обработчика событий пользователя.
    /// </summary>
    public abstract class UserEventHandler : IUserEventHandler
    {
        /// <summary>
        /// Вызывается после входа пользователя в систему.
        /// </summary>
        /// <param name="identity">Идентификационные данные пользователя.</param>
        public virtual Task OnAfterSignIn(IIdentity identity)
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Вызывается после входа пользователя в систему.
        /// </summary>
        /// <param name="identity">Идентификационные данные пользователя.</param>
        public virtual Task OnBeforeSignOut(IIdentity identity)
        {
            return Task.FromResult<object>(null);
        }
    }
}
using System.Diagnostics;

namespace InfinniPlatform.Authentication.UserStorage
{
    /// <summary>
    /// Сообщение кэша пользователей.
    /// </summary>
    [DebuggerDisplay("{UserId}")]
    internal class UserCacheMessage
    {
        public UserCacheMessage(string userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Ключ.
        /// </summary>
        public string UserId { get; set; }
    }
}
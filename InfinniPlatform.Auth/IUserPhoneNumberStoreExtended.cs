using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Предоставляет абстракции для работы с хранилищем телефонных номеров пользователей.
    /// </summary>
    /// <typeparam name="TUser">Пользователь.</typeparam>
    public interface IUserPhoneNumberStoreExtended<TUser> : IUserPhoneNumberStore<TUser> where TUser : AppUser
    {
        /// <summary>
        /// Возвращает пользователя по номеру телефона.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task<TUser> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
    }
}
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Фабрика для получения генератора хэшей для паролей <see cref="IPasswordHasher{TUser}"/>.
    /// </summary>
    public interface IPasswordHasherFactory
    {
        IPasswordHasher<TUser> Get<TUser>(IContainerResolver resolver) where TUser : AppUser;
    }
}
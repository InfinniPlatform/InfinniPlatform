using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Фабрика для получения хранилища пользователей <see cref="IUserStore{TUser}"/>.
    /// </summary>
    public interface IUserStoreFactory
    {
        IUserStore<TUser> Get<TUser>(IContainerResolver resolver) where TUser : AppUser;
    }
}
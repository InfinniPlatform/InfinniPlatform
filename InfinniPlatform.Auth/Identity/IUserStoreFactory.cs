using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity
{
    /// <summary>
    /// Фабрика для получения экземпляров <see cref="IUserStore{TUser}" />.
    /// </summary>
    public interface IUserStoreFactory
    {
        IUserStore<TUser> GetUserStore<TUser>() where TUser : AppUser;
    }
}
using InfinniPlatform.Auth;
using InfinniPlatform.Auth.Identity;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.ServiceHost
{
    public class MemoryUserStoreFactory : ICustomUserStoreFactory
    {
        public IUserStore<TUser> GetUserStore<TUser>(IContainerResolver resolver) where TUser : AppUser
        {
            return new MemoryUserStore<TUser>();
        }
    }
}
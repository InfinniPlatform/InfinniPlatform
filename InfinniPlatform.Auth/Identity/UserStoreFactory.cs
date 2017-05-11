using InfinniPlatform.Auth.Identity.DocumentStorage;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity
{
    internal class UserStoreFactory : IUserStoreFactory
    {
        private readonly AuthOptions _authOptions;
        private readonly IContainerResolver _containerResolver;
        private object _userStore;

        public UserStoreFactory(IContainerResolver containerResolver, AuthOptions authOptions)
        {
            _containerResolver = containerResolver;
            _authOptions = authOptions;
            _userStore = null;
        }

        public IUserStore<TUser> GetUserStore<TUser>() where TUser : AppUser
        {
            if (_userStore == null)
            {
                _userStore = _authOptions.UserStoreFactory.GetUserStore<TUser>(_containerResolver) ?? _containerResolver.Resolve<UserStore<TUser>>();
            }

            return (IUserStore<TUser>) _userStore;
        }
    }
}
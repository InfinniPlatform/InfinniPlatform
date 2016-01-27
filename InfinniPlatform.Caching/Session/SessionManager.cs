using System;

using InfinniPlatform.Caching.Properties;
using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Session;

namespace InfinniPlatform.Caching.Session
{
    internal sealed class SessionManager : ISessionManager
    {
        public SessionManager(ICache cache, IUserIdentityProvider userIdentityProvider)
        {
            _cache = cache;
            _userIdentityProvider = userIdentityProvider;
        }


        private readonly ICache _cache;
        private readonly IUserIdentityProvider _userIdentityProvider;


        public void SetSessionData(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var sessionKey = GetSessionKey(key);

            if (string.IsNullOrEmpty(value))
            {
                _cache.Remove(sessionKey);
            }
            else
            {
                _cache.Set(sessionKey, value);
            }
        }

        public string GetSessionData(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var sessionKey = GetSessionKey(key);

            return _cache.Get(sessionKey);
        }

        private string GetSessionKey(string key)
        {
            var userId = GetCurrentUserId();

            return $"{userId}{key}";
        }

        private string GetCurrentUserId()
        {
            var currentIdentity = _userIdentityProvider.GetUserIdentity();
            var currentUserId = currentIdentity.GetUserId();
            var isNotAuthenticated = string.IsNullOrEmpty(currentUserId);

            if (isNotAuthenticated)
            {
                throw new InvalidOperationException(Resources.RequestIsNotAuthenticated);
            }

            return currentUserId;
        }
    }
}
using System;
using System.Security.Claims;
using System.Threading;

using InfinniPlatform.Caching.Properties;
using InfinniPlatform.Core.Security;
using InfinniPlatform.Sdk.Session;

namespace InfinniPlatform.Caching.Session
{
    internal sealed class SessionManager : ISessionManager
    {
        public SessionManager(ICache cache)
        {
            _cache = cache;
        }


        private readonly ICache _cache;


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

        private static string GetSessionKey(string key)
        {
            var userId = GetCurrentUserId();

            return $"{userId}{key}";
        }

        private static string GetCurrentUserId()
        {
            var currentIdentity = Thread.CurrentPrincipal?.Identity;
            var currentUserId = currentIdentity?.FindFirstClaim(ClaimTypes.NameIdentifier);
            var isNotAuthenticated = string.IsNullOrEmpty(currentUserId);

            if (isNotAuthenticated)
            {
                throw new InvalidOperationException(Resources.RequestIsNotAuthenticated);
            }

            return currentUserId;
        }
    }
}
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

using InfinniPlatform.Api.Security;
using InfinniPlatform.Caching.Properties;
using InfinniPlatform.Sdk.ContextComponents;

namespace InfinniPlatform.Caching
{
	public sealed class SessionManager : ISessionManager
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
			var currentIdentity = GetCurrentIdentity();
			var currentUserId = currentIdentity.FindFirstClaim(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(currentUserId))
			{
				throw new InvalidOperationException(Resources.UserIdNotFound);
			}

			return currentUserId;
		}

		private static IIdentity GetCurrentIdentity()
		{
			if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity == null)
			{
				throw new InvalidOperationException(Resources.RequestIsNotAuthenticated);
			}

			return Thread.CurrentPrincipal.Identity;
		}
	}
}
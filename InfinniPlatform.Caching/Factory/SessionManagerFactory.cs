using System;

using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.Caching.Factory
{
	public sealed class SessionManagerFactory : ISessionManagerFactory
	{
		public SessionManagerFactory()
		{
			var cacheType = AppSettings.GetValue("CacheType", "Memory");

			ICache cache;

			if (string.Equals(cacheType, "Redis", StringComparison.OrdinalIgnoreCase))
			{
				cache = CacheFactory.Instance.GetSharedCache();
			}
			else if (string.Equals(cacheType, "TwoLayer", StringComparison.OrdinalIgnoreCase))
			{
				cache = CacheFactory.Instance.GetTwoLayerCache();
			}
			else
			{
				cache = CacheFactory.Instance.GetMemoryCache();
			}

			_sessionManager = new SessionManager(cache);
		}


		private readonly ISessionManager _sessionManager;


		public ISessionManager CreateSessionManager()
		{
			return _sessionManager;
		}
	}
}
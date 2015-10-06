using System;
using System.Collections.Concurrent;

using InfinniPlatform.Sdk.ContextComponents;

namespace InfinniPlatform.ContextComponents
{
	[Obsolete]
	public sealed class SharedCacheComponent : ISharedCacheComponent
	{
		readonly ConcurrentDictionary<string, object> _internalCache = new ConcurrentDictionary<string, object>();

		public object Get(string key)
		{
			object result = null;
			_internalCache.TryGetValue(key, out result);
			return result;
		}

		public void Set(string key, object item)
		{
			_internalCache.AddOrUpdate(key, item, (k, oldValue) => item);
		}

		public void Lock()
		{
			//Здесь необходимо установить распределенную блокировку изменения кэша
		}

		public void Unlock()
		{
			//Здесь необходимо снять распределенную блокировку изменения кэша
		}
	}
}
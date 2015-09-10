using System;
using System.Collections.Generic;

namespace InfinniPlatform.Caching.Tests.TwoLayer
{
	public sealed class FakeCacheImpl : ICache, IDisposable
	{
		public readonly Dictionary<string, string> Data
			= new Dictionary<string, string>();


		public bool Contains(string key)
		{
			return Data.ContainsKey(key);
		}

		public string Get(string key)
		{
			string value;
			if (Data.TryGetValue(key, out value)) return value;
			return null;
		}

		public bool TryGet(string key, out string value)
		{
			return Data.TryGetValue(key, out value);
		}

		public void Set(string key, string value)
		{
			Data[key] = value;
		}

		public bool Remove(string key)
		{
			return Data.Remove(key);
		}

		public void Clear()
		{
			Data.Clear();
		}
		public void Dispose()
		{
			Data.Clear();
		}
	}
}
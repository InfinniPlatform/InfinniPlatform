using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Runtime.Implementation.AssemblyDispatch
{
	/// <summary>
	/// Список версионированных кэшей точек расширения.
	/// </summary>
	[Obsolete]
	internal sealed class MethodInvokationCacheList : IMethodInvokationCacheList
	{
		private readonly List<IMethodInvokationCache> _versions = new List<IMethodInvokationCache>();

		public IEnumerable<IMethodInvokationCache> CacheList
		{
			get { return _versions.OrderByDescending(v => v.TimeStamp); }
		}

		public void AddCache(IMethodInvokationCache methodInvokationCache)
		{
			_versions.Add(methodInvokationCache);
		}

		public IMethodInvokationCache GetCache(string version, bool returnsActual)
		{
			if (string.IsNullOrEmpty(version))
			{
				return _versions.OrderByDescending(v => v.TimeStamp).FirstOrDefault();
			}

			var versionCache = _versions.FirstOrDefault(v => v.Version == version);

			if (returnsActual)
			{
				return versionCache ?? (_versions.OrderByDescending(v => v.TimeStamp).FirstOrDefault());
			}

			return versionCache;
		}

		public void UpdateCaches()
		{
		}
	}
}
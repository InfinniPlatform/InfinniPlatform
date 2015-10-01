using System.Collections.Generic;

namespace InfinniPlatform.Runtime.Implementation.AssemblyDispatch
{
	internal sealed class PackageMethodInvokationCacheList : IMethodInvokationCacheList
	{
		public PackageMethodInvokationCacheList()
		{
			_methodInvokationCache = new PackageMethodInvokationCache();
			_methodInvokationCaches = new[] { _methodInvokationCache };
		}


		private readonly PackageMethodInvokationCache _methodInvokationCache;
		private readonly IEnumerable<IMethodInvokationCache> _methodInvokationCaches;


		public IEnumerable<IMethodInvokationCache> CacheList
		{
			get { return _methodInvokationCaches; }
		}

		public void AddCache(IMethodInvokationCache methodInvokationCache)
		{
		}

		public IMethodInvokationCache GetCache(string version, bool returnsActual)
		{
			return _methodInvokationCache;
		}

		public void UpdateCaches()
		{
			_methodInvokationCache.Update();
		}
	}
}
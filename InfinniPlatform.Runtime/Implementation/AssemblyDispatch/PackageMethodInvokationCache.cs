using System;
using System.Collections.Generic;
using System.Reflection;

namespace InfinniPlatform.Runtime.Implementation.AssemblyDispatch
{
	internal sealed class PackageMethodInvokationCache : IMethodInvokationCache
	{
		// Члены помечены как static намеренно, т.к. кэш на самом деле
		// является общим на процесс, и не разделяется на версии и т.п.

		private static volatile AppDomainSctiptCache _sctiptCache;
		private static readonly object SctiptCacheSync = new object();


		public string Version
		{
			get { return string.Empty; }
		}

		public DateTime TimeStamp
		{
			get { return default(DateTime); }
		}

		public void AddVersionAssembly(IEnumerable<Assembly> assemblies)
		{
		}

		public MethodInfo FindMethodInfo(string type, string method)
		{
			var sctiptCache = _sctiptCache;

			if (sctiptCache == null)
			{
				lock (SctiptCacheSync)
				{
					sctiptCache = _sctiptCache;

					if (sctiptCache == null)
					{
						sctiptCache = new AppDomainSctiptCache();

						_sctiptCache = sctiptCache;
					}
				}
			}

			var scriptMethod = sctiptCache.GetScriptMethod(type, method);

			return (scriptMethod != null) ? scriptMethod.Value : null;
		}

		public void Update()
		{
			if (_sctiptCache != null)
			{
				lock (SctiptCacheSync)
				{
					_sctiptCache = null;
				}
			}
		}
	}
}
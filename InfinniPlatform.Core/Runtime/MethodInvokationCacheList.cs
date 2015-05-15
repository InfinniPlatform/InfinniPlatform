using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Runtime
{
	/// <summary>
	///   Список версионированных кэшей точек расширения
	/// </summary>
	public sealed class MethodInvokationCacheList
	{
		private readonly Dictionary<string,MethodInvokationCache> _versions = new Dictionary<string, MethodInvokationCache>();

	    public IEnumerable<MethodInvokationCache> CacheList
	    {
            get { return _versions.Select(v => v.Value).OrderByDescending(v => v.TimeStamp); }
	    }

	    public void AddCache(string version, MethodInvokationCache methodInvokationCache)
		{
			_versions.Add(version,methodInvokationCache);
		}

        public MethodInvokationCache GetCache(string version, bool returnsActual)
		{
			if (string.IsNullOrEmpty(version))
			{
				return _versions.Select(v => v.Value).OrderByDescending(v => v.TimeStamp).FirstOrDefault();
			}

			MethodInvokationCache result;
			_versions.TryGetValue(version, out result);

            if (returnsActual)
            {
                return result ?? (_versions.Select(v => v.Value).OrderByDescending(v => v.TimeStamp).FirstOrDefault());
            }
            return result;
		}

		public void RemoveCache(string version)
		{
			MethodInvokationCache cache;
			if (_versions.TryGetValue(version, out cache))
			{
				_versions.Remove(version);
			}
		}

		public void ClearCache()
		{
			_versions.Clear();
		}

	    public MethodInvokationCache GetActualCache()
	    {
	        return GetCache(null,true);
	    }
	}
}

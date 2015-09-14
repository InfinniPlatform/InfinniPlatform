using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Runtime
{
    /// <summary>
    ///     Список версионированных кэшей точек расширения
    /// </summary>
    public sealed class MethodInvokationCacheList
    {
        private readonly object _lockObject = new object();
        private readonly List<MethodInvokationCache> _versions = new List<MethodInvokationCache>();

        public IEnumerable<MethodInvokationCache> CacheList
        {
            get { return _versions.OrderByDescending(v => v.TimeStamp); }
        }

        public void AddCache(MethodInvokationCache methodInvokationCache)
        {
            _versions.Add(methodInvokationCache);
        }

        public MethodInvokationCache GetCache(string version, bool returnsActual)
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

        public void RemoveCache(string version)
        {
            lock (_lockObject)
            {
                var excludeVersion = _versions.FirstOrDefault(v => v.Version == version);
                if (excludeVersion != null)
                {
                    _versions.Remove(excludeVersion);
                }
            }
        }

        public void ClearCache()
        {
            _versions.Clear();
        }

        public MethodInvokationCache GetActualCache()
        {
            return GetCache(null, true);
        }
    }
}
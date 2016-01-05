using System.Collections.Generic;

namespace InfinniPlatform.Core.Runtime
{
	/// <summary>
	/// Список кэшей прикладный скриптов конфигурации для вызова точек расширения бизнес-логики.
	/// </summary>
	public interface IMethodInvokationCacheList
	{
		IEnumerable<IMethodInvokationCache> CacheList { get; }

		void AddCache(IMethodInvokationCache methodInvokationCache);

		IMethodInvokationCache GetCache(string version, bool returnsActual);

		void UpdateCaches();
	}
}
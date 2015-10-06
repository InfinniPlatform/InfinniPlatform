using System;

namespace InfinniPlatform.Sdk.ContextComponents
{
	[Obsolete]
	public interface ISharedCacheComponent
	{
		object Get(string key);
		void Set(string key, object item);
		void Lock();
		void Unlock();
	}
}
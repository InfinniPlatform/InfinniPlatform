namespace InfinniPlatform.Runtime.Implementation.AssemblyDispatch
{
	public sealed class PackageVersionLoader : IVersionLoader
	{
		public IMethodInvokationCacheList ConstructInvokationCache()
		{
			return new PackageMethodInvokationCacheList();
		}

		public void UpdateInvokationCache(IMethodInvokationCacheList versionCacheList)
		{
			versionCacheList.UpdateCaches();
		}
	}
}
namespace InfinniPlatform.Runtime.Implementation.AssemblyDispatch
{
	public sealed class PackageVersionLoader : IVersionLoader
	{
		public IMethodInvokationCacheList ConstructInvokationCache(string metadataConfigurationId)
		{
			return new PackageMethodInvokationCacheList();
		}

		public void UpdateInvokationCache(string metadataConfigurationId, IMethodInvokationCacheList versionCacheList)
		{
			versionCacheList.UpdateCaches();
		}
	}
}
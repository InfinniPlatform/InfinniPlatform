using InfinniPlatform.Factories;
using InfinniPlatform.Runtime.Implementation.AssemblyDispatch;

namespace InfinniPlatform.Runtime.Factories
{
	public sealed class ScriptFactoryBuilder : IScriptFactoryBuilder
	{
		public IScriptFactory BuildScriptFactory(string metadataConfigurationId)
		{
			return new ScriptFactory(new PackageVersionLoader(), metadataConfigurationId);
		}
	}
}
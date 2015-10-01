using InfinniPlatform.Factories;
using InfinniPlatform.Runtime.Implementation.AssemblyDispatch;

namespace InfinniPlatform.Runtime.Factories
{
	public sealed class ScriptFactoryBuilder : IScriptFactoryBuilder
	{
		public ScriptFactoryBuilder(IChangeListener changeListener)
		{
			_changeListener = changeListener;
		}


		private readonly IChangeListener _changeListener;


		public IScriptFactory BuildScriptFactory(string metadataConfigurationId, string version)
		{
			return new ScriptFactory(new PackageVersionLoader(), _changeListener, metadataConfigurationId, version);
		}
	}
}
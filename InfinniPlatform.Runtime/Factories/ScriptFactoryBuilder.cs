using InfinniPlatform.Factories;

namespace InfinniPlatform.Runtime.Factories
{
    internal sealed class ScriptFactoryBuilder : IScriptFactoryBuilder
    {
        public ScriptFactoryBuilder(IScriptFactory scriptFactory)
        {
            _scriptFactory = scriptFactory;
        }


        private readonly IScriptFactory _scriptFactory;


        public IScriptFactory BuildScriptFactory()
        {
            return _scriptFactory;
        }
    }
}
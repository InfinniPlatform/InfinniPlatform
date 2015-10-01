using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Scripts
{
    internal sealed class ScriptsBuilder : IObjectBuilder
    {
        private readonly IScriptCompiler _scriptCompiler;

        public ScriptsBuilder()
        {
            _scriptCompiler = new ScriptCompilerCSharp();
        }

        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            // Todo: Кэширование скриптов представления (ScriptCache)

            return _scriptCompiler.Compile(metadata);
        }
    }
}
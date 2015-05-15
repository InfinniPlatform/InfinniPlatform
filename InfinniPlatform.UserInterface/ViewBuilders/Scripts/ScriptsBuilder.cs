using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Scripts
{
	sealed class ScriptsBuilder : IObjectBuilder
	{
		public ScriptsBuilder()
		{
			_scriptCompiler = new ScriptCompilerCSharp();
		}


		private readonly IScriptCompiler _scriptCompiler;


		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			// Todo: Кэширование скриптов представления (ScriptCache)

			return _scriptCompiler.Compile(metadata);
		}
	}
}
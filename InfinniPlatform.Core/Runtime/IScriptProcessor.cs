using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.Runtime
{
	public interface IScriptProcessor
	{
		object InvokeScript(string scriptIdentifier, dynamic scriptContext);
	}
}

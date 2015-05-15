namespace InfinniPlatform.Api.Hosting
{
	public interface IExtensionPointHandlerInstance
	{
		IExtensionPointHandlerInstance RegisterExtensionPoint(string extensionPointTypeName, string stateMachineReference);
		ExtensionPointValue GetExtensionPoint(string extensionPointTypeName);
		string HandlerInstanceName { get; }
	}
}

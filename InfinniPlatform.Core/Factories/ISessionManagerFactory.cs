using InfinniPlatform.Sdk.ContextComponents;

namespace InfinniPlatform.Factories
{
	public interface ISessionManagerFactory
	{
		ISessionManager CreateSessionManager();
	}
}
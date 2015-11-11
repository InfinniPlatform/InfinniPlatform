namespace InfinniPlatform.Sdk.ContextComponents
{
	public interface ISessionManager
	{
		void SetSessionData(string key, string value);

		string GetSessionData(string key);
	}
}
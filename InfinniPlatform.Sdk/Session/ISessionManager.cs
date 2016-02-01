namespace InfinniPlatform.Sdk.Session
{
    public interface ISessionManager
    {
        void SetSessionData(string key, string value);

        string GetSessionData(string key);
    }
}
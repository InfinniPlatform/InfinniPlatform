namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// Базовый класс для REST-клиентов.
    /// </summary>
    public abstract class BaseRestClient
    {
        protected BaseRestClient(string server, int port)
        {
            var baseUri = $"http://{server}";

            if (port > 0)
            {
                baseUri += $":{port}";
            }

            _baseUri = baseUri;

            RequestExecutor = RequestExecutor.Instance;
        }

        private readonly string _baseUri;

        protected readonly RequestExecutor RequestExecutor;

        protected string BuildRequestUri(string path)
        {
            return $"{_baseUri}{path}";
        }
    }
}
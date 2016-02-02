using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// Базовый класс для REST-клиентов.
    /// </summary>
    public abstract class BaseRestClient
    {
        protected BaseRestClient(string server, int port, IJsonObjectSerializer serializer = null)
        {
            var baseUri = $"http://{server}";

            if (port > 0)
            {
                baseUri += $":{port}";
            }

            _baseUri = baseUri;

            Serializer = serializer ?? JsonObjectSerializer.Default;

            RequestExecutor = new RequestExecutor(Serializer);
        }

        private readonly string _baseUri;

        protected readonly IJsonObjectSerializer Serializer;
        protected readonly RequestExecutor RequestExecutor;

        protected string BuildRequestUri(string path)
        {
            return $"{_baseUri}{path}";
        }
    }
}
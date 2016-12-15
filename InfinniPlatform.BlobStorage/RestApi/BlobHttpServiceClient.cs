using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.RestApi.Blob
{
    /// <summary>
    /// Реализует REST-клиент для сервисов по работе с BLOB (Binary Large OBject).
    /// </summary>
    public class BlobHttpServiceClient : BaseRestClient
    {
        public BlobHttpServiceClient(string server, int port, IJsonObjectSerializer serializer = null) : base(server, port, serializer)
        {
        }


        /// <summary>
        /// Выполняет запрос на получение BLOB.
        /// </summary>
        /// <param name="blobId">Идентификатор BLOB.</param>
        public Task<Stream> GetAsync(string blobId)
        {
            var requestUri = BuildRequestUri($"/blob/{blobId}");

            return RequestExecutor.GetStreamAsync(requestUri);
        }
    }
}
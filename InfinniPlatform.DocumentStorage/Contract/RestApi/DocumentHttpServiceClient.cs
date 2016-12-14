using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Contract.Services;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.RestApi;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.DocumentStorage.Contract.RestApi
{
    /// <summary>
    /// Реализует REST-клиент для сервисов по работе с документами.
    /// </summary>
    public sealed class DocumentHttpServiceClient : BaseRestClient
    {
        public DocumentHttpServiceClient(string documentType, string server, int port, IJsonObjectSerializer serializer = null) : base(server, port, serializer)
        {
            _basePath = $"/documents/{documentType}/";
        }


        private readonly string _basePath;


        /// <summary>
        /// Выполняет запрос на получение документов.
        /// </summary>
        public Task<ServiceResult<DocumentGetQueryResult>> GetAsync(DocumentGetQueryClient query)
        {
            var requestPath = new StringBuilder(_basePath).Append("?");

            if (query != null)
            {
                if (!string.IsNullOrWhiteSpace(query.Search))
                {
                    requestPath.AppendFormat("search={0}&", query.Search);
                }

                if (!string.IsNullOrWhiteSpace(query.Filter))
                {
                    requestPath.AppendFormat("filter={0}&", query.Filter);
                }

                if (!string.IsNullOrWhiteSpace(query.Select))
                {
                    requestPath.AppendFormat("select={0}&", query.Select);
                }

                if (!string.IsNullOrWhiteSpace(query.Order))
                {
                    requestPath.AppendFormat("order={0}&", query.Order);
                }

                if (query.Count == true)
                {
                    requestPath.Append("count=true&");
                }

                if (query.Skip > 0)
                {
                    requestPath.AppendFormat("skip={0}&", query.Skip);
                }

                if (query.Take > 0)
                {
                    requestPath.AppendFormat("take={0}&", query.Take);
                }
            }

            requestPath.Remove(requestPath.Length - 1, 1);

            var requestUri = BuildRequestUri(requestPath.ToString());

            return RequestExecutor.GetAsync<ServiceResult<DocumentGetQueryResult>>(requestUri);
        }

        /// <summary>
        /// Выполняет запрос на сохранение документов.
        /// </summary>
        public Task<ServiceResult<DocumentPostQueryResult>> PostAsync(DocumentPostQueryClient query)
        {
            var requestUri = BuildRequestUri(_basePath);

            var requestContent = new MultipartFormDataContent
                                 {
                                     { new StringContent(Serializer.ConvertToString(query.Document)), "\"document\"" }
                                 };

            if (query.Files != null)
            {
                foreach (var file in query.Files)
                {
                    var fileContent = new StreamContent(file.Value);

                    if (!string.IsNullOrEmpty(file.ContentType))
                    {
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    }

                    requestContent.Add(fileContent, $"\"{file.Key}\"", $"\"{file.Name}\"");
                }
            }

            return RequestExecutor.PostAsync<ServiceResult<DocumentPostQueryResult>>(requestUri, requestContent);
        }

        /// <summary>
        /// Выполняет запрос на удаление документов.
        /// </summary>
        public Task<ServiceResult<DocumentDeleteQueryResult>> DeleteAsync(DocumentDeleteQueryClient query)
        {
            var requestPath = new StringBuilder(_basePath).Append("?");

            if (!string.IsNullOrWhiteSpace(query?.Filter))
            {
                requestPath.AppendFormat("filter={0}&", query.Filter);
            }

            var requestUri = BuildRequestUri(requestPath.ToString());

            return RequestExecutor.DeleteAsync<ServiceResult<DocumentDeleteQueryResult>>(requestUri);
        }
    }
}
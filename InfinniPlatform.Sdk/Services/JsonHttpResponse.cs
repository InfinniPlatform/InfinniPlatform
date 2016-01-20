using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// Ответ в виде JSON-объекта.
    /// </summary>
    public sealed class JsonHttpResponse : HttpResponse
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="content">Содержимое тела ответа.</param>
        public JsonHttpResponse(object content)
        {
            ContentType = HttpConstants.JsonContentType;

            if (content != null)
            {
                Content = stream => JsonObjectSerializer.Default.Serialize(stream, content);
            }
        }
    }
}
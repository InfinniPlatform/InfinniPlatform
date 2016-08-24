using System.Text;

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
        public JsonHttpResponse(object content, IJsonObjectSerializer serializer = null)
        {
            Serializer = serializer;
            ContentType = HttpConstants.JsonContentType;

            if (content != null)
            {
                Content = stream => Serializer.Serialize(stream, content);
            }
        }

        /// <summary>
        /// Сериализатор объектов.
        /// </summary>
        public IJsonObjectSerializer Serializer { get; set; }
    }
}
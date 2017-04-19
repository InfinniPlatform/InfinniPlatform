using System;

using InfinniPlatform.Core.Serialization;

namespace InfinniPlatform.Core.Http
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
                Content = stream =>
                          {
                              try
                              {
                                  Serializer.Serialize(stream, content);
                              }
                              catch (ObjectDisposedException)
                              {
                                  //Ignore when client connection closed before response was ready.
                              }
                          };
            }
        }

        /// <summary>
        /// Сериализатор объектов.
        /// </summary>
        public IJsonObjectSerializer Serializer { get; set; }
    }
}
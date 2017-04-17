using System.Text;

namespace InfinniPlatform.Core.Abstractions.Http
{
    /// <summary>
    /// Ответ в виде текста.
    /// </summary>
    public sealed class TextHttpResponse : HttpResponse
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="content">Содержимое тела ответа.</param>
        /// <param name="contentType">Тип содержимого тела ответа.</param>
        /// <param name="encoding">Кодировка содержимого тела ответа.</param>
        public TextHttpResponse(string content, string contentType = HttpConstants.TextContentType, Encoding encoding = null)
        {
            ContentType = contentType;

            if (!string.IsNullOrEmpty(content))
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                var bytes = encoding.GetBytes(content);

                Content = stream => stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
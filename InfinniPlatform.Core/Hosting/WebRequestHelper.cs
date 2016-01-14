using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Web;

using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Core.Hosting
{
    public static class WebRequestHelper
    {
        private const int RequestTimeout = 5*60*1000;
        private const string JsonContentType = "application/json; encoding='utf-8'";
        private const string StreamContentType = "application/octet-stream";
        private static readonly IObjectSerializer Serializer = JsonObjectSerializer.Default;

        /// <summary>
        ///     Выполняет GET-запрос.
        /// </summary>
        public static Stream Get(Uri address)
        {
            var request = WebRequest.Create(address);
            request.Timeout = RequestTimeout;
            request.ContentType = JsonContentType;
            request.Method = "GET";

            return ProcessRequest(request);
        }

        /// <summary>
        ///     Выполняет DELETE-запрос.
        /// </summary>
        public static Stream Delete(Uri address)
        {
            var request = WebRequest.Create(address);
            request.Timeout = RequestTimeout;
            request.ContentType = JsonContentType;
            request.Method = "DELETE";

            return ProcessRequest(request);
        }

        /// <summary>
        ///     Выполняет POST-запрос.
        /// </summary>
        public static Stream Post(Uri address, object content, bool compress = false)
        {
            var contentBytes = Serializer.Serialize(content);
            return Post(address, contentBytes, compress);
        }

        /// <summary>
        ///     Выполняет POST-запрос.
        /// </summary>
        public static Stream Post(Uri address, byte[] content, bool compress = false)
        {
            var request = WebRequest.Create(address);
            request.Timeout = RequestTimeout;
            request.Method = "POST";

            using (var memory = new MemoryStream())
            {
                if (compress)
                {
                    request.ContentType = StreamContentType;

                    using (var compressionStream = new GZipStream(memory, CompressionMode.Compress, true))
                    {
                        compressionStream.Write(content, 0, content.Length);
                    }
                }
                else
                {
                    request.ContentType = JsonContentType;

                    memory.Write(content, 0, content.Length);
                }

                memory.Position = 0;

                request.ContentLength = memory.Length;

                var requestStream = request.GetRequestStream();
                memory.CopyTo(requestStream);
                requestStream.Close();
            }

            return ProcessRequest(request);
        }

        private static Stream ProcessRequest(WebRequest request)
        {
            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse) request.GetResponse();
            }
            catch (WebException error)
            {
                if (error.Response is HttpWebResponse)
                {
                    response = (HttpWebResponse) error.Response;
                }
                else
                {
                    throw;
                }
            }

            var result = response.GetResponseStream();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                if (result != null)
                {
                    using (result)
                    {
                        using (var reader = new StreamReader(result))
                        {
                            throw new HttpException((int) response.StatusCode, reader.ReadToEnd());
                        }
                    }
                }

                throw new HttpException((int) response.StatusCode, response.StatusDescription);
            }

            return result;
        }

        /// <summary>
        ///     Преобразовать поток в объект.
        /// </summary>
        public static T AsObject<T>(this Stream stream)
        {
            if (stream != null)
            {
                if (stream.CanSeek)
                {
                    stream.Position = 0;
                }

                using (stream)
                {
                    return (T) Serializer.Deserialize(stream, typeof (T));
                }
            }

            return default(T);
        }

        /// <summary>
        ///     Преобразовать поток в массив байт.
        /// </summary>
        public static byte[] AsBytes(this Stream stream)
        {
            if (stream != null)
            {
                if (stream.CanSeek)
                {
                    stream.Position = 0;
                }

                using (stream)
                {
                    using (var memory = new MemoryStream())
                    {
                        stream.CopyTo(memory);
                        return memory.ToArray();
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///     Добавляет относительный адрес к базовому.
        /// </summary>
        public static Uri AppendRelative(this Uri baseUri, string relativeUri)
        {
            // Это пришлось сделать из-за какого-то странного поведения конструктора: Uri(Uri baseUri, string relativeUri)

            return new Uri(baseUri.ToString().TrimEnd('/') + "/" + (relativeUri ?? string.Empty).TrimStart('/'));
        }
    }
}
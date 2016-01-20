using System;
using System.IO;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// Ответ в виде потока байт.
    /// </summary>
    public sealed class StreamHttpResponse : HttpResponse
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="contentType">Тип содержимого тела ответа.</param>
        /// <exception cref="ArgumentNullException">Параметр <paramref name="filePath"/> не задан.</exception>
        public StreamHttpResponse(string filePath, string contentType = HttpConstants.StreamContentType)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                StatusCode = 404;
            }
            else
            {
                var fileLength = fileInfo.Length;

                FileName = fileInfo.Name;
                LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;

                ContentType = contentType;
                ContentLength = fileLength;

                if (fileLength > 0)
                {
                    Content = stream =>
                    {
                        using (var fileStream = File.OpenRead(filePath))
                        {
                            CopyStream(fileStream, stream, fileLength);
                        }
                    };
                }
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="fileStream">Содержимое файла.</param>
        /// <param name="contentType">Тип содержимого тела ответа.</param>
        /// <exception cref="ArgumentNullException">Параметр <paramref name="fileStream"/> не задан.</exception>
        public StreamHttpResponse(Stream fileStream, string contentType = HttpConstants.StreamContentType)
        {
            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            var fileLength = fileStream.Length;

            ContentType = contentType;
            ContentLength = fileLength;

            if (fileLength > 0)
            {
                Content = stream =>
                          {
                              CopyStream(fileStream, stream, fileLength);
                          };
            }
        }

        public StreamHttpResponse(byte[] fileContent, string contentType = HttpConstants.StreamContentType)
        {
            if (fileContent == null)
            {
                throw new ArgumentNullException(nameof(fileContent));
            }

            var fileLength = fileContent.Length;

            ContentType = contentType;
            ContentLength = fileLength;

            if (fileLength > 0)
            {
                Content = stream =>
                          {
                              using (var fileStream = new MemoryStream(fileContent))
                              {
                                  CopyStream(fileStream, stream, fileLength);
                              }
                          };
            }
        }


        /// <summary>
        /// Наименование файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Размер файла в байтах.
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        /// Время последнего изменения файла.
        /// </summary>
        public DateTime? LastWriteTimeUtc { get; set; }


        private static void CopyStream(Stream source, Stream destination, long sourseLength)
        {
            source.CopyTo(destination, (sourseLength < HttpConstants.FileBufferSize) ? (int)sourseLength : HttpConstants.FileBufferSize);
        }
    }
}
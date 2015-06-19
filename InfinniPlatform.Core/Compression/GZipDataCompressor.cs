using System.IO;
using System.IO.Compression;

namespace InfinniPlatform.Compression
{
    /// <summary>
    ///     Компрессор данных на базе GZIP.
    /// </summary>
    public sealed class GZipDataCompressor : IDataCompressor
    {
        /// <summary>
        ///     Сжимает исходный поток.
        /// </summary>
        /// <param name="source">Поток с исходными данными.</param>
        /// <param name="destination">Поток для записи результата.</param>
        public void Compress(Stream source, Stream destination)
        {
            ResetPosition(source);

            using (var compressionStream = new GZipStream(destination, CompressionLevel.Fastest, true))
            {
                source.CopyTo(compressionStream);
            }
        }

        /// <summary>
        ///     Распаковывает исходный поток.
        /// </summary>
        /// <param name="source">Поток с исходными данными.</param>
        /// <param name="destination">Поток для записи результата.</param>
        public void Decompress(Stream source, Stream destination)
        {
            ResetPosition(source);

            using (var decompressionStream = new GZipStream(source, CompressionMode.Decompress, true))
            {
                decompressionStream.CopyTo(destination);
            }
        }

        private static void ResetPosition(Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
        }
    }
}
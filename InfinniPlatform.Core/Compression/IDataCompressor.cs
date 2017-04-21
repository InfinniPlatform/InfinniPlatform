using System.IO;

namespace InfinniPlatform.Compression
{
    /// <summary>
    /// Компрессор данных.
    /// </summary>
    public interface IDataCompressor
    {
        /// <summary>
        /// Сжимает исходный поток.
        /// </summary>
        /// <param name="source">Поток с исходными данными.</param>
        /// <param name="destination">Поток для записи результата.</param>
        void Compress(Stream source, Stream destination);

        /// <summary>
        /// Распаковывает исходный поток.
        /// </summary>
        /// <param name="source">Поток с исходными данными.</param>
        /// <param name="destination">Поток для записи результата.</param>
        void Decompress(Stream source, Stream destination);
    }
}
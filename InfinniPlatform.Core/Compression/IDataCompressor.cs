using System;
using System.IO;
using InfinniPlatform.Json;

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

    /// <summary>
    ///   Расширения для задач сжатия данных
    /// </summary>
    public static class DataCompressorExtensions
    {
        /// <summary>
        ///   Сжать файл
        /// </summary>
        /// <param name="dataCompressor">Компрессор данных</param>
        /// <param name="filePath">Путь к сжимаемому файлу</param>
        /// <returns>Распакованный файл</returns>
        public static Stream DecompressFile(this IDataCompressor dataCompressor, string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException(string.Format("file '{0}' not exists", filePath));
            }
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var decompressedStream = new MemoryStream();
                dataCompressor.Decompress(fileStream,decompressedStream);
                return decompressedStream;
            }            
        }

        public static JsonArrayStreamEnumerable ReadAsJsonEnumerable(this IDataCompressor compressor, string filePath)
        {
            return new JsonArrayStreamEnumerable(compressor.DecompressFile(filePath));
        } 


    }

}
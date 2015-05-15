using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Compression;
using InfinniPlatform.Json;

namespace InfinniPlatform.Runtime
{

    /// <summary>
    ///   Расширения для записи пакетов обновления в файлы архива
    /// </summary>
    public static class PackageBuilderExtensions
    {
        /// <summary>
        ///   Записать все данные из потока в указанный файл
        /// </summary>
        /// <param name="stream">Поток для чтения</param>
        /// <param name="filePath">Путь к файлу хранения данных</param>
        public static void WriteAllBytes(this Stream stream, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                stream.Position = 0;
                stream.CopyTo(fileStream);
                fileStream.Flush();
            }
        }



    }
}

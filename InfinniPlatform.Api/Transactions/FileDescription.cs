using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Transactions
{

    /// <summary>
    ///   Описание связанного с документом файла
    /// </summary>
    public sealed class FileDescription
    {
        /// <summary>
        ///   Поле ссылки в документе, связанное с файлом
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        ///   Файловый поток
        /// </summary>
        public byte[] Bytes { get; set; }
    }
}

using System.IO;

using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.ContextTypes
{
    public sealed class UploadContext : CommonContext, IUploadContext
    {
        /// <summary>
        /// Поток с данными переданного файла
        /// </summary>
        public Stream FileContent { get; set; }

        /// <summary>
        /// Идентификатор связанного объекта
        /// </summary>
        public dynamic LinkedData { get; set; }
    }
}
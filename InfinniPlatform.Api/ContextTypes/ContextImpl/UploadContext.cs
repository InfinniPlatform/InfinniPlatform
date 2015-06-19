using System.IO;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.Api.ContextTypes.ContextImpl
{
    public sealed class UploadContext : IUploadContext
    {
        public IGlobalContext Context { get; set; }
        public dynamic ValidationMessage { get; set; }
        public bool IsValid { get; set; }
        public bool IsInternalServerError { get; set; }
        public string Version { get; set; }
        public string Configuration { get; set; }
        public string Metadata { get; set; }
        public string Action { get; set; }
        public string UserName { get; set; }

        /// <summary>
        ///     Поток с данными переданного файла
        /// </summary>
        public Stream FileContent { get; set; }

        /// <summary>
        ///     Идентификатор связанного объекта
        /// </summary>
        public dynamic LinkedData { get; set; }

        /// <summary>
        ///     Результат обработки
        /// </summary>
        public dynamic Result { get; set; }
    }
}
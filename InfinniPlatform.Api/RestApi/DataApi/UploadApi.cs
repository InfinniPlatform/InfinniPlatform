using System.IO;

using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    /// <summary>
    /// API для выгрузки двоичного контента
    /// </summary>
    public sealed class UploadApi
    {
        public UploadApi(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        /// <summary>
        /// Выгрузить на сервер бинарный контент
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="metadata">Идентификатор типа документа</param>
        /// <param name="documentId"></param>
        /// <param name="fieldName">Поле, содержащее ссылку на бинарный контент</param>
        /// <param name="filePath">Путь к загружаемому файлу</param>
        /// <returns>Результат загрузки файла</returns>
        public dynamic UploadBinaryContent(string applicationId, string metadata, string documentId, string fieldName, string filePath)
        {
            var linkedData = new
                             {
                                 Configuration = applicationId,
                                 Metadata = metadata,
                                 DocumentId = documentId,
                                 FieldName = fieldName
                             };

            return
                _restQueryApi.QueryPostFile("RestfulApi", "configuration", "uploadbinarycontent", linkedData, filePath).ToDynamic();
        }

        /// <summary>
        /// Выгрузить на сервер бинарный контент
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="documentType"></param>
        /// <param name="documentId"></param>
        /// <param name="fieldName">Поле, содержащее ссылку на бинарный контент</param>
        /// <param name="fileName">Путь к загружаемому файлу</param>
        /// <param name="stream">Результат загрузки файла</param>
        /// <returns>Результат загрузки файла</returns>
        public dynamic UploadBinaryContent(string applicationId, string documentType, string documentId, string fieldName, string fileName, Stream stream)
        {
            var linkedData = new
                             {
                                 Configuration = applicationId,
                                 Metadata = documentType,
                                 DocumentId = documentId,
                                 FieldName = fieldName,
                                 FileName = fileName
                             };

            return
                _restQueryApi.QueryPostFile("RestfulApi", "configuration", "uploadbinarycontent", linkedData, stream).ToDynamic();
        }

        /// <summary>
        /// Загрузить бинарный контент для указанного идентификатора ссылки на бинарный контент
        /// </summary>
        /// <param name="contentId">Идентификатор ссылки на бинарный контент</param>
        /// <returns>Данные бинарного контента</returns>
        public dynamic DownloadBinaryContent(string contentId)
        {
            return _restQueryApi.QueryGetUrlEncodedData("RestfulApi", "configuration", "downloadbinarycontent", new
                                                                                                                {
                                                                                                                    ContentId = contentId
                                                                                                                });
        }
    }
}
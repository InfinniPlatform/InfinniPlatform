using System.IO;

namespace InfinniPlatform.Sdk.RestApi
{
    public interface IFileApi
    {
        /// <summary>
        /// Загрузить файл на сервер
        /// </summary>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Идентификатор документа</param>
        /// <param name="documentId"></param>
        /// <param name="fieldName">Наименование поля в документе, хранящее ссылку на файл</param>
        /// <param name="fileName">Наименование файла</param>
        /// <param name="fileStream">Файловый поток</param>
        /// <returns>Результат загрузки файла на сервер</returns>
        dynamic UploadFile(string application, string documentType, string documentId, string fieldName, string fileName, Stream fileStream);

        /// <summary>
        /// Загрузить файл с сервера
        /// </summary>
        /// <param name="contentId">Идентификатор контента</param>
        /// <returns>Выгруженный контент</returns>
        dynamic DownloadFile(string contentId);
    }
}
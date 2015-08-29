using System.IO;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    ///   API для работы с файлами
    /// </summary>
    public sealed class InfinniFileApi : BaseApi, IFileApi
    {
        public InfinniFileApi(string server, string port, string route)
            : base(server, port, route)
        {
        }


        /// <summary>
        ///   Загрузить файл на сервер
        /// </summary>
        /// <param name="application">Приложение</param>
        /// <param name="documentType">Идентификатор документа</param>
        /// <param name="instanceId">Экземпляр документа</param>
        /// <param name="fieldName">Наименование поля в документе, хранящее ссылку на файл</param>
        /// <param name="fileName">Наименование файла</param>
        /// <param name="fileStream">Файловый поток</param>
        /// <returns>Результат загрузки файла на сервер</returns>
        public dynamic UploadFile(string application, string documentType, string instanceId, string fieldName, string fileName,
            Stream fileStream)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);


            var response = restQueryExecutor.QueryPostFile(RouteBuilder.BuildRestRoutingUploadFile(), instanceId, fieldName, fileName, fileStream);

            return ProcessAsObjectResult(response,string.Format(Resources.UnableToUploadFileOnServer, response.GetErrorContent()));   
        }

        /// <summary>
        ///   Загрузить файл с сервера
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns>Выгруженный контент</returns>
        public dynamic DownloadFile(string contentId)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var linkedData = new
            {
                ContentId = contentId,
            };

            var response = restQueryExecutor.QueryGetUrlEncodedData(
                    RouteBuilder.BuildRestRoutingDownloadFile(), linkedData);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDownloadFileFromServer, response.GetErrorContent())); 
        }

    }
}

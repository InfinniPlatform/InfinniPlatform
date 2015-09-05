using System.IO;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.RestApi.DataApi
{
	/// <summary>
    ///     API для выгрузки двоичного контента
	/// </summary>
	public sealed class UploadApi
	{
	    /// <summary>
	    ///     Выгрузить на сервер бинарный контент
	    /// </summary>
	    /// <param name="metadataType">Идентификатор типа документа</param>
	    /// <param name="instanceId">Идентификатор экземпляра</param>
	    /// <param name="fieldName">Поле, содержащее ссылку на бинарный контент</param>
	    /// <param name="filePath">Путь к загружаемому файлу</param>
	    /// <param name="applicationId">Идентификатор приложения</param>
	    /// <returns>Результат загрузки файла</returns>
	    public dynamic UploadBinaryContent(string applicationId, string metadataType, string instanceId, string fieldName, string filePath)
        {
            var linkedData = new
            {
                Configuration = applicationId,
                MetadataType =  metadataType,
                InstanceId = instanceId,
                FieldName = fieldName
            };

            return
                RestQueryApi.QueryPostFile("RestfulApi", "configuration", "uploadbinarycontent", linkedData, filePath).ToDynamic();
        }

        /// <summary>
        ///     Выгрузить на сервер бинарный контент
        /// </summary>
        /// <param name="applicationId">Идентификатор приложения</param>
        /// <param name="instanceId">Идентификатор экземпляра</param>
        /// <param name="fieldName">Поле, содержащее ссылку на бинарный контент</param>
        /// <param name="fileName">Путь к загружаемому файлу</param>
        /// <param name="stream">Результат загрузки файла</param>
        /// <param name="metadataType">Идентификатор типа документа</param>
        /// <returns>Результат загрузки файла</returns>
        public dynamic UploadBinaryContent(string applicationId, string metadataType, string instanceId, string fieldName,
            string fileName, Stream stream)
		{			
			var linkedData = new
			{
                Configuration = applicationId,
                Metadata = metadataType,
                InstanceId = instanceId,
				FieldName = fieldName,
                FileName = fileName
			};

            return
                RestQueryApi.QueryPostFile("RestfulApi", "configuration", "uploadbinarycontent", linkedData, stream).ToDynamic();
		}

        /// <summary>
        ///   Загрузить бинарный контент для указанного идентификатора ссылки на бинарный контент
        /// </summary>
        /// <param name="contentId">Идентификатор ссылки на бинарный контент</param>
        /// <returns>Данные бинарного контента</returns>
	    public dynamic DownloadBinaryContent(string contentId)
        {
            return RestQueryApi.QueryGetUrlEncodedData("RestfulApi", "configuration", "downloadbinarycontent", new
                {
                    ContentId = contentId
                });
        }
	}
}

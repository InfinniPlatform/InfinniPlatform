using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.RestApi.DataApi
{
	/// <summary>
	///   API для выгрузки двоичного контента
	/// </summary>
	public sealed class UploadApi
	{
		/// <summary>
		///   Выгрузить на сервер бинарный контент
		/// </summary>
		/// <param name="configuration">Конфигурация</param>
		/// <param name="metadata">Тип документа</param>
		/// <param name="documentId">Идентификатор экземпляра</param>
		/// <param name="fieldName">Поле, содержащее ссылку на бинарный контент</param>
		/// <param name="filePath">Путь к загружаемому файлу</param>
		/// <returns>Результат загрузки файла</returns>
		public dynamic UploadBinaryContent(string configuration, string metadata, string documentId, string fieldName, string filePath)
		{			
			var linkedData = new
			{
				Configuration = configuration,
				Metadata = metadata,
				DocumentId = documentId,
				FieldName = fieldName,
			};

			return RestQueryApi.QueryPostFile("RestfulApi", "configuration", "uploadbinarycontent", linkedData, filePath).ToDynamic();
		}

        /// <summary>
        ///   Выгрузить на сервер бинарный контент
        /// </summary>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="metadata">Тип документа</param>
        /// <param name="documentId">Идентификатор экземпляра</param>
        /// <param name="fieldName">Поле, содержащее ссылку на бинарный контент</param>
        /// <param name="fileName">Путь к загружаемому файлу</param>
        /// <param name="stream">Результат загрузки файла</param>
        /// <returns>Результат загрузки файла</returns>
	    public dynamic UploadBinaryContent(string configuration, string metadata, string documentId, string fieldName,
	        string fileName, Stream stream)
        {
            var linkedData = new
            {
                Configuration = configuration,
                Metadata = metadata,
                DocumentId = documentId,
                FieldName = fieldName,
            };

            return RestQueryApi.QueryPostFile("RestfulApi", "configuration", "uploadbinarycontent", linkedData, fileName, stream).ToDynamic();
	    }

		/// <summary>
		///   Загрузить бинарный контент для указанного поля указанного документа 
		/// </summary>
		/// <param name="configuration">Конфигурация</param>
		/// <param name="metadata">Тип документа</param>
		/// <param name="documentId">Идентификатор документа</param>
		/// <param name="fieldName">Поле бинарного контента</param>
		/// <returns>Данные бинарного контента</returns>
		public dynamic DownloadBinaryContent(string configuration, string metadata, string documentId, string fieldName)
		{
			return RestQueryApi.QueryGetUrlEncodedData("RestfulApi", "configuration", "downloadbinarycontent", new
				                                                                                                    {
																														Configuration = configuration,
																														Metadata = metadata,
					                                                                                                    DocumentId = documentId,
																														FieldName = fieldName
				                                                                                                    });
		}
	}
}

﻿using System;
using System.Collections.Generic;
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
		///   Выгрузить на сервер двоичный контент
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

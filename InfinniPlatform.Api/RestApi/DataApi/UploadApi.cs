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
	    private readonly string _version;

	    public UploadApi(string version)
	    {
	        _version = version;
	    }

	    /// <summary>
		///   Выгрузить на сервер бинарный контент
		/// </summary>
		/// <param name="instanceId">Идентификатор экземпляра</param>
		/// <param name="fieldName">Поле, содержащее ссылку на бинарный контент</param>
		/// <param name="filePath">Путь к загружаемому файлу</param>
		/// <returns>Результат загрузки файла</returns>
		public dynamic UploadBinaryContent(string instanceId, string fieldName, string filePath)
		{			
			var linkedData = new
			{
				InstanceId = instanceId,
				FieldName = fieldName,
			};

			return RestQueryApi.QueryPostFile("RestfulApi", "configuration", "uploadbinarycontent", linkedData, filePath,_version).ToDynamic();
		}


        /// <summary>
        ///   Выгрузить на сервер бинарный контент
        /// </summary>
        /// <param name="instanceId">Идентификатор экземпляра</param>
        /// <param name="fieldName">Поле, содержащее ссылку на бинарный контент</param>
        /// <param name="fileName">Путь к загружаемому файлу</param>
        /// <param name="stream">Результат загрузки файла</param>
        /// <returns>Результат загрузки файла</returns>
	    public dynamic UploadBinaryContent(string instanceId, string fieldName,
	        string fileName, Stream stream)
        {
            var linkedData = new
            {
                InstanceId = instanceId,
                FieldName = fieldName,
                FileName = fileName
            };

            return RestQueryApi.QueryPostFile("RestfulApi", "configuration", "uploadbinarycontent", linkedData, stream,_version).ToDynamic();
	    }

		/// <summary>
		///   Загрузить бинарный контент для указанного поля указанного документа 
		/// </summary>
		/// <param name="instanceId">Идентификатор документа</param>
		/// <param name="fieldName">Поле бинарного контента</param>
		/// <returns>Данные бинарного контента</returns>
		public dynamic DownloadBinaryContent(string instanceId, string fieldName)
		{
			return RestQueryApi.QueryGetUrlEncodedData("RestfulApi", "configuration", "downloadbinarycontent", new
			{
			    InstanceId = instanceId,
			    FieldName = fieldName
			},_version);
		}
	}
}

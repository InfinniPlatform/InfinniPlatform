using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using System;
using System.Collections.Generic;

namespace InfinniPlatform.Api.RestApi.CommonApi
{
	public static class UpdateApi
	{
		/// <summary>
		///   Установить JSON-конфигурацию системы
		/// </summary>
		/// <param name="configurationId">Идентификатор конфигурации</param>
		/// <param name="documentId">Идентификатор документа, если необходимо</param>
		/// <param name="metadataObject">Конфигурация системы</param>
		/// <param name="metadataType">Тип метаданных системы</param>
		public static void UpdateMetadataObject(string configurationId, string documentId, dynamic metadataObject, string metadataType)
		{
			if (metadataObject.Name == null)
			{
				throw new ArgumentException(Resources.ErrorConfigNameIsEmpty);
			}
			var request = new
				              {
					              ConfigurationId = configurationId,
								  DocumentId = documentId,
					              MetadataType = metadataType,
					              MetadataObject = metadataObject
				              };

            RestQueryApi.QueryPostJsonRaw("Update", "Package", "installjsonmetadata", null, request);
		}

		/// <summary>
		///   Обновить метаданные конфигурации в JSON
		/// </summary>
		/// <param name="version">Обновляемая версия</param>
		/// <param name="filePath">Путь к файлу JSON конфигурации</param>
		public static dynamic UpdateConfigFromJson(string version,string filePath)
		{
			var builder = new RestQueryBuilder("SystemConfig", "update", "updateconfigfromjson", null);
			var linkedData = new
			{
				Version = version
			};

			var response = builder.QueryPostFile(linkedData, filePath, null);

			if (!response.IsAllOk)
			{
				Console.WriteLine("===========package install error===============================");
				Console.WriteLine("Response content: " + response.Content);
				throw new ArgumentException(string.Format("Error update configuration: \"{0}\"  ", filePath));
			}
			var result = response.Content.ToDynamic();
			return result;
		}

		/// <summary>
		///   Принудительно вызвать обновление конфигурации
		/// </summary>
		public static void ForceReload(string configurationId)
		{
			var response = RestQueryApi.QueryPostNotify(configurationId);

			if (!response.IsAllOk)
			{
				Console.WriteLine("===========package install error===============================");
				Console.WriteLine("Response content: " + response.Content);
				throw new ArgumentException(string.Format("Error reload configuration: \"{0}\"  ", configurationId));
			}
		}

	    /// <summary>
	    ///   Установка пакетов обновления
	    ///   Необходимо переписать на выполнение запросов через высокоуровневый API системы
	    /// </summary>
	    /// <param name="updatePackages">Список устанавливаемых модулей</param>
	    public static void InstallPackages(IEnumerable<dynamic> updatePackages)
	    {
	        foreach (var updatePackage in updatePackages)
	        {
	            InstallPackage(updatePackage);
	            ForceReload(updatePackage.ConfigurationName.ToString());
	        }
	    }

        /// <summary>
        ///   Принудительно вызвать процедуру создания необходимых контейнеров для конфигурации
        /// </summary>
        public static void UpdateStore(string configurationId)
	    {
	        // Создаем индексы под системные конфигурации в случае необходимости
	        RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "runmigration",null,
            
	        new
	        {
	            MigrationName = "UpdateStoreMigration",
                ConfigurationName = configurationId
	        });


	    }

	    /// <summary>
		///   Установить пакет обновления конфигурации
		/// </summary>
		/// <param name="updatePackage">Пакет обновления</param>
		private static void InstallPackage(dynamic updatePackage)
		{
			//выполняем обновление метаданных с помощью пакета обновления
            var response = RestQueryApi.QueryPostJsonRaw("Update", "Package", "Install", null, updatePackage);


			if (!response.IsAllOk)
			{
				Console.WriteLine("===========package install error===============================");
				Console.WriteLine("Response content: " + response.Content);
				throw new ArgumentException(string.Format("Error install package {0} :{1} ", updatePackage.PackageHeader.Value, response.Content));
			}
		}
	}
}

using System.Collections.Generic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders
{
	/// <summary>
	///   Контракт провайдера метаданных для чтения
	/// </summary>
	public abstract class MetadataReader : IDataReader
	{
	    private readonly string _version;
	    private readonly string _configurationId;
		
		/// <summary>
		///   Индекс, по которому выполняем IQL запросы
		/// </summary>
		private readonly string _metadataSearchIndex;

		protected MetadataReader(string version, string configurationId, string metadataType)
		{
		    _version = version;
		    _configurationId = configurationId;
			_metadataSearchIndex = MetadataExtension.GetMetadataIndex(metadataType);
		}

		/// <summary>
		///   Идентификатор конфигурации, для которой реализован провайдер данных для чтения
		/// </summary>
		protected string ConfigurationId
		{
			get { return _configurationId; }
		}

		/// <summary>
		///   Индекс, по которому выполняем IQL запросы
		/// </summary>
		protected string MetadataSearchIndex
		{
			get { return _metadataSearchIndex; }
		}

        /// <summary>
        /// Версия конфигурации
        /// </summary>
	    public string Version
	    {
	        get { return _version; }
	    }


	    /// <summary>
		///   Получить метаданные объекта в кратком виде (ссылки на метаданные объектов конфигурации)
		/// </summary>
		/// <returns>Список описаний метаданных объекта в кратком формате</returns>
		public abstract IEnumerable<dynamic> GetItems();

	    /// <summary>
	    ///   Получить метаданные конкретного объекта
	    /// </summary>
	    /// <param name="metadataName">наименование объекта</param>
	    /// <returns>Метаданные объекта конфигурации</returns>
	    public abstract dynamic GetItem(string metadataName);



	}
}

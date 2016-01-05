using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    /// <summary>
    /// API для работы с CRUD операциями элементов метаданных
    /// </summary>
    public class MetadataManagerElement : IDataManager
    {
        public MetadataManagerElement(string parentUid, MetadataCacheRefresher metadataCacheRefresher, IDataReader metadataReader, IMetadataContainerInfo metadataContainerInfo, string ownerMetadataType)
        {
        }

        /// <summary>
        /// Провайдер метаданных для чтения
        /// </summary>
        public IDataReader MetadataReader { get; }

        /// <summary>
        /// Индекс, в котором хранятся ссылки (заголовки) на полные метаданные типов
        /// Например: metadata хранит объекты конфигурации, содержащие ссылки на documentmetadata, menumetadata, etc
        /// </summary>
        protected string RootMetadataIndex
        {
            get { return null; }
        }

        /// <summary>
        /// Сформировать предзаполненный объект метаданных
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Предзаполненный объект метаданных</returns>
        public dynamic CreateItem(string name)
        {
            return new DynamicWrapper();
        }

        /// <summary>
        /// Добавить метаданные объекта конфигурации
        /// </summary>
        /// <param name="objectToCreate">Метаданные создаваемого объекта</param>
        public void InsertItem(dynamic objectToCreate)
        {
            MergeItem(objectToCreate);
        }

        /// <summary>
        /// Удалить метаданные указанного объекта в указанной конфигурации
        /// </summary>
        /// <param name="metadataObject">Удаляемый объект</param>
        public void DeleteItem(dynamic metadataObject)
        {
        }

        /// <summary>
        /// Обновить метаданные указанного объекта  в указанной конфигурации
        /// </summary>
        /// <param name="objectToCreate">Метаданные создаваемого объекта метаданных</param>
        public void MergeItem(dynamic objectToCreate)
        {
        }
    }
}
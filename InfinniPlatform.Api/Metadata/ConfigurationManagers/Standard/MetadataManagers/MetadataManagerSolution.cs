using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    public sealed class MetadataManagerSolution : IDataManager
    {
        public MetadataManagerSolution(IDataReader metadataReader)
        {
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
        }

        /// <summary>
        /// Удалить метаданные указанного объекта в указанной конфигурации
        /// </summary>
        /// <param name="metadataObject"></param>
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
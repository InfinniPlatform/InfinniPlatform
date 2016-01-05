using System.Linq;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    /// <summary>
    ///     Удалить при дальнейшем рефакторинге системы
    /// </summary>
    public static class MetadataExtensions
    {
        /// <summary>
        ///     Получить сохраненные метаданные для указанного объекта метаданных
        /// </summary>
        /// <param name="dataReader">Провайдер данных для чтения</param>
        /// <param name="metadataObject">Экземпляр объекта метаданных в памяти</param>
        /// <returns>Сохраненный объект метаданных</returns>
        public static dynamic GetStoredMetadata(IDataReader dataReader, dynamic metadataObject)
        {
            var items = dataReader.GetItems().ToList();

            if (metadataObject.Id != null)
            {
                foreach (var item in items)
                {
                    if (item.Id == metadataObject.Id)
                    {
                        return item;
                    }
                }
            }
            return items.FirstOrDefault(i => i.Name == metadataObject.Name);
        }
    }
}
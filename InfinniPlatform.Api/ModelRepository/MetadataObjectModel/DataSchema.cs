using System.Collections.Generic;

namespace InfinniPlatform.Api.ModelRepository.MetadataObjectModel
{
    public class DataSchema
    {
        public DataSchema()
        {
            Properties = new Dictionary<string, DataSchema>();
            TypeInfo = new Dictionary<string, object>();
        }

        /// <summary>
        ///     Тип данных
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Информация о типе данных
        /// </summary>
        public IDictionary<string, object> TypeInfo { get; set; }

        /// <summary>
        ///     Заголовок модели данных
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        ///     Описание модели данных
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Описание элементов массива
        /// </summary>
        public DataSchema Items { get; set; }

        /// <summary>
        ///     Описание свойств модели данных
        /// </summary>
        public IDictionary<string, DataSchema> Properties { get; set; }
    }
}
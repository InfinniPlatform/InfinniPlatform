using System.Collections.Generic;

namespace InfinniPlatform.Api.ModelRepository.MetadataObjectModel
{
    public class DocumentSchema
    {
        public DocumentSchema()
        {
            Items = new List<DocumentSchema>();
            Properties = new List<DocumentSchema>();
        }

        /// <summary>
        /// Тип данных
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// Заголовок модели
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Описание модели
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Список элементов
        /// </summary>
        public IList<DocumentSchema> Items { get; private set; }

        /// <summary>
        /// Список свойств
        /// </summary>
        public IList<DocumentSchema> Properties { get; set; }
    }
}
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace InfinniPlatform.Sdk.Environment.Index
{
    /// <summary>
    ///     Тип данных поля документа, используемый для хранения проиндексированного объекта
    /// </summary>
    public enum PropertyDataType
    {
        String,
        Integer,
        Float,
        Date,
        Boolean,
        Binary,
        Object
    }

    /// <summary>
    ///     Определяет параметры маппинга для поля типа, хранящегося в индексе
    /// </summary>
    [DebuggerDisplay("{Name}, {DataType}")]
    public sealed class PropertyMapping
    {
        public PropertyMapping()
        {
        }

        public PropertyMapping(string name, IEnumerable<PropertyMapping> childProperties)
        {
            DataType = PropertyDataType.Object;
            AddSortField = false;
            ChildProperties = new List<PropertyMapping>(childProperties);
            Name = name;
        }

        public PropertyMapping(string name, PropertyDataType dataType, bool addSortField = false)
        {
            ChildProperties = new PropertyMapping[0];
            Name = name;
            DataType = dataType;
            AddSortField = addSortField;
        }

        public string Name { get; set; }
        // Список параметров подлежит расширению,
        // возможно понадобиться дополнительно определять формат, правила сортировки, search_analyzer

        /// <summary>
        ///     Возвращает тип свойства
        /// </summary>
        public PropertyDataType DataType { get; set; }

        /// <summary>
        ///     True если необходимо добавить дополнительное поле сортировки
        /// </summary>
        public bool AddSortField { get; set; }

        /// <summary>
        ///     Возвращает вложенные свойства
        /// </summary>
        public IList<PropertyMapping> ChildProperties { get; set; }
    }
}
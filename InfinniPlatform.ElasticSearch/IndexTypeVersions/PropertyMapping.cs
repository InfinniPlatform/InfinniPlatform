using System;
using System.Collections.Generic;
using System.Diagnostics;

using Nest;

namespace InfinniPlatform.ElasticSearch.IndexTypeVersions
{
    /// <summary>
    /// Определяет параметры маппинга для поля типа, хранящегося в индексе
    /// </summary>
    [DebuggerDisplay("{Name}, {DataType}, {NestType}")]
    public sealed class PropertyMapping
    {
        public PropertyMapping()
        {
        }

        public PropertyMapping(string name, IEnumerable<PropertyMapping> childProperties)
        {
            DataType = FieldType.Object;
            AddSortField = false;
            ChildProperties = new List<PropertyMapping>(childProperties);
            Name = name;
        }

        public PropertyMapping(string name, FieldType dataType, bool addSortField = false, Type nestType = null)
        {
            ChildProperties = new PropertyMapping[0];
            Name = name;
            DataType = dataType;
            NestType = nestType;
            AddSortField = addSortField;
        }

        public string Name { get; set; }
        // Список параметров подлежит расширению,
        // возможно понадобиться дополнительно определять формат, правила сортировки, search_analyzer

        /// <summary>
        /// Возвращает тип свойства
        /// </summary>
        public FieldType DataType { get; set; }

        public Type NestType { get; set; }

        /// <summary>
        /// True если необходимо добавить дополнительное поле сортировки
        /// </summary>
        public bool AddSortField { get; set; }

        /// <summary>
        /// Возвращает вложенные свойства
        /// </summary>
        public IList<PropertyMapping> ChildProperties { get; set; }
    }
}
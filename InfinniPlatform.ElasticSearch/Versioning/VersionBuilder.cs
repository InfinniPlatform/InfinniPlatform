using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.ElasticSearch.ElasticProviders;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.ElasticSearch.Versioning
{
    /// <summary>
    /// Конструктор версий индексов
    /// </summary>
    internal sealed class VersionBuilder : IVersionBuilder
    {
        public VersionBuilder(ElasticTypeManager elasticTypeManager, string indexName, string typeName)
        {
            _elasticTypeManager = elasticTypeManager;
            _indexName = indexName;
            _typeName = typeName;
        }

        private readonly ElasticTypeManager _elasticTypeManager;
        private readonly string _indexName;
        private readonly string _typeName;

        /// <summary>
        /// Проверяет, что версия индекса метаданных существует.
        /// Если в параметрах указан маппинг, то дополнительно будет проверено соответствие имеющиегося маппинга с новым.
        /// Считается, что версия существует, если все свойства переданного маппинга соответсвуют по типу всем имеющимся свойствам,
        /// в противном случае нужно создавать новую версию.
        /// </summary>
        public bool VersionExists(IList<PropertyMapping> properties = null)
        {
            var isTypeExists = _elasticTypeManager.TypeExists(_indexName, _typeName);

            var isPropertiesMatch = true;

            if (properties != null && isTypeExists)
            {
                var currentProperties = _elasticTypeManager.GetPropertyMappings(_indexName, _typeName);

                foreach (var newMappingProperty in properties)
                {
                    var propertyToCheck = currentProperties.FirstOrDefault(p => p.Name == newMappingProperty.Name);
                    if (propertyToCheck != null)
                    {
                        if (!CheckPropertiesEquality(propertyToCheck, newMappingProperty))
                        {
                            isPropertiesMatch = false;
                            break;
                        }
                    }
                    else
                    {
                        // Это значит, что было добавлено новое свойство
                        isPropertiesMatch = false;
                        break;
                    }
                }
            }

            return isTypeExists && isPropertiesMatch;
        }

        /// <summary>
        /// Создать версию индекса метаданных
        /// </summary>
        /// <param name="deleteExisting">Флаг, показывающий нужно ли удалять версию харнилища, если она уже существует</param>
        /// <param name="properties">Первоначальный список полей справочника</param>
        public void CreateVersion(bool deleteExisting = false, IList<PropertyMapping> properties = null)
        {
            _elasticTypeManager.CreateType(_indexName, _typeName, properties, deleteExisting);
        }

        /// <summary>
        /// Проверяет соответствие маппинга свойства
        /// </summary>
        private static bool CheckPropertiesEquality(PropertyMapping propertyToCheck, PropertyMapping newMappingProperty)
        {
            // Первая проверка на соответствие типа данных
            if (!CheckTypePropertyEquality(propertyToCheck, newMappingProperty))
            {
                return false;
            }

            // Проверяем возможность сортировки по полю
            if (propertyToCheck.AddSortField != newMappingProperty.AddSortField)
            {
                return false;
            }

            if (propertyToCheck.DataType == PropertyDataType.Object)
            {
                foreach (var childProperty in newMappingProperty.ChildProperties)
                {
                    var childPropertyToCheck =
                        propertyToCheck.ChildProperties.FirstOrDefault(p => p.Name == childProperty.Name);

                    if (childPropertyToCheck != null)
                    {
                        if (!CheckPropertiesEquality(childPropertyToCheck, childProperty))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool CheckTypePropertyEquality(PropertyMapping propertyToCheck, PropertyMapping newMappingProperty)
        {
            if (newMappingProperty.DataType == PropertyDataType.Binary)
            {
                return propertyToCheck.DataType == PropertyDataType.Object;
            }
            return propertyToCheck.DataType == newMappingProperty.DataType;
        }
    }
}
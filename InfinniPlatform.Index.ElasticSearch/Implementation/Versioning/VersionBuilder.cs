using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Versioning
{
	/// <summary>
	///   Конструктор версий индексов
	/// </summary>
	public sealed class VersionBuilder : IVersionBuilder
	{
		private readonly IIndexStateProvider _indexStateProvider;
	    private readonly string _indexName;
	    private readonly string _typeName;

	    private readonly ElasticConnection _connection;

	    public VersionBuilder(IIndexStateProvider indexStateProvider,  string indexName, string typeName)
		{
			_indexStateProvider = indexStateProvider;
		    _indexName = indexName;
	        _typeName = typeName;

	        _connection = new ElasticConnection(); 
		}

        /// <summary>
        ///   Проверяет, что версия индекса метаданных существует.
        /// Если в параметрах указан маппинг, то дополнительно будет проверено соответствие имеющиегося маппинга с новым.
        /// Считается, что версия существует, если все свойства переданного маппинга соответсвуют по типу всем имеющимся свойствам,
        /// в противном случае нужно создавать новую версию. 
        /// </summary>
		public bool VersionExists(IList<PropertyMapping> properties = null)
        {
            var isTypeExists = _indexStateProvider.GetIndexStatus(_indexName, _typeName) == IndexStatus.Exists;

            var isPropertiesMatch = true;

            if (properties != null && isTypeExists)
            {
                var currentProperties = _connection.GetIndexTypeMapping(_indexName, _typeName);

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
	    ///   Создать версию индекса метаданных
	    /// </summary>
	    /// <param name="deleteExisting">Флаг, показывающий нужно ли удалять версию харнилища, если она уже существует</param>
	    /// <param name="properties">Первоначальный список полей справочника</param>
		public void CreateVersion(bool deleteExisting = false, IList<PropertyMapping> properties = null)
	    {
	        _indexStateProvider.CreateIndexType(
	            _indexName,
	            _typeName,
	            deleteExisting,
	            properties);
	    }

	    /// <summary>
	    /// Проверяет соответствие маппинга свойства
	    /// </summary>
	    private bool CheckPropertiesEquality(PropertyMapping propertyToCheck, PropertyMapping newMappingProperty)
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

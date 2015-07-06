using InfinniPlatform.Api.Index;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeSelectors;
using InfinniPlatform.Sdk.Environment.Index;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinniPlatform.Sdk.Dynamic;
using PropertyMapping = InfinniPlatform.Sdk.Environment.Index.PropertyMapping;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
	/// <summary>
	///   Elastic search provider implementation
	/// </summary>
	public sealed class ElasticSearchProvider : ICrudOperationProvider
	{
	    private readonly string _indexName;
	    private readonly string _typeName;
		private readonly string _routing;
	    private readonly string _version;
	    private readonly ElasticConnection _elasticConnection;
		
	    private IEnumerable<IndexToTypeAccordance> _derivedTypeNames;

	    /// <summary>
	    ///  Установливает соединение с указанным индексом
	    /// </summary>
	    /// <param name="indexName">Наименование индекса</param>
	    /// <param name="typeName">Тип объекта для получения данных из индекса</param>
	    /// <param name="routing">Роутинг для получения данных</param>
	    /// <param name="version">Версия документа</param>
	    public ElasticSearchProvider(string indexName, string typeName, string routing, string version = null)
		{
            if (string.IsNullOrEmpty(indexName))
            {
                throw new ArgumentException("Index name should not be empty.");
            }

            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException("Index type name should not be empty.");
            }

			if (string.IsNullOrEmpty(routing))
			{
				throw new ArgumentException("Routing should not be empty");
			}

            _indexName = indexName.ToLowerInvariant();
            _typeName = typeName.ToLowerInvariant();
		    _routing = routing;
	        _version = version;


	        _elasticConnection = new ElasticConnection();


            //все версии типа в индексе
            RefreshMapping();

            _elasticConnection.ConnectIndex();
		}

		/// <summary>
        ///   Обновление позволяет делать запросы к только что добавленным данным
		/// </summary>
		/// Операция Refresh должна использоваться только в случае необходимости получения данных, ранее записанных в том же потоке
		/// Во всех остальных случаях достаточно выполнения операции Set. Предназначена для ручного управления логом транзакций elasticsearch
		public void Refresh()
		{
		    _elasticConnection.Client.Refresh(r=>r.Force());
		}

	    /// <summary>
	    ///   Обновить данные о текущей структуре индексов
	    /// </summary>
	    public void RefreshMapping()
	    {
            _derivedTypeNames = _elasticConnection.GetAllTypes(new[] { _indexName }, new[] { _typeName });
	    }


	    /// <summary>
	    ///   Индексировать список динамических объектов
	    /// </summary>
	    /// <param name="itemsToIndex">Динамические объекты для индексации</param>
	    public void SetItems(IEnumerable<dynamic> itemsToIndex)
	    {
	        foreach (var item in itemsToIndex)
	        {
	            Set(item);
	        }
	    }

	    /// <summary>
	    ///   Индексировать динамический объект с использованием стратегии по умолчанию
	    ///   (Стратегия по умолчанию следующая (UpdateItemStrategy): 
	    ///    1) Если объект существует в индексе - он апдейтится
	    ///    2) Если объект не существует в индексе - он вставляется в индекс 
	    /// </summary>
	    /// <param name="item">Объект для индексации</param>
	    public void Set(dynamic item)
	    {
	        Set(item, IndexItemStrategy.Update);
	    }

	    /// <summary>
	    ///   Индексировать динамический объект с указанием конкретной стратегии индексации
	    /// </summary>
	    /// <param name="item">Объект для индексации</param>
	    /// <param name="indexItemStrategy">Стратегия индексации объекта</param>
	    public void Set(dynamic item, IndexItemStrategy indexItemStrategy)
	    {
	        var actualTypeName = ActualTypeName;
            if (string.IsNullOrEmpty(actualTypeName))
            {
                throw new ArgumentException("actual index type not found.");
            }

	        dynamic jInstance = ((object) item).ToDynamic();

	        if (jInstance.Id == null)
	        {
	            throw new ArgumentException("Id field must be added!");
	        }

	        string idCheck = jInstance.Id.ToString();
	        if (idCheck.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Length > 1)
	        {
	            throw new ArgumentException("Id field should contain only one token without special symbols.");
	        }

	        jInstance["Id"] = jInstance["Id"].ToString().ToLowerInvariant();

            var objectToIndex = new IndexObject
	        {
                Id = jInstance["Id"].ToString().ToLowerInvariant(),
	            TimeStamp = DateTime.Now,
	            Values = jInstance,
                Version = _version
	        };

	        BaseResponse response;

			dynamic existingItem = null;
	        if (indexItemStrategy == IndexItemStrategy.Insert)
	        {
	            objectToIndex.Id = Guid.NewGuid().ToString().ToLowerInvariant();
	            response =
	                (BaseResponse)
	                    _elasticConnection.Client.Index(objectToIndex, d => d.Index(_indexName).Type(actualTypeName).Routing(_routing));
	        }
	        else 
	        {
	            // Удаляем предыдующую версию этого документа.
                // Документ с данным идентификатором может находиться в любом из типов

                // Удалить одним запросом _elasticConnection.Client.DeleteByQuery<dynamic>(q => q.Index(_indexName).Id(objectToIndex.Id));
                // не получается из-за бага в NEST - операция DeleteByQuery требует индекс по умолчанию https://github.com/elasticsearch/elasticsearch-net/issues/646,
                // Поэтому удаляем по отдельности из каждого типа. 


                foreach (var indexType in _derivedTypeNames.SelectMany(d => d.TypeNames))
                {
					//сохраняем элемент для rollback в случае ошибки
	                existingItem = GetItem(objectToIndex.Id);

                    _elasticConnection.Client.Delete<dynamic>(
                        d => d.Index(_indexName).Type(indexType).Routing(_routing).Id(objectToIndex.Id));
                }

	            // Добавляем документ в актуальный тип
	            response = (BaseResponse)_elasticConnection.Client.Index(objectToIndex, d => d.Index(_indexName).Type(actualTypeName).Routing(_routing));
	        }

	        if (!response.IsValid)
	        {

		        string rollbackMessage = "";
				//восстанавливаем предыдущие удаленные записи обратно
				if (existingItem != null)
				{
					try
					{
						Set(existingItem);
					}
					catch (Exception e)
					{
						rollbackMessage = string.Format("Rollback operation on element: {0} failed. Error: {1} ", e.Message,
						                                existingItem.ToString());
					}
				}

				// Пытаемся выяснить причину, почему проиндексировать объект не удалось.
                // Возможно, маппинг для типа индекса не соответствует типам данных полей 
                // индексируемого объекта item

			    var currentMapping = _elasticConnection.GetIndexTypeMapping(_indexName, _typeName);

			    var propertiesMismatchMessage = new StringBuilder();

		        if (rollbackMessage != null)
		        {
			        propertiesMismatchMessage.Append(rollbackMessage);
		        }

		        TryToFindPropertiesMismatch(item, currentMapping, propertiesMismatchMessage);

                // Обнаружено несоответствие маппинга индексируемого объекта
			    if (!string.IsNullOrEmpty(propertiesMismatchMessage.ToString()))
			    {
                    throw new ArgumentException(propertiesMismatchMessage.ToString());
			    }

                // Несоответствие маппинга не выявлено, возвращаем сообщение эластика
			    if (response.ConnectionStatus.OriginalException != null &&
			        !string.IsNullOrEmpty(response.ConnectionStatus.OriginalException.Message))
			    {
			        throw new ArgumentException(response.ConnectionStatus.OriginalException.Message);
			    }


			    throw new ArgumentException(string.Format("Index type mapping does not according actual document scheme.\r\nDocument: {0},\r\nIndexName: {1},\r\nTypeName: {2},\r\nActualMapping: {3}",
                    item.ToString(),
                    _indexName,
                    _typeName,
                    currentMapping));
			}
		}

	    public string ActualTypeName
		{
			get { return _derivedTypeNames.GetActualTypeName(_typeName); }
		}
        
	    /// <summary>
	    ///   Удалить объект из индекса
	    /// </summary>
	    /// <param name="key">Идентификатор удаляемого объекта индекса</param>
	    public void Remove(string key)
	    {
	        // Удаление реализуем через обновление статуса документа.
	        foreach (var indexType in _derivedTypeNames.SelectMany(d => d.TypeNames))
	        {
	            var type = indexType;
	            var response = _elasticConnection.Client.Search<dynamic>(
	                q => q
	                    .Index(_indexName)
	                    .Type(type)
						.Routing(_routing)
	                    .Query(m => m.Term(
	                        ElasticConstants.IndexObjectPath + ElasticConstants.IndexObjectIdentifierField,
	                        key.ToLowerInvariant()))
	                );

	            dynamic indexObject = response.Documents.FirstOrDefault();

	            if (indexObject != null)
	            {
	                var itemToUpdate = DynamicWrapperExtensions.ToDynamic(indexObject.Values);
                    itemToUpdate.Status = "Deleted";
	                Set(itemToUpdate, IndexItemStrategy.Update);
                    break;
	            }
	        }
	    }

	    /// <summary>
	    ///   Удалить объекты с идентификаторами из списка
	    /// </summary>
	    /// <param name="ids">Список идентификаторов</param>
	    public void RemoveItems(IEnumerable<string> ids)
	    {
	        foreach (var id in ids)
	        {
	            Remove(id);
	        }
	    }

	    /// <summary>
	    ///   Получить объект по идентификатору
	    /// </summary>
	    /// <param name="key">Идентификатор индексируемого объекта</param>
	    /// <returns>Индексируемый объект</returns>
	    public dynamic GetItem(string key)
	    {
	        //для объектов типа IndexObject мы не осуществляем
	        //поиск по их идентификатору Id. Дело в том, что
	        //IndexObject является только служебной оберткой для индексируемого объекта,
	        //который находится в свойстве Values. Таким образом, для поиска по идентификатору
	        //мы осуществляем поиск по вложенному свойству Values.Id

	        var response = _elasticConnection.Client.Search<dynamic>(
	            q => q
                    .AllIndices()
                    .AllTypes()
					.Routing(_routing)
	                .Query(f => f.Term(
	                    ElasticConstants.IndexObjectPath + ElasticConstants.IndexObjectIdentifierField,
	                    key.ToLowerInvariant())
	                )
	            );

	        dynamic indexObject =
	            response.Documents.FirstOrDefault(
                     d => d.Values.Status == null || 
                     (d.Values.Status.ToString() != "Deleted" && 
                     d.Values.Status.ToString() != "Invalid"));

	        if (indexObject != null)
	        {
	            return DynamicWrapperExtensions.ToDynamic(indexObject.Values);
	        }
	        return null;
	    }

	    /// <summary>
	    ///   Получить список объектов из индекса
	    /// </summary>
	    /// <param name="ids">Список идентификаторов</param>
	    /// <returns>Список документов индекса</returns>
	    public IEnumerable<dynamic> GetItems(IEnumerable<string> ids)
	    {
			var items = ids.ToList();
			const int batchSize = 300;

			var indexObjects = new List<dynamic>();

	        if (items.Any())
	        {
	            int i = 0;

	            while (items.Skip(i*batchSize).Count() != 0)
	            {
	                var itemsToIndex = items.Skip(i*batchSize).Take(batchSize).ToList();

	                var searchResponse = _elasticConnection.Client.Search<dynamic>(
	                    q => q
                            .BuildSearchForType(new[] { _indexName }, _derivedTypeNames.SelectMany(d => d.TypeNames), _routing, false, false)
	                        .Size(batchSize)
	                        .Filter(m => m.Terms(
	                                    ElasticConstants.IndexObjectPath + ElasticConstants.IndexObjectIdentifierField,
	                                    itemsToIndex.Select(batchItem => batchItem.ToLowerInvariant()))));

                    indexObjects.AddRange(searchResponse.Documents.Where(d => d.Values.Status != "Deleted" && d.Values.Status != "Invalid"));
	                i++;
	            }
	        }

	        return indexObjects.Select(i => ((object) i.Values).ToDynamic()).ToList();
	    }


	    /// <summary>
	    /// Получить количество записей в индексе
	    /// </summary>
	    /// <returns>Количество записей в индексе</returns>
	    public int GetTotalCount()
		{
            return _elasticConnection.Client
                .Search<dynamic>(q => q
					.Routing(_routing)
                    .BuildSearchForType(new[] { _indexName }, _derivedTypeNames.SelectMany(d => d.TypeNames), _routing, false, false)
                    .Query(qr => qr.MatchAll())).Hits.Count();
		}

        /// <summary>
        /// Метод проверяет, удовлетворяет ли переданный объект маппингу типа индекса
        /// </summary>
        private static void TryToFindPropertiesMismatch(
            dynamic item, 
            IEnumerable<PropertyMapping> currentMapping, 
            StringBuilder propertiesMismatchMessage)
        {
            foreach (var currentMappingProperty in currentMapping)
            {
                dynamic itemValue = null;

                var itemProperty = item.GetType().GetProperty(currentMappingProperty.Name);

                if (itemProperty != null)
                {
                    itemValue = itemProperty.GetValue(item, null);
                }
                else if (item is DynamicWrapper)
                {
                    itemValue = item[currentMappingProperty.Name];
                }

                if (itemValue != null)
                {
                    // В объекте обнаружено свойство из маппинга типа, проверяем корректность его типа

                    bool valueHasCorrectType = true;

                    if (currentMappingProperty.ChildProperties == null ||
                        currentMappingProperty.ChildProperties.Count == 0)
                    {
                        switch (currentMappingProperty.DataType)
                        {
                            case PropertyDataType.String:
                                try
                                {
                                    var value = (string)itemValue;
                                }
                                catch (Exception)
                                {
                                    valueHasCorrectType = false;
                                }
                                break;
                            case PropertyDataType.Integer:
                                try
                                {
                                    var value = (int)itemValue;
                                }
                                catch (Exception)
                                {
                                    valueHasCorrectType = false;
                                }
                                break;
                            case PropertyDataType.Float:
                                try
                                {
                                    var value = (double)itemValue;
                                }
                                catch (Exception)
                                {
                                    valueHasCorrectType = false;
                                }
                                break;
                            case PropertyDataType.Date:
                                try
                                {
                                    var value = (DateTime)itemValue;
                                }
                                catch (Exception)
                                {
                                    valueHasCorrectType = false;
                                }
                                break;
                            case PropertyDataType.Boolean:
                                try
                                {
                                    var value = (bool)itemValue;
                                }
                                catch (Exception)
                                {
                                    valueHasCorrectType = false;
                                }
                                break;
                            case PropertyDataType.Binary:
                            case PropertyDataType.Object:
                                // always ok
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("item");
                        }

                        if (!valueHasCorrectType)
                        {
                            propertiesMismatchMessage.AppendLine();
                            propertiesMismatchMessage.AppendFormat(
                                "Expected value for field '{0}' should have {1} type, but value has {2} type ('{3}')",
                                currentMappingProperty.Name,
                                currentMappingProperty.DataType,
                                itemValue.GetType(),
                                itemValue);
                        }
                    }
                    else
                    {
                        TryToFindPropertiesMismatch(
                            itemValue, 
                            currentMappingProperty.ChildProperties,
                            propertiesMismatchMessage);
                    }
                }
            }
        }

	}
}
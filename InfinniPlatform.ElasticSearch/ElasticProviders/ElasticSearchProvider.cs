using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.RestApi.Auth;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.ElasticSearch.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.ElasticSearch.IndexTypeSelectors;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

using Nest;

using PropertyMapping = InfinniPlatform.Sdk.Environment.Index.PropertyMapping;

namespace InfinniPlatform.ElasticSearch.ElasticProviders
{
    /// <summary>
    /// Elastic search provider implementation
    /// </summary>
    public sealed class ElasticSearchProvider : ICrudOperationProvider
    {
        public ElasticSearchProvider(ElasticConnection elasticConnection, ElasticTypeManager elasticTypeManager, ITenantProvider tenantProvider, string indexName, string typeName)
        {
            _elasticConnection = elasticConnection;
            _elasticTypeManager = elasticTypeManager;
            _tenantProvider = tenantProvider;
            _indexName = indexName.ToLowerInvariant();
            _typeName = typeName.ToLowerInvariant();

            // все версии типа в индексе
            _derivedTypeNames = new Lazy<IEnumerable<TypeMapping>>(() => elasticTypeManager.GetTypeMappings(_indexName, _typeName));
        }


        private readonly ElasticConnection _elasticConnection;
        private readonly ElasticTypeManager _elasticTypeManager;
        private readonly ITenantProvider _tenantProvider;
        private readonly string _indexName;
        private readonly string _typeName;

        private readonly Lazy<IEnumerable<TypeMapping>> _derivedTypeNames;


        private string ActualTypeName => _derivedTypeNames.Value.GetTypeActualName();


        /// <summary>
        /// Обновление позволяет делать запросы к только что добавленным данным
        /// </summary>
        /// Операция Refresh должна использоваться только в случае необходимости получения данных, ранее записанных в том же потоке
        /// Во всех остальных случаях достаточно выполнения операции Set. Предназначена для ручного управления логом транзакций elasticsearch
        public void Refresh()
        {
            _elasticConnection.Refresh();
        }

        /// <summary>
        /// Индексировать список динамических объектов
        /// </summary>
        /// <param name="itemsToIndex">Динамические объекты для индексации</param>
        public void SetItems(IEnumerable<dynamic> itemsToIndex)
        {
            var actualTypeName = ActualTypeName;
            if (string.IsNullOrEmpty(actualTypeName))
            {
                throw new ArgumentException($"Actual index type not found. Type: '{_typeName}'.");
            }

            var tenantId = _tenantProvider.GetTenantId();

            var objectsToIndex = itemsToIndex.Select(item => PrepareObjectToIndex(item, tenantId)).Cast<IndexObject>().ToList();

            var objectIds = objectsToIndex.Select(o => o.Id).ToArray();

            //сохраняем элементы для rollback в случае ошибки
            var existingItems = GetItems(objectIds).ToArray();

            if (existingItems.Any())
            {
                // Удаляем предыдущие версии документов.
                // Документы могут находиться в любом из типов.
                _elasticConnection.Client.DeleteByQuery<dynamic>(
                    d =>
                        d.Index(_indexName)
                         .AllTypes()
                         .Query(queryDescriptor => queryDescriptor.Ids(objectIds) &&
                                                   queryDescriptor.Term(ElasticConstants.TenantIdField, tenantId) &&
                                                   queryDescriptor.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid)));
            }

            // Добавляем документы в актуальный тип
            var response = (BaseResponse)_elasticConnection.Client.Bulk(
                bd => bd.IndexMany(objectsToIndex, (s1, s2) => s1.Index(_indexName).Type(actualTypeName)));

            if (response.IsValid)
            {
                return;
            }

            var rollbackMessage = new StringBuilder();

            //восстанавливаем предыдущие удаленные записи обратно
            foreach (var existingItem in existingItems)
            {
                try
                {
                    Set(existingItem);
                }
                catch (Exception e)
                {
                    rollbackMessage.AppendLine($"Rollback operation on element: {e.Message} failed. Error: {existingItem} ");
                }
            }

            // Пытаемся выяснить причину, почему проиндексировать объект не удалось.
            // Возможно, маппинг для типа индекса не соответствует типам данных полей 
            // индексируемого объекта item

            var currentMapping = _elasticTypeManager.GetPropertyMappings(_indexName, _typeName);

            var propertiesMismatchMessage = new StringBuilder();

            if (!string.IsNullOrEmpty(rollbackMessage.ToString()))
            {
                propertiesMismatchMessage.Append(rollbackMessage);
            }

            foreach (var item in itemsToIndex)
            {
                TryToFindPropertiesMismatch(item, currentMapping, propertiesMismatchMessage);
            }

            // Обнаружено несоответствие маппинга индексируемого объекта
            if (!string.IsNullOrEmpty(propertiesMismatchMessage.ToString()))
            {
                throw new ArgumentException(propertiesMismatchMessage.ToString());
            }

            // Несоответствие маппинга не выявлено, возвращаем сообщение эластика
            // Возвращаем сообщение эластика
            if (response.ConnectionStatus.OriginalException != null &&
                !string.IsNullOrEmpty(response.ConnectionStatus.OriginalException.Message))
            {
                throw new ArgumentException(response.ConnectionStatus.OriginalException.Message);
            }

            // Если и эластик не вернул внятного сообщения, выводим достаточно общее сообщение
            // (ничего более конкретного сказать не можем)
            throw new ArgumentException("Incorrect request for index data");
        }

        /// <summary>
        /// Индексировать динамический объект с использованием стратегии по умолчанию
        /// (Стратегия по умолчанию следующая (UpdateItemStrategy):
        /// 1) Если объект существует в индексе - он апдейтится
        /// 2) Если объект не существует в индексе - он вставляется в индекс
        /// </summary>
        /// <param name="item">Объект для индексации</param>
        public void Set(dynamic item)
        {
            Set(item, IndexItemStrategy.Update);
        }

        /// <summary>
        /// Индексировать динамический объект с указанием конкретной стратегии индексации
        /// </summary>
        /// <param name="item">Объект для индексации</param>
        /// <param name="indexItemStrategy">Стратегия индексации объекта</param>
        public void Set(dynamic item, IndexItemStrategy indexItemStrategy)
        {
            var actualTypeName = ActualTypeName;
            if (string.IsNullOrEmpty(actualTypeName))
            {
                throw new ArgumentException($"Actual index type not found. Type: '{_typeName}'.");
            }

            var tenantId = _tenantProvider.GetTenantId();
            IndexObject objectToIndex = PrepareObjectToIndex(item, tenantId);


            BaseResponse response;

            dynamic existingItem = null;
            if (indexItemStrategy == IndexItemStrategy.Insert)
            {
                objectToIndex.Id = Guid.NewGuid().ToString().ToLowerInvariant();
                response =
                    (BaseResponse)
                        _elasticConnection.Client.Index(objectToIndex, d => d.Index(_indexName).Type(actualTypeName));
            }
            else
            {
                //сохраняем элемент для rollback в случае ошибки
                existingItem = GetItem(objectToIndex.Id);

                // Удаляем предыдущую версию этого документа.
                // Документ с данным идентификатором может находиться в любом из типов
                _elasticConnection.Client.DeleteByQuery<dynamic>(
                    d => d.Index(_indexName).AllTypes().Query(
                        queryDescriptor => queryDescriptor.Ids(new[] { objectToIndex.Id }) &&
                                           queryDescriptor.Term(ElasticConstants.TenantIdField, tenantId) &&
                                           queryDescriptor.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid)));

                // Добавляем документ в актуальный тип
                response =
                    (BaseResponse)
                        _elasticConnection.Client.Index(objectToIndex, d => d.Index(_indexName).Type(actualTypeName));
            }

            if (response.IsValid)
            {
                return;
            }

            var rollbackMessage = "";

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

            var currentMapping = _elasticTypeManager.GetPropertyMappings(_indexName, _typeName);

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

            throw new ArgumentException("Index type mapping does not according actual document scheme." +
                                        $"\r\nDocument: {item.ToString()}," +
                                        $"\r\nIndexName: {_indexName}," +
                                        $"\r\nTypeName: {_typeName}," +
                                        $"\r\nActualMapping: {currentMapping}");
        }

        /// <summary>
        /// Удалить объект из индекса
        /// </summary>
        /// <param name="key">Идентификатор удаляемого объекта индекса</param>
        public void Remove(string key)
        {
            var tenantId = _tenantProvider.GetTenantId();
            // Удаление реализуем через обновление статуса документа.
            foreach (var indexType in _derivedTypeNames.Value.GetMappingsTypeNames())
            {
                var type = indexType;
                var response = _elasticConnection.Client.Search<dynamic>(
                    q => q
                        .Index(_indexName)
                        .Type(type)
                        .Query(
                            m =>
                                m.Term(ElasticConstants.IndexObjectPath + ElasticConstants.IndexObjectIdentifierField,
                                    key.ToLowerInvariant())
                                && m.Term(ElasticConstants.TenantIdField, tenantId)
                                && m.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid))
                    );

                var indexObject = response.Documents.FirstOrDefault();

                if (indexObject != null)
                {
                    IndexObject objectToDelete = PrepareObjectToIndex(DynamicWrapperExtensions.ToDynamic(indexObject.Values), tenantId);

                    objectToDelete.Status = IndexObjectStatus.Deleted;

                    _elasticConnection.Client.Index(objectToDelete, d => d.Index(_indexName).Type(type));

                    break;
                }
            }
        }

        /// <summary>
        /// Удалить объекты с идентификаторами из списка
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
        /// Получить объект по идентификатору
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
                    .Index(_indexName)
                    .Types(_derivedTypeNames.Value.GetMappingsTypeNames())
                    .Query(
                        f =>
                        {
                            var tenantId = _tenantProvider.GetTenantId();

                            return f.Term(ElasticConstants.IndexObjectPath + ElasticConstants.IndexObjectIdentifierField, key.ToLowerInvariant())
                                   && f.Term(ElasticConstants.TenantIdField, tenantId)
                                   && f.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid);
                        })
                );

            dynamic indexObject = response.Documents.FirstOrDefault();

            if (indexObject != null)
            {
                return DynamicWrapperExtensions.ToDynamic(indexObject.Values);
            }
            return null;
        }

        /// <summary>
        /// Получить список объектов из индекса
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
                var i = 0;

                var tenantId = _tenantProvider.GetTenantId();

                while (items.Skip(i * batchSize).Count() != 0)
                {
                    var itemsToIndex = items.Skip(i * batchSize).Take(batchSize).ToList();

                    var searchResponse = _elasticConnection.Client.Search<dynamic>(
                        q => q
                            .BuildSearchForType(new[] { _indexName }, _derivedTypeNames.Value.GetMappingsTypeNames(),
                                false, false)
                            .Size(batchSize)
                            .Filter(
                                m => m.Terms(ElasticConstants.IndexObjectPath + ElasticConstants.IndexObjectIdentifierField, itemsToIndex.Select(batchItem => batchItem.ToLowerInvariant()))
                                     && m.Term(ElasticConstants.TenantIdField, tenantId)
                                     && m.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid)
                            ));

                    indexObjects.AddRange(searchResponse.Documents);
                    i++;
                }
            }

            return indexObjects.Select(i => ((object)i.Values).ToDynamic()).ToList();
        }

        /// <summary>
        /// Получить количество записей в индексе
        /// </summary>
        /// <returns>Количество записей в индексе</returns>
        public int GetTotalCount()
        {
            var tenantId = _tenantProvider.GetTenantId();

            return _elasticConnection.Client
                                     .Search<dynamic>(q => q
                                         .BuildSearchForType(new[] { _indexName }, _derivedTypeNames.Value.GetMappingsTypeNames(), false, false)
                                         .Query(qr => qr.Term(ElasticConstants.TenantIdField, tenantId)
                                                      && qr.Term(ElasticConstants.IndexObjectStatusField, IndexObjectStatus.Valid)
                                         )).Hits.Count();
        }

        private IndexObject PrepareObjectToIndex(dynamic item, string tenantId)
        {
            tenantId = AuthorizationStorageExtensions.AnonymousUser;

            dynamic jInstance = ((object)item).ToDynamic();

            if (jInstance.Id == null)
            {
                throw new ArgumentException("Id field must be added!");
            }

            string idCheck = jInstance.Id.ToString();
            if (idCheck.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
            {
                throw new ArgumentException("Id field should contain only one token without special symbols.");
            }

            jInstance["Id"] = jInstance["Id"].ToString().ToLowerInvariant();

            var objectToIndex = new IndexObject
                                {
                                    Id = jInstance["Id"].ToString().ToLowerInvariant(),
                                    TimeStamp = DateTime.Now,
                                    Values = jInstance,
                                    TenantId = tenantId.ToLowerInvariant(),
                                    Status = IndexObjectStatus.Valid
                                };
            return objectToIndex;
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

                    var valueHasCorrectType = true;

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
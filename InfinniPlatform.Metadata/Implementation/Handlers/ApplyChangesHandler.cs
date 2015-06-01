using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Events;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestQuery.EventObjects;
using InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Factories;
using InfinniPlatform.Hosting;
using Newtonsoft.Json;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
    /// <summary>
    ///   Обработчик http - запросов  обработки событийной модели
    /// </summary>
    public sealed class ApplyChangesHandler : IWebRoutingHandler
    {
        private readonly IGlobalContext _globalContext;


        public IConfigRequestProvider ConfigRequestProvider { get; set; }

        public ApplyChangesHandler(
            IGlobalContext globalContext)
        {
            _globalContext = globalContext;
        }

        /// <summary>
        ///   Применить изменения, представленные в виде объекта
        /// </summary>
        /// <param name="id">Идентификатор объекта, к которому следует применить изменения</param>
        /// <param name="changesObject">Объект JSON, содержащий события изменения объекта</param>
        /// <param name="replace">Заменить объект в хранилище</param>
        /// <returns>Результат обработки JSON объекта</returns>
        public dynamic ApplyJsonObject(string id, dynamic changesObject, bool replace = false)
        {
            IEnumerable<EventDefinition> events = null;
            if (changesObject != null)
            {
                events = new ObjectToEventSerializerStandard(changesObject).GetEvents();
            }
            else
            {
                events = new List<EventDefinition>();
            }
            return ApplyEventsWithMetadata(id, events, replace);
        }

        /// <summary>
        ///   Применить список событий изменения к объекту с указанным идентификатором
        ///   (К новому объекту, если идентификатор объекта не указан)
        /// </summary>
        /// <param name="id">Идентификатор изменяемого объекта</param>
        /// <param name="events">Список событий на изменение объекта</param>
        /// <param name="replace">Заменить объект в хранилище после применения изменений</param>
        /// <returns>Результат обработки изменений</returns>
        public dynamic ApplyEventsWithMetadata(string id, IEnumerable<EventDefinition> events, bool replace = false)
        {
            var documentId = ConfigRequestProvider.GetMetadataIdentifier();

            var appliedConfig =
                _globalContext.GetComponent<IMetadataConfigurationProvider>().GetMetadataConfiguration(ConfigRequestProvider.GetConfiguration());

            if (string.IsNullOrEmpty(documentId))
            {
                throw new ArgumentException("document type undefined");
            }

            //заполнение контекста обработчика применения событий
            var target = new ProcessEventContext();
            target.Context = _globalContext;
            target.Id = id;
            target.IsValid = true;
            target.Configuration = ConfigRequestProvider.GetConfiguration();
            target.Metadata = ConfigRequestProvider.GetMetadataIdentifier();
            target.UserName = ConfigRequestProvider.GetUserName();
            target.Events = events.ToList();
            target.Item = new AggregateProvider().CreateAggregate();

            //Сохранение агрегата в кассандре
            var eventStorage = _globalContext.GetComponent<IEventStorageComponent>().GetEventStorage();

            if (eventStorage != null)
            {
                //временно отключено сохранение событий в cassandra из-за периодического свопа кассандры
                //eventStorage.AddEvents(new Guid(item.Id), target.Events.Select(JsonConvert.SerializeObject).ToList());
            }


            var profiler = target.Context.GetComponent<IProfilerComponent>().GetOperationProfiler("FilterEventsPoint",
                                       string.Format("Config: {0}, Metadata {1}, ActionPoint {2}", target.Configuration, target.Metadata, appliedConfig.GetExtensionPointValue(ConfigRequestProvider, "FilterEvents")));
            profiler.Reset();
            appliedConfig.MoveWorkflow(documentId, appliedConfig.GetExtensionPointValue(ConfigRequestProvider, "FilterEvents"), target);
            
            if (!target.IsValid)
            {
                return AggregateExtensions.PrepareInvalidFilterAggregate(target);
            }

            profiler.TakeSnapshot();


            //Применение списка событий
            dynamic item = target.Item;
            new AggregateProvider().ApplyChanges(ref item, target.Events);


            //Перевод объекта в нужное состояние
            //пытаемся перевести в указанное состояние.
            //для указанного workflow сконфигурировано в ActionUnits действие на сохранение объекта после выполнения действия по переходу

            var targetMove = new ApplyContext
            {
                Id = item.Id,
                Context = _globalContext,
                Item = item,
                Type = documentId,
                DefaultProperties = target.DefaultProperties,
                Events = target.Events,
                Configuration = ConfigRequestProvider.GetConfiguration(),
                Metadata = ConfigRequestProvider.GetMetadataIdentifier(),
                Action = ConfigRequestProvider.GetServiceName(),
                UserName = ConfigRequestProvider.GetUserName(),
                TransactionMarker = target.Item.TransactionMarker ?? Guid.NewGuid().ToString()
            };

            //получаем менеджер для управления распределенной транзакцией
            var transaction =
                _globalContext.GetComponent<TransactionComponent>()
                    .GetTransactionManager()
                    .GetTransaction(targetMove.TransactionMarker);


            //если не заполнена версия документа, то формируем ее
            if (string.IsNullOrEmpty(target.Item.Version))
            {
                target.Item.Version = appliedConfig.ActualVersion;
            }

            profiler = target.Context.GetComponent<ProfilerComponent>().GetOperationProfiler("MovePoint",
               string.Format("Config: {0}, Metadata {1}, ActionPoint {2}", target.Configuration, target.Metadata, appliedConfig.GetExtensionPointValue(ConfigRequestProvider, "Move")));
            profiler.Reset();
            appliedConfig.MoveWorkflow(documentId, appliedConfig.GetExtensionPointValue(ConfigRequestProvider, "Move"), targetMove, target.Item.Status);
            if (!targetMove.IsValid)
            {
                return AggregateExtensions.PrepareInvalidFilterAggregate(targetMove);
            }
            profiler.TakeSnapshot();

            var targetResult = new ApplyResultContext
            {
                Context = _globalContext,
                Result = targetMove.Result ?? targetMove.Item,
                Item = targetMove.Item,
                Configuration = ConfigRequestProvider.GetConfiguration(),
                Metadata = ConfigRequestProvider.GetMetadataIdentifier(),
                Action = ConfigRequestProvider.GetServiceName()
            };

            profiler = target.Context.GetComponent<IProfilerComponent>().GetOperationProfiler("GetResultEventsPoint",
                           string.Format("Config: {0}, Metadata {1}, ActionPoint {2}", target.Configuration, target.Metadata, appliedConfig.GetExtensionPointValue(ConfigRequestProvider, "GetResult")));
            profiler.Reset();
            //формируем результат выполнения запроса
            appliedConfig.MoveWorkflow(documentId, appliedConfig.GetExtensionPointValue(ConfigRequestProvider, "GetResult"), targetResult);

            profiler.TakeSnapshot();

            if (!targetResult.IsValid)
            {
                return AggregateExtensions.PrepareInvalidResultAggregate(targetResult);
            }

            transaction.CommitTransaction();

            return AggregateExtensions.PrepareResultAggregate(targetResult.Result);
        }


    }
}
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
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.AuthApi;
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
	    private readonly IAggregateProvider _aggregateProvider;
		private readonly IGlobalContext _globalContext;
	    private string _documentVersion;

		public IConfigRequestProvider ConfigRequestProvider { get; set; }

        public ApplyChangesHandler(
            IAggregateProvider aggregateProvider, 
            IGlobalContext globalContext)
		{
		    _aggregateProvider = aggregateProvider;
			_globalContext = globalContext;
            
		}

	    /// <summary>
	    ///   Применить изменения, представленные в виде объекта
	    /// </summary>
	    /// <param name="id">Идентификатор объекта, к которому следует применить изменения</param>
	    /// <param name="changesObject">Объект JSON, содержащий события изменения объекта</param>
	    /// <param name="replace">Заменить объект в хранилище</param>
        /// <returns>Результат обработки JSON объекта</returns>
	    public dynamic ApplyJsonObject(string id, dynamic changesObject, bool  replace = false)
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
	    public dynamic ApplyEventsWithMetadata(string id, IEnumerable<EventDefinition> events, bool replace = false )
	    {
			var idType = ConfigRequestProvider.GetMetadataIdentifier();

		    var appliedConfig =
			    _globalContext.GetComponent<IConfigurationMediatorComponent>().GetConfiguration(ConfigRequestProvider);

			if (string.IsNullOrEmpty(idType))
			{
				throw new ArgumentException("index type undefined");
			}


            var target = new FilterEventContext();
            target.Context = _globalContext;
            target.Id = id;
            //target.Type = idType;
            target.IsValid = true;
	        target.Configuration = ConfigRequestProvider.GetConfiguration();
	        target.Metadata = ConfigRequestProvider.GetMetadataIdentifier();
            
			//1 ЭТАП: Редактируем список полученных событий в соответствие с указанными правилами 

            target.Events = events.ToList();

		    var profiler = target.Context.GetComponent<IProfilerComponent>().GetOperationProfiler("FilterEventsPoint",
                                       string.Format("Config: {0}, Metadata {1}, ActionPoint {2}", target.Configuration, target.Metadata, appliedConfig.MetadataConfiguration.GetExtensionPointValue(ConfigRequestProvider, "FilterEvents")));
			profiler.Reset();
			appliedConfig.MetadataConfiguration.MoveWorkflow(idType, appliedConfig.MetadataConfiguration.GetExtensionPointValue(ConfigRequestProvider, "FilterEvents"), target);
			if (!target.IsValid)
			{
				return AggregateExtensions.PrepareInvalidFilterAggregate(target);
			}
			profiler.TakeSnapshot();

            //2 ЭТАП: Получение агрегата
            var searchProvider = appliedConfig
	            .GetDocumentProvider(ConfigRequestProvider.GetMetadataIdentifier(),
	                target.Context.GetComponent<ISecurityComponent>()
	                    .GetClaim(AuthorizationStorageExtensions.OrganizationClaim, target.UserName) ??
	                AuthorizationStorageExtensions.AnonimousUser);

            dynamic item = null;
            if (!string.IsNullOrEmpty(target.Id) && !replace)
            {
	            item = searchProvider.GetDocument(target.Id);
	            //указываем версию конфигурации метаданных документа
	            if (item != null)
	            {
	                _documentVersion = item.Version;
	            }
            }
	        if (item == null)
	        {
                item = _aggregateProvider.CreateAggregate();
				if (target.Id != null)
				{
					item.Id = target.Id;
				}
			}

			//3 ЭТАП: Сохранение агрегата в кассандре
            var eventStorage = _globalContext.GetComponent<IEventStorageComponent>().GetEventStorage();

            if (eventStorage != null)
            {
				//временно отключено сохранение событий в cassandra из-за периодического свопа кассандры
                //eventStorage.AddEvents(new Guid(item.Id), target.Events.Select(JsonConvert.SerializeObject).ToList());
            }


            //4 ЭТАП: Применение списка событий
			_aggregateProvider.ApplyChanges(ref item, target.Events);


			//5 ЭТАП: Перевод объекта в нужное состояние
			//пытаемся перевести в указанное состояние.
			//для указанного workflow сконфигурировано в ActionUnits действие на сохранение объекта после выполнения действия по переходу

	        var targetMove = new ApplyContext
	        {
	            Id = item.Id,
	            Context = _globalContext,
	            Item = item,
	            Type = idType,
	            DefaultProperties = target.DefaultProperties,
	            Events = target.Events,
	            Version = _documentVersion,
	            Configuration = ConfigRequestProvider.GetConfiguration(),
	            Metadata = ConfigRequestProvider.GetMetadataIdentifier(),
	            Action = ConfigRequestProvider.GetServiceName(),
	            UserName = ConfigRequestProvider.GetUserName(),
	            TransactionMarker = item.TransactionMarker ?? Guid.NewGuid().ToString()
	        };

            //получаем менеджер для управления распределенной транзакцией
	        var transaction =
	            _globalContext.GetComponent<ITransactionComponent>()
	                .GetTransactionManager()
	                .GetTransaction(targetMove.TransactionMarker);
	        
            
            //если не заполнена версия документа, то формируем ее
	        if (string.IsNullOrEmpty(item.Version))
            {
				item.Version = appliedConfig.GetConfigurationVersion();
            }

			profiler = target.Context.GetComponent<IProfilerComponent>().GetOperationProfiler("MovePoint",
			   string.Format("Config: {0}, Metadata {1}, ActionPoint {2}", target.Configuration, target.Metadata, appliedConfig.MetadataConfiguration.GetExtensionPointValue(ConfigRequestProvider, "Move")));
			profiler.Reset();
            appliedConfig.MetadataConfiguration.MoveWorkflow(idType, appliedConfig.MetadataConfiguration.GetExtensionPointValue(ConfigRequestProvider, "Move"), targetMove, item.Status);
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
	            Version = _documentVersion,
	            Configuration = ConfigRequestProvider.GetConfiguration(),
	            Metadata = ConfigRequestProvider.GetMetadataIdentifier(),
	            Action = ConfigRequestProvider.GetServiceName()
	        };

			profiler = target.Context.GetComponent<IProfilerComponent>().GetOperationProfiler("GetResultEventsPoint",
						   string.Format("Config: {0}, Metadata {1}, ActionPoint {2}", target.Configuration, target.Metadata, appliedConfig.MetadataConfiguration.GetExtensionPointValue(ConfigRequestProvider, "GetResult")));
			profiler.Reset();
			//формируем результат выполнения запроса
			appliedConfig.MetadataConfiguration.MoveWorkflow(idType, appliedConfig.MetadataConfiguration.GetExtensionPointValue(ConfigRequestProvider, "GetResult"), targetResult);

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
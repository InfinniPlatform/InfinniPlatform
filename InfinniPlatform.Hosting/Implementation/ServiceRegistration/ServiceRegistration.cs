using System;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Hosting.Implementation.ServiceRegistration
{
    /// <summary>
    ///     Регистрация сервиса
    /// </summary>
    public class ServiceRegistration : IServiceRegistration
    {
        private QueryHandler _queryHandler;
        private readonly string _metadataName;
        private readonly string _serviceName;
        private readonly IServiceTemplate _serviceTemplate;

        public ServiceRegistration(string metadataName, string serviceName, IServiceTemplate serviceTemplate)
        {
            _metadataName = metadataName;
            _serviceName = serviceName;
            _serviceTemplate = serviceTemplate;
        }

        /// <summary>
        ///     Наименование метода обработчика
        /// </summary>
        public string HandlerName
        {
            get { return ExtensionPointHandler.ActionName; }
        }

        /// <summary>
        ///     Наименование объекта метаданных
        /// </summary>
        public string MetadataName
        {
            get { return _metadataName; }
        }

        /// <summary>
        ///     Обработчик запроса
        /// </summary>
        public IQueryHandler QueryHandler
        {
            get { return _queryHandler; }
        }

        /// <summary>
        ///     Обработчик действия запроса
        /// </summary>
        public IExtensionPointHandler ExtensionPointHandler { get; private set; }

        /// <summary>
        ///     Наименование зарегистрированного сервиса
        /// </summary>
        public string ServiceName
        {
            get { return _serviceName; }
        }

        /// <summary>
        ///     Зарегистрировать сервис для конфигурации
        /// </summary>
        /// <param name="extensionPointHandler"></param>
        /// <param name="queryHandlerAction">инициализатор обработчика запроса</param>
        /// <returns></returns>
        public IServiceRegistration RegisterService(Func<string, IExtensionPointHandler> extensionPointHandler,
            Action<IQueryHandler> queryHandlerAction = null)
        {
            if (_serviceTemplate == null || _serviceTemplate.ServiceInstanceType == null)
            {
                throw new ArgumentException(
                    string.Format(
                        "service template type \"{0}\" not registered. Possibly, need to register service type?",
                        _serviceName));
            }

            var typeQueryHandler = _serviceTemplate.ServiceInstanceType;
            if (typeQueryHandler.IsInterface || typeQueryHandler.IsAbstract)
            {
                throw new ArgumentException("concrete type should be registered as template");
            }

            _queryHandler = new QueryHandler(typeQueryHandler, HttpResultHandlerType.Standard);


            if (extensionPointHandler != null)
            {
                SetExtensionPointHandler(extensionPointHandler);
            }

            if (queryHandlerAction != null)
            {
                queryHandlerAction.Invoke(QueryHandler);
            }


            return this;
        }

        public IServiceRegistration SetExtensionPointHandler(Func<string, IExtensionPointHandler> extensionPointHandler)
        {
            ExtensionPointHandler = _serviceTemplate.Build(extensionPointHandler);
            QueryHandler.ForAction(ExtensionPointHandler);
            return this;
        }

        /// <summary>
        ///     Установить обработчик результата запроса
        /// </summary>
        public IServiceRegistration SetResultHandler(HttpResultHandlerType httpResultHandlerType)
        {
            _queryHandler.SetResultHandler(httpResultHandlerType);


            return this;
        }
    }
}
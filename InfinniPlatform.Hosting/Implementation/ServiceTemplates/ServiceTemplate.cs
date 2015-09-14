using System;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Hosting.Implementation.ServiceTemplates
{
    /// <summary>
    ///     Шаблон сервиса
    /// </summary>
    public sealed class ServiceTemplate : IServiceTemplate
    {
        private readonly IExtensionPointHandlerConfig _handlerConfig;
        private readonly string _methodName;
        private readonly Type _type;

        public ServiceTemplate(Type type, string methodName, IExtensionPointHandlerConfig handlerConfig)
        {
            _type = type;
            _methodName = methodName;
            _handlerConfig = handlerConfig;
        }

        /// <summary>
        ///     Тип класса, предоставляющего сервис
        /// </summary>
        public Type ServiceInstanceType
        {
            get { return _type; }
        }

        /// <summary>
        ///     Конфигурация обработчика
        /// </summary>
        public IExtensionPointHandlerConfig HandlerConfig
        {
            get { return _handlerConfig; }
        }

        /// <summary>
        ///     Создать экземпляр сконфигурированного обработчика запросов
        /// </summary>
        /// <returns></returns>
        public IExtensionPointHandler Build(Func<string, IExtensionPointHandler> buildActionHandler)
        {
            var handler = buildActionHandler(_methodName);
            if (HandlerConfig != null)
            {
                HandlerConfig.BuildHandler(handler);
            }
            return handler;
        }
    }
}
using System.Collections.Generic;

using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Core.Hosting
{
    /// <summary>
    ///     Конфигурация метаданных сервисов
    /// </summary>
    public sealed class ExtensionPointHandlerConfig : IExtensionPointHandlerConfig
    {
        private VerbType _verbType = VerbType.Get;

        private readonly Dictionary<string, ContextTypeKind> _workflowExtensionPoints =
            new Dictionary<string, ContextTypeKind>();

        /// <summary>
        ///     Тип вызываемого сервиса
        /// </summary>
        public VerbType VerbType
        {
            get { return _verbType; }
        }

        public Dictionary<string, ContextTypeKind> WorkflowExtensionPoints
        {
            get { return _workflowExtensionPoints; }
        }

        /// <summary>
        ///     Инициализировать обработчик на основе конфигурации
        /// </summary>
        /// <param name="handler">Обработчик</param>
        public void BuildHandler(IExtensionPointHandler handler)
        {
            foreach (var workflowExtensionPoint in WorkflowExtensionPoints)
            {
                handler.AddExtensionPoint(workflowExtensionPoint.Key, workflowExtensionPoint.Value);
            }
            handler.AsVerb(_verbType);
        }

        /// <summary>
        ///     Регистрация точки расширения логики
        /// </summary>
        /// <param name="pointName">Наименование точки расширения</param>
        /// <param name="contextTypeKind">Тип точки расширения</param>
        /// <returns>Конфигурация  метаданных сервиса</returns>
        public ExtensionPointHandlerConfig AddExtensionPoint(string pointName, ContextTypeKind contextTypeKind)
        {
            _workflowExtensionPoints.Add(pointName, contextTypeKind);
            return this;
        }

        /// <summary>
        ///     Сконфигурировать тип сервиса
        /// </summary>
        /// <param name="verbType">Тип сервиса</param>
        /// <returns>Конфигурация метаданных сервиса</returns>
        public ExtensionPointHandlerConfig AsVerb(VerbType verbType)
        {
            _verbType = verbType;
            return this;
        }
    }
}
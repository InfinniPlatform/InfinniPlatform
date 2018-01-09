using System;
using System.Reflection;

using InfinniPlatform.IoC;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Extensions methods for components registration in <see cref="IContainerBuilder"/>.
    /// </summary>
    public static class DocumentHttpServiceExtensions
    {
        /// <summary>
        /// Регистрирует обработчик по умолчанию для сервиса по работе с документами указанного типа.
        /// </summary>
        /// <param name="builder">Dependency container builder.</param>
        /// <param name="documentType">Имя типа документа.</param>
        public static void RegisterDocumentHttpService(this IContainerBuilder builder, string documentType)
        {
            RegisterDocumentHttpServiceInstance(builder, new DocumentHttpServiceHandler(documentType));
        }

        /// <summary>
        /// Регистрирует обработчик по умолчанию для сервиса по работе с документами указанного типа.
        /// </summary>
        /// <typeparam name="TDocument">Тип документа.</typeparam>
        /// <param name="builder">Dependency container builder.</param>
        /// <param name="documentType">Имя типа документа.</param>
        public static void RegisterDocumentHttpService<TDocument>(this IContainerBuilder builder, string documentType = null) where TDocument : Document
        {
            RegisterDocumentHttpServiceInstance(builder, new DocumentHttpServiceHandler<TDocument>(documentType));
        }

        /// <summary>
        /// Регистрирует тип обработчика для сервиса по работе с документами.
        /// </summary>
        /// <param name="builder">Dependency container builder.</param>
        /// <param name="serviceHandlerType">Тип обработчика для сервиса по работе с документами.</param>
        /// <example>
        /// <code>
        /// RegisterDocumentHttpServiceType(typeof(MyDocumentHttpServiceHandler))
        /// </code>
        /// </example>
        public static void RegisterDocumentHttpServiceType(this IContainerBuilder builder, Type serviceHandlerType)
        {
            builder.RegisterType(serviceHandlerType).As<IDocumentHttpServiceHandlerBase>();
        }

        /// <summary>
        /// Регистрирует тип обработчика для сервиса по работе с документами.
        /// </summary>
        /// <typeparam name="TServiceHandler">Тип обработчика для сервиса по работе с документами.</typeparam>
        /// <example>
        /// <code>
        /// RegisterDocumentHttpServiceType&lt;MyDocumentHttpServiceHandler&gt;()
        /// </code>
        /// </example>
        public static void RegisterDocumentHttpServiceType<TServiceHandler>(this IContainerBuilder builder) where TServiceHandler : class, IDocumentHttpServiceHandlerBase
        {
            builder.RegisterType<TServiceHandler>().As<IDocumentHttpServiceHandlerBase>();
        }

        /// <summary>
        /// Регистрирует экземпляр обработчика для сервиса по работе с документами.
        /// </summary>
        /// <typeparam name="TServiceHandler">Тип обработчика для сервиса по работе с документами.</typeparam>
        /// <example>
        /// <code>
        /// RegisterDocumentHttpServiceInstance(new MyDocumentHttpServiceHandler())
        /// </code>
        /// </example>
        public static void RegisterDocumentHttpServiceInstance<TServiceHandler>(this IContainerBuilder builder, TServiceHandler serviceHandler) where TServiceHandler : class, IDocumentHttpServiceHandlerBase
        {
            builder.RegisterInstance(serviceHandler).As<IDocumentHttpServiceHandlerBase>();
        }

        /// <summary>
        /// Регистрирует все обработчики для сервисов по работе с документами.
        /// </summary>
        /// <remarks>
        /// Обработчики будут зарегистрированы со стратегией SingleInstance().
        /// </remarks>
        /// <example>
        /// <code>
        /// RegisterDocumentHttpServices(GetType().Assembly)
        /// </code>
        /// </example>
        public static void RegisterDocumentHttpServices(this IContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly, t => typeof(IDocumentHttpServiceHandlerBase).GetTypeInfo().IsAssignableFrom(t), r => r.As<IDocumentHttpServiceHandlerBase>().SingleInstance());
        }
    }
}
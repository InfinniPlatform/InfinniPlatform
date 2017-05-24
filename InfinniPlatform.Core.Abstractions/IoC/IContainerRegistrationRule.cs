using System;

namespace InfinniPlatform.IoC
{
    /// <summary>
    /// Правила регистрации компонента.
    /// </summary>
    public interface IContainerRegistrationRule
    {
        /// <summary>
        /// Определяет, что зависимость создается один раз и навсегда.
        /// </summary>
        IContainerRegistrationRule SingleInstance();

        /// <summary>
        /// Определяет, что зависимость никогда не освобождается контейнером.
        /// </summary>
        IContainerRegistrationRule ExternallyOwned();

        /// <summary>
        /// Определяет, что зависимость должна создаваться при каждом получении.
        /// </summary>
        IContainerRegistrationRule InstancePerDependency();

        /// <summary>
        /// Определяет, что зависимость должна создаваться на время выполнения запроса.
        /// </summary>
        IContainerRegistrationRule InstancePerLifetimeScope();

        /// <summary>
        /// Определяет сервис, который предоставляет компонент.
        /// </summary>
        /// <typeparam name="TService">Тип сервиса.</typeparam>
        IContainerRegistrationRule As<TService>();

        /// <summary>
        /// Определяет сервис, который предоставляет компонент.
        /// </summary>
        /// <param name="serviceTypes">Типы сервисов.</param>
        IContainerRegistrationRule As(params Type[] serviceTypes);

        /// <summary>
        /// Определяет, что сервисом является сам компонент.
        /// </summary>
        IContainerRegistrationRule AsSelf();

        /// <summary>
        /// Определяет, что сервисами являются все интерфейсы, реализованные компонентом.
        /// </summary>
        IContainerRegistrationRule AsImplementedInterfaces();
    }
}
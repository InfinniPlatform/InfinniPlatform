using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.IoC
{
    /// <summary>
    /// Провайдер разрешения зависимостей.
    /// </summary>
    public interface IContainerResolver
    {
        /// <summary>
        /// Возвращает список зарегистрированных сервисов.
        /// </summary>
        IEnumerable<Type> Services { get; }

        /// <summary>
        /// Определяет, зарегистрирован ли сервис указанного типа.
        /// </summary>
        /// <typeparam name="TService">Тип сервиса.</typeparam>
        /// <returns>Возвращает <c>true</c>, если сервис указанного типа зарегистрирован, иначе - <c>false</c>.</returns>
        /// <example>
        /// IsRegistered&lt;IMyService&gt;()
        /// </example>
        bool IsRegistered<TService>() where TService : class;

        /// <summary>
        /// Определяет, зарегистрирован ли сервис указанного типа.
        /// </summary>
        /// <param name="serviceType">Тип сервиса.</param>
        /// <returns>Возвращает <c>true</c>, если сервис указанного типа зарегистрирован, иначе - <c>false</c>.</returns>
        /// <example>
        /// IsRegistered(typeof(IMyService))
        /// </example>
        bool IsRegistered(Type serviceType);

        /// <summary>
        /// Осуществляет попытку получения экземпляра сервиса.
        /// </summary>
        /// <typeparam name="TService">Тип сервиса.</typeparam>
        /// <param name="serviceInstance">Экземпляр сервиса, если он был зарегистрирован, иначе <c>null</c>.</param>
        /// <returns>Возвращает <c>true</c>, если сервис указанного типа зарегистрирован, иначе - <c>false</c>.</returns>
        /// <example>
        /// TryResolve&lt;IMyService&gt;(out serviceInstance)
        /// </example>
        bool TryResolve<TService>(out TService serviceInstance) where TService : class;

        /// <summary>
        /// Осуществляет попытку получения экземпляра сервиса.
        /// </summary>
        /// <param name="serviceType">Тип сервиса.</param>
        /// <param name="serviceInstance">Экземпляр сервиса, если он был зарегистрирован, иначе <c>null</c>.</param>
        /// <example>
        /// TryResolve(typeof(IMyService), out serviceInstance)
        /// </example>
        bool TryResolve(Type serviceType, out object serviceInstance);

        /// <summary>
        /// Возвращает экземпляр сервиса.
        /// </summary>
        /// <typeparam name="TService">Тип сервиса.</typeparam>
        /// <returns>Экземпляр сервиса.</returns>
        /// <remarks>Если сервис не был зарегистрирован, метод вернет исключение.</remarks>
        /// <example>
        /// Resolve&lt;IMyService&gt;()
        /// </example>
        TService Resolve<TService>() where TService : class;

        /// <summary>
        /// Возвращает экземпляр сервиса.
        /// </summary>
        /// <param name="serviceType">Тип сервиса.</param>
        /// <returns>Экземпляр сервиса.</returns>
        /// <remarks>Если сервис не был зарегистрирован, метод вернет исключение.</remarks>
        /// <example>
        /// Resolve(typeof(IMyService))
        /// </example>
        object Resolve(Type serviceType);

        /// <summary>
        /// Возвращает экземпляр сервиса, если он был зарегистрирован.
        /// </summary>
        /// <typeparam name="TService">Тип сервиса.</typeparam>
        /// <returns>Экземпляр сервиса или <c>null</c>, если сервис не был зарегистрирован.</returns>
        /// <example>
        /// ResolveOptional&lt;IMyService&gt;()
        /// </example>
        TService ResolveOptional<TService>() where TService : class;

        /// <summary>
        /// Возвращает экземпляр сервиса, если он был зарегистрирован.
        /// </summary>
        /// <param name="serviceType">Тип сервиса.</param>
        /// <returns>Экземпляр сервиса или <c>null</c>, если сервис не был зарегистрирован.</returns>
        /// <example>
        /// ResolveOptional(typeof(IMyService))
        /// </example>
        object ResolveOptional(Type serviceType);
    }
}
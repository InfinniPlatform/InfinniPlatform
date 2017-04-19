using System;

namespace InfinniPlatform.Core.IoC
{
    /// <summary>
    /// Регистратор зависимостей и правил их разрешения.
    /// </summary>
    public interface IContainerBuilder
    {
        /// <summary>
        /// Регистрирует компонент по его типу.
        /// </summary>
        /// <typeparam name="TComponent">Тип, реализующий компонент.</typeparam>
        /// <returns>Правила регистрации компонента.</returns>
        /// <remarks>Компонент будет создан с помощью Reflection.</remarks>
        /// <example>
        /// RegisterType&lt;MyComponent&gt;()
        /// </example>
        IContainerRegistrationRule RegisterType<TComponent>() where TComponent : class;

        /// <summary>
        /// Регистрирует компонент по его типу.
        /// </summary>
        /// <param name="componentType">Тип, реализующий компонент.</param>
        /// <returns>Правила регистрации компонента.</returns>
        /// <remarks>Компонент будет создан с помощью Reflection.</remarks>
        /// <example>
        /// RegisterType(typeof(MyComponent))
        /// </example>
        IContainerRegistrationRule RegisterType(Type componentType);

        /// <summary>
        /// Регистрирует компонент по его generic-типу.
        /// </summary>
        /// <param name="componentType">Тип, реализующий компонент.</param>
        /// <returns>Правила регистрации компонента.</returns>
        /// <remarks>Компонент будет создан с помощью Reflection.</remarks>
        /// <example>
        /// RegisterGeneric(typeof(MyComponent&lt;&gt;))
        /// </example>
        IContainerRegistrationRule RegisterGeneric(Type componentType);

        /// <summary>
        /// Регистрирует экземпляр компонента.
        /// </summary>
        /// <typeparam name="TComponent">Тип, реализующий компонент.</typeparam>
        /// <param name="componentInstance">Экземпляр компонента.</param>
        /// <returns>Правила регистрации компонента.</returns>
        /// <example>
        /// RegisterInstance(new MyComponent())
        /// </example>
        IContainerRegistrationRule RegisterInstance<TComponent>(TComponent componentInstance) where TComponent : class;

        /// <summary>
        /// Регистрирует фабричный метод компонента.
        /// </summary>
        /// <typeparam name="TComponent">Тип, реализующий компонент.</typeparam>
        /// <param name="componentFactory">Фабричный метод компонента.</param>
        /// <returns>Правила регистрации компонента.</returns>
        /// <example>
        /// RegisterFactory(r =&gt; new MyComponent())
        /// </example>
        IContainerRegistrationRule RegisterFactory<TComponent>(Func<IContainerResolver, TComponent> componentFactory) where TComponent : class;

        /// <summary>
        /// Определяет обработчик на событие создания зависимости.
        /// </summary>
        /// <param name="parameterResolver">Обработчик разрешения зависимостей, передаваемых через параметры конструкторов.</param>
        void OnCreateInstance(IContainerParameterResolver parameterResolver);

        /// <summary>
        /// Определяет обработчик на событие инициализации зависимости.
        /// </summary>
        /// <param name="instanceActivator">Обработчик инициализации экземпляра зависимости.</param>
        void OnActivateInstance(IContainerInstanceActivator instanceActivator);
    }
}
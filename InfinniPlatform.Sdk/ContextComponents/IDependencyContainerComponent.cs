namespace InfinniPlatform.Sdk.ContextComponents
{
    /// <summary>
    ///     Компонент для регистрации пользовательских зависимостей
    /// </summary>
    public interface IDependencyContainerComponent
    {
        /// <summary>
        ///     Зарегистрировать пользовательскую зависимость
        /// </summary>
        /// <typeparam name="TImplementation">Реализация зависимости</typeparam>
        void RegisterDependencyType<TImplementation>();

        /// <summary>
        ///     Зарегистрировать пользовательскую зависимость
        /// </summary>
        /// <typeparam name="TImplementation">Тип зависимости</typeparam>
        /// <param name="instance">Объект зависимости</param>
        void RegisterDependencyInstance<TImplementation>(TImplementation instance);

        /// <summary>
        ///     Разрешить зависимость
        /// </summary>
        /// <typeparam name="T">Тип регистрируемой зависимости</typeparam>
        T ResolveDependency<T>();

        /// <summary>
        ///     Обновить зависимости
        /// </summary>
        void UpdateDependencies();
    }
}
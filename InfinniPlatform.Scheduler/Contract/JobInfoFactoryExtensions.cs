using System;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Методы расширения для <see cref="IJobInfoFactory"/>.
    /// </summary>
    public static class JobInfoFactoryExtensions
    {
        /// <summary>
        /// Имя группы по умолчанию.
        /// </summary>
        public const string DefaultGroup = "Default";

        /// <summary>
        /// Создает новый экземпляр информации о задании <see cref="IJobInfo"/>.
        /// </summary>
        /// <typeparam name="THandler">Тип обработчика задания.</typeparam>
        /// <param name="target">Фабрика для создания информации о задании <see cref="IJobInfo"/>.</param>
        /// <param name="jobInfoBuilder">Функция для определения информации о задании.</param>
        /// <returns>Информация о задании.</returns>
        public static IJobInfo CreateJobInfo<THandler>(this IJobInfoFactory target, Action<IJobInfoBuilder> jobInfoBuilder) where THandler : IJobHandler
        {
            return CreateJobInfo<THandler>(target, Guid.NewGuid().ToString("N"), jobInfoBuilder);
        }

        /// <summary>
        /// Создает новый экземпляр информации о задании <see cref="IJobInfo"/>.
        /// </summary>
        /// <typeparam name="THandler">Тип обработчика задания.</typeparam>
        /// <param name="target">Фабрика для создания информации о задании <see cref="IJobInfo"/>.</param>
        /// <param name="name">Имя задания.</param>
        /// <param name="jobInfoBuilder">Функция для определения информации о задании.</param>
        /// <returns>Информация о задании.</returns>
        public static IJobInfo CreateJobInfo<THandler>(this IJobInfoFactory target, string name, Action<IJobInfoBuilder> jobInfoBuilder) where THandler : IJobHandler
        {
            return target.CreateJobInfo<THandler>(name, DefaultGroup, jobInfoBuilder);
        }
    }
}
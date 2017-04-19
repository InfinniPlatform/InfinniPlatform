using System;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Методы расширения для <see cref="IJobInfoFactory"/>.
    /// </summary>
    public static class JobInfoFactoryExtensions
    {
        /// <summary>
        /// Создает новый экземпляр информации о задании <see cref="IJobInfo"/>.
        /// </summary>
        /// <typeparam name="THandler">Тип обработчика заданий.</typeparam>
        /// <param name="target">Фабрика для создания информации о задании <see cref="IJobInfo"/>.</param>
        /// <param name="jobName">Имя задания.</param>
        /// <param name="jobGroup">Группа задания.</param>
        /// <param name="jobInfoBuilder">Функция для определения информации о задании.</param>
        /// <returns>Информация о задании.</returns>
        public static IJobInfo CreateJobInfo<THandler>(this IJobInfoFactory target, string jobName, string jobGroup, Action<IJobInfoBuilder> jobInfoBuilder) where THandler : IJobHandler
        {
            return target.CreateJobInfo(typeof(THandler), jobName, jobGroup, jobInfoBuilder);
        }

        /// <summary>
        /// Создает новый экземпляр информации о задании <see cref="IJobInfo"/>.
        /// </summary>
        /// <typeparam name="THandler">Тип обработчика заданий.</typeparam>
        /// <param name="target">Фабрика для создания информации о задании <see cref="IJobInfo"/>.</param>
        /// <param name="jobName">Имя задания.</param>
        /// <param name="jobInfoBuilder">Функция для определения информации о задании.</param>
        /// <returns>Информация о задании.</returns>
        public static IJobInfo CreateJobInfo<THandler>(this IJobInfoFactory target, string jobName, Action<IJobInfoBuilder> jobInfoBuilder) where THandler : IJobHandler
        {
            return target.CreateJobInfo(typeof(THandler), jobName, SchedulerExtensions.DefaultGroupName, jobInfoBuilder);
        }

        /// <summary>
        /// Создает новый экземпляр информации о задании <see cref="IJobInfo"/>.
        /// </summary>
        /// <param name="target">Фабрика для создания информации о задании <see cref="IJobInfo"/>.</param>
        /// <param name="jobHandler">Тип обработчика заданий.</param>
        /// <param name="jobName">Имя задания.</param>
        /// <param name="jobInfoBuilder">Функция для определения информации о задании.</param>
        /// <returns>Информация о задании.</returns>
        public static IJobInfo CreateJobInfo(this IJobInfoFactory target, Type jobHandler, string jobName, Action<IJobInfoBuilder> jobInfoBuilder)
        {
            return target.CreateJobInfo(jobHandler, jobName, SchedulerExtensions.DefaultGroupName, jobInfoBuilder);
        }
    }
}
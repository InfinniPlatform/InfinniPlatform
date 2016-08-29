using System;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Фабрика для создания информации о задании <see cref="JobInfo"/>.
    /// </summary>
    public interface IJobInfoFactory
    {
        /// <summary>
        /// Создает новый экземпляр информации о задании <see cref="JobInfo"/>.
        /// </summary>
        /// <typeparam name="THandler">Тип обработчика задания.</typeparam>
        /// <param name="name">Имя задания.</param>
        /// <param name="group">Группа задания.</param>
        /// <param name="jobInfoBuilder">Функция для определения информации о задании.</param>
        /// <returns>Информация о задании.</returns>
        JobInfo CreateJobInfo<THandler>(string name, string group, Action<IJobInfoBuilder> jobInfoBuilder) where THandler : IJobHandler;
    }
}
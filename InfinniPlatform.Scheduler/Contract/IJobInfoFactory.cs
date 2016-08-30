using System;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Фабрика для создания информации о задании <see cref="IJobInfo"/>.
    /// </summary>
    public interface IJobInfoFactory
    {
        /// <summary>
        /// Создает новый экземпляр информации о задании <see cref="IJobInfo"/>.
        /// </summary>
        /// <typeparam name="THandler">Тип обработчика задания.</typeparam>
        /// <param name="name">Имя задания.</param>
        /// <param name="group">Группа задания.</param>
        /// <param name="jobInfoBuilder">Функция для определения информации о задании.</param>
        /// <returns>Информация о задании.</returns>
        IJobInfo CreateJobInfo<THandler>(string name, string group, Action<IJobInfoBuilder> jobInfoBuilder) where THandler : IJobHandler;
    }
}
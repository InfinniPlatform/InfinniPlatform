using System;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Фабрика для создания информации о задании <see cref="IJobInfo"/>.
    /// </summary>
    public interface IJobInfoFactory
    {
        /// <summary>
        /// Создает новый экземпляр информации о задании <see cref="IJobInfo"/>.
        /// </summary>
        /// <param name="jobHandler">Тип обработчика заданий.</param>
        /// <param name="jobName">Имя задания.</param>
        /// <param name="jobGroup">Группа задания.</param>
        /// <param name="jobInfoBuilder">Функция для определения информации о задании.</param>
        /// <returns>Информация о задании.</returns>
        IJobInfo CreateJobInfo(Type jobHandler, string jobName, string jobGroup, Action<IJobInfoBuilder> jobInfoBuilder);
    }
}
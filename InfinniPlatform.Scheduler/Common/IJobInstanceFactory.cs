using System;

namespace InfinniPlatform.Scheduler.Common
{
    /// <summary>
    /// Предоставляет метод для создания уникального идентификатора экземпляра задания.
    /// </summary>
    internal interface IJobInstanceFactory
    {
        /// <summary>
        /// Создает уникальный идентификатор экземпляра задания.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        /// <param name="scheduledFireTimeUtc">Запланированное время срабатывания задания.</param>
        string CreateJobInstance(string jobId, DateTimeOffset scheduledFireTimeUtc);
    }
}
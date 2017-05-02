using System.Collections.Generic;

using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Scheduler.Clusterization
{
    /// <summary>
    /// Событие приостановки планирования заданий.
    /// </summary>
    [QueueName(SchedulerExtensions.ObjectNamePrefix + nameof(PauseJobEvent))]
    internal class PauseJobEvent
    {
        /// <summary>
        /// Признак необходимости приостановки планирования всех заданий.
        /// </summary>
        public bool All { get; set; }

        /// <summary>
        /// Список с уникальными идентификаторами заданий, планирование которых необходимо остановить.
        /// </summary>
        public IEnumerable<string> JobIds { get; set; }
    }
}
﻿using System.Collections.Generic;

using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.Scheduler.Common
{
    /// <summary>
    /// Событие удаления заданий.
    /// </summary>
    [QueueName(SchedulerExtensions.ObjectNamePrefix + nameof(DeleteJobEvent))]
    internal class DeleteJobEvent
    {
        /// <summary>
        /// Признак необходимости удаления всех заданий.
        /// </summary>
        public bool All { get; set; }

        /// <summary>
        /// Список с уникальными идентификаторами заданий, которые необходимо удалить.
        /// </summary>
        public IEnumerable<string> JobIds { get; set; }
    }
}
﻿using System.Collections.Generic;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.Scheduler.Contract;

namespace InfinniPlatform.Scheduler.Common
{
    /// <summary>
    /// Событие возобновления планирования заданий.
    /// </summary>
    [QueueName(SchedulerExtensions.ObjectNamePrefix + nameof(ResumeJobEvent))]
    internal class ResumeJobEvent
    {
        /// <summary>
        /// Признак необходимости возобновления планирования всех заданий.
        /// </summary>
        public bool All { get; set; }

        /// <summary>
        /// Список с уникальными идентификаторами заданий, планирование которых необходимо возобновить.
        /// </summary>
        public IEnumerable<string> JobIds { get; set; }
    }
}
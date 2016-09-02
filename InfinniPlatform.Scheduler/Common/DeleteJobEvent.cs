using System.Collections.Generic;

using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.Scheduler.Common
{
    /// <summary>
    /// ������� �������� �������.
    /// </summary>
    [QueueName(SchedulerConstants.ObjectNamePrefix + nameof(DeleteJobEvent))]
    internal class DeleteJobEvent
    {
        /// <summary>
        /// ������� ������������� �������� ���� �������.
        /// </summary>
        public bool All { get; set; }

        /// <summary>
        /// ������ � ����������� ���������������� �������, ������� ���������� �������.
        /// </summary>
        public IEnumerable<string> JobIds { get; set; }
    }
}
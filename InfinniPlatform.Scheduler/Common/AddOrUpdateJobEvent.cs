using System.Collections.Generic;

using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.Scheduler.Common
{
    /// <summary>
    /// ������� ���������� ��� ���������� �������.
    /// </summary>
    [QueueName(SchedulerConstants.ObjectNamePrefix + nameof(AddOrUpdateJobEvent))]
    internal class AddOrUpdateJobEvent
    {
        /// <summary>
        /// ������ � ����������� � ��������, ������� ���������� �������� ��� �������.
        /// </summary>
        public IEnumerable<JobInfo> JobInfos { get; set; }
    }
}
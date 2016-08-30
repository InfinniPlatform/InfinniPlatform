using System;

using InfinniPlatform.Scheduler.Contract;

namespace InfinniPlatform.Scheduler.Common
{
    internal class JobHandlerContext : IJobHandlerContext
    {
        public string InstanceId { get; set; }

        public DateTimeOffset FireTimeUtc { get; set; }

        public DateTimeOffset ScheduledFireTimeUtc { get; set; }

        public DateTimeOffset? PreviousFireTimeUtc { get; set; }

        public DateTimeOffset? NextFireTimeUtc { get; set; }

        public object Data { get; set; }
    }
}
using System;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.Scheduler
{
    public class JobHandlerContext : IJobHandlerContext
    {
        public string InstanceId { get; set; }

        public DateTimeOffset FireTimeUtc { get; set; }

        public DateTimeOffset ScheduledFireTimeUtc { get; set; }

        public DateTimeOffset? PreviousFireTimeUtc { get; set; }

        public DateTimeOffset? NextFireTimeUtc { get; set; }

        public DynamicDocument Data { get; set; }
    }
}
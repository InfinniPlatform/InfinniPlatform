using System;

using InfinniPlatform.Core.Abstractions.Dynamic;

namespace InfinniPlatform.Scheduler.Common
{
    internal class JobHandlerContext : IJobHandlerContext
    {
        public string InstanceId { get; set; }

        public DateTimeOffset FireTimeUtc { get; set; }

        public DateTimeOffset ScheduledFireTimeUtc { get; set; }

        public DateTimeOffset? PreviousFireTimeUtc { get; set; }

        public DateTimeOffset? NextFireTimeUtc { get; set; }

        public DynamicWrapper Data { get; set; }
    }
}
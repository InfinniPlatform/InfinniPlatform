using System;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.Scheduler
{
    /// <inheritdoc />
    public class JobHandlerContext : IJobHandlerContext
    {
        /// <inheritdoc />
        public string InstanceId { get; set; }

        /// <inheritdoc />
        public DateTimeOffset FireTimeUtc { get; set; }

        /// <inheritdoc />
        public DateTimeOffset ScheduledFireTimeUtc { get; set; }

        /// <inheritdoc />
        public DateTimeOffset? PreviousFireTimeUtc { get; set; }

        /// <inheritdoc />
        public DateTimeOffset? NextFireTimeUtc { get; set; }

        /// <inheritdoc />
        public DynamicDocument Data { get; set; }
    }
}
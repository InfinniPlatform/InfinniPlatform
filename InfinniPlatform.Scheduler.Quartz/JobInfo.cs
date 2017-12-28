using System;

using InfinniPlatform.DocumentStorage;
using InfinniPlatform.DocumentStorage.Attributes;
using InfinniPlatform.Dynamic;

namespace InfinniPlatform.Scheduler
{
    /// <inheritdoc cref="IJobInfo" />
    [DocumentType(SchedulerExtensions.ObjectNamePrefix + nameof(JobInfo))]
    public class JobInfo : Document, IJobInfo
    {
        /// <inheritdoc />
        [DocumentIgnore]
        public string Id
        {
            get => (string)_id;
            set => _id = value;
        }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string Group { get; set; }

        /// <inheritdoc />
        public JobState State { get; set; }

        /// <inheritdoc />
        public string Description { get; set; }

        /// <inheritdoc />
        public string HandlerType { get; set; }

        /// <inheritdoc />
        public string CronExpression { get; set; }

        /// <inheritdoc />
        public DateTimeOffset? StartTimeUtc { get; set; }

        /// <inheritdoc />
        public DateTimeOffset? EndTimeUtc { get; set; }

        /// <inheritdoc />
        public JobMisfirePolicy MisfirePolicy { get; set; }

        /// <inheritdoc />
        public DynamicDocument Data { get; set; }
    }
}
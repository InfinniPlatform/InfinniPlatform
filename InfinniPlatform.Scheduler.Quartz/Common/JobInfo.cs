using System;

using InfinniPlatform.DocumentStorage;
using InfinniPlatform.DocumentStorage.Attributes;
using InfinniPlatform.Dynamic;

namespace InfinniPlatform.Scheduler.Common
{
    [DocumentType(SchedulerExtensions.ObjectNamePrefix + nameof(JobInfo))]
    internal class JobInfo : Document, IJobInfo
    {
        [DocumentIgnore]
        public string Id
        {
            get { return (string)_id; }
            set { _id = value; }
        }

        public string Name { get; set; }

        public string Group { get; set; }

        public JobState State { get; set; }

        public string Description { get; set; }

        public string HandlerType { get; set; }

        public string CronExpression { get; set; }

        public DateTimeOffset? StartTimeUtc { get; set; }

        public DateTimeOffset? EndTimeUtc { get; set; }

        public JobMisfirePolicy MisfirePolicy { get; set; }

        public DynamicDocument Data { get; set; }
    }
}
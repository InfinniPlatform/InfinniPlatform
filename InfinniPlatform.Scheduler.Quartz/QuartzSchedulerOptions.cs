using System;

using InfinniPlatform.IoC;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Quartz scheduler configuration options.
    /// </summary>
    public class QuartzSchedulerOptions : IOptions
    {
        /// <inheritdoc />
        public string SectionName => "quartzScheduler";

        /// <summary>
        /// Default instance of <see cref="QuartzSchedulerOptions"/>.
        /// </summary>
        public static readonly QuartzSchedulerOptions Default = new QuartzSchedulerOptions();


        /// <summary>
        /// Initializes a new instance of <see cref="QuartzSchedulerOptions"/>.
        /// </summary>
        public QuartzSchedulerOptions()
        {
        }


        /// <summary>
        /// Expiration timeout for job handlers invocation history in seconds.
        /// </summary>
        public int? ExpireHistoryAfter { get; set; }

        /// <summary>
        /// Factory for creating <see cref="IJobSchedulerStateObserver"/> instance.
        /// </summary>
        public Func<IContainerResolver, IJobSchedulerStateObserver> JobSchedulerStateObserver { get; set; }

        /// <summary>
        /// Factory for creating <see cref="IJobSchedulerRepository"/> instance.
        /// </summary>
        public Func<IContainerResolver, IJobSchedulerRepository> JobSchedulerRepository { get; set; }
    }
}
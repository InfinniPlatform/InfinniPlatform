using System;

using InfinniPlatform.Core.Abstractions.IoC;

using Quartz;
using Quartz.Spi;

namespace InfinniPlatform.Scheduler.Dispatcher
{
    /// <summary>
    /// Фабрика обработчиков заданий Quartz.
    /// </summary>
    internal class QuartzJobFactory : IJobFactory
    {
        public QuartzJobFactory(IContainerResolver resolver)
        {
            _resolver = resolver;
        }


        private readonly IContainerResolver _resolver;


        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobType = bundle.JobDetail.JobType;

            return (IJob)_resolver.Resolve(jobType);
        }

        public void ReturnJob(IJob job)
        {
            // ReSharper disable SuspiciousTypeConversion.Global
            var disposable = job as IDisposable;
            // ReSharper restore SuspiciousTypeConversion.Global

            disposable?.Dispose();
        }
    }
}
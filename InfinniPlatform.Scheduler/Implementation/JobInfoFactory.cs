using System;

using InfinniPlatform.Scheduler.Contract;

namespace InfinniPlatform.Scheduler.Implementation
{
    internal class JobInfoFactory : IJobInfoFactory
    {
        public JobInfoFactory(IJobHandlerTypeSerializer handlerTypeSerializer)
        {
            _handlerTypeSerializer = handlerTypeSerializer;
        }


        private readonly IJobHandlerTypeSerializer _handlerTypeSerializer;


        public JobInfo CreateJobInfo<THandler>(string name, string group, Action<IJobInfoBuilder> jobInfoBuilder) where THandler : IJobHandler
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(group))
            {
                throw new ArgumentNullException(nameof(group));
            }

            var handlerType = _handlerTypeSerializer.Serialize(typeof(THandler));

            var jobInfo = new JobInfo(name, group, handlerType);

            var builder = new JobInfoBuilder(jobInfo);

            jobInfoBuilder?.Invoke(builder);

            jobInfo = builder.Build();

            jobInfo._id = $"{group}.{name}";

            return jobInfo;
        }
    }
}
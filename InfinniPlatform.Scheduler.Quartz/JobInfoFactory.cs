using System;

using InfinniPlatform.Scheduler.Properties;

namespace InfinniPlatform.Scheduler
{
    internal class JobInfoFactory : IJobInfoFactory
    {
        public JobInfoFactory(IJobHandlerTypeSerializer handlerTypeSerializer)
        {
            _handlerTypeSerializer = handlerTypeSerializer;
        }


        private readonly IJobHandlerTypeSerializer _handlerTypeSerializer;


        public IJobInfo CreateJobInfo(Type jobHandler, string jobName, string jobGroup, Action<IJobInfoBuilder> jobInfoBuilder)
        {
            if (jobHandler == null)
            {
                throw new ArgumentNullException(nameof(jobHandler));
            }

            if (string.IsNullOrEmpty(jobName))
            {
                throw new ArgumentNullException(nameof(jobName));
            }

            if (string.IsNullOrEmpty(jobGroup))
            {
                throw new ArgumentNullException(nameof(jobGroup));
            }

            if (!_handlerTypeSerializer.CanSerialize(jobHandler))
            {
                throw new ArgumentException(string.Format(Resources.CannotSerializeJobHandlerType, jobHandler.FullName, typeof(IJobHandler).FullName), nameof(jobHandler));
            }

            var handlerType = _handlerTypeSerializer.Serialize(jobHandler);

            var jobInfo = new JobInfo
            {
                _id = $"{jobGroup}.{jobName}",

                Name = jobName,
                Group = jobGroup,
                HandlerType = handlerType
            };

            var builder = new JobInfoBuilder(jobInfo);

            jobInfoBuilder?.Invoke(builder);

            jobInfo = builder.Build();

            return jobInfo;
        }
    }
}
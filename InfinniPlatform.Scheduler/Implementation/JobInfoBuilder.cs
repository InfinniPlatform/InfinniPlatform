using System;

using InfinniPlatform.Scheduler.Contract;

namespace InfinniPlatform.Scheduler.Implementation
{
    internal class JobInfoBuilder : IJobInfoBuilder
    {
        public JobInfoBuilder(JobInfo jobInfo)
        {
            _jobInfo = jobInfo;
        }


        private readonly JobInfo _jobInfo;


        public IJobInfoBuilder State(JobState state)
        {
            _jobInfo.State = state;
            return this;
        }

        public IJobInfoBuilder Description(string description)
        {
            _jobInfo.Description = description;
            return this;
        }

        public IJobInfoBuilder CronExpression(string cronExpression)
        {
            _jobInfo.CronExpression = cronExpression;
            return this;
        }

        public IJobInfoBuilder StartTimeUtc(DateTimeOffset? startTimeUtc)
        {
            _jobInfo.StartTimeUtc = startTimeUtc;
            return this;
        }

        public IJobInfoBuilder EndTimeUtc(DateTimeOffset? endTimeUtc)
        {
            _jobInfo.EndTimeUtc = endTimeUtc;
            return this;
        }

        public IJobInfoBuilder MisfirePolicy(JobMisfirePolicy? misfirePolicy)
        {
            _jobInfo.MisfirePolicy = misfirePolicy;
            return this;
        }

        public IJobInfoBuilder Data(object data)
        {
            _jobInfo.Data = data;
            return this;
        }


        public JobInfo Build()
        {
            return _jobInfo;
        }
    }
}
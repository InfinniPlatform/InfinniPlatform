using System;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.Scheduler
{
    /// <inheritdoc />
    public class JobInfoBuilder : IJobInfoBuilder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="JobInfoBuilder"/>.
        /// </summary>
        /// <param name="jobInfo">Job information.</param>
        public JobInfoBuilder(JobInfo jobInfo)
        {
            _jobInfo = jobInfo;
        }


        private readonly JobInfo _jobInfo;


        /// <inheritdoc />
        public IJobInfoBuilder State(JobState state)
        {
            _jobInfo.State = state;
            return this;
        }

        /// <inheritdoc />
        public IJobInfoBuilder Description(string description)
        {
            _jobInfo.Description = description;
            return this;
        }

        /// <inheritdoc />
        public IJobInfoBuilder CronExpression(string cronExpression)
        {
            _jobInfo.CronExpression = cronExpression;
            return this;
        }

        /// <inheritdoc />
        public IJobInfoBuilder StartTimeUtc(DateTimeOffset? startTimeUtc)
        {
            _jobInfo.StartTimeUtc = startTimeUtc;
            return this;
        }

        /// <inheritdoc />
        public IJobInfoBuilder EndTimeUtc(DateTimeOffset? endTimeUtc)
        {
            _jobInfo.EndTimeUtc = endTimeUtc;
            return this;
        }

        /// <inheritdoc />
        public IJobInfoBuilder MisfirePolicy(JobMisfirePolicy misfirePolicy)
        {
            _jobInfo.MisfirePolicy = misfirePolicy;
            return this;
        }

        /// <inheritdoc />
        public IJobInfoBuilder Data(DynamicDocument data)
        {
            _jobInfo.Data = data;
            return this;
        }


        /// <summary>
        /// Returns current <see cref="JobInfo"/> instance.
        /// </summary>
        public JobInfo Build()
        {
            return _jobInfo;
        }
    }
}
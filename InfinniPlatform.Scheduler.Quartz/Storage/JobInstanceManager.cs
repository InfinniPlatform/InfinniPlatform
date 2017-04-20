using System;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage;
using InfinniPlatform.Scheduler.Common;

namespace InfinniPlatform.Scheduler.Storage
{
    internal class JobInstanceManager : IJobInstanceManager
    {
        public JobInstanceManager(ISystemDocumentStorageFactory documentStorageFactory)
        {
            _jobLockStorage = documentStorageFactory.GetStorage<JobInstance>();
        }


        private readonly ISystemDocumentStorage<JobInstance> _jobLockStorage;


        public async Task<bool> IsHandled(string jobInstance)
        {
            if (string.IsNullOrEmpty(jobInstance))
            {
                throw new ArgumentNullException(nameof(jobInstance));
            }

            var jobLock = new JobInstance { _id = jobInstance };

            var result = await _jobLockStorage.SaveOneAsync(jobLock);

            return (result.UpdateStatus != DocumentUpdateStatus.Inserted);
        }
    }
}
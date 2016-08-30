using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Sdk.Queues.Producers;

namespace InfinniPlatform.Scheduler.Queues
{
    internal class JobSchedulerMessageQueue : IJobSchedulerMessageQueue
    {
        public JobSchedulerMessageQueue(ITaskProducer taskProducer)
        {
            _taskProducer = taskProducer;
        }


        private readonly ITaskProducer _taskProducer;


        public Task PublishHandleJob(HandleJobMessage message)
        {
            return _taskProducer.PublishAsync(message);
        }
    }
}
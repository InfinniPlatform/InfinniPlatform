using System;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.RabbitMq.Management;

namespace InfinniPlatform.MessageQueue.RabbitMQ.Hosting
{
    public class MessageQueueThreadPool : IMessageQueueThreadPool
    {
        public MessageQueueThreadPool(RabbitMqConnectionSettings settings)
        {
            _semaphore = new SemaphoreSlim(settings.MaxConcurrentThreads);
        }

        private readonly SemaphoreSlim _semaphore;

        public async Task Enqueue(Func<Task> func)
        {
            await _semaphore.WaitAsync();

            try
            {
                await func.Invoke();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
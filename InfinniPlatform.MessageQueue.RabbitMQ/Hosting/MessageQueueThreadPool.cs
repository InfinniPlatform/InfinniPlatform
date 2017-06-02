using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue.Hosting
{
    public class MessageQueueThreadPool
    {
        public MessageQueueThreadPool(RabbitMqMessageQueueOptions settings)
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
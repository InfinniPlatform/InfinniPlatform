using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue.Hosting
{
    /// <summary>
    /// Contols number of concurrent messages processing.
    /// </summary>
    public class MessageQueueThreadPool
    {
        private readonly SemaphoreSlim _semaphore;

        /// <summary>
        /// Initializes a new instance of <see cref="MessageQueueThreadPool" />.
        /// </summary>
        /// <param name="settings">RabbitMQ message queue configuration options.</param>
        public MessageQueueThreadPool(RabbitMqMessageQueueOptions settings)
        {
            _semaphore = new SemaphoreSlim(settings.MaxConcurrentThreads);
        }

        /// <summary>
        /// Invokes message processing func.
        /// </summary>
        /// <param name="func">Message processing func</param>
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
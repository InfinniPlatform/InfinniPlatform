using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.UserInterface.ViewBuilders.Messaging
{
    /// <summary>
    ///     Очередь сообщений.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    public sealed class MessageQueue<TMessage> : IDisposable
    {
        private readonly AutoResetEvent _enqueueEvent;
        private readonly Action<TMessage> _messageHandler;
        private readonly ConcurrentQueue<TMessage> _messageQueue;
        private readonly CancellationTokenSource _queueTaskManager;

        /// <summary>
        ///     Конструктор.
        /// </summary>
        /// <param name="messageHandler">Обработчик сообщения.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MessageQueue(Action<TMessage> messageHandler)
        {
            if (messageHandler == null)
            {
                throw new ArgumentNullException("messageHandler");
            }

            _messageHandler = messageHandler;
            _enqueueEvent = new AutoResetEvent(false);
            _messageQueue = new ConcurrentQueue<TMessage>();
            _queueTaskManager = new CancellationTokenSource();

            // Создание потока для асинхронной обработки сообщений
            Task.Factory.StartNew(QueueHandler, _queueTaskManager.Token);
        }

        public void Dispose()
        {
            _queueTaskManager.Cancel();
        }

        private void QueueHandler()
        {
            var cancelToken = _queueTaskManager.Token;
            var waitHandles = new[] {cancelToken.WaitHandle, _enqueueEvent};

            while (true)
            {
                // Ожидание завершения обработчика или новых сообщений
                WaitHandle.WaitAny(waitHandles);

                if (cancelToken.IsCancellationRequested)
                {
                    return;
                }

                TMessage message;

                while (_messageQueue.TryDequeue(out message))
                {
                    try
                    {
                        _messageHandler(message);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        ///     Поставить сообщение в очередь.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public void Enqueue(TMessage message)
        {
            _messageQueue.Enqueue(message);
            _enqueueEvent.Set();
        }
    }
}
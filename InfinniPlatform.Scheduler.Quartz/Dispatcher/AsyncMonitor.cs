using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler.Dispatcher
{
    /// <summary>
    /// Предоставляет механизм для синхронизации доступа к объектам в асинхронном коде (async/await).
    /// </summary>
    internal class AsyncMonitor
    {
        private static readonly Task CompletedTask = Task.FromResult(true);


        private readonly ConcurrentDictionary<object, Queue<TaskCompletionSource<bool>>> _waiters
            = new ConcurrentDictionary<object, Queue<TaskCompletionSource<bool>>>();


        /// <summary>
        /// Получает эксклюзивную блокировку указанного объекта.
        /// </summary>
        /// <param name="obj">Объект, для которого получается блокировка.</param>
        /// <exception cref="ArgumentNullException">Значение параметра <paramref name="obj"/> равно <c>null</c>.</exception>
        /// <returns>Задача для ожидания эксклюзивной блокировки.</returns>
        /// <example>
        /// <code>
        /// var monitor = new AsyncMonitor();
        /// ...
        /// await monitor.EnterAsync(obj);
        /// 
        /// try
        /// {
        ///    // protected code with obj
        /// }
        /// finally
        /// {
        ///    monitor.Exit(obj);
        /// }
        /// </code>
        /// </example>
        public Task EnterAsync(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            // Эксклюзивная блокировка на время операции над объектом
            lock (obj)
            {
                Queue<TaskCompletionSource<bool>> objWaiters;

                // Если у объекта есть ожидающие потоки
                if (_waiters.TryGetValue(obj, out objWaiters))
                {
                    // Добавление нового ожидающего потока в очередь
                    var waiter = new TaskCompletionSource<bool>();
                    objWaiters.Enqueue(waiter);
                    return waiter.Task;
                }

                // Объект никто не ожидает, можно произвести его захват
                _waiters[obj] = new Queue<TaskCompletionSource<bool>>();
            }

            return CompletedTask;
        }


        /// <summary>
        /// Освобождает эксклюзивную блокировку указанного объекта.
        /// </summary>
        /// <param name="obj">Объект, блокировка которого освобождается.</param>
        /// <exception cref="ArgumentNullException">Значение параметра <paramref name="obj"/> равно <c>null</c>.</exception>
        /// <example>
        /// <code>
        /// var monitor = new AsyncMonitor();
        /// ...
        /// await monitor.EnterAsync(obj);
        /// 
        /// try
        /// {
        ///    // protected code with obj
        /// }
        /// finally
        /// {
        ///    monitor.Exit(obj);
        /// }
        /// </code>
        /// </example>
        public void Exit(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            // Эксклюзивная блокировка на время операции над объектом
            lock (obj)
            {
                Queue<TaskCompletionSource<bool>> objWaiters;

                // Если у объекта есть ожидающие потоки
                if (_waiters.TryGetValue(obj, out objWaiters))
                {
                    // Если очередь не пустая
                    if (objWaiters.Count > 0)
                    {
                        // Объект захватывает следующий в очереди поток
                        var waiter = objWaiters.Dequeue();
                        waiter.SetResult(true);
                    }
                    // Если очередь пустая
                    else
                    {
                        // Объект полностью освобождается
                        Queue<TaskCompletionSource<bool>> deleted;
                        _waiters.TryRemove(obj, out deleted);
                    }
                }
            }
        }


        /// <summary>
        /// Получает эксклюзивную блокировку указанного объекта и возвращает объект для освобождения блокировки.
        /// </summary>
        /// <param name="obj">Объект, для которого получается блокировка.</param>
        /// <exception cref="ArgumentNullException">Значение параметра <paramref name="obj"/> равно <c>null</c>.</exception>
        /// <returns>Задача для ожидания эксклюзивной блокировки с объектом для освобождения.</returns>
        /// <example>
        /// <code>
        /// var monitor = new AsyncMonitor();
        /// ...
        /// using (await monitor.LockAsync(obj))
        /// {
        ///    // protected code with obj
        /// }
        /// </code>
        /// </example>
        public Task<IDisposable> LockAsync(object obj)
        {
            var waitTask = EnterAsync(obj);
            var releaser = new Releaser(this, obj);

            if (waitTask.IsCompleted)
            {
                return Task.FromResult<IDisposable>(releaser);
            }

            return waitTask.ContinueWith((t, s) => (IDisposable)s, releaser, CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }


        /// <summary>
        /// Класс для автоматического освобождения блокировки объекта.
        /// </summary>
        private struct Releaser : IDisposable
        {
            public Releaser(AsyncMonitor monitor, object obj)
            {
                _monitor = monitor;
                _obj = obj;
            }


            private readonly AsyncMonitor _monitor;
            private readonly object _obj;


            public void Dispose()
            {
                _monitor.Exit(_obj);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinniPlatform.Caching.Memory
{
    /// <summary>
    /// Реализует интерфейс шины сообщений для отслеживания изменений в кэше <see cref="MemoryCacheImpl"/>.
    /// </summary>
    internal sealed class MemoryCacheMessageBusImpl : ICacheMessageBus, IDisposable
    {
        private readonly Dictionary<string, List<Subscriber>> _subscribers
            = new Dictionary<string, List<Subscriber>>();


        public Task Publish(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Task.Run(() =>
            {
                lock (this)
                {
                    List<Subscriber> keySubscribers;

                    if (_subscribers.TryGetValue(key, out keySubscribers))
                    {
                        foreach (var subscriber in keySubscribers)
                        {
                            subscriber.Handle(key, value);
                        }
                    }
                }
            });
        }

        public IDisposable Subscribe(string key, Action<string, string> handler)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            Subscriber subscriber;

            lock (this)
            {
                List<Subscriber> keySubscribers;

                if (!_subscribers.TryGetValue(key, out keySubscribers))
                {
                    keySubscribers = new List<Subscriber>();

                    _subscribers.Add(key, keySubscribers);
                }

                subscriber = new Subscriber(handler, s =>
                {
                    lock (this)
                    {
                        keySubscribers.Remove(s);
                    }
                });

                keySubscribers.Add(subscriber);
            }

            return subscriber;
        }

        public void Dispose()
        {
            lock (this)
            {
                _subscribers.Clear();
            }
        }


        private sealed class Subscriber : IDisposable
        {
            public Subscriber(Action<string, string> handler, Action<Subscriber> unsubscribe)
            {
                _handler = handler;
                _unsubscribe = unsubscribe;
            }


            private readonly Action<string, string> _handler;
            private readonly Action<Subscriber> _unsubscribe;


            public void Handle(string key, string value)
            {
                _handler(key, value);
            }

            public void Dispose()
            {
                _unsubscribe(this);
            }
        }
    }
}
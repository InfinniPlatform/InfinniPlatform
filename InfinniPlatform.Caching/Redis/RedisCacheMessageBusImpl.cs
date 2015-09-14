using System;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace InfinniPlatform.Caching.Redis
{
	/// <summary>
	/// Реализует интерфейс шины сообщений для отслеживания изменений в кэше <see cref="RedisCacheImpl"/>.
	/// </summary>
	public sealed class RedisCacheMessageBusImpl : ICacheMessageBus, IDisposable
	{
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="name">Пространство имен для ключей.</param>
		/// <param name="connectionString">Строка подключения к Redis.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public RedisCacheMessageBusImpl(string name, string connectionString)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}

			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentNullException("connectionString");
			}

			_name = name;
			_exchange = new Lazy<ISubscriber>(() => CreateExchange(connectionString));
		}


		private readonly string _name;
		private readonly Lazy<ISubscriber> _exchange;


		public Task Publish(string key, string value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}

			return Task.Run(() =>
			{
				var wrappedKey = key.WrapCacheKey(_name);

				TryPublish(wrappedKey, value);
			});
		}

		public IDisposable Subscribe(string key, Action<string, string> handler)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}

			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}

			var wrappedKey = new RedisChannel(key.WrapCacheKey(_name), RedisChannel.PatternMode.Literal);

			var subscriber = new Subscriber(() => TryUnsubscribe(wrappedKey));

			TrySubscribe(wrappedKey, handler);

			return subscriber;
		}

		public void Dispose()
		{
			if (_exchange.IsValueCreated)
			{
				_exchange.Value.Multiplexer.Dispose();
			}
		}


		private static ISubscriber CreateExchange(string connectionString)
		{
			var connection = ConnectionMultiplexer.Connect(connectionString);
			var exchange = connection.GetSubscriber();
			return exchange;
		}

		private void TryPublish(string wrappedKey, string value)
		{
			CachingHelpers.TryExecute(() => _exchange.Value.Publish(wrappedKey, value));
		}

		private void TrySubscribe(RedisChannel wrappedKey, Action<string, string> handler)
		{
			CachingHelpers.TryExecute(() => _exchange.Value.Subscribe(wrappedKey, (k, v) => TryHandle(k, v, handler)));
		}

		private void TryUnsubscribe(RedisChannel wrappedKey)
		{
			CachingHelpers.TryExecute(() => _exchange.Value.Unsubscribe(wrappedKey));
		}

		private void TryHandle(string wrappedKey, string value, Action<string, string> handler)
		{
			var unwrappedKey = wrappedKey.UnwrapCacheKey(_name);

			if (!string.IsNullOrEmpty(unwrappedKey))
			{
				try
				{
					handler(unwrappedKey, value);
				}
				catch
				{
				}
			}
		}


		private sealed class Subscriber : IDisposable
		{
			public Subscriber(Action unsubscribe)
			{
				_unsubscribe = unsubscribe;
			}


			private readonly Action _unsubscribe;


			public void Dispose()
			{
				_unsubscribe();
			}
		}
	}
}
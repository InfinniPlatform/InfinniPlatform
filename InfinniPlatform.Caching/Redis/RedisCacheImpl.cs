using System;
using System.Linq;

using StackExchange.Redis;

namespace InfinniPlatform.Caching.Redis
{
	/// <summary>
	/// Реализует интерфейс для управления распределенным кэшем на базе Redis.
	/// </summary>
	public sealed class RedisCacheImpl : ICache, IDisposable
	{
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="name">Пространство имен для ключей.</param>
		/// <param name="connectionString">Строка подключения к Redis.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public RedisCacheImpl(string name, string connectionString)
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
			_database = new Lazy<IDatabase>(() => CreateDatabase(connectionString));
		}


		private readonly string _name;
		private readonly Lazy<IDatabase> _database;


		public bool Contains(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}

			return TryExecute((d, k) => d.KeyExists(k), key);
		}

		public string Get(string key)
		{
			string value;

			TryGet(key, out value);

			return value;
		}

		public bool TryGet(string key, out string value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}

			var cacheValue = TryExecute((d, k) => d.StringGet(k), key);

			value = cacheValue;

			return cacheValue.HasValue;
		}

		public void Set(string key, string value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}

			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			TryExecute((d, k) => d.StringSet(k, value), key);
		}

		public bool Remove(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}

			return TryExecute((d, k) => d.KeyDelete(k), key);
		}

		public void Clear()
		{
			TryExecute((d, k) =>
			{
				var endpoints = d.Multiplexer.GetEndPoints();

				if (endpoints != null && endpoints.Length > 0)
				{
					var server = d.Multiplexer.GetServer(endpoints[0]);

					if (server != null)
					{
						var allKeys = server.Keys(d.Database, k).ToArray();

						d.KeyDelete(allKeys);
					}
				}

				return true;
			}, "*");
		}

		public void Dispose()
		{
			if (_database.IsValueCreated)
			{
				var connection = _database.Value.Multiplexer;
				connection.Dispose();
			}
		}


		private static IDatabase CreateDatabase(string connectionString)
		{
			var connection = ConnectionMultiplexer.Connect(connectionString);
			var database = connection.GetDatabase();
			return database;
		}

		private T TryExecute<T>(Func<IDatabase, string, T> action, string key)
		{
			var wrappedKey = key.WrapCacheKey(_name);

			return CachingExtensions.TryExecute(() => action(_database.Value, wrappedKey));
		}
	}
}
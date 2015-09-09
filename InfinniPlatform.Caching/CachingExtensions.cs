using System;
using System.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Caching
{
	public static class CachingExtensions
	{
		internal const int AttempCount = 5;

		internal const int AttempDelay = 10 * 1000;


		/// <summary>
		/// Пытается выполнить метод за указанное количество попыток и с указанным ожиданием между попытками.
		/// </summary>
		/// <param name="action">Метод.</param>
		/// <param name="attempCount">Количество попыток.</param>
		/// <param name="attempDelay">Ожиданием между попытками.</param>
		internal static void TryExecute(Action action, int attempCount = AttempCount, int attempDelay = AttempDelay)
		{
			TryExecute<object>(() => { action(); return null; }, attempCount, attempDelay);
		}

		/// <summary>
		/// Пытается выполнить метод за указанное количество попыток и с указанным ожиданием между попытками.
		/// </summary>
		/// <param name="action">Метод.</param>
		/// <param name="attempCount">Количество попыток.</param>
		/// <param name="attempDelay">Ожиданием между попытками.</param>
		internal static T TryExecute<T>(Func<T> action, int attempCount = AttempCount, int attempDelay = AttempDelay)
		{
			Exception error = null;

			for (var i = 1; i <= attempCount; ++i)
			{
				try
				{
					return action();
				}
				catch (Exception e)
				{
					error = e;
				}

				if (i < attempCount)
				{
					Thread.Sleep(i * attempDelay);
				}
			}

			throw error ?? new InvalidOperationException();
		}


		/// <summary>
		/// Возвращает ключ кэширования с указанием пространства имен.
		/// </summary>
		/// <param name="unwrappedKey">Ключ кэширования без указания пространства имен.</param>
		/// <param name="name">Пространство имен.</param>
		/// <returns>Ключ кэширования с указанием пространства имен.</returns>
		internal static string WrapCacheKey(this string unwrappedKey, string name)
		{
			return string.Format("{0}.{1}", name, unwrappedKey);
		}

		/// <summary>
		/// Возвращает ключ кэширования без указания пространства имен.
		/// </summary>
		/// <param name="wrappedKey">Ключ кэширования с указанием пространства имен.</param>
		/// <param name="name">Пространство имен.</param>
		/// <returns>Ключ кэширования без указания пространства имен.</returns>
		internal static string UnwrapCacheKey(this string wrappedKey, string name)
		{
			string unwrappedKey = null;

			if (!string.IsNullOrEmpty(name))
			{
				if (!string.IsNullOrEmpty(wrappedKey) && wrappedKey.StartsWith(name + "."))
				{
					unwrappedKey = wrappedKey.Substring(name.Length + 1);
				}
			}
			else
			{
				unwrappedKey = wrappedKey;
			}

			return unwrappedKey;
		}


		public static object GetObject(this ICache cache, string key)
		{
			object value;

			TryGetObject(cache, key, out value);

			return value;
		}

		public static bool TryGetObject(this ICache cache, string key, out object value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}

			string stringValue;

			if (cache.TryGet(key, out stringValue))
			{
				value = JToken.Parse(stringValue);
				return true;
			}

			value = null;
			return false;
		}

		public static void SetObject(this ICache cache, string key, object value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}

			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			string stringValue = JsonConvert.SerializeObject(value);
			cache.Set(key, stringValue);
		}
	}
}
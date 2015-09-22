using System;
using System.Threading;

using InfinniPlatform.Api.Settings;

namespace InfinniPlatform.Caching
{
	internal static class CachingHelpers
	{
		public const int AttempCount = 5;

		public const int AttempDelay = 10 * 1000;

		public const string DefaultCacheName = "ApplicationCache";

		public const string DefaultRedisConnectionString = "localhost";


		/// <summary>
		/// Пытается выполнить метод за указанное количество попыток и с указанным ожиданием между попытками.
		/// </summary>
		/// <param name="action">Метод.</param>
		/// <param name="attempCount">Количество попыток.</param>
		/// <param name="attempDelay">Ожиданием между попытками.</param>
		public static void TryExecute(Action action, int attempCount = AttempCount, int attempDelay = AttempDelay)
		{
			TryExecute<object>(() =>
							   {
								   action();
								   return null;
							   }, attempCount, attempDelay);
		}

		/// <summary>
		/// Пытается выполнить метод за указанное количество попыток и с указанным ожиданием между попытками.
		/// </summary>
		/// <param name="action">Метод.</param>
		/// <param name="attempCount">Количество попыток.</param>
		/// <param name="attempDelay">Ожиданием между попытками.</param>
		public static T TryExecute<T>(Func<T> action, int attempCount = AttempCount, int attempDelay = AttempDelay)
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
		public static string WrapCacheKey(this string unwrappedKey, string name)
		{
			return string.Format("{0}.{1}", name, unwrappedKey);
		}

		/// <summary>
		/// Возвращает ключ кэширования без указания пространства имен.
		/// </summary>
		/// <param name="wrappedKey">Ключ кэширования с указанием пространства имен.</param>
		/// <param name="name">Пространство имен.</param>
		/// <returns>Ключ кэширования без указания пространства имен.</returns>
		public static string UnwrapCacheKey(this string wrappedKey, string name)
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


		/// <summary>
		/// Возвращает пространство имен для ключей из файла конфигурации.
		/// </summary>
		public static string GetConfigCacheName()
		{
			return AppSettings.GetValue("CacheName", DefaultCacheName);
		}

		/// <summary>
		/// Возвращает строку подключения к Redis из файла конфигурации.
		/// </summary>
		public static string GetConfigRedisConnectionString()
		{
			return AppSettings.GetValue("RedisConnectionString", DefaultRedisConnectionString);
		}
	}
}
using System.Collections.Generic;

using InfinniPlatform.Api.Hosting;

namespace InfinniPlatform.Hosting
{
	/// <summary>
	/// Контекст подсистемы хостинга.
	/// </summary>
	public interface IHostingContext
	{
		/// <summary>
		/// Настройки подсистемы хостинга.
		/// </summary>
		HostingConfig Configuration { get; }


		/// <summary>
		/// Окружение подсистемы хостинга.
		/// </summary>
		IDictionary<string, object> Environment { get; }


		/// <summary>
		/// Возвращает значение переменной окружения подсистемы хостинга.
		/// </summary>
		T Get<T>();

		/// <summary>
		/// Возвращает значение переменной окружения подсистемы хостинга.
		/// </summary>
		T Get<T>(string key);


		/// <summary>
		/// Устанавливает значение переменной окружения подсистемы хостинга.
		/// </summary>
		void Set<T>(T value);

		/// <summary>
		/// Устанавливает значение переменной окружения подсистемы хостинга.
		/// </summary>
		void Set<T>(string key, T value);
	}
}
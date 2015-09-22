using System;
using System.Threading.Tasks;

namespace InfinniPlatform.Caching
{
	/// <summary>
	/// Предоставляет интерфейс шины сообщений для отслеживания изменений в кэше <see cref="ICache"/>.
	/// </summary>
	public interface ICacheMessageBus
	{
		/// <summary>
		/// Публикует оповещение об изменении значения ключа.
		/// </summary>
		/// <param name="key">Ключ.</param>
		/// <param name="value">Значение.</param>
		/// <returns>Задача публикации.</returns>
		Task Publish(string key, string value);

		/// <summary>
		/// Подписывает на события изменения указанного ключа.
		/// </summary>
		/// <param name="key">Ключ.</param>
		/// <param name="handler">Обработчик.</param>
		/// <returns>Интерфейс отписки.</returns>
		IDisposable Subscribe(string key, Action<string, string> handler);
	}
}
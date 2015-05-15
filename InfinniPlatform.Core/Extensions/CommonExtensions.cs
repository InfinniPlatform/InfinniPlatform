using System;

namespace InfinniPlatform.Extensions
{
	/// <summary>
	/// Содержит общие и часто используемые методы расширения.
	/// </summary>
	public static class CommonExtensions
	{
		/// <summary>
		/// Выполнить действие над объектом с подавлением возможных исключений.
		/// </summary>
		/// <typeparam name="T">Тип объекта.</typeparam>
		/// <param name="target">Вызываемый объкт.</param>
		/// <param name="action">Действие над объектом.</param>
		/// <returns>Возвращает true, если действие выполнено без ошибок; иначе false.</returns>
		public static bool ExecuteSilent<T>(this T target, Action<T> action)
		{
			var success = false;

			try
			{
				action(target);
				success = true;
			}
			catch
			{
			}

			return success;
		}

		/// <summary>
		/// Выполнить действие над объектом с подавлением возможных исключений.
		/// </summary>
		/// <typeparam name="T">Тип объекта.</typeparam>
		/// <typeparam name="TResult">Тип результата.</typeparam>
		/// <param name="target">Вызываемый объкт.</param>
		/// <param name="action">Действие над объектом.</param>
		/// <param name="result">Результат работы.</param>
		/// <returns>Возвращает true, если действие выполнено без ошибок; иначе false.</returns>
		public static bool ExecuteSilent<T, TResult>(this T target, Func<T, TResult> action, out TResult result)
		{
			var success = false;

			result = default(TResult);

			try
			{
				result = action(target);
				success = true;
			}
			catch
			{
			}

			return success;
		}
	}
}
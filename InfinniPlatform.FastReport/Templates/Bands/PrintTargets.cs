using System;

namespace InfinniPlatform.FastReport.Templates.Bands
{
	/// <summary>
	/// Местоположение печати.
	/// </summary>
	[Flags]
	public enum PrintTargets
	{
		/// <summary>
		/// Не печатать.
		/// </summary>
		None = 0,

		/// <summary>
		/// Первая страница.
		/// </summary>
		FirstPage = 1,

		/// <summary>
		/// Последняя страница.
		/// </summary>
		LastPage = 2,

		/// <summary>
		/// Нечетные страницы.
		/// </summary>
		OddPages = 4,

		/// <summary>
		/// Четные страницы.
		/// </summary>
		EvenPages = 8,

		/// <summary>
		/// На всех страницах.
		/// </summary>
		All = FirstPage | LastPage | OddPages | EvenPages
	}
}
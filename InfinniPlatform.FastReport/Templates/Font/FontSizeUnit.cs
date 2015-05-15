namespace InfinniPlatform.FastReport.Templates.Font
{
	/// <summary>
	/// Единицы измерения размера шрифта.
	/// </summary>
	public enum FontSizeUnit
	{
		/// <summary>
		/// Пункт.
		/// </summary>
		/// <remarks>
		/// 1 pt == (96/72) px == (1/72) in == (25.4/72) mm
		/// </remarks>
		Pt = 0,

		/// <summary>
		/// Пиксель.
		/// </summary>
		/// <remarks>
		/// 1 px == (72/96) pt == (1/96) in == (25.4/96) mm
		/// </remarks>
		Px = 1,

		/// <summary>
		/// Дюйм.
		/// </summary>
		/// <remarks>
		/// 1 in == 72 pt == 96 px == 25.4 mm
		/// </remarks>
		In = 2,

		/// <summary>
		/// Миллиметр.
		/// </summary>
		/// <remarks>
		/// 1 mm == (72/25.4) pt == (96/25.4) px == (1/25.4) in
		/// </remarks>
		Mm = 4,
	}
}
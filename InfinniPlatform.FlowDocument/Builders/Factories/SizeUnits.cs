namespace InfinniPlatform.FlowDocument.Builders.Factories
{
	/// <summary>
	/// Единицы измерения.
	/// </summary>
	static class SizeUnits
	{
		/// <summary>
		/// Количество пунктов в пикселе.
		/// </summary>
		/// <remarks>
		/// 1 pt == (96/72) px == (1/72) in == (25.4/72) mm
		/// </remarks>
		public const double Pt = 96.0 / 72.0;

		/// <summary>
		/// Количество пикселей в пикселе.
		/// </summary>
		/// <remarks>
		/// 1 px == (72/96) pt == (1/96) in == (25.4/96) mm
		/// </remarks>
		public const double Px = 1.0;

		/// <summary>
		/// Количество пикселей в дюйме.
		/// </summary>
		/// <remarks>
		/// 1 in == 72 pt == 96 px == 25.4 mm
		/// </remarks>
		public const double In = 96.0;

		/// <summary>
		/// Количество пикселей в сантиметре.
		/// </summary>
		/// <remarks>
		/// 1 cm == (720/25.4) pt == (960/25.4) px == (10/25.4) in
		/// </remarks>
		public const double Cm = 960.0 / 25.4;

		/// <summary>
		/// Количество пикселей в миллиметре.
		/// </summary>
		/// <remarks>
		/// 1 mm == (72/25.4) pt == (96/25.4) px == (1/25.4) in
		/// </remarks>
		public const double Mm = 96.0 / 25.4;
	}
}
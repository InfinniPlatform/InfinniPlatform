namespace InfinniPlatform.FastReport.Templates.Font
{
	/// <summary>
	/// Степень растягивания шрифта по горизонтали.
	/// </summary>
	/// <remarks>
	/// Определяет степень растягивания шрифта по горизонтали относительно обычных пропорций шрифта. 
	/// Значения данного перечисления соответствуют спецификации шрифтов OpenType и спецификации
	/// атрибута font-stretch в CSS3.
	/// </remarks>
	public enum FontStretch
	{
		/// <summary>
		/// Ультра-уплотненный (50%).
		/// </summary>
		UltraCondensed = 1,

		/// <summary>
		/// Экстра-уплотненный (62.5%).
		/// </summary>
		ExtraCondensed = 2,

		/// <summary>
		/// Уплотненный (75.0%).
		/// </summary>
		Condensed = 3,

		/// <summary>
		/// Полууплотненный (87.5%).
		/// </summary>
		SemiCondensed = 4,

		/// <summary>
		/// Нормальный (100%).
		/// </summary>
		Normal = 5,

		/// <summary>
		/// Полурастянутый (112.5%).
		/// </summary>
		SemiExpanded = 6,

		/// <summary>
		/// Растянутый (125.0%).
		/// </summary>
		Expanded = 7,

		/// <summary>
		/// Экстра-растянутый (150.0%).
		/// </summary>
		ExtraExpanded = 8,

		/// <summary>
		/// Ультра-растянутый (200.0%).
		/// </summary>
		UltraExpanded = 9
	}
}
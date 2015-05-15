namespace InfinniPlatform.FastReport.Templates.Font
{
	/// <summary>
	/// Насыщенность шрифта.
	/// </summary>
	/// <remarks>
	/// Насыщенность шрифта определяет, насколько лекие (тонкие) или тяжелые (толстые) контуры будут у символов.
	/// Значения данного перечисления соответствуют спецификации шрифтов OpenType и спецификации атрибута
	/// font-weight в CSS3.
	/// </remarks>
	public enum FontWeight
	{
		/// <summary>
		/// Ультра-тонкий (100).
		/// </summary>
		UltraLight = 100,

		/// <summary>
		/// Экстра-тонкий (200).
		/// </summary>
		ExtraLight = 200,

		/// <summary>
		/// Тонкий (300).
		/// </summary>
		Light = 300,

		/// <summary>
		/// Нормальный (400).
		/// </summary>
		Normal = 400,

		/// <summary>
		/// Средний (500).
		/// </summary>
		Medium = 500,

		/// <summary>
		/// Полужирный (600).
		/// </summary>
		SemiBold = 600,

		/// <summary>
		/// Жирный (700).
		/// </summary>
		Bold = 700,

		/// <summary>
		/// Экстра-жирный (800).
		/// </summary>
		ExtraBold = 800,

		/// <summary>
		/// Ультра-жирный (900).
		/// </summary>
		UltraBold = 900
	}
}
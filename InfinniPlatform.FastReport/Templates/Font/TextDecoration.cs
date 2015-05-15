namespace InfinniPlatform.FastReport.Templates.Font
{
	/// <summary>
	/// Оформление текста.
	/// </summary>
	/// <remarks>
	/// Определяет оформление текста в виде его подчеркивания, перечеркивания, линии над текстом.
	/// Значения данного перечисления соответствуют спецификации атрибута text-decoration в CSS3.
	/// </remarks>
	public enum TextDecoration
	{
		/// <summary>
		/// Нет линий.
		/// </summary>
		Normal = 0,

		/// <summary>
		/// Линия над текстом.
		/// </summary>
		Overline = 1,

		/// <summary>
		/// Линия перечеркивает текст.
		/// </summary>
		LineThrough = 2,

		/// <summary>
		/// Линия под текстом.
		/// </summary>
		Underline = 4
	}
}
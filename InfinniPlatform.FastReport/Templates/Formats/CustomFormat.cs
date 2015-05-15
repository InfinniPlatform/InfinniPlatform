namespace InfinniPlatform.FastReport.Templates.Formats
{
	/// <summary>
	/// Пользовательский формат отображения.
	/// </summary>
	public sealed class CustomFormat : IFormat
	{
		/// <summary>
		/// Строка форматирования.
		/// </summary>
		/// <remarks>
		/// Возможно использование любой строки форматирования, которую поддерживает метод String.Format. Например,
		/// для форматирования числового значения может быть использована строка "N2", тогда будут отображены только
		/// два знака после запятой.
		/// </remarks>
		public string Format { get; set; }
	}
}
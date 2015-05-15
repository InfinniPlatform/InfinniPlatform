using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.Templates.Elements
{
	/// <summary>
	/// Элемент с возможностью форматирования значения.
	/// </summary>
	public interface IFormatElement
	{
		/// <summary>
		/// Формат отображения значения.
		/// </summary>
		IFormat Format { get; set; }
	}
}
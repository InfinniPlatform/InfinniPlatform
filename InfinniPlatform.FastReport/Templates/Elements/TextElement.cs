using InfinniPlatform.FastReport.Templates.Borders;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.Templates.Elements
{
	/// <summary>
	/// Текстовое поле.
	/// </summary>
	public sealed class TextElement : IElement, IFormatElement, IDataBindElement
	{
		/// <summary>
		/// Границы элемента.
		/// </summary>
		public Border Border { get; set; }

		/// <summary>
		/// Расположение элемента.
		/// </summary>
		public ElementLayout Layout { get; set; }

		/// <summary>
		/// Начертание текста.
		/// </summary>
		public TextElementStyle Style { get; set; }


		/// <summary>
		/// Формат отображения.
		/// </summary>
		public IFormat Format { get; set; }

		/// <summary>
		/// Привязка данных
		/// </summary>
		public IDataBind DataBind { get; set; }


		/// <summary>
		/// Может увеличиваться
		/// </summary>
		public bool CanGrow { get; set; }

		/// <summary>
		/// Может сокращаться
		/// </summary>
		public bool CanShrink { get; set; }
	}
}
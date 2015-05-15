using System;

using InfinniPlatform.FastReport.Templates.Borders;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.TemplatesFluent.Borders;
using InfinniPlatform.FastReport.TemplatesFluent.Data;
using InfinniPlatform.FastReport.TemplatesFluent.Formats;

namespace InfinniPlatform.FastReport.TemplatesFluent.Elements
{
	/// <summary>
	/// Интерфейс для настройки текстового поля.
	/// </summary>
	public sealed class TextElementConfig
	{
		internal TextElementConfig(TextElement textElement)
		{
			if (textElement == null)
			{
				throw new ArgumentNullException("textElement");
			}

			_textElement = textElement;
		}


		private readonly TextElement _textElement;


		/// <summary>
		/// Источник данных.
		/// </summary>
		public TextElementConfig Bind(Action<DataBindConfig> action)
		{
			var configuration = new DataBindConfig(dataBind => _textElement.DataBind = dataBind);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Начертание текста.
		/// </summary>
		public TextElementConfig Style(Action<TextElementStyleConfig> action)
		{
			if (_textElement.Style == null)
			{
				_textElement.Style = new TextElementStyle();
			}

			var configuration = new TextElementStyleConfig(_textElement.Style);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Формат отображения.
		/// </summary>
		public TextElementConfig Format(Action<FormatConfig> action)
		{
			var configuration = new FormatConfig(_textElement);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Границы элемента.
		/// </summary>
		public TextElementConfig Border(Action<BorderConfig> action)
		{
			if (_textElement.Border == null)
			{
				_textElement.Border = new Border();
			}

			var configuration = new BorderConfig(_textElement.Border);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Расположение элемента.
		/// </summary>
		public TextElementConfig Layout(Action<ElementLayoutConfig> action)
		{
			if (_textElement.Layout == null)
			{
				_textElement.Layout = new ElementLayout();
			}

			var configuration = new ElementLayoutConfig(_textElement.Layout);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Может увеличиваться.
		/// </summary>
		public TextElementConfig CanGrow()
		{
			_textElement.CanGrow = true;

			return this;
		}

		/// <summary>
		/// Может сокращаться.
		/// </summary>
		public TextElementConfig CanShrink()
		{
			_textElement.CanShrink = true;

			return this;
		}
	}
}
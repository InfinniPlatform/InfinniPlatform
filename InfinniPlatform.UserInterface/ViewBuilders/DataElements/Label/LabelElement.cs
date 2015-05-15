using System;
using System.Windows;
using System.Windows.Controls;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

using DataFormat = InfinniPlatform.UserInterface.ViewBuilders.DisplayFormats.DataFormat;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.Label
{
	/// <summary>
	/// Элемент представления для текстовой метки.
	/// </summary>
	public sealed class LabelElement : BaseElement<TextBlock>
	{
		public LabelElement(View view)
			: base(view)
		{
		}


		// Text

		public override void SetText(string value)
		{
			base.SetText(value);

			Control.InvokeControl(() =>
								  {
									  Control.Text = value;
								  });
		}


		// Format

		private DisplayFormats.DataFormat _format;

		/// <summary>
		/// Возвращает формат отображения данных.
		/// </summary>
		public DisplayFormats.DataFormat GetFormat()
		{
			return _format;
		}

		/// <summary>
		/// Устанавливает формат отображения данных.
		/// </summary>
		public void SetFormat(DataFormat value)
		{
			_format = value;

			UpdateText();
		}


		// Value

		private object _value;

		/// <summary>
		/// Возвращает значение.
		/// </summary>
		public object GetValue()
		{
			return _value;
		}

		/// <summary>
		/// Устанавливает значение.
		/// </summary>
		public void SetValue(object value)
		{
			_value = value;

			UpdateText();
		}


		private void UpdateText()
		{
			string text = null;

			var value = _value;

			if (value != null)
			{
				var format = _format;

				text = (format != null)
					? format.Format(value)
					: value.ToString();
			}

			SetText(text);
		}


		// TextAlign

		private HorizontalTextAlignment _horizontalTextAlignment;

		/// <summary>
		/// Возвращает способ выравнивания текста. 
		/// </summary>
		public HorizontalTextAlignment GetHorizontalTextAlignment()
		{
			return _horizontalTextAlignment;
		}

		/// <summary>
		/// Устанавливает способ выравнивания текста. 
		/// </summary>
		public void SetHorizontalTextAlignment(HorizontalTextAlignment value)
		{
			if (_horizontalTextAlignment != value)
			{
				_horizontalTextAlignment = value;

				switch (value)
				{
					case HorizontalTextAlignment.Left:
						Control.TextAlignment = TextAlignment.Left;
						break;
					case HorizontalTextAlignment.Right:
						Control.TextAlignment = TextAlignment.Right;
						break;
					case HorizontalTextAlignment.Center:
						Control.TextAlignment = TextAlignment.Center;
						break;
					case HorizontalTextAlignment.Justify:
						Control.TextAlignment = TextAlignment.Justify;
						break;
					default:
						Control.TextAlignment = TextAlignment.Left;
						break;
				}
			}
		}


		// LineCount

		private int _lineCount;

		/// <summary>
		/// Возвращает видимое количество строк.
		/// </summary>
		public int GetLineCount()
		{
			return _lineCount;
		}

		/// <summary>
		/// Устанавливает видимое количество строк.
		/// </summary>
		public void SetLineCount(int value)
		{
			var correct = Math.Max(value, 0);

			if (_lineCount != correct)
			{
				_lineCount = correct;

				Control.Height = (correct > 0) ? correct * Math.Ceiling(Control.FontSize * Control.FontFamily.LineSpacing * 96 / 72) : double.NaN;
			}
		}
	}
}
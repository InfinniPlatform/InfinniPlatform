using System;

using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.TemplatesFluent.Formats
{
	/// <summary>
	/// Интерфейс для настройки формата отображения.
	/// </summary>
	public sealed class FormatConfig
	{
		internal FormatConfig(IFormatElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			_element = element;
		}


		private readonly IFormatElement _element;


		/// <summary>
		/// Формат отображения даты.
		/// </summary>
		public DateFormatConfig Date()
		{
			var format = new DateFormat();

			_element.Format = format;

			return new DateFormatConfig(format);
		}

		/// <summary>
		/// Формат отображения времени.
		/// </summary>
		public TimeFormatConfig Time()
		{
			var format = new TimeFormat();

			_element.Format = format;

			return new TimeFormatConfig(format);
		}

		/// <summary>
		/// Формат отображения логического значения.
		/// </summary>
		public BooleanFormatConfig Boolean()
		{
			var format = new BooleanFormat();

			_element.Format = format;

			return new BooleanFormatConfig(format);
		}

		/// <summary>
		/// Формат отображения числового значения.
		/// </summary>
		public NumberFormatConfig Number()
		{
			var format = new NumberFormat();

			_element.Format = format;

			return new NumberFormatConfig(format);
		}

		/// <summary>
		/// Формат отображения денежных единицы.
		/// </summary>
		public CurrencyFormatConfig Currency()
		{
			var format = new CurrencyFormat();

			_element.Format = format;

			return new CurrencyFormatConfig(format);
		}

		/// <summary>
		/// Формат отображения процентов.
		/// </summary>
		public PercentFormatConfig Percent()
		{
			var format = new PercentFormat();

			_element.Format = format;

			return new PercentFormatConfig(format);
		}

		/// <summary>
		/// Пользовательский формат отображения.
		/// </summary>
		/// <param name="value">Строка форматирования.</param>
		/// <remarks>
		/// Возможно использование любой строки форматирования, которую поддерживает
		/// метод String.Format. Например, для форматирования числового значения может
		/// быть использована строка "N2", тогда будут отображены только два знака
		/// после запятой.
		/// </remarks>
		public void Custom(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException("value");
			}

			_element.Format = new CustomFormat { Format = value };
		}
	}
}
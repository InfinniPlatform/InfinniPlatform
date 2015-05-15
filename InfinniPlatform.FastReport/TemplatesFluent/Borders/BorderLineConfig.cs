using System;

using InfinniPlatform.FastReport.Templates.Borders;

namespace InfinniPlatform.FastReport.TemplatesFluent.Borders
{
	/// <summary>
	/// Интерфейс для настройки линии границы.
	/// </summary>
	public sealed class BorderLineConfig
	{
		internal BorderLineConfig(BorderLine line)
		{
			if (line == null)
			{
				throw new ArgumentNullException("line");
			}

			_line = line;
		}


		private readonly BorderLine _line;


		/// <summary>
		/// Толщина линии.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public BorderLineConfig Width(float value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException();
			}

			_line.Width = value;

			return this;
		}

		/// <summary>
		/// Цвет линии.
		/// </summary>
		public BorderLineConfig Color(string value)
		{
			_line.Color = value;

			return this;
		}

		/// <summary>
		/// Стиль линии.
		/// </summary>
		public BorderLineConfig Style(BorderLineStyle value)
		{
			_line.Style = value;

			return this;
		}
	}
}
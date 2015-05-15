using System;

using InfinniPlatform.FastReport.Templates.Print;

namespace InfinniPlatform.FastReport.TemplatesFluent.Print
{
	/// <summary>
	/// Интерфейс для настройки отступов на листе при печати.
	/// </summary>
	public sealed class PrintPaperMarginConfig
	{
		internal PrintPaperMarginConfig(PrintPaperMargin paperMargin)
		{
			if (paperMargin == null)
			{
				throw new ArgumentNullException("paperMargin");
			}

			_paperMargin = paperMargin;
		}


		private readonly PrintPaperMargin _paperMargin;


		/// <summary>
		/// Отступ слева.
		/// </summary>
		/// <param name="value">Значение отступа в миллиметрах.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public PrintPaperMarginConfig Left(float value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_paperMargin.Left = value;

			return this;
		}

		/// <summary>
		/// Отступ справа.
		/// </summary>
		/// <param name="value">Значение отступа в миллиметрах.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public PrintPaperMarginConfig Right(float value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_paperMargin.Right = value;

			return this;
		}

		/// <summary>
		/// Отступ сверху.
		/// </summary>
		/// <param name="value">Значение отступа в миллиметрах.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public PrintPaperMarginConfig Top(float value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_paperMargin.Top = value;

			return this;
		}

		/// <summary>
		/// Отступ снизу.
		/// </summary>
		/// <param name="value">Значение отступа в миллиметрах.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public PrintPaperMarginConfig Bottom(float value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_paperMargin.Bottom = value;

			return this;
		}


		/// <summary>
		/// Все отступы одинаковые.
		/// </summary>
		public PrintPaperMarginConfig All(float value)
		{
			return Left(value).Right(value).Top(value).Bottom(value);
		}


		/// <summary>
		/// Зеркально отражать отступы на нечетных страницах.
		/// </summary>
		public PrintPaperMarginConfig MirrorOnEvenPages()
		{
			_paperMargin.MirrorOnEvenPages = true;

			return this;
		}
	}
}
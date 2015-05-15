using System;

using InfinniPlatform.FastReport.Templates.Print;

namespace InfinniPlatform.FastReport.TemplatesFluent.Print
{
	/// <summary>
	/// Интерфейс для настройки формата листа бумаги.
	/// </summary>
	public sealed class PrintPaperConfig
	{
		internal PrintPaperConfig(PrintPaper paper)
		{
			if (paper == null)
			{
				throw new ArgumentNullException("paper");
			}

			_paper = paper;
		}


		private readonly PrintPaper _paper;


		/// <summary>
		/// Ширина (меньшая сторона) листа.
		/// </summary>
		/// <param name="value">Значение ширины в миллиметрах.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public PrintPaperConfig Width(float value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_paper.Width = value;

			return this;
		}

		/// <summary>
		/// Высота (большая сторона) листа.
		/// </summary>
		/// <param name="value">Значение высоты в миллиметрах.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public PrintPaperConfig Height(float value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_paper.Height = value;

			return this;
		}

		/// <summary>
		/// Портретная ориентация.
		/// </summary>
		public PrintPaperConfig Portrait()
		{
			_paper.Orientation = PaperOrientation.Portrait;

			return this;
		}

		/// <summary>
		/// Альбомная ориентация.
		/// </summary>
		public PrintPaperConfig Landscape()
		{
			_paper.Orientation = PaperOrientation.Landscape;

			return this;
		}
	}
}
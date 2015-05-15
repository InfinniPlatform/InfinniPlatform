using System;

using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.TemplatesFluent.Elements
{
	/// <summary>
	/// Интерфейс для настройки расположения элемента представления.
	/// </summary>
	public sealed class ElementLayoutConfig
	{
		internal ElementLayoutConfig(ElementLayout layout)
		{
			if (layout == null)
			{
				throw new ArgumentNullException("layout");
			}

			_layout = layout;
		}


		private readonly ElementLayout _layout;


		/// <summary>
		/// Отступ сверху.
		/// </summary>
		/// <param name="value">Отступ сверху в миллиметрах.</param>
		public ElementLayoutConfig Top(float value)
		{
			_layout.Top = value;

			return this;
		}

		/// <summary>
		/// Отступ слева.
		/// </summary>
		/// <param name="value">Отступ слева в миллиметрах.</param>
		public ElementLayoutConfig Left(float value)
		{
			_layout.Left = value;

			return this;
		}

		/// <summary>
		/// Ширина.
		/// </summary>
		/// <param name="value">Ширина в миллиметрах.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public ElementLayoutConfig Width(float value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_layout.Width = value;

			return this;
		}

		/// <summary>
		/// Высота.
		/// </summary>
		/// <param name="value">Высота в миллиметрах.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public ElementLayoutConfig Height(float value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_layout.Height = value;

			return this;
		}
	}
}
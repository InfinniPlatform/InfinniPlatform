using System;

using InfinniPlatform.FastReport.Templates.Borders;

namespace InfinniPlatform.FastReport.TemplatesFluent.Borders
{
	/// <summary>
	/// Интерфейс для настройки границ объекта.
	/// </summary>
	public sealed class BorderConfig
	{
		internal BorderConfig(Border border)
		{
			if (border == null)
			{
				throw new ArgumentNullException("border");
			}

			_border = border;
		}


		private readonly Border _border;


		/// <summary>
		/// Левая граница.
		/// </summary>
		public BorderConfig Left(Action<BorderLineConfig> action)
		{
			if (_border.Left == null)
			{
				_border.Left = new BorderLine();
			}

			return ConfigureBorderLine(_border.Left, action);
		}

		/// <summary>
		/// Правая граница.
		/// </summary>
		public BorderConfig Right(Action<BorderLineConfig> action)
		{
			if (_border.Right == null)
			{
				_border.Right = new BorderLine();
			}

			return ConfigureBorderLine(_border.Right, action);
		}

		/// <summary>
		/// Верхняя граница.
		/// </summary>
		public BorderConfig Top(Action<BorderLineConfig> action)
		{
			if (_border.Top == null)
			{
				_border.Top = new BorderLine();
			}

			return ConfigureBorderLine(_border.Top, action);
		}

		/// <summary>
		/// Нижняя граница.
		/// </summary>
		public BorderConfig Bottom(Action<BorderLineConfig> action)
		{
			if (_border.Bottom == null)
			{
				_border.Bottom = new BorderLine();
			}

			return ConfigureBorderLine(_border.Bottom, action);
		}


		/// <summary>
		/// Все границы.
		/// </summary>
		public BorderConfig All(Action<BorderLineConfig> action)
		{
			return Left(action).Right(action).Top(action).Bottom(action);
		}


		private BorderConfig ConfigureBorderLine(BorderLine line, Action<BorderLineConfig> action)
		{
			var configuration = new BorderLineConfig(line);
			action(configuration);
			return this;
		}
	}
}
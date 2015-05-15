using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.TemplatesFluent.Elements;

namespace InfinniPlatform.FastReport.TemplatesFluent.Bands
{
	/// <summary>
	/// Интерфейс для настройки содержимого блока отчета.
	/// </summary>
	public sealed class ReportBandElementsConfig
	{
		internal ReportBandElementsConfig(ICollection<IElement> elements)
		{
			if (elements == null)
			{
				throw new ArgumentNullException("elements");
			}

			_elements = elements;
		}


		private readonly ICollection<IElement> _elements;


		/// <summary>
		/// Текстовое поле.
		/// </summary>
		public ReportBandElementsConfig Text(Action<TextElementConfig> action)
		{
			var element = new TextElement();
			var configuration = new TextElementConfig(element);

			action(configuration);

			_elements.Add(element);

			return this;
		}

		/// <summary>
		/// Таблица.
		/// </summary>
		public ReportBandElementsConfig Table(Action<TableElementConfig> action)
		{
			var element = new TableElement();
			var configuration = new TableElementConfig(element);

			action(configuration);

			_elements.Add(element);

			return this;
		}
	}
}
using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Builders
{
	/// <summary>
	/// Контекст для построения элемента печатного представления.
	/// </summary>
	sealed class PrintElementBuildContext
	{
		/// <summary>
		/// Режим дизайнера.
		/// </summary>
		public bool IsDesignMode { get; set; }


		/// <summary>
		/// Данные печатного представления.
		/// </summary>
		public object PrintViewSource { get; set; }

		/// <summary>
		/// Список стилей печатного представления.
		/// </summary>
		public IDictionary<string, object> PrintViewStyles { get; set; }

		/// <summary>
		/// Построитель элементов печатного представления.
		/// </summary>
		public PrintElementBuilder ElementBuilder { get; set; }

		/// <summary>
		/// Соответствие между элементами печатного представления и метаданными.
		/// </summary>
		public PrintElementMetadataMap ElementMetadataMap { get; set; }


		/// <summary>
		/// Выражение над данными элемента печатного представления.
		/// </summary>
		public string ElementSourceExpression { get; set; }

		/// <summary>
		/// Свойство данных элемента печатного представления.
		/// </summary>
		public string ElementSourceProperty { get; set; }

		/// <summary>
		/// Значение данных элемента печатного представления.
		/// </summary>
		public object ElementSourceValue { get; set; }

		/// <summary>
		/// Стиль элемента печатного представления.
		/// </summary>
		public object ElementStyle { get; set; }

		/// <summary>
		/// Ширина элемента печатного представления.
		/// </summary>
		public double ElementWidth { get; set; }


		/// <summary>
		/// Находит и возвращает стиль печатного представления.
		/// </summary>
		public object FindStyle(object styleName)
		{
			object styleObject = null;

			if (PrintViewStyles != null)
			{
				string styleNameString;

				if (ConvertHelper.TryToNormString(styleName, out styleNameString))
				{
					PrintViewStyles.TryGetValue(styleNameString, out styleObject);
				}
			}

			return styleObject;
		}


		/// <summary>
		/// Установливает соответствие между элементом и его метаданными.
		/// </summary>
		public void MapElement(object element, object elementMetadata)
		{
			if (element != null && ElementMetadataMap != null)
			{
				ElementMetadataMap.Map(element, elementMetadata);
			}
		}


		/// <summary>
		/// Создает контекст для построения элемента печатного представления. 
		/// </summary>
		public PrintElementBuildContext Create(double elementWidth)
		{
			return new PrintElementBuildContext
				   {
					   IsDesignMode = IsDesignMode,
					   PrintViewSource = PrintViewSource,
					   PrintViewStyles = PrintViewStyles,
					   ElementBuilder = ElementBuilder,
					   ElementMetadataMap = ElementMetadataMap,
					   ElementSourceExpression = ElementSourceExpression,
					   ElementSourceProperty = ElementSourceProperty,
					   ElementSourceValue = ElementSourceValue,
					   ElementStyle = ElementStyle,
					   ElementWidth = elementWidth
				   };
		}


		/// <summary>
		/// Создает копию контекста для построения элемента печатного представления. 
		/// </summary>
		public PrintElementBuildContext Clone()
		{
			return new PrintElementBuildContext
				   {
					   IsDesignMode = IsDesignMode,
					   PrintViewSource = PrintViewSource,
					   PrintViewStyles = PrintViewStyles,
					   ElementBuilder = ElementBuilder,
					   ElementMetadataMap = ElementMetadataMap,
					   ElementSourceExpression = ElementSourceExpression,
					   ElementSourceProperty = ElementSourceProperty,
					   ElementSourceValue = ElementSourceValue,
					   ElementStyle = ElementStyle,
					   ElementWidth = ElementWidth
				   };
		}
	}
}
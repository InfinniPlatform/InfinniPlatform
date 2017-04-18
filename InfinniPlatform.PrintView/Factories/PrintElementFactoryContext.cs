using System.Collections.Generic;

using InfinniPlatform.PrintView.Abstractions;

namespace InfinniPlatform.PrintView.Factories
{
    /// <summary>
    /// Контекст элемента.
    /// </summary>
    internal class PrintElementFactoryContext
    {
        /// <summary>
        /// Режим редактора.
        /// </summary>
        public bool IsDesignMode { get; set; }

        /// <summary>
        /// Данные документа.
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// Список стилей.
        /// </summary>
        public IDictionary<string, PrintStyle> Styles { get; set; }


        /// <summary>
        /// Выражение над данными элемента.
        /// </summary>
        public string ElementSourceExpression { get; set; }

        /// <summary>
        /// Свойство данных элемента.
        /// </summary>
        public string ElementSourceProperty { get; set; }

        /// <summary>
        /// Значение данных элемента.
        /// </summary>
        public object ElementSourceValue { get; set; }

        /// <summary>
        /// Ширина элемента.
        /// </summary>
        public double ElementWidth { get; set; }

        /// <summary>
        /// Стиль элемента.
        /// </summary>
        public PrintStyle ElementStyle { get; set; }


        /// <summary>
        /// Универсальная фабрика для создания элементов.
        /// </summary>
        public PrintElementBuilder Factory { get; set; }

        /// <summary>
        /// Соответствие между элементами и шаблонами.
        /// </summary>
        public PrintDocumentMap DocumentMap { get; set; }


        /// <summary>
        /// Находит и возвращает стиль.
        /// </summary>
        public PrintStyle FindStyle(string styleName)
        {
            PrintStyle style = null;

            if (!string.IsNullOrEmpty(styleName))
            {
                Styles?.TryGetValue(styleName, out style);
            }

            return style;
        }


        /// <summary>
        /// Устанавливает соответствие между элементом и его шаблоном.
        /// </summary>
        public void MapElement(object element, object elementMetadata)
        {
            if (element != null)
            {
                DocumentMap?.Map(element, elementMetadata);
            }
        }


        /// <summary>
        /// Создает контекст для построения элемента печатного представления.
        /// </summary>
        public PrintElementFactoryContext Create(double elementWidth)
        {
            return new PrintElementFactoryContext
            {
                IsDesignMode = IsDesignMode,
                Source = Source,
                Styles = Styles,
                Factory = Factory,
                DocumentMap = DocumentMap,
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
        public PrintElementFactoryContext Clone()
        {
            return new PrintElementFactoryContext
            {
                IsDesignMode = IsDesignMode,
                Source = Source,
                Styles = Styles,
                Factory = Factory,
                DocumentMap = DocumentMap,
                ElementSourceExpression = ElementSourceExpression,
                ElementSourceProperty = ElementSourceProperty,
                ElementSourceValue = ElementSourceValue,
                ElementStyle = ElementStyle,
                ElementWidth = ElementWidth
            };
        }
    }
}
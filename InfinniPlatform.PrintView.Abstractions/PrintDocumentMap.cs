using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Abstractions
{
    /// <summary>
    /// Хранит соответствие между элементами документа печатного представления <see cref="PrintDocument"/> и их шаблонами.
    /// </summary>
    public class PrintDocumentMap
    {
        private readonly Dictionary<object, object> _elementToMetadata
            = new Dictionary<object, object>();

        private readonly Dictionary<object, object> _templateToElement
            = new Dictionary<object, object>();


        /// <summary>
        /// Устанавливает соответствие между элементом и шаблоном.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="template">Шаблон.</param>
        public void Map(object element, object template)
        {
            if (element != null)
            {
                _elementToMetadata[element] = template;
            }

            if (template != null)
            {
                _templateToElement[template] = element;
            }
        }


        /// <summary>
        /// Возвращает элемент, созданный на базе указанного шаблона.
        /// </summary>
        /// <param name="template">Шаблон.</param>
        public object GetElement(object template)
        {
            object element = null;

            if (template != null)
            {
                _templateToElement.TryGetValue(template, out element);
            }

            return element;
        }

        /// <summary>
        /// Возвращает шаблон, по которому был создан указанный элемент.
        /// </summary>
        /// <param name="element">Элемент.</param>
        public object GetTemplate(object element)
        {
            object template = null;

            if (element != null)
            {
                _elementToMetadata.TryGetValue(element, out template);
            }

            return template;
        }
    }
}
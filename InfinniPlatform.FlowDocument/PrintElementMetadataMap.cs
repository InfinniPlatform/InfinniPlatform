using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument
{
    /// <summary>
    ///     Соответствие между элементами печатного представления и метаданными.
    /// </summary>
    public sealed class PrintElementMetadataMap
    {
        private readonly Dictionary<object, object> _elementToMetadata
            = new Dictionary<object, object>();

        private readonly Dictionary<object, object> _metadataToElement
            = new Dictionary<object, object>();

        public void Map(object element, object metadata)
        {
            if (element != null)
            {
                _elementToMetadata[element] = metadata;
            }

            if (metadata != null)
            {
                _metadataToElement[metadata] = element;
            }
        }

        public object GetElement(object metadata)
        {
            object element = null;

            if (metadata != null)
            {
                _metadataToElement.TryGetValue(metadata, out element);
            }

            return element;
        }

        public object GetMetadata(object element)
        {
            object metadata = null;

            if (element != null)
            {
                _elementToMetadata.TryGetValue(element, out metadata);
            }

            return metadata;
        }
    }
}
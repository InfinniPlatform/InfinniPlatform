using System;
using System.Collections;
using System.Collections.Generic;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree
{
    internal sealed class ConfigElementNodeBuilder
    {
        private readonly string _server;
        private readonly int _port;

        private readonly Dictionary<string, IConfigElementNodeFactory> _factories
            = new Dictionary<string, IConfigElementNodeFactory>(StringComparer.OrdinalIgnoreCase);


        public ConfigElementNodeBuilder(string server, int port)
        {
            _server = server;
            _port = port;
        }

        public IConfigElementEditPanel EditPanel { get; set; }

        public int Port
        {
            get { return _port; }
        }

        public string Server
        {
            get { return _server; }
        }

        public void Register(string elementType, IConfigElementNodeFactory elementFactory)
        {
            if (string.IsNullOrEmpty(elementType))
            {
                throw new ArgumentNullException("elementType");
            }

            if (elementFactory == null)
            {
                throw new ArgumentNullException("elementFactory");
            }

            _factories.Add(elementType, elementFactory);
        }

        public void BuildElement(ICollection<ConfigElementNode> elements, ConfigElementNode elementParent,
            dynamic elementMetadata, string elementType)
        {
            if (!ReferenceEquals(elementMetadata, null))
            {
                IConfigElementNodeFactory factory;

                if (_factories.TryGetValue(elementType, out factory))
                {
                    var element = new ConfigElementNode(elementParent, elementType, elementMetadata);

                    if (elementParent != null)
                    {
                        element.ConfigId = elementParent.ConfigId;
                        element.DocumentId = elementParent.DocumentId;
                        element.Version = elementParent.Version;
                    }

                    elements.Add(element);

                    if (elementParent != null)
                    {
                        elementParent.Nodes.Add(element);
                    }

                    factory.Create(this, elements, element);
                }
            }
        }

        public void BuildElements(ICollection<ConfigElementNode> elements, ConfigElementNode elementParent,
            IEnumerable elementMetadata, string elementType)
        {
            if (!ReferenceEquals(elementMetadata, null))
            {
                foreach (var itemMetadata in elementMetadata)
                {
                    BuildElement(elements, elementParent, itemMetadata, elementType);
                }
            }
        }
    }
}
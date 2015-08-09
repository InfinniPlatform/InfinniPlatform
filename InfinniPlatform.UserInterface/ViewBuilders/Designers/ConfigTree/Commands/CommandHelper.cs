using System.Collections.Generic;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.Data.DataProviders;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands
{
    internal static class CommandHelper
    {
        public static ConfigElementNode Clipboard { get; set; }

        public static IDataProvider GetMetadataProvider(ConfigElementNode elementNode, string elementType, string server, int port)
        {
            var metadataProvider = new MetadataProvider(elementType, server, port);
            metadataProvider.SetConfigId(elementNode.ConfigId);
            metadataProvider.SetDocumentId(elementNode.DocumentId);
            metadataProvider.SetVersion(elementNode.Version);
            return metadataProvider;
        }

        public static void RemoveNode(ICollection<ConfigElementNode> elements, ConfigElementNode elementNode)
        {
            RemoveChildNodes(elements, elementNode);
            elements.Remove(elementNode);

            if (elementNode.Parent != null)
            {
                elementNode.Parent.Nodes.Remove(elementNode);
            }
        }

        public static void RemoveChildNodes(ICollection<ConfigElementNode> elements, ConfigElementNode elementNode)
        {
            foreach (var childNode in elementNode.Nodes)
            {
                RemoveChildNodes(elements, childNode);
                elements.Remove(childNode);
            }

            elementNode.Nodes.Clear();
        }
    }
}
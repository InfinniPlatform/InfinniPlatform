using System.Collections.Generic;
using System.Configuration;
using System.IO;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
    internal sealed class ConfigContainerNodeFactory : IConfigElementNodeFactory
    {
        public const string ElementType = MetadataType.Configuration + "Container";

        private static readonly string[] ElementChildrenTypes =
        {
            MetadataType.Configuration
        };

        public void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements,
            ConfigElementNode elementNode)
        {
            var contentDirectory = ConfigurationManager.AppSettings["ContentDirectory"];

            if (string.IsNullOrEmpty(contentDirectory))
            {
                contentDirectory = Path.Combine("..", "Assemblies", "content");
            }

            var contentPath = Path.GetFullPath(contentDirectory);

            elementNode.ElementId = contentPath;
            elementNode.ElementName = contentPath;
            elementNode.Version = builder.Version;

            elementNode.RefreshCommand = new RefreshElementCommand(builder, elements, elementNode, MetadataType.Configuration);
            elementNode.RegisterAddCommands(builder, ElementChildrenTypes);
            elementNode.EditCommands = new[] { new ReconnectCommand(builder, elementNode, "EditView") { Text = Resources.Reconnect, Image = ElementType } };
            elementNode.DeleteCommand = FactoryHelper.NoDeleteCommand;
            elementNode.CopyCommand = FactoryHelper.NoCopyCommand;
            elementNode.PasteCommand = FactoryHelper.NoPasteCommand;

            elementNode.RefreshCommand.TryExecute();
        }
    }
}
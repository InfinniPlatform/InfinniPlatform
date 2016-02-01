using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
    internal sealed class DocumentContainerNodeFactory : IConfigElementNodeFactory
    {
        public const string ElementType = MetadataType.Document + "Container";

        private static readonly string[] ElementChildrenTypes =
        {
            MetadataType.Document
        };

        public void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements,
            ConfigElementNode elementNode)
        {
            elementNode.ElementName = Resources.DocumentContainer;

            elementNode.RefreshCommand = new RefreshElementCommand(builder, elements, elementNode, MetadataType.Document);
            elementNode.RegisterAddCommands(builder, ElementChildrenTypes);
            elementNode.DeleteCommand = FactoryHelper.NoDeleteCommand;
            elementNode.CopyCommand = FactoryHelper.NoCopyCommand;
            elementNode.PasteCommand = FactoryHelper.NoPasteCommand;

            FactoryHelper.AddEmptyElement(elements, elementNode);

			elementNode.RefreshCommand.TryExecute();
        }
    }
}
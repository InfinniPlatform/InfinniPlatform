using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
    internal sealed class ValidationWarningContainerNodeFactory : IConfigElementNodeFactory
    {
        public const string ElementType = MetadataType.ValidationWarning + "Container";

        private static readonly string[] ElementChildrenTypes =
        {
            MetadataType.ValidationWarning
        };

        public void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements,
            ConfigElementNode elementNode)
        {
            elementNode.ElementName = Resources.ValidationWarningContainer;

            elementNode.RefreshCommand = new RefreshElementCommand(builder, elements, elementNode,
                MetadataType.ValidationWarning);
            elementNode.RegisterAddCommands(builder, ElementChildrenTypes);
            elementNode.DeleteCommand = FactoryHelper.NoDeleteCommand;
            elementNode.CopyCommand = FactoryHelper.NoCopyCommand;
            elementNode.RegisterPasteCommand(builder);

            FactoryHelper.AddEmptyElement(elements, elementNode);
        }
    }
}
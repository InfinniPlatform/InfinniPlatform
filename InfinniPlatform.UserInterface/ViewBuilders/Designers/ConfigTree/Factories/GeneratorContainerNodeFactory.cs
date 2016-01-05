using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
    internal sealed class GeneratorContainerNodeFactory : IConfigElementNodeFactory
    {
        public const string ElementType = MetadataType.Generator + "Container";

        private static readonly string[] ElementChildrenTypes =
        {
            MetadataType.Generator
        };

        public void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements,
            ConfigElementNode elementNode)
        {
            elementNode.ElementName = Resources.GeneratorContainer;

            elementNode.RefreshCommand = new RefreshElementCommand(builder, elements, elementNode,
                MetadataType.Generator);
            elementNode.RegisterAddCommands(builder, ElementChildrenTypes);
            elementNode.DeleteCommand = FactoryHelper.NoDeleteCommand;
            elementNode.CopyCommand = FactoryHelper.NoCopyCommand;
            elementNode.RegisterPasteCommand(builder);

            FactoryHelper.AddEmptyElement(elements, elementNode);
        }
    }
}
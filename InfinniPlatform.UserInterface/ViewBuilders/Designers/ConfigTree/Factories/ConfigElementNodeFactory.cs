using System.Collections.Generic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
    internal sealed class ConfigElementNodeFactory : IConfigElementNodeFactory
    {
        public const string ElementType = MetadataType.Configuration;

        private static readonly string[] ElementChildrenTypes =
        {
            MetadataType.Menu,
            MetadataType.Document,
            MetadataType.Assembly,
            MetadataType.Register,
            MetadataType.Report
        };

        public void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements,
            ConfigElementNode elementNode)
        {
            elementNode.ConfigId = elementNode.ElementMetadata.Name;
            elementNode.Version = elementNode.ElementMetadata.Version;
            elementNode.ElementId = FactoryHelper.BuildId(elementNode);
            elementNode.ElementName = FactoryHelper.BuildName(elementNode);

            elementNode.RefreshCommand = new RefreshContainerCommand(elementNode);
            elementNode.RegisterAddCommands(builder, ElementChildrenTypes);
            elementNode.RegisterEditCommand(builder);
            elementNode.RegisterEditCommand(builder, "DeployView", Resources.DeployView, "Deploy");
            elementNode.RegisterEditCommand(builder, "MigrationView", Resources.MigrationView, "Migration");
            elementNode.DeleteCommand = new DeleteElementCommand(builder, elements, elementNode);
            elementNode.CopyCommand = FactoryHelper.NoCopyCommand;
            elementNode.PasteCommand = FactoryHelper.NoPasteCommand;

            builder.BuildElement(elements, elementNode, elementNode.ElementMetadata,
                MenuContainerNodeFactory.ElementType);
            builder.BuildElement(elements, elementNode, elementNode.ElementMetadata,
                DocumentContainerNodeFactory.ElementType);
            builder.BuildElement(elements, elementNode, elementNode.ElementMetadata,
                AssemblyContainerNodeFactory.ElementType);
            builder.BuildElement(elements, elementNode, elementNode.ElementMetadata,
                RegisterContainerNodeFactory.ElementType);
            builder.BuildElement(elements, elementNode, elementNode.ElementMetadata,
                ReportContainerNodeFactory.ElementType);
        }
    }
}
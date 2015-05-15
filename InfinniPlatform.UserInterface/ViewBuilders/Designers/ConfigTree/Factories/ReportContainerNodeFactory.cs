using System.Collections.Generic;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
	sealed class ReportContainerNodeFactory : IConfigElementNodeFactory
	{
		public const string ElementType = MetadataType.Report + "Container";

		private static readonly string[] ElementChildrenTypes =
		{
			MetadataType.Report
		};

		public void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements, ConfigElementNode elementNode)
		{
			elementNode.ElementName = Resources.ReportContainer;

			elementNode.RefreshCommand = new RefreshElementCommand(builder, elements, elementNode, MetadataType.Report);
			elementNode.RegisterAddCommands(builder, ElementChildrenTypes);
			elementNode.DeleteCommand = FactoryHelper.NoDeleteCommand;
			elementNode.CopyCommand = FactoryHelper.NoCopyCommand;
			elementNode.RegisterPasteCommand(builder);

			FactoryHelper.AddEmptyElement(elements, elementNode);
		}
	}
}
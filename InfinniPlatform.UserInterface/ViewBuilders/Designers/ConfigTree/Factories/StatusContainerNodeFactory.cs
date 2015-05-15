using System.Collections.Generic;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
	sealed class StatusContainerNodeFactory : IConfigElementNodeFactory
	{
		public const string ElementType = MetadataType.Status + "Container";

		private static readonly string[] ElementChildrenTypes =
		{
			MetadataType.Status
		};

		public void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements, ConfigElementNode elementNode)
		{
			elementNode.ElementName = Resources.StatusContainer;

			elementNode.RefreshCommand = new RefreshElementCommand(builder, elements, elementNode, MetadataType.Status);
			elementNode.RegisterAddCommands(builder, ElementChildrenTypes);
			elementNode.DeleteCommand = FactoryHelper.NoDeleteCommand;
			elementNode.CopyCommand = FactoryHelper.NoCopyCommand;
			elementNode.RegisterPasteCommand(builder);

			FactoryHelper.AddEmptyElement(elements, elementNode);
		}
	}
}
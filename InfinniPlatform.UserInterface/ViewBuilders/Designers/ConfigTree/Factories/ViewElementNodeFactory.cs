using System.Collections.Generic;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
	sealed class ViewElementNodeFactory : IConfigElementNodeFactory
	{
		public const string ElementType = MetadataType.View;

		public void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements, ConfigElementNode elementNode)
		{
			elementNode.ElementId = FactoryHelper.BuildId(elementNode);
			elementNode.ElementName = FactoryHelper.BuildName(elementNode);

			elementNode.RefreshCommand = FactoryHelper.NoRefreshCommand;
			elementNode.RegisterEditCommand(builder);
			elementNode.RegisterEditCommand(builder, "DesignerView", Resources.DesignerView, "Designer");
			elementNode.DeleteCommand = new DeleteElementCommand(builder, elements, elementNode);
			elementNode.RegisterCopyCommand();
			elementNode.PasteCommand = FactoryHelper.NoPasteCommand;
		}
	}
}
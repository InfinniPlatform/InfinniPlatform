﻿using System.Collections.Generic;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
	sealed class ValidationErrorContainerNodeFactory : IConfigElementNodeFactory
	{
		public const string ElementType = MetadataType.ValidationError + "Container";

		private static readonly string[] ElementChildrenTypes =
		{
			MetadataType.ValidationError
		};

		public void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements, ConfigElementNode elementNode)
		{
			elementNode.ElementName = Resources.ValidationErrorContainer;

			elementNode.RefreshCommand = new RefreshElementCommand(builder, elements, elementNode, MetadataType.ValidationError);
			elementNode.RegisterAddCommands(builder, ElementChildrenTypes);
			elementNode.DeleteCommand = FactoryHelper.NoDeleteCommand;
			elementNode.CopyCommand = FactoryHelper.NoCopyCommand;
			elementNode.RegisterPasteCommand(builder);

			FactoryHelper.AddEmptyElement(elements, elementNode);
		}
	}
}
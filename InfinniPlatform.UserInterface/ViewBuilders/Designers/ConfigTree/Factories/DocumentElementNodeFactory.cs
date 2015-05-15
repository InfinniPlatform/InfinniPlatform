﻿using System.Collections.Generic;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
	sealed class DocumentElementNodeFactory : IConfigElementNodeFactory
	{
		public const string ElementType = MetadataType.Document;

		private static readonly string[] ElementChildrenTypes =
		{
			MetadataType.View,
			MetadataType.PrintView,
			MetadataType.ValidationError,
			MetadataType.ValidationWarning,
			MetadataType.Scenario,
			MetadataType.Process,
			MetadataType.Service,
			MetadataType.Generator,
			MetadataType.Status
		};

		public void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements, ConfigElementNode elementNode)
		{
			elementNode.DocumentId = elementNode.ElementMetadata.Name;
			elementNode.ElementId = FactoryHelper.BuildId(elementNode);
			elementNode.ElementName = FactoryHelper.BuildName(elementNode);

			elementNode.RefreshCommand = new RefreshContainerCommand(elementNode);
			elementNode.RegisterAddCommands(builder, ElementChildrenTypes);
			elementNode.RegisterEditCommand(builder);
			elementNode.DeleteCommand = new DeleteElementCommand(builder, elements, elementNode);
			elementNode.CopyCommand = FactoryHelper.NoCopyCommand;
			elementNode.RegisterPasteCommand(builder);

			builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, ViewContainerNodeFactory.ElementType);
			builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, PrintViewContainerNodeFactory.ElementType);
			builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, ValidationErrorContainerNodeFactory.ElementType);
			builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, ValidationWarningContainerNodeFactory.ElementType);
			builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, ScenarioContainerNodeFactory.ElementType);
			builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, ProcessContainerNodeFactory.ElementType);
			builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, ServiceContainerNodeFactory.ElementType);
			builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, GeneratorContainerNodeFactory.ElementType);
			builder.BuildElement(elements, elementNode, elementNode.ElementMetadata, StatusContainerNodeFactory.ElementType);
		}
	}
}
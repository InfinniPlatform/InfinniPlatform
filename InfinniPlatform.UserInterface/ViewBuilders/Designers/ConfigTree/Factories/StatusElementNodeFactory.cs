﻿using System.Collections.Generic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
    internal sealed class StatusElementNodeFactory : IConfigElementNodeFactory
    {
        public const string ElementType = MetadataType.Status;

        public void Create(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements,
            ConfigElementNode elementNode)
        {
            elementNode.ElementId = FactoryHelper.BuildId(elementNode);
            elementNode.ElementName = FactoryHelper.BuildName(elementNode);

            elementNode.RefreshCommand = FactoryHelper.NoRefreshCommand;
            elementNode.RegisterEditCommand(builder);
            elementNode.RegisterEditCommand(builder, "MetadataView", Resources.MetadataView, "Metadata");
            elementNode.DeleteCommand = new DeleteElementCommand(builder, elements, elementNode);
            elementNode.RegisterCopyCommand();
            elementNode.PasteCommand = FactoryHelper.NoPasteCommand;
        }
    }
}
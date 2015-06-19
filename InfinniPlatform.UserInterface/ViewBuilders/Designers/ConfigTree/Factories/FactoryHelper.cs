using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Commands;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Factories
{
    internal static class FactoryHelper
    {
        public static readonly ICommand<bool> NoRefreshCommand = EmptyCommand<bool>.Instance;
        public static readonly ICommand<object> NoDeleteCommand = EmptyCommand<object>.Instance;
        public static readonly ICommand<object> NoCopyCommand = EmptyCommand<object>.Instance;
        public static readonly ICommand<object> NoPasteCommand = EmptyCommand<object>.Instance;

        private static readonly Dictionary<string, string> MetadataTypeNames
            = new Dictionary<string, string>
            {
                {MetadataType.Configuration, Resources.MetadataTypeConfiguration},
                {MetadataType.Menu, Resources.MetadataTypeMenu},
                {MetadataType.Document, Resources.MetadataTypeDocument},
                {MetadataType.Register, Resources.MetadataTypeRegister},
                {MetadataType.Assembly, Resources.MetadataTypeAssembly},
                {MetadataType.View, Resources.MetadataTypeView},
                {MetadataType.PrintView, Resources.MetadataTypePrintView},
                {MetadataType.ValidationError, Resources.MetadataTypeValidationError},
                {MetadataType.ValidationWarning, Resources.MetadataTypeValidationWarning},
                {MetadataType.Scenario, Resources.MetadataTypeScenario},
                {MetadataType.Process, Resources.MetadataTypeProcess},
                {MetadataType.Service, Resources.MetadataTypeService},
                {MetadataType.Generator, Resources.MetadataTypeGenerator},
                {MetadataType.Report, Resources.MetadataTypeReport},
                {MetadataType.Status, Resources.MetadataTypeStatus}
            };

        public static void AddEmptyElement(ICollection<ConfigElementNode> elements, ConfigElementNode elementParent)
        {
            var emptyElement = new ConfigElementNode(elementParent, null, null) {ElementName = Resources.Loading};
            elementParent.Nodes.Add(emptyElement);
            elements.Add(emptyElement);
        }

        public static string BuildId(ConfigElementNode elementNode)
        {
            var elementMetadata = elementNode.ElementMetadata;

            var name = ((elementMetadata.Name as string) ?? String.Empty).Trim();

            return name;
        }

        public static string BuildName(ConfigElementNode elementNode)
        {
            var elementMetadata = elementNode.ElementMetadata;

            var name = ((elementMetadata.Name as string) ?? String.Empty).Trim();
            var caption = ((elementMetadata.Caption as string) ?? String.Empty).Trim();

            if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(caption))
            {
                if (name != caption)
                {
                    return String.Format("{0} ({1})", name, caption);
                }

                return name;
            }

            if (!String.IsNullOrEmpty(caption))
            {
                return caption;
            }

            return name;
        }

        public static void RegisterAddCommands(this ConfigElementNode elementNode, ConfigElementNodeBuilder builder,
            string[] elementChildrenTypes)
        {
            elementNode.ElementChildrenTypes = elementChildrenTypes;

            foreach (var elementType in elementChildrenTypes)
            {
                elementNode.RegisterAddCommand(builder, elementType);
            }
        }

        private static void RegisterAddCommand(this ConfigElementNode elementNode, ConfigElementNodeBuilder builder,
            string elementType, string elementEditor = null, string commandText = null, string commandImage = null)
        {
            if (elementNode.AddCommands == null)
            {
                elementNode.AddCommands = new List<ICommand<object>>();
            }

            if (string.IsNullOrEmpty(elementEditor))
            {
                elementEditor = "EditView";
            }

            if (string.IsNullOrEmpty(commandText))
            {
                MetadataTypeNames.TryGetValue(elementType, out commandText);
            }

            if (string.IsNullOrEmpty(commandImage))
            {
                commandImage = elementType;
            }

            elementNode.AddCommands.AddItem(new AddElementCommand(builder, elementNode, elementEditor, elementType)
            {
                Text = commandText,
                Image = commandImage
            });
        }

        public static void RegisterEditCommand(this ConfigElementNode elementNode, ConfigElementNodeBuilder builder,
            string elementEditor = null, string commandText = null, string commandImage = null)
        {
            if (elementNode.EditCommands == null)
            {
                elementNode.EditCommands = new List<ICommand<object>>();
            }

            if (string.IsNullOrEmpty(elementEditor))
            {
                elementEditor = "EditView";
            }

            if (string.IsNullOrEmpty(commandText))
            {
                commandText = Resources.Edit;
            }

            if (string.IsNullOrEmpty(commandImage))
            {
                commandImage = "Actions/Edit";
            }

            elementNode.EditCommands.AddItem(new EditElementCommand(builder, elementNode, elementEditor)
            {
                Text = commandText,
                Image = commandImage
            });
        }

        public static void RegisterCopyCommand(this ConfigElementNode elementNode)
        {
            elementNode.CopyCommand = new CopyElementCommand(elementNode);
        }

        public static void RegisterPasteCommand(this ConfigElementNode elementNode, ConfigElementNodeBuilder builder,
            string elementEditor = null)
        {
            if (string.IsNullOrEmpty(elementEditor))
            {
                elementEditor = "EditView";
            }

            elementNode.PasteCommand = new PasteElementCommand(builder, elementNode, elementEditor);
        }
    }
}
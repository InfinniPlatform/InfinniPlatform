using System.Collections;
using InfinniPlatform.UserInterface.Services.Metadata;
using InfinniPlatform.UserInterface.ViewBuilders.Actions;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.MenuBar
{
    internal sealed class MenuBarElementBuilder : IObjectBuilder
    {
        public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
        {
            var menuBar = new MenuBarElement(parent,
                () => GetMenuListMetadata(metadata.Version, metadata.ConfigId),
                menuItem => ExecuteMenuItemAction(context, parent, menuItem));

            menuBar.ApplyElementMeatadata((object) metadata);

            return menuBar;
        }

        private IEnumerable GetMenuListMetadata(string version, string configId)
        {
            var menuMetadataService = new MenuMetadataService(version, configId);
            return menuMetadataService.GetItems();
        }

        private static void ExecuteMenuItemAction(ObjectBuilderContext context, View parent, dynamic menuItemMetadata)
        {
            BaseAction action = context.Build(parent, menuItemMetadata.Action);

            if (action != null)
            {
                action.Execute();
            }
        }
    }
}
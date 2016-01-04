using System.Collections.Generic;

using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Administration.Menu.ActionUnits
{
	public sealed class ActionUnitDeleteMenu
	{
		public void Action(IApplyContext target)
		{
			var isDeleted = false;
			var editMenuItem = target.Item.Document;

			if (editMenuItem != null)
			{
				MenuCache.EnterWrite();

				try
				{
					isDeleted = DeleteMenuItem(editMenuItem);

					MenuCache.ExitWrite(!isDeleted);
				}
				catch
				{
					MenuCache.ExitWrite(false);
				}
			}

			target.Result = isDeleted;
		}

		private static bool DeleteMenuItem(dynamic editMenuItem)
		{
			dynamic editMenu;
			dynamic parentMenuItem;

			if (FindMenuItem(editMenuItem, out editMenu, out parentMenuItem))
			{
				var menuItems = (parentMenuItem.Items == null) ? new List<object>() : new List<object>(parentMenuItem.Items);

				var exists = false;
				var subItemIndex = 0;

				foreach (dynamic subItemInfo in menuItems)
				{
					var subItemId = MenuHelper.GetMenuItemId(editMenuItem.ConfigId, editMenuItem.MenuId, editMenuItem.Level, subItemIndex, editMenuItem.ParentId);

					if (subItemId == editMenuItem.Id)
					{
						exists = true;

						ObjectHelper.RemoveItem(menuItems, subItemInfo);

						break;
					}

					++subItemIndex;
				}

				if (exists)
				{
					parentMenuItem.Items = menuItems;

					// Сохранение меню
					var menuManager = new ManagerFactoryConfiguration(editMenuItem.ConfigId).BuildMenuManager();
					menuManager.MergeItem(editMenu);

					// Сохранение ролей
					MenuHelper.DeleteMenuItemRoles(editMenuItem.ConfigId, editMenuItem.MenuId, editMenuItem.Level, subItemIndex);

					return true;
				}
			}

			return false;
		}

        private static bool FindMenuItem(dynamic editMenuItem, out dynamic resultMenu, out dynamic resultMenuItem)
        {
            resultMenu = null;
            resultMenuItem = null;

            //дальше следует жуткий говнокод, абсолютно невозможный для рефакторинга

            // Конфигурация является корневым пунктом меню
            var configMenuItemId = "/PatientEhr__0_0";

            var configFactory = new ManagerFactoryConfiguration("PatientEhr");
            var menuReader = configFactory.BuildMenuMetadataReader();
            var menuInfoItems = MenuHelper.GetMenuInfoItems(menuReader);

            foreach (var menuInfo in menuInfoItems)
            {
                var menuId = menuInfo.Name;

                if (editMenuItem.MenuId == menuId)
                {
                    var menu = menuReader.GetItem(menuId);

                    if (menu != null && menu.Items != null)
                    {
                        var subItemIndex = 0;

                        // Элементы меню конфигурации являются вложенными пунктами
                        foreach (var subItemInfo in menu.Items)
                        {
                            var item = MenuHelper.FindMenuItem("PatientEhr", menuId, 1, subItemIndex++, configMenuItemId, subItemInfo, editMenuItem.ParentId);

                            if (item != null)
                            {
                                resultMenu = menu;
                                resultMenuItem = item;
                                return true;
                            }
                        }
                    }
                }
            }


            return false;
        }
	}
}
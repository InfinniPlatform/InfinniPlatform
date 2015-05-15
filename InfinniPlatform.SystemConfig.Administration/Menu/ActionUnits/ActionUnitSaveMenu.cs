using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Administration.Menu.ActionUnits
{
	public sealed class ActionUnitSaveMenu
	{
		public void Action(IApplyContext target)
		{
			var isUpdated = false;
			var editMenuItem = target.Item.Document;

			if (editMenuItem != null)
			{
				MenuCache.EnterWrite();

				try
				{
					isUpdated = UpdateMenuItem(editMenuItem);

					MenuCache.ExitWrite(!isUpdated);
				}
				catch
				{
					MenuCache.ExitWrite(false);
				}
			}

			target.Result = isUpdated;
		}

		private static bool UpdateMenuItem(dynamic editMenuItem)
		{
			dynamic editMenu;
			dynamic parentMenuItem;

			if (FindMenuItem(editMenuItem, out editMenu, out parentMenuItem))
			{
				var menuItems = (parentMenuItem.Items == null) ? new List<object>() : new List<object>(parentMenuItem.Items);

				var exists = false;
				var subItemIndex = 0;
				var countSubItems = menuItems.Count;

				int editMenuItemOldIndex = -1;
				int editMenuItemNewIndex = -1;
				IEnumerable editMenuItemRoles = null;

				//foreach (dynamic subItemInfo in menuItems)
				//{
				//	var subItemId = MenuHelper.GetMenuItemId(editMenuItem.ConfigId, editMenuItem.MenuId, editMenuItem.Level, subItemIndex, editMenuItem.ParentId);

				//	// Если элемент существует
				//	if (subItemId == editMenuItem.Id)
				//	{
				//		exists = true;

				//		editMenuItemOldIndex = subItemIndex;
				//		editMenuItemNewIndex = subItemIndex;

				//		// Обновление свойств элемента
				//		subItemInfo.Text = editMenuItem.Text;
				//		subItemInfo.Action = MenuHelper.ObjectFromString(editMenuItem.Action as string);


				//		// Перемещение элемента в списке
				//		if (editMenuItem.Index != null && Convert.ToInt32(editMenuItem.Index) != subItemIndex)
				//		{
				//			var newIndex = Math.Min(Math.Max(Convert.ToInt32(editMenuItem.Index), 0), countSubItems - 1);
				//			ObjectHelper.MoveItem(menuItems, subItemInfo, newIndex - subItemIndex);
				//			editMenuItemNewIndex = newIndex;
				//		}

				//		break;
				//	}

				//	++subItemIndex;
				//}

				// Если элемент не существует
				//if (!exists)
				//{
				dynamic subItemInfo = new DynamicWrapper();

				editMenuItemOldIndex = countSubItems;
				editMenuItemNewIndex = countSubItems;
				editMenuItemRoles = editMenuItem.Roles;

				// Установка свойств элемента
				subItemInfo.Text = editMenuItem.Text;
				subItemInfo.Action = MenuHelper.ObjectFromString(editMenuItem.Action as string);

				// Добавление элемента в список
				ObjectHelper.AddItem(menuItems, subItemInfo);



				// Перемещение элемента в списке
				if (editMenuItem.Index != null)
				{
					var newIndex = Math.Min(Math.Max(Convert.ToInt32(editMenuItem.Index), 0), countSubItems);

					var api = new DocumentApi();
					var rolesUpdate = new List<dynamic>();
					for (int menuItemIndex = newIndex; menuItemIndex < countSubItems; menuItemIndex++)
					{
						

						var subItemId = MenuHelper.GetMenuItemId(editMenuItem.ConfigId, editMenuItem.MenuId, editMenuItem.Level,
																 menuItemIndex, editMenuItem.ParentId);

						var roles = api.GetDocument("Administration", "MenuItemSettings",
																  f =>
																  f.AddCriteria(
																	  cr => cr.IsEquals(subItemId).Property("MenuItemId")), 0, 100).ToList();

						foreach (var role in roles)
						{
							role.MenuItemId = MenuHelper.GetMenuItemId(editMenuItem.ConfigId, editMenuItem.MenuId, editMenuItem.Level, menuItemIndex  + 1, editMenuItem.ParentId);
							rolesUpdate.Add(role);
						}
					}

					foreach (dynamic role in rolesUpdate)
					{
						api.SetDocument("Administration", "MenuItemSettings", role);
					}

					ObjectHelper.MoveItem(menuItems, subItemInfo, newIndex - countSubItems);
					editMenuItemNewIndex = newIndex;
				}
				//}

				parentMenuItem.Items = menuItems;

				// Сохранение меню
				var menuManager = new ManagerFactoryConfiguration(editMenuItem.ConfigId).BuildMenuManager();
				menuManager.MergeItem(editMenu);

				// Сохранение ролей
				MenuHelper.SetMenuItemRoles(editMenuItem.ConfigId, editMenuItem.MenuId, editMenuItem.Level, editMenuItemOldIndex, editMenuItemNewIndex, editMenuItemRoles);

				return true;
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

			// Если пункт меню конфигурации
			if (editMenuItem.ParentId == configMenuItemId)
			{
				object menu;

				var menuInfo = menuInfoItems.FirstOrDefault();

				// Если в конфигурации нет ни одного меню, создаем его
				if (menuInfo == null)
				{
					dynamic newMenu = new DynamicWrapper();
					newMenu.Id = Guid.NewGuid();
					newMenu.Name = "MainMenu";

					var menuManager = configFactory.BuildMenuManager();
					menuManager.MergeItem(newMenu);

					menu = menuReader.GetItem(newMenu.Name);
				}
				else
				{
					menu = menuReader.GetItem(menuInfo.Name);
				}

				resultMenu = menu;
				resultMenuItem = menu;

				return true;
			}

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
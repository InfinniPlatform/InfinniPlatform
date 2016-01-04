using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Administration.Menu.ActionUnits
{
    public sealed class ActionUnitGetMenu
    {
        public void Action(IApplyContext target)
        {
            IEnumerable<object> menuItems;

            var parent = target.Item.Document;
            var parentId = (parent != null) ? parent.Id : null;

            if (!MenuCache.TryGetMenuItems(parentId, out menuItems))
            {
                MenuCache.EnterWrite();

                try
                {
                    FillMenuCache();
                    MenuCache.ExitWrite(true);
                    MenuCache.TryGetMenuItems(parentId, out menuItems);
                }
                catch
                {
                    MenuCache.ExitWrite(false);
                }
            }

            target.Result = menuItems ?? new object[] { };
        }

        private static void FillMenuCache()
        {
            var configIndex = 0;

            var configId = "PatientEhr";

            // Конфигурация является корневым пунктом меню
            var configMenuItem = CreateMenuItem(configId, null, 0, configIndex++, "Система", null);

            var menuReader = new ManagerFactoryConfiguration(configId).BuildMenuMetadataReader();
            var menuInfoItems = GetMenuInfoItems(menuReader);

            foreach (var menuInfo in menuInfoItems)
            {
                var menuId = menuInfo.Name;
                var menu = menuReader.GetItem(menuId);

                if (menu != null && menu.Items != null)
                {
                    var subItemIndex = 0;

                    // Элементы меню конфигурации являются вложенными пунктами
                    foreach (var subItemInfo in menu.Items)
                    {
                        AddMenuItems(configId, menu, 1, subItemIndex++, subItemInfo, configMenuItem);
                    }
                }
            }

        }

        private static IEnumerable<dynamic> GetConfigInfoItems(IDataReader configReader)
        {
            var items = configReader.GetItems();

            return (items != null) ? items.OrderBy(i => i.Caption).ToArray() : Enumerable.Empty<dynamic>();
        }

        private static IEnumerable<dynamic> GetMenuInfoItems(IDataReader menuReader)
        {
            var items = menuReader.GetItems();

            return (items != null) ? items.OrderBy(i => i.Caption).ToArray() : Enumerable.Empty<dynamic>();
        }

        private static void AddMenuItems(string configId, dynamic menu, int level, int index, dynamic itemInfo, dynamic parent)
        {
            var menuId = menu.Name;
            var menuRoles = menu.Roles;

            var menuItem = CreateMenuItem(configId, menuId, level, index, itemInfo.Text, parent);
            menuItem.Action = ActionToString(itemInfo.Action);
            menuItem.Roles = menuRoles;

            if (itemInfo.Items != null)
            {
                var subItemIndex = 0;

                foreach (var subItemInfo in itemInfo.Items)
                {
                    AddMenuItems(configId, menu, level + 1, subItemIndex++, subItemInfo, menuItem);
                }
            }
        }

        private static dynamic CreateMenuItem(string configId, string menuId, int level, int index, string text, dynamic parent)
        {
            dynamic menuItem = new DynamicWrapper();

            var parentId = (parent != null) ? parent.Id : null;
            var id = GetMenuItemId(configId, menuId, level, index, parentId);

            // Идентификаторы элемента для отображения его в визуальном дереве
            menuItem.Id = id;
            menuItem.ParentId = parentId;

            // Идентификаторы для поиска элемента при сохранении и удалении
            menuItem.ConfigId = configId;
            menuItem.MenuId = menuId;
            menuItem.Level = level;
            menuItem.Index = index;

            // Свойства для отображения и редактирования
            menuItem.Text = text;

            MenuCache.AddMenuItem(id, parentId, menuItem);

            return menuItem;
        }

        private static string GetMenuItemId(object configId, object menuId, object level, object index, object parentId)
        {
            return string.Format("{0}/{1}_{2}_{3}_{4}", parentId, configId, menuId, level, index);
        }

        private static string ActionToString(object action)
        {
            if (action != null)
            {
                try
                {
                    var serializer = new JsonObjectSerializer(withFormatting: true);
                    var data = serializer.Serialize(action);
                    return Encoding.UTF8.GetString(data);
                }
                catch
                {
                }
            }

            return null;
        }
    }
}
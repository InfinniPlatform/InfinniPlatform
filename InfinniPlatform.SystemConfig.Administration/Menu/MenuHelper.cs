using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Serialization;

namespace InfinniPlatform.SystemConfig.Administration.Menu
{
	static class MenuHelper
	{
		// Настройка ролей пунктов меню

		public static void SetMenuItemRoles(string configId, string menuId, int level, int oldIndex, int newIndex, IEnumerable roles)
		{
			// Todo
		}

		public static void DeleteMenuItemRoles(string configId, string menuId, int level, int index)
		{
			// Todo
		}

		public static IEnumerable GetMenuItemRoles(string configId, string menuId, int level, int index)
		{
			// Todo
			return null;
		}


		// Методы для обхода дерева пунктов меню

		public static dynamic FindMenuItem(string configId, string menuId, int level, int index, string parentId, dynamic itemInfo, string itemId)
		{
			var menuItemId = GetMenuItemId(configId, menuId, level, index, parentId);

			if (menuItemId == itemId)
			{
				return itemInfo;
			}

			if (itemInfo.Items != null)
			{
				var subItemIndex = 0;

				foreach (var subItemInfo in itemInfo.Items)
				{
					var item = FindMenuItem(configId, menuId, level + 1, subItemIndex++, menuItemId, subItemInfo, itemId);

					if (item != null)
					{
						return item;
					}
				}
			}

			return null;
		}

		public static IEnumerable<dynamic> GetConfigInfoItems(IDataReader configReader)
		{
			var items = configReader.GetItems();

			return (items != null) ? items.OrderBy(i => i.Caption).ToArray() : Enumerable.Empty<dynamic>();
		}

		public static IEnumerable<dynamic> GetMenuInfoItems(IDataReader menuReader)
		{
			var items = menuReader.GetItems();

			return (items != null) ? items.OrderBy(i => i.Caption).ToArray() : Enumerable.Empty<dynamic>();
		}

		public static string GetMenuItemId(object configId, object menuId, object level, object index, object parentId)
		{
			return string.Format("{0}/{1}_{2}_{3}_{4}", parentId, configId, menuId, level, index);
		}

		public static object ObjectFromString(string action)
		{
			if (action != null)
			{
				try
				{
					var serializer = new JsonObjectSerializer(withFormatting: true);
					var data = Encoding.UTF8.GetBytes(action);
					return serializer.Deserialize(data);
				}
				catch
				{
				}
			}

			return null;
		}

		public static string ObjectToString(object action)
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
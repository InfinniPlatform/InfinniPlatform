using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.MenuBar
{
	/// <summary>
	/// Меню.
	/// </summary>
	public sealed class MenuBarElement : BaseElement<MenuBarControl>
	{
		public MenuBarElement(View view, Func<IEnumerable> menuListMetadata, Action<dynamic> menuItemAction)
			: base(view)
		{
			_menuListMetadata = menuListMetadata;
			_menuItemAction = menuItemAction;

			Control.Loaded += OnLoadedMenuBar;
			Control.OnSelectMenuItem += OnSelectMenuItem;
		}


		private readonly Func<IEnumerable> _menuListMetadata;
		private readonly Action<dynamic> _menuItemAction;


		private async void OnLoadedMenuBar(object sender, RoutedEventArgs e)
		{
			var task = Task.Run(_menuListMetadata);
			await task;

			Control.SetMenu(task.Result);
		}

		private void OnSelectMenuItem(object sender, EventArgs e)
		{
			var menuItemMetadata = Control.SelectedMenuItem;

			if (menuItemMetadata != null)
			{
				_menuItemAction(menuItemMetadata);
			}
		}
	}
}
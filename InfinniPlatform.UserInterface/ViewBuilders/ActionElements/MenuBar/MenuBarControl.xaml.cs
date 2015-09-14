using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;
using InfinniPlatform.UserInterface.ViewBuilders.Images;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.MenuBar
{
    /// <summary>
    ///     Элемент управления для меню.
    /// </summary>
    sealed partial class MenuBarControl : UserControl
    {
        public MenuBarControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Возвращает выбранный элемент меню.
        /// </summary>
        public dynamic SelectedMenuItem
        {
            get
            {
                var item = MenuTreeList.SelectedItem as MenuItem;
                return (item != null) ? item.Metadata : null;
            }
        }

        private void OnMenuSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MenuTreeList.Height = MenuTreeListRow.ActualHeight;
        }

        /// <summary>
        ///     Событие выбора элемента меню.
        /// </summary>
        public event EventHandler OnSelectMenuItem;

        private void InvokeOnSelectMenuItem()
        {
            var handler = OnSelectMenuItem;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnSelectMenu(object sender, RoutedEventArgs e)
        {
            var selectedMenu = MenuComboBox.SelectedItem as Menu;
            MenuTreeList.ItemsSource = (selectedMenu != null) ? selectedMenu.Items : null;
            MenuTreeListView.ExpandAllNodes();
        }

        private void OnMenuItemDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            if (e.HitInfo.InRow)
            {
                InvokeOnSelectMenuItem();
            }
        }

        /// <summary>
        ///     Установить список меню.
        /// </summary>
        /// <param name="menuListMetadata">Список метаданных меню.</param>
        public void SetMenu(IEnumerable menuListMetadata)
        {
            var menuList = new List<Menu>();

            if (menuListMetadata != null)
            {
                foreach (dynamic menuMetadata in menuListMetadata)
                {
                    var menu = new Menu
                    {
                        Text = menuMetadata.Caption,
                        Items = new List<MenuItem>()
                    };

                    var menuItemId = 0;
                    FillMenuItems(ref menuItemId, -1, menu.Items, menuMetadata.Items);

                    menuList.Add(menu);
                }
            }

            MenuComboBox.ItemsSource = menuList;
            MenuComboBox.SelectedIndex = (menuList.Count > 0) ? 0 : -1;
        }

        private static void FillMenuItems(ref int id, int? parent, List<MenuItem> menuItems,
            IEnumerable menuItemsMetadata)
        {
            if (menuItemsMetadata != null)
            {
                foreach (dynamic menuItemMetadata in menuItemsMetadata)
                {
                    if (menuItems.Any(i => Equals(i.Metadata, menuItemMetadata)) == false)
                    {
                        var menuItem = new MenuItem
                        {
                            Key = ++id,
                            Parent = parent,
                            Text = menuItemMetadata.Text,
                            Image = ImageRepository.GetImage(menuItemMetadata.Image),
                            Metadata = menuItemMetadata
                        };

                        menuItems.Add(menuItem);

                        FillMenuItems(ref id, menuItem.Key, menuItems, menuItemMetadata.Items);
                    }
                }
            }
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class Menu
        {
            public string Text { get; set; }
            public List<MenuItem> Items { get; set; }
        }

        private class MenuItem
        {
            public int Key { get; set; }
            public int? Parent { get; set; }
            public string Text { get; set; }
            public object Image { get; set; }
            public dynamic Metadata { get; set; }
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}
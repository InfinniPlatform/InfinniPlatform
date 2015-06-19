using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InfinniPlatform.UserInterface.ViewBuilders.Actions;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Images;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ContextMenu
{
    /// <summary>
    ///     Элемент контекстного меню.
    /// </summary>
    public sealed class ContextMenuItem : ContextMenuItemBase<MenuItem>
    {
        // Action

        private BaseAction _action;
        // Image

        private string _image;
        // Items

        private readonly ObservableCollection<FrameworkElement> _items;

        public ContextMenuItem(View view)
            : base(view)
        {
            _items = new ObservableCollection<FrameworkElement>();

            Control.ItemsSource = _items;

            Control.Click += OnClickHandler;
        }

        // OnClick

        /// <summary>
        ///     Возвращает или устанавливает обработчик события нажатия на элемент.
        /// </summary>
        public ScriptDelegate OnClick { get; set; }

        private void OnClickHandler(object sender, RoutedEventArgs e)
        {
            this.InvokeScript(OnClick);

            var action = GetAction();

            if (action != null)
            {
                action.Execute();
            }
        }

        // Text

        public override void SetText(string value)
        {
            base.SetText(value);

            Control.Header = value;
        }

        // Hotkey

        public override void SetHotkey(string value)
        {
            base.SetHotkey(value);

            Control.InputGestureText = value;
        }

        /// <summary>
        ///     Возвращает изображение элемента.
        /// </summary>
        public string GetImage()
        {
            return _image;
        }

        /// <summary>
        ///     Устанавливает изображение элемента.
        /// </summary>
        public void SetImage(string value)
        {
            _image = value;

            var image = ImageRepository.GetImage(value);
            Control.Icon = (image != null) ? new Image {Source = image} : null;
        }

        /// <summary>
        ///     Возвращает действие при нажатии на элемент.
        /// </summary>
        public BaseAction GetAction()
        {
            return _action;
        }

        /// <summary>
        ///     Устанавливает действие при нажатии на элемент.
        /// </summary>
        public void SetAction(BaseAction value)
        {
            _action = value;
        }

        /// <summary>
        ///     Добавляет элемент в список.
        /// </summary>
        public void AddItem(IContextMenuItem item)
        {
            _items.Add(item.GetControl<FrameworkElement>());
        }

        /// <summary>
        ///     Удаляет элемент из списка.
        /// </summary>
        public void RemoveItem(IContextMenuItem item)
        {
            _items.Remove(item.GetControl<FrameworkElement>());
        }

        /// <summary>
        ///     Возвращает элемент по имени.
        /// </summary>
        public IContextMenuItem GetItem(string name)
        {
            var itemControl = _items.FirstOrDefault(i => i.Name == name);

            return (itemControl != null) ? ContextMenuElement.GetMenuItem(itemControl) : null;
        }

        /// <summary>
        ///     Возвращает список элементов.
        /// </summary>
        public IEnumerable<IContextMenuItem> GetItems()
        {
            return _items.Select(ContextMenuElement.GetMenuItem).ToArray();
        }

        // Click

        /// <summary>
        ///     Осуществляет программное нажатие на элемент.
        /// </summary>
        public void Click()
        {
            Control.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
        }

        // Elements

        public override IEnumerable<IElement> GetChildElements()
        {
            return GetItems();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DevExpress.Xpf.Editors;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.Buttons
{
    /// <summary>
    ///     Элемент управления для кнопки со всплывающим окном.
    /// </summary>
    sealed partial class PopupButtonControl : PopupBaseEdit
    {
        private ButtonControl _button;
        private StackPanel _popup;

        private readonly List<UIElement> _items
            = new List<UIElement>();

        public PopupButtonControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Возвращает или устанавливает изображение.
        /// </summary>
        public ImageSource Image
        {
            get { return (ImageSource) GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        private void OnButtonInit(object sender, EventArgs e)
        {
            _button = sender as ButtonControl;
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var handler = Click;

            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private static void OnButtonImage(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as ButtonControl;

            if (button != null)
            {
                button.Image = e.NewValue as ImageSource;
            }
        }

        private void OnPopupInit(object sender, EventArgs e)
        {
            var popup = sender as StackPanel;

            if (popup != null)
            {
                foreach (var item in _items)
                {
                    if (_popup != null)
                    {
                        _popup.Children.Remove(item);
                    }

                    popup.Children.Add(item);
                }
            }

            _popup = popup;
        }

        /// <summary>
        ///     Событие нажатия на кнопку.
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        ///     Осуществляет программное нажатие на кнопку.
        /// </summary>
        public void PerformClick()
        {
            var button = _button;

            if (button != null)
            {
                button.PerformClick();
            }
        }

        /// <summary>
        ///     Добавляет элемент во всплывающее окно.
        /// </summary>
        public void AddItem(UIElement item)
        {
            _items.Add(item);
        }

        /// <summary>
        ///     Удаляет элемент из всплывающего окна.
        /// </summary>
        public void RemoveItem(UIElement item)
        {
            _items.Remove(item);
        }

        /// <summary>
        ///     Возвращает элементы высплывающего окна.
        /// </summary>
        public IEnumerable<UIElement> GetItems()
        {
            return _items.AsReadOnly();
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image",
            typeof (ImageSource), typeof (PopupButtonControl), new FrameworkPropertyMetadata(null, OnButtonImage));
    }
}
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Editors;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigSelector
{
    public partial class ConfigSelectorControl : UserControl
    {
        // RefreshClick

        public static readonly RoutedEvent RefreshClickEvent = EventManager.RegisterRoutedEvent("RefreshClick",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (ConfigSelectorControl));

        // EditValueChanged

        public static readonly RoutedEvent EditValueChangedEvent = EventManager.RegisterRoutedEvent("EditValueChanged",
            RoutingStrategy.Bubble, typeof (ValueChangedRoutedEventHandler), typeof (ConfigSelectorControl));

        public ConfigSelectorControl()
        {
            InitializeComponent();
        }

        // EditValue

        /// <summary>
        ///     Значение.
        /// </summary>
        public object EditValue
        {
            get { return Editor.EditValue; }
            set { Editor.EditValue = value; }
        }

        // ItemSource

        /// <summary>
        ///     Список элементов.
        /// </summary>
        public object ItemsSource
        {
            get { return Editor.ItemsSource; }
            set { Editor.ItemsSource = value; }
        }

        /// <summary>
        ///     Событие нажатия на кнопку обновления.
        /// </summary>
        public event RoutedEventHandler RefreshClick
        {
            add { AddHandler(RefreshClickEvent, value); }
            remove { RemoveHandler(RefreshClickEvent, value); }
        }

        private void OnRefreshClickHandler(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(RefreshClickEvent));
        }

        /// <summary>
        ///     Событие изменения значения.
        /// </summary>
        public event ValueChangedRoutedEventHandler EditValueChanged
        {
            add { AddHandler(EditValueChangedEvent, value); }
            remove { RemoveHandler(EditValueChangedEvent, value); }
        }

        private void OnEditValueChangedHandler(object sender, EditValueChangedEventArgs e)
        {
            RaiseEvent(new ValueChangedRoutedEventArgs(EditValueChangedEvent)
            {
                OldValue = e.OldValue,
                NewValue = e.NewValue
            });
        }

        private void OnClearClickHandler(object sender, RoutedEventArgs e)
        {
            EditValue = null;
        }
    }
}
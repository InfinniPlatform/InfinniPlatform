using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.ToggleButton
{
    /// <summary>
    ///     Элемент управления для отображения и редактирования логического значения в виде переключателя.
    /// </summary>
    sealed partial class ToggleButtonControl : UserControl
    {
        public ToggleButtonControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Возвращает или устанавливает текст на включенное состояние.
        /// </summary>
        public string TextOn
        {
            get { return (string) GetValue(TextOnProperty); }
            set { SetValue(TextOnProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает текст на выключенное состояние.
        /// </summary>
        public string TextOff
        {
            get { return (string) GetValue(TextOffProperty); }
            set { SetValue(TextOffProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает значение, определяющее, запрещено ли редактирование значения.
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool) GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает значение.
        /// </summary>
        public object EditValue
        {
            get { return GetValue(EditValueProperty); }
            set { SetValue(EditValueProperty, value); }
        }

        private static void OnEditValueChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ToggleButtonControl;

            if (control != null)
            {
                var value = (e.NewValue as bool?) == true;

                if (value)
                {
                    control.ButtonOn.Visibility = Visibility.Visible;
                    control.ButtonOff.Visibility = Visibility.Collapsed;
                }
                else
                {
                    control.ButtonOn.Visibility = Visibility.Collapsed;
                    control.ButtonOff.Visibility = Visibility.Visible;
                }

                control.RaiseEvent(new RoutedEventArgs(OnEditValueChangedEvent));
            }
        }

        /// <summary>
        ///     Событие изменения значения.
        /// </summary>
        public event RoutedEventHandler OnEditValueChanged
        {
            add { AddHandler(OnEditValueChangedEvent, value); }
            remove { RemoveHandler(OnEditValueChangedEvent, value); }
        }

        // Handlers

        private void OnClickByTextOn(object sender, MouseButtonEventArgs e)
        {
            ToggleValue(false);
        }

        private void OnClickByButtonOn(object sender, RoutedEventArgs e)
        {
            ToggleValue(false);
        }

        private void OnClickByTextOff(object sender, MouseButtonEventArgs e)
        {
            ToggleValue(true);
        }

        private void OnClickByButtonOff(object sender, RoutedEventArgs e)
        {
            ToggleValue(true);
        }

        private void ToggleValue(bool newValue)
        {
            if (IsReadOnly == false)
            {
                EditValue = newValue;

                if (newValue)
                {
                    ButtonOn.Focus();
                }
                else
                {
                    ButtonOff.Focus();
                }
            }
        }

        // TextOn

        public static readonly DependencyProperty TextOnProperty = DependencyProperty.Register("TextOn", typeof (string),
            typeof (ToggleButtonControl), new FrameworkPropertyMetadata(Properties.Resources.ToggleButtonControlTextOn));

        // TextOff

        public static readonly DependencyProperty TextOffProperty = DependencyProperty.Register("TextOff",
            typeof (string), typeof (ToggleButtonControl),
            new FrameworkPropertyMetadata(Properties.Resources.ToggleButtonControlTextOff));

        // IsReadOnly

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly",
            typeof (bool), typeof (ToggleButtonControl), new FrameworkPropertyMetadata(false));

        // EditValue

        public static readonly DependencyProperty EditValueProperty = DependencyProperty.Register("EditValue",
            typeof (object), typeof (ToggleButtonControl),
            new FrameworkPropertyMetadata(null, OnEditValueChangedHandler));

        // OnEditValueChanged

        public static readonly RoutedEvent OnEditValueChangedEvent =
            EventManager.RegisterRoutedEvent("OnEditValueChanged", RoutingStrategy.Bubble, typeof (RoutedEventHandler),
                typeof (ToggleButtonControl));
    }
}
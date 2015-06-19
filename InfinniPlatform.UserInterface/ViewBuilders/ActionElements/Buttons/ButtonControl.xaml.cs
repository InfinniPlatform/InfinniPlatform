using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InfinniPlatform.UserInterface.ViewBuilders.ActionElements.Buttons
{
    /// <summary>
    ///     Элемент управления для кнопки.
    /// </summary>
    sealed partial class ButtonControl : Button
    {
        public ButtonControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Возвращает или устанавливает текст.
        /// </summary>
        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает изображение.
        /// </summary>
        public ImageSource Image
        {
            get { return (ImageSource) GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as ButtonControl;

            if (button != null)
            {
                var text = e.NewValue as string;
                button.TextElement.Text = text;
                button.TextElement.Visibility = string.IsNullOrEmpty(text) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as ButtonControl;

            if (button != null)
            {
                var image = e.NewValue as ImageSource;
                button.ImageElement.Source = image;
                button.ImageElement.Visibility = (image == null) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        ///     Осуществляет программное нажатие на кнопку.
        /// </summary>
        public void PerformClick()
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent));
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof (string),
            typeof (ButtonControl), new FrameworkPropertyMetadata(null, OnTextChanged));

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image",
            typeof (ImageSource), typeof (ButtonControl), new FrameworkPropertyMetadata(null, OnImageChanged));
    }
}
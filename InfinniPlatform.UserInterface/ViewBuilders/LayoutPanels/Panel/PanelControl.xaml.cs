using System.Windows;
using System.Windows.Media;
using DevExpress.Xpf.LayoutControl;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.Panel
{
    /// <summary>
    ///     Элемент управления для контейнера элементов представления в виде сворачиваемой прямоугольной области.
    /// </summary>
    sealed partial class PanelControl : LayoutGroup
    {
        public PanelControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Возвращает или устанавливает текст заголовка.
        /// </summary>
        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        ///     Возвращает или устанавливает изображение заголовка.
        /// </summary>
        public ImageSource Image
        {
            get { return (ImageSource) GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as PanelControl;

            if (panel != null)
            {
                var text = e.NewValue as string;

                if (string.IsNullOrEmpty(text))
                {
                    panel.TextElement.Text = null;
                    panel.TextElement.Visibility = Visibility.Collapsed;
                    panel.View = (panel.Image == null) ? LayoutGroupView.Group : LayoutGroupView.GroupBox;
                }
                else
                {
                    panel.TextElement.Text = text;
                    panel.TextElement.Visibility = Visibility.Visible;
                    panel.View = LayoutGroupView.GroupBox;
                }
            }
        }

        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as PanelControl;

            if (panel != null)
            {
                var image = e.NewValue as ImageSource;

                if (image == null)
                {
                    panel.ImageElement.Source = null;
                    panel.ImageElement.Visibility = Visibility.Collapsed;
                    panel.View = string.IsNullOrEmpty(panel.Text) ? LayoutGroupView.Group : LayoutGroupView.GroupBox;
                }
                else
                {
                    panel.ImageElement.Source = image;
                    panel.ImageElement.Visibility = Visibility.Visible;
                    panel.View = LayoutGroupView.GroupBox;
                }
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof (string),
            typeof (PanelControl), new FrameworkPropertyMetadata(null, OnTextChanged));

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image",
            typeof (ImageSource), typeof (PanelControl), new FrameworkPropertyMetadata(null, OnImageChanged));
    }
}
using System.Windows;
using System.Windows.Controls;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Controls
{
    /// <summary>
    ///     Элемент управления для отображения и редактирования конфигураций.
    /// </summary>
    public sealed partial class ConfigEditorControl : UserControl
    {
        public ConfigEditorControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Панель для размещения редакторов элементов конфигурации.
        /// </summary>
        public IConfigElementEditPanel EditPanel
        {
            get { return (IConfigElementEditPanel) GetValue(EditPanelProperty); }
            set { SetValue(EditPanelProperty, value); }
        }

        private static void OnEditPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ConfigEditorControl;

            if (control != null)
            {
                var editPanel = e.NewValue as IConfigElementEditPanel;
                control.ConfigEdit.Content = (editPanel != null) ? editPanel.LayoutPanel : null;
            }
        }

        // EditPanel

        public static readonly DependencyProperty EditPanelProperty = DependencyProperty.Register("EditPanel",
            typeof (IConfigElementEditPanel), typeof (ConfigEditorControl), new PropertyMetadata(OnEditPanelChanged));
    }
}
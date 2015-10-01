using System.Windows.Controls;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigVerifyDesigner
{
    /// <summary>
    ///     Элемент управления для проверки конфигурации.
    /// </summary>
    sealed partial class ConfigVerifyDesignerControl : UserControl
    {
        public ConfigVerifyDesignerControl()
        {
            InitializeComponent();
        }

        public object Value
        {
            get { return Designer.Value; }
            set { Designer.Value = value; }
        }
    }
}
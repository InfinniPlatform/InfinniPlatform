using System;
using System.Windows.Forms;

namespace InfinniPlatform.MetadataDesigner.Views
{
    /// <summary>
    /// Элемент управления для развертывания конфигурации.
    /// </summary>
    public sealed partial class ConfigDeployDesignerView : UserControl
    {
        public ConfigDeployDesignerView()
        {
            InitializeComponent();
        }

        public dynamic Value { get; set; }

        private string GetSolutionName()
        {
            return TextEditSolutionName.Text;
        }

        private string GetNewSolutionVersion()
        {
            return TextEditSolutionVersionNew.Text;
        }

        private string GetSolutionVersion()
        {
            return TextEditSolutionVersion.Text;
        }

        private void ButtonImportSolutionClick(object sender, EventArgs e)
        {
        }

        private void ButtonExportSolutionClick(object sender, EventArgs e)
        {
        }

        private void ButtonExportConfigClick(object sender, EventArgs e)
        {
        }

        private void ImportButtonClick(object sender, EventArgs e)
        {
        }
    }
}
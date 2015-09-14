using System.Windows.Forms;
using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.ReportDesigner.Views.Designer
{
    /// <summary>
    ///     Форма редактора отчета.
    /// </summary>
    sealed partial class ReportDesignerForm : Form
    {
        public ReportDesignerForm()
        {
            InitializeComponent();

            ReportDesigner.EditValue = new ReportTemplate();
        }
    }
}
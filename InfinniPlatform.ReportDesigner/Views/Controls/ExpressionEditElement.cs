using System;
using System.ComponentModel;
using System.Windows.Forms;
using FastReport;

namespace InfinniPlatform.ReportDesigner.Views.Controls
{
    /// <summary>
    ///     Элемент для редактирования выражения.
    /// </summary>
    sealed partial class ExpressionEditElement : UserControl
    {
        public ExpressionEditElement()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Шаблон отчета FastReport.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Report Report { get; set; }

        /// <summary>
        ///     Текст выражения.
        /// </summary>
        public string ExpressionText
        {
            get { return ExpressionEdit.Text; }
            set { ExpressionEdit.Text = value; }
        }

        private void OnExpressionButton(object sender, EventArgs e)
        {
            using (var expressionEditor = new ExpressionEditDialog(Report))
            {
                expressionEditor.ExpressionText = ExpressionEdit.Text;

                if (expressionEditor.ShowDialog(this) == DialogResult.OK)
                {
                    ExpressionEdit.Text = expressionEditor.ExpressionText;
                }
            }
        }

        public override bool ValidateChildren()
        {
            return (string.IsNullOrWhiteSpace(ExpressionEdit.Text) == false);
        }
    }
}
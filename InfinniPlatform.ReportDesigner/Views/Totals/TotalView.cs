using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using FastReport;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Properties;
using InfinniPlatform.ReportDesigner.Views.Designer;

namespace InfinniPlatform.ReportDesigner.Views.Totals
{
    /// <summary>
    ///     Представление для отображения и редактирования информации о итоге отчета.
    /// </summary>
    sealed partial class TotalView : UserControl
    {
        private IEnumerable<DesignerDataBand> _dataBands;
        private IEnumerable<DesignerPrintBand> _printBands;
        private Report _report;

        public TotalView()
        {
            InitializeComponent();

            Text = Resources.TotalView;

            UpdateFunctions();
        }

        /// <summary>
        ///     Шаблон отчета FastReport.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Report Report
        {
            get { return _report; }
            set
            {
                _report = value;

                ExpressionEdit.Report = value;
            }
        }

        /// <summary>
        ///     Список блоков данных отчета.
        /// </summary>
        public IEnumerable<DesignerDataBand> DataBands
        {
            get { return _dataBands; }
            set
            {
                _dataBands = value;

                DataBandEdit.Items = value;
            }
        }

        /// <summary>
        ///     Список блоков отчетов для печати.
        /// </summary>
        public IEnumerable<DesignerPrintBand> PrintBands
        {
            get { return _printBands; }
            set
            {
                _printBands = value;

                PrintBandEdit.Items = value;
            }
        }

        /// <summary>
        ///     Информация об итоге.
        /// </summary>
        public TotalInfo TotalInfo
        {
            get
            {
                return new TotalInfo
                {
                    Name = NameEdit.Text,
                    DataBand = GetSelectedDataBand(),
                    PrintBand = GetSelectedPrintBand(),
                    TotalFunc = (TotalFunc) TotalFuncEdit.SelectedItem,
                    Expression =
                        (ExpressionEdit.Enabled) ? ExpressionTextToDataBind(ExpressionEdit.ExpressionText) : null
                };
            }
            set
            {
                if (value != null)
                {
                    NameEdit.Text = value.Name;
                    DataBandEdit.SelectedItem = FindDataBandByName(value.DataBand);
                    PrintBandEdit.SelectedItem = FindPrintBandByName(value.PrintBand);
                    TotalFuncEdit.SelectedItem = value.TotalFunc;
                    ExpressionEdit.ExpressionText = ExpressionTextFromDataBind(value.Expression);
                }
                else
                {
                    NameEdit.Text = null;
                    DataBandEdit.SelectedItem = null;
                    PrintBandEdit.SelectedItem = null;
                    TotalFuncEdit.SelectedItem = null;
                    ExpressionEdit.ExpressionText = null;
                }
            }
        }

        private static IDataBind ExpressionTextToDataBind(string expressionText)
        {
            if (!string.IsNullOrEmpty(expressionText))
            {
                return new ConstantBind {Value = expressionText};
            }

            return null;
        }

        private static string ExpressionTextFromDataBind(IDataBind dataBind)
        {
            if (dataBind is ConstantBind)
            {
                return ((ConstantBind) dataBind).Value as string;
            }

            return null;
        }

        private void UpdateFunctions()
        {
            TotalFuncEdit.Items = Enum.GetValues(typeof (TotalFunc));
        }

        private void OnFunctionChanged(object sender, EventArgs e)
        {
            // При выборе функции "Count", редактировать выражение нельзя
            ExpressionEdit.Enabled = Equals(TotalFuncEdit.SelectedItem, TotalFunc.Count) == false;
        }

        private string GetSelectedDataBand()
        {
            var selectedDataBand = DataBandEdit.SelectedItem as DesignerDataBand;
            return (selectedDataBand != null) ? selectedDataBand.Name : null;
        }

        private string GetSelectedPrintBand()
        {
            var selectedPrintBand = PrintBandEdit.SelectedItem as DesignerPrintBand;
            return (selectedPrintBand != null) ? selectedPrintBand.Name : null;
        }

        private DesignerDataBand FindDataBandByName(string dataBandName)
        {
            return (DataBands != null) ? DataBands.FirstOrDefault(i => i.Name == dataBandName) : null;
        }

        private DesignerPrintBand FindPrintBandByName(string printBandName)
        {
            return (PrintBands != null) ? PrintBands.FirstOrDefault(i => i.Name == printBandName) : null;
        }

        public override bool ValidateChildren()
        {
            if (NameEdit.Text.IsValidName() == false)
            {
                Resources.EnterValidName.ShowError();
                NameEdit.Focus();
                return false;
            }

            if (TotalFuncEdit.SelectedItem == null)
            {
                Resources.SelectTotalFunc.ShowError();
                TotalFuncEdit.Focus();
                return false;
            }

            if (DataBandEdit.SelectedItem == null)
            {
                Resources.SelectDataBand.ShowError();
                DataBandEdit.Focus();
                return false;
            }

            if (ExpressionEdit.ValidateChildren() == false)
            {
                Resources.SpecifyExpression.ShowError();
                ExpressionEdit.Focus();
                return false;
            }

            if (PrintBandEdit.SelectedItem == null)
            {
                Resources.SelectPrintBand.ShowError();
                PrintBandEdit.Focus();
                return false;
            }

            return true;
        }
    }
}
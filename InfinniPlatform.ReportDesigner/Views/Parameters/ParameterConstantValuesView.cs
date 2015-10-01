using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views.Parameters
{
    /// <summary>
    ///     Представление для указания значений параметров отчета в виде предопределенного списка.
    /// </summary>
    sealed partial class ParameterConstantValuesView : UserControl
    {
        public ParameterConstantValuesView()
        {
            InitializeComponent();

            ShowLabel = true;
        }

        /// <summary>
        ///     Отображать наименование значения.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowLabel
        {
            get { return LabelPropertyColumn.Visible; }
            set { LabelPropertyColumn.Visible = value; }
        }

        /// <summary>
        ///     Значения параметров отчета в виде предопределенного списка.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        public ParameterConstantValueProviderInfo ParameterValues
        {
            get { return GetParameterValues(); }
            set { SetParameterValues(value); }
        }

        private void SetParameterValues(ParameterConstantValueProviderInfo parameterValues)
        {
            ValueListView.Rows.Clear();

            if (parameterValues != null && parameterValues.Items != null)
            {
                foreach (var item in parameterValues.Items)
                {
                    var label = item.Key;
                    var value = item.Value as ConstantBind;

                    if (value != null)
                    {
                        ValueListView.Rows.Add(label, value.Value);
                    }
                }
            }
        }

        private ParameterConstantValueProviderInfo GetParameterValues()
        {
            var parameterValues = new ParameterConstantValueProviderInfo
            {
                Items = new Dictionary<string, IDataBind>()
            };

            var index = 0;

            foreach (DataGridViewRow row in ValueListView.Rows)
            {
                var label = ShowLabel ? (string) row.Cells[0].Value : string.Format("Item{0}", index++);
                var value = new ConstantBind {Value = row.Cells[1].Value};

                parameterValues.Items.Add(label, value);
            }

            return parameterValues;
        }

        private void OnAddClick(object sender, EventArgs e)
        {
            var newRowIndex = ValueListView.Rows.Add();
            var newRow = ValueListView.Rows[newRowIndex];
            newRow.Cells[ShowLabel ? 0 : 1].Selected = true;
            ValueListView.BeginEdit(true);
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            if (ValueListView.SelectedCells.Count > 0)
            {
                var selectedRowIndex = ValueListView.SelectedCells[0].RowIndex;
                ValueListView.Rows.RemoveAt(selectedRowIndex);
            }
        }

        private void OnUpClick(object sender, EventArgs e)
        {
            if (ValueListView.SelectedCells.Count > 0)
            {
                var selectedRowIndex = ValueListView.SelectedCells[0].RowIndex;

                if (selectedRowIndex > 0)
                {
                    var selectedRow = ValueListView.Rows[selectedRowIndex];

                    ValueListView.Rows.Remove(selectedRow);
                    ValueListView.Rows.Insert(selectedRowIndex - 1, selectedRow);
                    selectedRow.Cells[ShowLabel ? 0 : 1].Selected = true;
                }
            }
        }

        private void OnDownClick(object sender, EventArgs e)
        {
            if (ValueListView.SelectedCells.Count > 0)
            {
                var selectedRowIndex = ValueListView.SelectedCells[0].RowIndex;

                if (selectedRowIndex < ValueListView.Rows.Count - 1)
                {
                    var selectedRow = ValueListView.Rows[selectedRowIndex];

                    ValueListView.Rows.Remove(selectedRow);
                    ValueListView.Rows.Insert(selectedRowIndex + 1, selectedRow);
                    selectedRow.Cells[ShowLabel ? 0 : 1].Selected = true;
                }
            }
        }

        private void OnCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (ShowLabel && e.ColumnIndex == 0)
            {
                var labelValue = e.FormattedValue as string;

                if (string.IsNullOrEmpty(labelValue))
                {
                    e.Cancel = true;
                    ShowWarning(Resources.LabelValueCannotBeEmpty);
                }
                else if (
                    ValueListView.Rows.Cast<DataGridViewRow>()
                        .Any(r => labelValue.Equals(r.Cells[0].Value) && r.Index != e.RowIndex))
                {
                    e.Cancel = true;
                    ShowWarning(Resources.LabelValueAlreadyExists, labelValue);
                }
                else
                {
                    HideWarning();
                }
            }
        }

        private void ShowWarning(string message, params object[] args)
        {
            WarningMessage.Text = string.Format(message, args);
            WarningPanel.Visible = true;
        }

        private void HideWarning()
        {
            WarningMessage.Text = string.Empty;
            WarningPanel.Visible = false;
        }
    }
}
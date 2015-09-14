using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views.Parameters
{
    /// <summary>
    ///     Представление для отображения и редактирования информации о параметре отчета.
    /// </summary>
    sealed partial class ParameterView : UserControl
    {
        private IEnumerable<DataSourceInfo> _dataSources;

        public ParameterView()
        {
            InitializeComponent();

            Text = Resources.ParameterView;

            ParameterInfo = new ParameterInfo();
        }

        /// <summary>
        ///     Список источников данных.
        /// </summary>
        public IEnumerable<DataSourceInfo> DataSources
        {
            get { return _dataSources; }
            set
            {
                _dataSources = value;

                AvailableValuesEdit.DataSources = value;
                DefaultValuesEdit.DataSources = value;
            }
        }

        /// <summary>
        ///     Информация о параметре отчета.
        /// </summary>
        public ParameterInfo ParameterInfo
        {
            get
            {
                SchemaDataType dataType;

                if (Enum.TryParse(DataTypeEdit.Text, out dataType) == false)
                {
                    dataType = SchemaDataType.None;
                }

                return new ParameterInfo
                {
                    Type = dataType,
                    Name = NameEdit.Text,
                    Caption = CaptionEdit.Text,
                    AllowNullValue = AllowNullValueEdit.Checked,
                    AllowMultiplyValues = AllowMultiplyValuesEdit.Checked,
                    AvailableValues = AvailableValuesEdit.ParameterValues,
                    DefaultValues = DefaultValuesEdit.ParameterValues
                };
            }
            set
            {
                if (value != null)
                {
                    DataTypeEdit.Text = value.Type.ToString();
                    NameEdit.Text = value.Name;
                    CaptionEdit.Text = value.Caption;
                    AllowNullValueEdit.Checked = value.AllowNullValue;
                    AllowMultiplyValuesEdit.Checked = value.AllowMultiplyValues;
                    AvailableValuesEdit.ParameterValues = value.AvailableValues;
                    DefaultValuesEdit.ParameterValues = value.DefaultValues;
                }
                else
                {
                    DataTypeEdit.Text = null;
                    NameEdit.Text = null;
                    CaptionEdit.Text = null;
                    AllowNullValueEdit.Checked = false;
                    AllowMultiplyValuesEdit.Checked = false;
                    AvailableValuesEdit.ParameterValues = null;
                    DefaultValuesEdit.ParameterValues = null;
                }
            }
        }

        public override bool ValidateChildren()
        {
            if (string.IsNullOrWhiteSpace(DataTypeEdit.Text))
            {
                Resources.SelectDataType.ShowError();
                MainTabControl.SelectTab(GeneralTabPage);
                DataTypeEdit.Focus();
                return false;
            }

            if (NameEdit.Text.IsValidName() == false)
            {
                Resources.EnterValidName.ShowError();
                MainTabControl.SelectTab(GeneralTabPage);
                NameEdit.Focus();
                return false;
            }

            if (AvailableValuesEdit.ValidateChildren() == false)
            {
                MainTabControl.SelectTab(AvailableValuesTabPage);
                AvailableValuesEdit.Focus();
                return false;
            }

            if (DefaultValuesEdit.ValidateChildren() == false)
            {
                MainTabControl.SelectTab(DefaultValuesTabPage);
                DefaultValuesEdit.Focus();
                return false;
            }

            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.ReportDesigner.Views.Parameters
{
	/// <summary>
	/// Представление для указания значений параметров отчета.
	/// </summary>
	sealed partial class ParameterValuesView : UserControl
	{
		public ParameterValuesView()
		{
			InitializeComponent();

			ShowLabel = true;
		}


		/// <summary>
		/// Отображать наименование значения.
		/// </summary>
		[DefaultValue(true)]
		public bool ShowLabel { get; set; }


		private IEnumerable<DataSourceInfo> _dataSources;

		/// <summary>
		/// Список источников данных.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public IEnumerable<DataSourceInfo> DataSources
		{
			get
			{
				return _dataSources;
			}
			set
			{
				_dataSources = value;

				_dataSourceValuesView.DataSources = value;
			}
		}


		/// <summary>
		/// Значения параметров отчета.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public IParameterValueProviderInfo ParameterValues
		{
			get
			{
				return GetParameterValues();
			}
			set
			{
				SetParameterValues(value);
			}
		}


		private readonly ParameterConstantValuesView _constantValuesView
			= new ParameterConstantValuesView { Dock = DockStyle.Fill };

		private readonly ParameterDataSourceValuesView _dataSourceValuesView
			= new ParameterDataSourceValuesView { Dock = DockStyle.Fill };


		private IParameterValueProviderInfo GetParameterValues()
		{
			if (ConstantValuesRadioButton.Checked)
			{
				return _constantValuesView.ParameterValues;
			}

			if (DataSourceValuesRadioButton.Checked)
			{
				return _dataSourceValuesView.ParameterValues;
			}

			return null;
		}

		private void SetParameterValues(IParameterValueProviderInfo parameterValues)
		{
			_constantValuesView.ParameterValues = null;
			_dataSourceValuesView.ParameterValues = null;

			if (parameterValues == null)
			{
				NoneValuesRadioButton.Checked = true;
			}
			else if (parameterValues is ParameterConstantValueProviderInfo)
			{
				ConstantValuesRadioButton.Checked = true;

				_constantValuesView.ParameterValues = (ParameterConstantValueProviderInfo)parameterValues;
			}
			else if (parameterValues is ParameterDataSourceValueProviderInfo)
			{
				DataSourceValuesRadioButton.Checked = true;

				_dataSourceValuesView.ParameterValues = (ParameterDataSourceValueProviderInfo)parameterValues;
			}
		}


		private void OnSelectParameterValues(object sender, EventArgs e)
		{
			ValuesPanel.Controls.Clear();

			if (ConstantValuesRadioButton.Checked)
			{
				_constantValuesView.ShowLabel = ShowLabel;

				ValuesPanel.Controls.Add(_constantValuesView);
			}
			else if (DataSourceValuesRadioButton.Checked)
			{
				_dataSourceValuesView.ShowLabel = ShowLabel;

				ValuesPanel.Controls.Add(_dataSourceValuesView);
			}
		}


		public override bool ValidateChildren()
		{
			return NoneValuesRadioButton.Checked
				   || (ConstantValuesRadioButton.Checked && _constantValuesView.ValidateChildren())
				   || (DataSourceValuesRadioButton.Checked && _dataSourceValuesView.ValidateChildren());
		}
	}
}
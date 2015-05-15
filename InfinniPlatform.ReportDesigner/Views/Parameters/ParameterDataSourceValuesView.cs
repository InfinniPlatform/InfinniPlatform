using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Properties;
using InfinniPlatform.ReportDesigner.Views.DataSources;

namespace InfinniPlatform.ReportDesigner.Views.Parameters
{
	/// <summary>
	/// Представление для указания значений параметров отчета в виде списка, получаемого из источника данных.
	/// </summary>
	sealed partial class ParameterDataSourceValuesView : UserControl
	{
		public ParameterDataSourceValuesView()
		{
			InitializeComponent();

			ShowLabel = true;

			_propertySelectForm = new DialogView<DataSourcePropertySelectView>();
		}


		private readonly DialogView<DataSourcePropertySelectView> _propertySelectForm;


		/// <summary>
		/// Отображать наименование значения.
		/// </summary>
		[DefaultValue(true)]
		public bool ShowLabel
		{
			get { return LabelPropertyPanel.Visible; }
			set { LabelPropertyPanel.Visible = value; }
		}


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

				RenderDataSourceList(value);
			}
		}


		/// <summary>
		/// Значения параметров отчета в виде списка, получаемого из источника данных.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public ParameterDataSourceValueProviderInfo ParameterValues
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


		private void RenderDataSourceList(IEnumerable<DataSourceInfo> dataSources)
		{
			DataSourceEdit.Items.Clear();

			if (dataSources != null)
			{
				foreach (var dataSource in dataSources)
				{
					DataSourceEdit.Items.Add(dataSource.Name);
				}

				if (DataSourceEdit.Items.Count > 0)
				{
					DataSourceEdit.SelectedIndex = 0;
				}
			}
		}

		private ParameterDataSourceValueProviderInfo GetParameterValues()
		{
			return new ParameterDataSourceValueProviderInfo
					   {
						   DataSource = DataSourceEdit.Text,
						   LabelProperty = LabelPropertyEdit.Text,
						   ValueProperty = ValuePropertyEdit.Text
					   };
		}

		private void SetParameterValues(ParameterDataSourceValueProviderInfo parameterValues)
		{
			if (parameterValues != null)
			{
				DataSourceEdit.Text = parameterValues.DataSource;
				LabelPropertyEdit.Text = parameterValues.LabelProperty;
				ValuePropertyEdit.Text = parameterValues.ValueProperty;
			}
			else
			{
				DataSourceEdit.Text = null;
				LabelPropertyEdit.Text = null;
				ValuePropertyEdit.Text = null;
			}
		}


		private void OnDataSourceChanged(object sender, EventArgs e)
		{
			LabelPropertyEdit.Text = null;
			ValuePropertyEdit.Text = null;
		}

		private void OnLabelPropertyEdit(object sender, EventArgs e)
		{
			SelectDataSourceProperty(LabelPropertyEdit);
		}

		private void OnValuePropertyEdit(object sender, EventArgs e)
		{
			SelectDataSourceProperty(ValuePropertyEdit);
		}

		private void SelectDataSourceProperty(Control propertyEditor)
		{
			if (DataSources != null)
			{
				var selectedDataSourceName = DataSourceEdit.Text;

				if (string.IsNullOrWhiteSpace(selectedDataSourceName) == false)
				{
					var selectedDataSource = DataSources.FirstOrDefault(ds => ds.Name == selectedDataSourceName);

					if (selectedDataSource != null)
					{
						_propertySelectForm.View.DataSourceName = selectedDataSourceName;
						_propertySelectForm.View.DataSourceSchema = selectedDataSource.Schema;
						_propertySelectForm.View.DataSourceProperty = propertyEditor.Text;

						if (_propertySelectForm.ShowDialog(this) == DialogResult.OK)
						{
							propertyEditor.Text = _propertySelectForm.View.DataSourceProperty;
						}
					}
				}
			}
		}


		public override bool ValidateChildren()
		{
			if (string.IsNullOrEmpty(DataSourceEdit.Text))
			{
				Resources.SelectDataSource.ShowError();
				DataSourceEdit.Focus();
				return false;
			}

			if (ShowLabel && string.IsNullOrEmpty(LabelPropertyEdit.Text))
			{
				Resources.SelectLabelProperty.ShowError();
				LabelPropertyButton.Focus();
				return false;
			}

			if (string.IsNullOrEmpty(ValuePropertyEdit.Text))
			{
				Resources.SelectValueProperty.ShowError();
				ValuePropertyButton.Focus();
				return false;
			}

			return true;
		}
	}
}
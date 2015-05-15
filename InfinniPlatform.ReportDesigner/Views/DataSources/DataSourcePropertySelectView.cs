using System.Windows.Forms;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	/// <summary>
	/// Представление для выбора свойства источника данных.
	/// </summary>
	sealed partial class DataSourcePropertySelectView : UserControl
	{
		public DataSourcePropertySelectView()
		{
			InitializeComponent();

			Text = Resources.DataSourcePropertySelectView;
		}


		/// <summary>
		/// Наименование источника данных.
		/// </summary>
		public string DataSourceName
		{
			get { return DataSchemaEdit.DataSourceName; }
			set { DataSchemaEdit.DataSourceName = value; }
		}

		/// <summary>
		/// Схема источника данных.
		/// </summary>
		public DataSchema DataSourceSchema
		{
			get { return DataSchemaEdit.DataSourceSchema; }
			set { DataSchemaEdit.DataSourceSchema = value; }
		}

		/// <summary>
		/// Путь к выбранному свойству источника данных.
		/// </summary>
		public string DataSourceProperty
		{
			get { return DataSchemaEdit.SelectedPropertyPath; }
			set { DataSchemaEdit.SelectedPropertyPath = value; }
		}


		public override bool ValidateChildren()
		{
			var selectedPropertyType = DataSchemaEdit.SelectedPropertyType;

			if (selectedPropertyType == SchemaDataType.None)
			{
				Resources.SelectDataSourceProperty.ShowError();
				return false;
			}

			if (selectedPropertyType == SchemaDataType.Object || selectedPropertyType == SchemaDataType.Array)
			{
				Resources.SelectedPropertyMustHaveSimpleDataType.ShowError();
				return false;
			}

			return true;
		}
	}
}
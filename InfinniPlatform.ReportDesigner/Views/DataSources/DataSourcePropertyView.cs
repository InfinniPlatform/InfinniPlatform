using System;
using System.Windows.Forms;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
    /// <summary>
    ///     Представление для отображения и редактирования свойства источника данных.
    /// </summary>
    sealed partial class DataSourcePropertyView : UserControl
    {
        public DataSourcePropertyView()
        {
            InitializeComponent();

            Text = Resources.DataSourcePropertyView;
        }

        /// <summary>
        ///     Наименование свойства.
        /// </summary>
        public string PropertyName
        {
            get { return NameEdit.Text; }
            set { NameEdit.Text = value; }
        }

        /// <summary>
        ///     Тип данных свойства.
        /// </summary>
        public SchemaDataType PropertyType
        {
            get
            {
                SchemaDataType propertyType;

                if (Enum.TryParse(DataTypeEdit.Text, out propertyType) == false)
                {
                    propertyType = SchemaDataType.None;
                }

                return propertyType;
            }
            set { DataTypeEdit.Text = value.ToString(); }
        }

        public override bool ValidateChildren()
        {
            if (string.IsNullOrWhiteSpace(DataTypeEdit.Text))
            {
                Resources.SelectDataType.ShowError();
                DataTypeEdit.Focus();
                return false;
            }

            if (NameEdit.Text.IsValidName() == false)
            {
                Resources.EnterValidName.ShowError();
                NameEdit.Focus();
                return false;
            }

            return true;
        }
    }
}
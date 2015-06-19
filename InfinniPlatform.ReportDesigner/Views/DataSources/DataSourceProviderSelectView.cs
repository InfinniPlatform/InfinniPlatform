using System.Linq;
using System.Windows.Forms;
using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
    /// <summary>
    ///     Представление для выбора типа поставщика данных.
    /// </summary>
    sealed partial class DataSourceProviderSelectView : UserControl
    {
        public DataSourceProviderSelectView()
        {
            InitializeComponent();

            Text = Resources.DataSourceProviderSelectView;

            SetProviders();
            ResetDefaults();
        }

        /// <summary>
        ///     Выбранный тип поставщика данных.
        /// </summary>
        public DataSourceProviderType SelectedProvider
        {
            get
            {
                var selectedItem = (DataProviderItem) DataProviderEdit.SelectedItem;

                return (selectedItem != null) ? selectedItem.Value : DataSourceProviderType.Register;
            }
            set
            {
                var selectedItem = FindProvider(value);

                DataProviderEdit.SelectedItem = selectedItem;
            }
        }

        private void SetProviders()
        {
            AddProvider(Resources.DataProviderRegister, DataSourceProviderType.Register);
            AddProvider(Resources.DataProviderRestService, DataSourceProviderType.RestService);
            AddProvider(Resources.DataProviderMsSqlServer, DataSourceProviderType.MsSqlServer);
            AddProvider(Resources.DataProviderFirebird, DataSourceProviderType.Firebird);
        }

        private void AddProvider(string name, DataSourceProviderType value)
        {
            DataProviderEdit.Items.Add(new DataProviderItem {Name = name, Value = value});
        }

        private object FindProvider(DataSourceProviderType value)
        {
            return DataProviderEdit.Items.Cast<DataProviderItem>().FirstOrDefault(i => i.Value == value);
        }

        public void ResetDefaults()
        {
            SelectedProvider = DataSourceProviderType.Register;
        }

        private class DataProviderItem
        {
            public string Name;
            public DataSourceProviderType Value;

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
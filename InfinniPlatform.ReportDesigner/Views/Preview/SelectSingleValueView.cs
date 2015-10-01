using System.Collections.Generic;
using System.Windows.Forms;
using InfinniPlatform.ReportDesigner.Properties;

namespace InfinniPlatform.ReportDesigner.Views.Preview
{
    /// <summary>
    ///     Представление для выбора значения из списка.
    /// </summary>
    sealed partial class SelectSingleValueView : UserControl
    {
        private IDictionary<string, object> _availableValues;

        public SelectSingleValueView()
        {
            InitializeComponent();

            Text = Resources.SelectSingleValueView;
        }

        /// <summary>
        ///     Список значений.
        /// </summary>
        public IDictionary<string, object> AvailableValues
        {
            get { return _availableValues; }
            set
            {
                _availableValues = value;

                ItemsEdit.Items.Clear();

                if (value != null)
                {
                    foreach (var item in value)
                    {
                        ItemsEdit.Items.Add(new ParameterValue(item.Value, item.Key));
                    }
                }

                ResetSelection();
            }
        }

        /// <summary>
        ///     Выбранное значение.
        /// </summary>
        public object SelectedValue
        {
            get
            {
                var selectedItem = ItemsEdit.SelectedItem as ParameterValue;

                return (selectedItem != null) ? selectedItem.Value : null;
            }
            set
            {
                var index = 0;
                var selectedIndex = -1;

                foreach (ParameterValue item in ItemsEdit.Items)
                {
                    if (Equals(item.Value, value))
                    {
                        selectedIndex = index;
                        break;
                    }

                    index++;
                }

                ItemsEdit.SelectedIndex = selectedIndex;
            }
        }

        private void ResetSelection()
        {
            ItemsEdit.SelectedIndex = -1;
        }
    }
}
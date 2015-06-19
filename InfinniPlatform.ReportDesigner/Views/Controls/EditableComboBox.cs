using System;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Controls
{
    /// <summary>
    ///     Выпадающий список с предустановленными значениями и возможностью указать свой вариант.
    /// </summary>
    sealed partial class EditableComboBox : UserControl
    {
        public EditableComboBox()
        {
            InitializeComponent();

            AddItem(string.Empty, string.Empty);

            ComboBoxEdit.TextChanged += (s, e) => InvokeValueChanged();
        }

        public string SelectedName
        {
            get
            {
                var selectedItem = ComboBoxEdit.SelectedItem as ComboBoxItem;
                return (selectedItem != null && ComboBoxEdit.SelectedIndex != 0) ? selectedItem.Name : ComboBoxEdit.Text;
            }
        }

        public string SelectedValue
        {
            get
            {
                var selectedItem = ComboBoxEdit.SelectedItem as ComboBoxItem;
                return (selectedItem != null && ComboBoxEdit.SelectedIndex != 0)
                    ? selectedItem.Value
                    : ComboBoxEdit.Text;
            }
        }

        public event EventHandler ValueChanged;

        private void InvokeValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }

        public void AddItem(string name, string value)
        {
            ComboBoxEdit.Items.Add(new ComboBoxItem(name, value));
        }

        private void OnLoad(object sender, EventArgs e)
        {
            ComboBoxEdit.SelectedIndex = (ComboBoxEdit.Items.Count > 1) ? 1 : 0;

            Load -= OnLoad;
        }

        private void OnSelectedChanged(object sender, EventArgs e)
        {
            var selectedItem = ComboBoxEdit.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                ComboBoxEdit.DropDownStyle = (ComboBoxEdit.SelectedIndex > 0)
                    ? ComboBoxStyle.DropDownList
                    : ComboBoxStyle.DropDown;

                ComboBoxEdit.Text = selectedItem.Value;
            }
            else
            {
                ComboBoxEdit.SelectedIndex = 0;
            }
        }

        private class ComboBoxItem
        {
            public readonly string Name;
            public readonly string Value;

            public ComboBoxItem(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
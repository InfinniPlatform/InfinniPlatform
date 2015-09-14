using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InfinniPlatform.ReportDesigner.Properties;
using InfinniPlatform.ReportDesigner.Views.Editors;

namespace InfinniPlatform.ReportDesigner.Views.Preview
{
    /// <summary>
    ///     Представление для отображения редактора значения параметра отчета.
    /// </summary>
    sealed partial class ParameterValueEditor : UserControl
    {
        private IDictionary<string, object> _availableValues;
        private readonly EditorBase _editor;

        public ParameterValueEditor(EditorBase editor)
        {
            InitializeComponent();

            EditorPanel.Controls.Add(editor);
            editor.Dock = DockStyle.Fill;

            _editor = editor;
        }

        public string Caption
        {
            get { return CaptionLabel.Text; }
            set { CaptionLabel.Text = value; }
        }

        public bool ShowSelectButton
        {
            get { return SelectPanel.Visible; }
            set { SelectPanel.Visible = value; }
        }

        public bool ShowNullButton
        {
            get { return NullPanel.Visible; }
            set { NullPanel.Visible = value; }
        }

        public bool AllowNullValue
        {
            get { return NullPanel.Enabled; }
            set
            {
                NullPanel.Enabled = value;
                EditorPanel.Enabled = !value || !NullEdit.Checked;
                SelectPanel.Enabled = !value || !NullEdit.Checked;
            }
        }

        /// <summary>
        ///     Список доступных значений.
        /// </summary>
        public IDictionary<string, object> AvailableValues
        {
            get { return _availableValues; }
            set
            {
                var availableValues = new Dictionary<string, object>();

                if (value != null)
                {
                    availableValues = new Dictionary<string, object>();

                    foreach (var item in value)
                    {
                        var castItem = _editor.CastObjectValue(item.Value);

                        if (castItem != null)
                        {
                            availableValues.Add(item.Key, castItem);
                        }
                    }
                }

                _availableValues = availableValues;
            }
        }

        /// <summary>
        ///     Выбранное значение параметра.
        /// </summary>
        public object Value
        {
            get { return (AllowNullValue && NullEdit.Checked) ? null : _editor.Value; }
            set
            {
                if (ChangingValue != null)
                {
                    var args = new SelectParameterValueEventArgs(value);

                    ChangingValue(this, args);

                    if (args.Cancel == false)
                    {
                        _editor.Value = args.Value;
                        _editor.Text = args.Label;
                    }
                }
                else
                {
                    _editor.Value = value;
                }
            }
        }

        /// <summary>
        ///     Событие выбора значения.
        /// </summary>
        public event EventHandler<SelectParameterValueEventArgs> SelectValue;

        /// <summary>
        ///     Событие изменения значения.
        /// </summary>
        public event EventHandler<SelectParameterValueEventArgs> ChangingValue;

        private void OnSelectValue(object sender, EventArgs e)
        {
            if (SelectValue != null)
            {
                var args = new SelectParameterValueEventArgs(Value);

                SelectValue(sender, args);

                if (args.Cancel == false)
                {
                    Value = args.Value;
                }
            }
        }

        private void OnNullValue(object sender, EventArgs e)
        {
            EditorPanel.Enabled = !AllowNullValue || !NullEdit.Checked;
            SelectPanel.Enabled = !AllowNullValue || !NullEdit.Checked;
        }

        public override bool ValidateChildren()
        {
            if (AllowNullValue == false && Value == null)
            {
                Resources.EnterParameterValue.ShowError(Caption);
                _editor.Focus();
                return false;
            }

            return true;
        }
    }
}
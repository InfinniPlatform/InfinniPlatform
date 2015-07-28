using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.ReportDesigner.Views.Preview
{
    /// <summary>
    ///     Представление для отображения редактора параметров отчета.
    /// </summary>
    internal sealed class ParameterListView : UserControl
    {
        private static readonly ParameterValueEditorFactory EditorFactory = new ParameterValueEditorFactory();
        private IEnumerable<ParameterInfo> _parameters;

        public ParameterListView()
        {
            AutoScroll = true;
        }

        /// <summary>
        ///     Список параметров.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        public IEnumerable<ParameterInfo> Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;

                Controls.Clear();

                if (value != null)
                {
                    foreach (var parameterInfo in value)
                    {
                        var parameterEditor = EditorFactory.CreateEditor(parameterInfo);
                        parameterEditor.Dock = DockStyle.Top;
                        Controls.Add(parameterEditor);
                        Controls.SetChildIndex(parameterEditor, 0);
                    }
                }
            }
        }

        /// <summary>
        ///     Задать значения параметров по умолчанию.
        /// </summary>
        public void SetDefaultParameterValues(IDictionary<string, ParameterValues> parameterValues)
        {
            foreach (ParameterValueEditor parameterEditor in Controls)
            {
                ParameterValues values;

                if (parameterValues != null && parameterValues.TryGetValue(parameterEditor.Name, out values) &&
                    values != null)
                {
                    parameterEditor.AvailableValues = values.AvailableValues;
                    parameterEditor.Value = (values.DefaultValues != null) ? values.DefaultValues.Values : null;
                }
                else
                {
                    parameterEditor.AvailableValues = null;
                    parameterEditor.Value = null;
                }
            }
        }

        /// <summary>
        ///     Получить выбранные значения параметров.
        /// </summary>
        public IDictionary<string, object> GetSelectedParameterValues()
        {
            var parameterValues = new Dictionary<string, object>();

            foreach (ParameterValueEditor parameterEditor in Controls)
            {
                parameterValues.Add(parameterEditor.Name, parameterEditor.Value);
            }

            return parameterValues;
        }

        public override bool ValidateChildren()
        {
            foreach (ParameterValueEditor control in Controls)
            {
                if (control.ValidateChildren() == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
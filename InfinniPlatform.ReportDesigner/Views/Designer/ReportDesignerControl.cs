using System;
using System.Windows.Forms;
using InfinniPlatform.FastReport.Serialization;
using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.ReportDesigner.Views.Designer
{
    /// <summary>
    ///     Элемент управления для редактирования отчетов.
    /// </summary>
    public sealed partial class ReportDesignerControl : UserControl
    {
        private dynamic _editValue;
        private bool _initializing;
        private bool _isChanged;

        public ReportDesignerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Редактируемый отчет.
        /// </summary>
        public object EditValue
        {
            get { return GetEditValue(); }
            set
            {
                if (!Equals(_editValue, value))
                {
                    SetEditValue(value);
                }
            }
        }

        private object GetEditValue()
        {
            if (_isChanged && _editValue != null)
            {
                // Формирование метаданных отчета на основе шаблона
                var reportTemplate = Editor.EditValue;
                dynamic reportContent = ReportTemplateSerializer.Instance.SerializeToDynamic(reportTemplate);

                // Копирование информации об отчете из шаблона в метаданные
                if (reportTemplate != null && reportTemplate.Info != null)
                {
                    _editValue.Name = reportTemplate.Info.Name;
                    _editValue.Caption = reportTemplate.Info.Caption;
                    _editValue.Description = reportTemplate.Info.Description;
                }

                _editValue.Content = reportContent;
            }

            return _editValue;
        }

        private void SetEditValue(dynamic value)
        {
            _editValue = value;

            // Формирование шаблона отчета на основе метаданных
            dynamic reportContent = (value != null) ? value.Content : null;
            ReportTemplate reportTemplate = ReportTemplateSerializer.Instance.DeserializeFromDynamic(reportContent);

            // Копирование информации об отчете из метаданных в шаблон
            if (value != null && reportTemplate != null)
            {
                if (reportTemplate.Info == null)
                {
                    reportTemplate.Info = new ReportInfo();
                }

                reportTemplate.Info.Name = value.Name;
                reportTemplate.Info.Caption = value.Caption;
                reportTemplate.Info.Description = value.Description;
            }

            // Отображение шаблона отчета в редакторе
            InitializeEditValue(() =>
            {
                Editor.EditValue = reportTemplate;

                _isChanged = false;
            });
        }

        /// <summary>
        ///     Событие изменения отчета.
        /// </summary>
        public event EventHandler EditValueChanged;

        private void OnEditValueChanged(object sender, EventArgs e)
        {
            InitializeEditValue(() =>
            {
                _isChanged = true;

                var handler = EditValueChanged;

                if (handler != null)
                {
                    handler(sender, e);
                }
            });
        }

        private void InitializeEditValue(Action action)
        {
            if (!_initializing)
            {
                _initializing = true;

                try
                {
                    action();
                }
                finally
                {
                    _initializing = false;
                }
            }
        }
    }
}
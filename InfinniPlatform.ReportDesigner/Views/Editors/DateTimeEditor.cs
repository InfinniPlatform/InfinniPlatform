using System;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Editors
{
    internal sealed class DateTimeEditor : EditorBase
    {
        private readonly DateTimePicker _editControl;

        public DateTimeEditor(string valueFormat = null, DateTime? defaultValue = null)
            : base(typeof (DateTime), valueFormat, defaultValue ?? DateTime.Today)
        {
            _editControl = CreateEdit<DateTimePicker>();
            _editControl.MinDate = DateTime.MinValue;
            _editControl.MaxDate = DateTime.MaxValue;
            _editControl.Format = DateTimePickerFormat.Custom;
            _editControl.CustomFormat = valueFormat;
            _editControl.Value = defaultValue ?? DateTime.Today;
        }

        public override object Value
        {
            get { return _editControl.Value.Date; }
            set { _editControl.Value = (DateTime) CastObjectValue(value); }
        }

        public override string Text
        {
            get { return base.Text; }
            set { }
        }
    }
}
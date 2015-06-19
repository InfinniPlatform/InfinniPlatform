using InfinniPlatform.ReportDesigner.Views.Controls;

namespace InfinniPlatform.ReportDesigner.Views.Editors
{
    internal sealed class IntegerEditor : EditorBase
    {
        private readonly NumericEditor _editControl;

        public IntegerEditor(string valueFormat = null, int defaultValue = 0)
            : base(typeof (int), valueFormat, defaultValue)
        {
            _editControl = CreateEdit<NumericEditor>();
            _editControl.IsRealNumber = false;
        }

        public override object Value
        {
            get { return _editControl.Value; }
            set { _editControl.Value = (int) CastObjectValue(value); }
        }

        public override string Text
        {
            get { return base.Text; }
            set { }
        }
    }
}
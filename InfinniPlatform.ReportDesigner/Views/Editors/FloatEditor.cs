using InfinniPlatform.ReportDesigner.Views.Controls;

namespace InfinniPlatform.ReportDesigner.Views.Editors
{
    internal sealed class FloatEditor : EditorBase
    {
        private readonly NumericEditor _editControl;

        public FloatEditor(string valueFormat = null, double defaultValue = 0)
            : base(typeof (double), valueFormat, defaultValue)
        {
            _editControl = CreateEdit<NumericEditor>();
            _editControl.IsRealNumber = true;
        }

        public override object Value
        {
            get { return _editControl.Value; }
            set { _editControl.Value = (double) CastObjectValue(value); }
        }

        public override string Text
        {
            get { return base.Text; }
            set { }
        }
    }
}
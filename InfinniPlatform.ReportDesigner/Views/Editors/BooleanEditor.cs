using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Editors
{
    internal sealed class BooleanEditor : EditorBase
    {
        private readonly CheckBox _editControl;

        public BooleanEditor(string valueFormat = null, bool defaultValue = false)
            : base(typeof (bool), valueFormat, defaultValue)
        {
            _editControl = CreateEdit<CheckBox>();
            _editControl.Checked = defaultValue;
        }

        public override object Value
        {
            get { return _editControl.Checked; }
            set { _editControl.Checked = (bool) CastObjectValue(value); }
        }

        public override string Text
        {
            get { return base.Text; }
            set { }
        }
    }
}
using System;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Editors
{
    internal sealed class ObjectEditor : EditorBase
    {
        private object _value;

        public ObjectEditor(Type valueType, string valueFormat = null, object defaultValue = null)
            : base(valueType, valueFormat, defaultValue)
        {
            var editControl = CreateEdit<TextBox>();
            editControl.ReadOnly = true;
        }

        public override object Value
        {
            get { return _value; }
            set { _value = CastObjectValue(value); }
        }
    }
}
using System;
using System.Collections;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Editors
{
    internal sealed class ArrayEditor : EditorBase
    {
        private IEnumerable _value;

        public ArrayEditor(Type itemType, string itemFormat = null, object defaultItem = null)
            : base(itemType, itemFormat, defaultItem)
        {
            var editControl = CreateEdit<TextBox>();
            editControl.ReadOnly = true;
        }

        public override object Value
        {
            get { return _value; }
            set { _value = CastArrayValue(value as IEnumerable); }
        }
    }
}
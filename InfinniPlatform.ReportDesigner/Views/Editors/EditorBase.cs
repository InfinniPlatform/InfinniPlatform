using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Editors
{
    internal abstract class EditorBase : Control
    {
        private Control _editor;
        private readonly object _defaultValue;
        private readonly string _valueFormatString;
        private readonly Type _valueType;

        protected EditorBase(Type valueType, string valueFormat = "", object defaultValue = null)
        {
            _valueType = valueType;
            _defaultValue = defaultValue;
            _valueFormatString = string.IsNullOrEmpty(valueFormat) ? "{0}" : "{0:" + valueFormat + "}";
        }

        public abstract object Value { get; set; }

        public override string Text
        {
            get { return _editor.Text; }
            set { _editor.Text = value; }
        }

        protected TEdit CreateEdit<TEdit>() where TEdit : Control, new()
        {
            var edit = new TEdit {Dock = DockStyle.Fill};
            edit.KeyDown += (s, e) => OnKeyDown(e);
            GotFocus += (s, e) => edit.Focus();
            Controls.Add(edit);

            _editor = edit;

            return edit;
        }

        public object CastObjectValue(object value)
        {
            if (value != null)
            {
                try
                {
                    return Convert.ChangeType(value, _valueType);
                }
                catch
                {
                }
            }

            return _defaultValue;
        }

        public IEnumerable CastArrayValue(IEnumerable value)
        {
            if (value != null)
            {
                var castArray = new List<object>();

                foreach (var item in value)
                {
                    var castItem = CastObjectValue(item);

                    castArray.Add(castItem);
                }

                return castArray;
            }

            return null;
        }

        public string FormatObjectValue(object value, bool cast = true)
        {
            var castValue = cast ? CastObjectValue(value) : value;

            if (castValue != null)
            {
                return string.Format(_valueFormatString, castValue);
            }

            return null;
        }

        public string FormatArrayValue(IEnumerable value, bool cast = true)
        {
            var castArray = cast ? CastArrayValue(value) : value;

            if (castArray != null)
            {
                return string.Join("; ", castArray.Cast<object>().Select(i => string.Format(_valueFormatString, i)));
            }

            return null;
        }
    }
}
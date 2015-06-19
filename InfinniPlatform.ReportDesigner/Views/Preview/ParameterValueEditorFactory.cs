using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Views.Editors;

namespace InfinniPlatform.ReportDesigner.Views.Preview
{
    internal sealed class ParameterValueEditorFactory
    {
        private static readonly EditorFactory EditorFactory = new EditorFactory();

        public ParameterValueEditor CreateEditor(ParameterInfo parameterInfo)
        {
            ParameterValueEditor editor;

            // Если значения параметра должны выбираться из предопределенного списка
            if (parameterInfo.AvailableValues != null)
            {
                editor = parameterInfo.AllowMultiplyValues
                    ? EditorWithMultipleSelect(parameterInfo)
                    : EditorWithSingleSelect(parameterInfo);
            }
            // Если значения параметра пользователь вводит самостоятельно
            else
            {
                editor = parameterInfo.AllowMultiplyValues
                    ? EditorWithManualMultipleSelect(parameterInfo)
                    : EditorWithoutSelect(parameterInfo);
            }

            editor.Name = parameterInfo.Name;
            editor.Caption = string.IsNullOrWhiteSpace(parameterInfo.Caption)
                ? parameterInfo.Name
                : parameterInfo.Caption;
            editor.AllowNullValue = parameterInfo.AllowNullValue;

            return editor;
        }

        private static ParameterValueEditor EditorWithSingleSelect(ParameterInfo parameterInfo)
        {
            // Выбор одного значения из предопределенного списка

            var parameterValueControl = EditorFactory.CreateObjectEditor(parameterInfo.Type);
            var selectValueDlg = new DialogView<SelectSingleValueView>();

            var editor = new ParameterValueEditor(parameterValueControl)
            {
                ShowNullButton = false,
                ShowSelectButton = true
            };

            editor.ChangingValue += (sender, args) =>
            {
                // Устанавливаем значение, только если оно есть в списке

                object newValue = null;
                string newLabel = null;

                if (editor.AvailableValues.IsNullOrEmpty() == false)
                {
                    var selectedValue = parameterValueControl.CastObjectValue(ValueOrFirstItem(args.Value));

                    if (selectedValue != null)
                    {
                        foreach (var item in editor.AvailableValues)
                        {
                            if (Equals(item.Value, selectedValue))
                            {
                                newValue = item.Value;
                                newLabel = item.Key;
                                break;
                            }
                        }
                    }
                }

                args.Value = newValue;
                args.Label = newLabel;
            };

            editor.SelectValue += (sender, args) =>
            {
                // Показываем диалог, только если есть доступные значения

                args.Cancel = true;

                if (editor.AvailableValues.IsNullOrEmpty() == false)
                {
                    selectValueDlg.View.AvailableValues = editor.AvailableValues;
                    selectValueDlg.View.SelectedValue = editor.Value;

                    if (selectValueDlg.ShowDialog() == DialogResult.OK)
                    {
                        args.Value = selectValueDlg.View.SelectedValue;
                        args.Cancel = false;
                    }
                }
            };

            return editor;
        }

        private static ParameterValueEditor EditorWithMultipleSelect(ParameterInfo parameterInfo)
        {
            // Выбор нескольких значений из предопределенного списка

            var parameterValueControl = EditorFactory.CreateArrayEditor(parameterInfo.Type);
            var selectValueDlg = new DialogView<SelectMultipleValueView>();

            var editor = new ParameterValueEditor(parameterValueControl)
            {
                ShowNullButton = false,
                ShowSelectButton = true
            };

            editor.ChangingValue += (sender, args) =>
            {
                // Выбираем значения, только если они есть в списке

                object newValue = null;
                string newLabel = null;

                if (editor.AvailableValues.IsNullOrEmpty() == false)
                {
                    var selectedValues = parameterValueControl.CastArrayValue(args.Value as IEnumerable);

                    if (selectedValues != null)
                    {
                        var allowSelectedValues = new List<object>();
                        var allowSelectedLabels = new List<string>();

                        foreach (var value in selectedValues)
                        {
                            foreach (var item in editor.AvailableValues)
                            {
                                if (Equals(item.Value, value))
                                {
                                    allowSelectedValues.Add(item.Value);
                                    allowSelectedLabels.Add(item.Key);
                                    break;
                                }
                            }
                        }

                        if (allowSelectedValues.Count > 0)
                        {
                            newValue = allowSelectedValues;
                            newLabel = parameterValueControl.FormatArrayValue(allowSelectedLabels, false);
                        }
                    }
                }

                args.Value = newValue;
                args.Label = newLabel;
            };

            editor.SelectValue += (sender, args) =>
            {
                // Показываем диалог, только если есть доступные значения

                args.Cancel = true;

                if (editor.AvailableValues.IsNullOrEmpty() == false)
                {
                    selectValueDlg.View.AvailableValues = editor.AvailableValues;
                    selectValueDlg.View.SelectedValues = editor.Value as IEnumerable;

                    if (selectValueDlg.ShowDialog() == DialogResult.OK)
                    {
                        args.Value = selectValueDlg.View.SelectedValues;
                        args.Cancel = false;
                    }
                }
            };

            return editor;
        }

        private static ParameterValueEditor EditorWithManualMultipleSelect(ParameterInfo parameterInfo)
        {
            // Выбор нескольких значений из вручную сформированного списка

            var parameterValueControl = EditorFactory.CreateArrayEditor(parameterInfo.Type);
            var parameterValueEditor = EditorFactory.CreateSimpleEditor(parameterInfo.Type);
            var selectValueView = new SelectMultipleValueView(parameterValueEditor);
            var selectValueDlg = new DialogView<SelectMultipleValueView>(selectValueView);

            var editor = new ParameterValueEditor(parameterValueControl)
            {
                ShowNullButton = false,
                ShowSelectButton = true
            };

            editor.ChangingValue += (sender, args) =>
            {
                args.Value = parameterValueControl.CastArrayValue(args.Value as IEnumerable);
                args.Label = parameterValueControl.FormatArrayValue(args.Value as IEnumerable);
            };

            editor.SelectValue += (sender, args) =>
            {
                args.Cancel = true;

                selectValueDlg.View.SelectedValues = editor.Value as IEnumerable;

                if (selectValueDlg.ShowDialog() == DialogResult.OK)
                {
                    args.Value = selectValueDlg.View.SelectedValues;
                    args.Cancel = false;
                }
            };

            return editor;
        }

        private static ParameterValueEditor EditorWithoutSelect(ParameterInfo parameterInfo)
        {
            // Ручной ввод одного значения

            var parameterValueControl = EditorFactory.CreateSimpleEditor(parameterInfo.Type);

            var editor = new ParameterValueEditor(parameterValueControl)
            {
                ShowNullButton = parameterInfo.AllowNullValue,
                ShowSelectButton = false
            };

            editor.ChangingValue += (sender, args) => { args.Value = ValueOrFirstItem(args.Value); };

            return editor;
        }

        private static object ValueOrFirstItem(object value)
        {
            if (!(value is string))
            {
                var collection = value as IEnumerable;

                if (collection != null)
                {
                    var enumerator = collection.GetEnumerator();

                    return enumerator.MoveNext() ? enumerator.Current : null;
                }
            }

            return value;
        }
    }
}
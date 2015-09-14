using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace InfinniPlatform.DesignControls.PropertyEditors
{
    public sealed class ValueListEditor : IPropertyEditor
    {
        private readonly IEnumerable<string> _items;
        private readonly RepositoryItemLookUpEdit _repositoryItem = new RepositoryItemLookUpEdit();

        public ValueListEditor(IEnumerable<string> items)
        {
            _items = items;
        }

        public RepositoryItem GetRepositoryItem(object value)
        {
            _repositoryItem.NullText = "";
            _repositoryItem.TextEditStyle = TextEditStyles.DisableTextEditor;

            _repositoryItem.Buttons.Clear();
            var comboButton = new EditorButton(ButtonPredefines.Combo);
            _repositoryItem.Buttons.Add(comboButton);
            comboButton.IsLeft = true;

            var clearButton = new EditorButton(ButtonPredefines.Delete);
            _repositoryItem.Buttons.Add(clearButton);

            _repositoryItem.ButtonClick += RepositoryItemOnButtonClick;

            _repositoryItem.DisplayMember = "Value";
            _repositoryItem.ValueMember = "Value";

            var list = new List<ValueObject>();
            foreach (var item in _items)
            {
                list.Add(new ValueObject
                {
                    Value = item
                });
            }

            _repositoryItem.DataSource = list;

            return _repositoryItem;
        }

        public Func<string, dynamic> ItemPropertyFunc { get; set; }

        private void RepositoryItemOnButtonClick(object sender, ButtonPressedEventArgs buttonPressedEventArgs)
        {
            if (buttonPressedEventArgs.Button.Kind == ButtonPredefines.Combo)
            {
                ((LookUpEdit) sender).ShowPopup();
            }
            else
            {
                ((LookUpEdit) sender).EditValue = null;
            }
        }

        public sealed class ValueObject
        {
            public string Value { get; set; }
        }
    }

    public static class ValueListEditorExtensions
    {
        public static ValueListEditor CreateViewTypeEditor()
        {
            return new ValueListEditor(new[]
            {
                "ListView",
                "EditView",
                "SelectView"
            });
        }

        public static ValueListEditor CreateOpenModeEditor()
        {
            return new ValueListEditor(new[]
            {
                "Page",
                "Application",
                "Dialog"
            });
        }
    }
}
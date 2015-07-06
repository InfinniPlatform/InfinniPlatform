using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.DesignControls.PropertyEditors
{
    public sealed class CriteriaTypeEditor : IPropertyEditor
    {
        private readonly RepositoryItemLookUpEdit _repositoryItem = new RepositoryItemLookUpEdit();

        public RepositoryItem GetRepositoryItem(object value1)
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

            _repositoryItem.ValueMember = "Value";
            _repositoryItem.DisplayMember = "Value";


            var dataSource = new List<CriteriaTypeValue>();
            foreach (var enumValue in Enum.GetValues(typeof (CriteriaType)))
            {
                if ((int) enumValue == (int) CriteriaType.Script || (int) enumValue == (int) CriteriaType.ValueSet)
                {
                    continue;
                }

                var value = new CriteriaTypeValue();
                //печаль
                value.Value = Enum.GetName(typeof (CriteriaType), enumValue); //(int) enumValue;
                dataSource.Add(value);
            }

            _repositoryItem.DataSource = dataSource;

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

        public sealed class CriteriaTypeValue
        {
            public string Value { get; set; }
        }
    }
}
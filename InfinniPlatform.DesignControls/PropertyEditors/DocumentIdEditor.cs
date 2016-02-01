using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace InfinniPlatform.DesignControls.PropertyEditors
{
    public sealed class DocumentIdEditor : IPropertyEditor
    {
        private dynamic _selectedValue;
        private readonly RepositoryItemLookUpEdit _repositoryItem = new RepositoryItemLookUpEdit();

        public RepositoryItem GetRepositoryItem(object value)
        {
            _repositoryItem.NullText = "";
            _repositoryItem.TextEditStyle = TextEditStyles.DisableTextEditor;

            _repositoryItem.Buttons.Clear();
            var comboButton = new EditorButton(ButtonPredefines.Combo);
            _repositoryItem.Buttons.Add(comboButton);

            var clearButton = new EditorButton(ButtonPredefines.Delete);
            _repositoryItem.Buttons.Add(clearButton);

            _repositoryItem.ButtonClick += RepositoryItemOnButtonClick;
            _repositoryItem.QueryPopUp += RepositoryItemOnQueryPopUp;
            _repositoryItem.QueryCloseUp += RepositoryItemOnQueryCloseUp;

            _repositoryItem.ValueMember = "DocumentId";
            _repositoryItem.DisplayMember = "DocumentId";

            _selectedValue = value;


            if (value != null)
            {
                _repositoryItem.DataSource = new List<DocumentIdValue>
                {
                    new DocumentIdValue
                    {
                        DocumentId = ((dynamic) value).Value.ToString()
                    }
                };
            }


            return _repositoryItem;
        }

        public Func<string, dynamic> ItemPropertyFunc { get; set; }

        private void RepositoryItemOnQueryCloseUp(object sender, CancelEventArgs cancelEventArgs)
        {
        }

        private void RepositoryItemOnQueryPopUp(object sender, CancelEventArgs cancelEventArgs)
        {
            IEnumerable<dynamic> items = GetDocumentList(ItemPropertyFunc("ConfigId"));
            var prop = ((LookUpEdit) sender).Properties;

            var listItems = items.Select(c => new DocumentIdValue
            {
                DocumentId = c
            }).ToList();

            prop.DataSource = listItems;

            if (_selectedValue != null &&
                listItems.FirstOrDefault(d => d.DocumentId == _selectedValue.Value.ToString()) == null)
            {
                listItems.Add(new DocumentIdValue
                {
                    DocumentId = _selectedValue.Value.ToString()
                });
            }

            _repositoryItem.DataSource = listItems;
        }

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

        private IEnumerable<dynamic> GetDocumentList(string configId)
        {
            if (!string.IsNullOrEmpty(configId))
            {
                return Enumerable.Empty<object>();
            }
            return new List<string>();
        }

        public sealed class DocumentIdValue
        {
            public string DocumentId { get; set; }
        }
    }
}
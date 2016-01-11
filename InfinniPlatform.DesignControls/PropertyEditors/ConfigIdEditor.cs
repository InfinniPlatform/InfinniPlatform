using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

using InfinniPlatform.Core.RestApi.DataApi;

namespace InfinniPlatform.DesignControls.PropertyEditors
{
    public sealed class ConfigIdEditor : IPropertyEditor
    {
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
            _repositoryItem.TextEditStyle = TextEditStyles.Standard;
            _repositoryItem.SearchMode = SearchMode.OnlyInPopup;


            _repositoryItem.ProcessNewValue += (sender, args) =>
            {
                var list = (_repositoryItem.DataSource as IEnumerable).Cast<ConfigIdValue>().ToList();
                list.Add(new ConfigIdValue
                {
                    ConfigId = args.DisplayValue.ToString()
                });
                _repositoryItem.DataSource = list;
            };

            _repositoryItem.DisplayMember = "ConfigId";
            _repositoryItem.ValueMember = "ConfigId";

            var dataSource = GetConfigList().Select(c => new ConfigIdValue
            {
                ConfigId = c
            }).ToList();

            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                var valueItem = dataSource.FirstOrDefault(f => f.ConfigId == ((dynamic) value).Value.ToString());
                if (valueItem == null)
                {
                    dataSource.Add(new ConfigIdValue
                    {
                        ConfigId = ((dynamic) value).Value.ToString()
                    });
                }
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

        private IEnumerable<dynamic> GetConfigList()
        {
            return Enumerable.Empty<object>();
        }

        public sealed class ConfigIdValue
        {
            public string ConfigId { get; set; }
        }
    }
}
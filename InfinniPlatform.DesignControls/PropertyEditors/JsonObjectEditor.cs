using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using InfinniPlatform.DesignControls.ScriptEditor;

namespace InfinniPlatform.DesignControls.PropertyEditors
{
    /// <summary>
    ///     Кастомный редактор для редактирования свойства в виде JSON-объекта
    /// </summary>
    public sealed class JsonObjectEditor : IPropertyEditor
    {
        private readonly RepositoryItemButtonEdit _repositoryItem = new RepositoryItemButtonEdit();

        public RepositoryItem GetRepositoryItem(object value)
        {
            _repositoryItem.ButtonClick += RepositoryItemOnButtonClick;
            _repositoryItem.TextEditStyle = TextEditStyles.DisableTextEditor;
            return _repositoryItem;
        }

        public Func<string, dynamic> ItemPropertyFunc { get; set; }

        private void RepositoryItemOnButtonClick(object sender, ButtonPressedEventArgs buttonPressedEventArgs)
        {
            if (buttonPressedEventArgs.Button.Kind == ButtonPredefines.Ellipsis)
            {
                var scriptForm = new ScriptEditForm();
                scriptForm.Script = ((BaseEdit) sender).EditValue != null
                    ? ((BaseEdit) sender).EditValue.ToString()
                    : "";
                if (scriptForm.ShowDialog() == DialogResult.OK)
                {
                    ((BaseEdit) sender).EditValue = scriptForm.Script;
                }
            }
        }
    }
}
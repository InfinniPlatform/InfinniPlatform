using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using InfinniPlatform.DesignControls.ObjectInspector;

namespace InfinniPlatform.DesignControls.PropertyEditors
{
	/// <summary>
	///   Редактор для выбора зарегистрированного скрипта
	/// </summary>
	public sealed class ScriptIdEditor : IPropertyEditor
	{
		public sealed class ScriptIdValue
		{
			public string ScriptId { get; set; }
		}


		private readonly RepositoryItemLookUpEdit _repositoryItem = new RepositoryItemLookUpEdit();
		private readonly ObjectInspectorTree _inspector;

		public ScriptIdEditor(ObjectInspectorTree inspector)
		{
			_inspector = inspector;
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

			_repositoryItem.DisplayMember = "ScriptId";
			_repositoryItem.ValueMember = "ScriptId";

			_repositoryItem.DataSource = _inspector.Scripts.Select(c => new ScriptIdValue()
			{
				ScriptId = c.ScriptSourceName
			});

			return _repositoryItem;
		}

		public Func<string, dynamic> ItemPropertyFunc { get; set; }


		private void RepositoryItemOnButtonClick(object sender, ButtonPressedEventArgs buttonPressedEventArgs)
		{
			if (buttonPressedEventArgs.Button.Kind == ButtonPredefines.Combo)
			{

				((LookUpEdit)sender).ShowPopup();
			}
			else
			{
				((LookUpEdit)sender).EditValue = null;
			}
		}
	}
}

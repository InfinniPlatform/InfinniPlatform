using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using InfinniPlatform.DesignControls.ObjectInspector;

namespace InfinniPlatform.DesignControls.PropertyEditors
{
	public sealed class ControlTypeEditor : IPropertyEditor
	{
		public sealed class ControlTypeValue
		{
			public string Name { get; set; }
		}

		private readonly RepositoryItemLookUpEdit _repositoryItem = new RepositoryItemLookUpEdit();

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
			
			_repositoryItem.ValueMember = "Name";
			_repositoryItem.DisplayMember = "Name";

			_repositoryItem.DataSource = ControlRepository.Instance.GetDataControls.Select(c => new ControlTypeValue()
			{
				Name = c.Item1
			}).ToList(); ;

			return _repositoryItem;

		}


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

		public Func<string, dynamic> ItemPropertyFunc { get; set; }
	}
}

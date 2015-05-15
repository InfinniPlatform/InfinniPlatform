using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace InfinniPlatform.DesignControls.PropertyEditors
{
	public sealed class AlignmentEditor : IPropertyEditor
	{
		public sealed class AlignmentValue
		{
			public string Value { get; set; }
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

			_repositoryItem.DisplayMember = "Value";
			_repositoryItem.ValueMember = "Value";

			_repositoryItem.DataSource = new List<AlignmentValue>()
				                             {
					                             new AlignmentValue()
						                             {
							                             Value = "Stretch",
						                             },
												 new AlignmentValue()
						                             {
							                             Value = "Center",
						                             },
												 new AlignmentValue()
						                             {
							                             Value = "Left",
						                             },
												 new AlignmentValue()
						                             {
							                             Value = "Right",
						                             }
				                             };

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

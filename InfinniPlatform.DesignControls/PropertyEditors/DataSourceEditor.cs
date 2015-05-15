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
	/// <summary>
	///   Редактор источника данных
	/// </summary>
	public sealed class DataSourceEditor : IPropertyEditor
	{
		private readonly ObjectInspectorTree _inspector;

		public sealed class DataSourceValue
		{
			public string DataSource { get; set; }
		}


		public DataSourceEditor(ObjectInspectorTree inspector)
		{
			_inspector = inspector;
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

			_repositoryItem.ValueMember = "DataSource";
			_repositoryItem.DisplayMember = "DataSource";

			IEnumerable<dynamic> items = _inspector.DataSources.Select(i => i.DataSourceName).ToList();
			var listItems = items.Select(c => new DataSourceValue()
			{
				DataSource = c
			}).ToList();

			_repositoryItem.DataSource = listItems;

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

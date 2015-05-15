using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using InfinniPlatform.DesignControls.PropertyEditors;

namespace InfinniPlatform.DesignControls.PropertyDesigner
{
	public sealed class EditorRepository
	{
		private readonly dynamic _inspectedItem;
		private readonly Dictionary<string,Func<IPropertyEditor>> _editors = new Dictionary<string, Func<IPropertyEditor>>();

		public EditorRepository(Func<string,dynamic> inspectedItem)
		{
			_inspectedItem = inspectedItem;
		}

		public Dictionary<string, Func<IPropertyEditor>> Editors
		{
			get { return _editors; }
		}

		public RepositoryItem GetRepositoryItem(string propertyName, object value)
		{
			if (Editors.ContainsKey(propertyName))
			{
				var editor = Editors[propertyName]();
				editor.ItemPropertyFunc = _inspectedItem;
				return editor.GetRepositoryItem(value);
			}
			return null;
		}

		public void RegisterEditor(string propertyName, Func<IPropertyEditor> propertyEditor)
		{
			Editors.Add(propertyName, propertyEditor);
		}


	}
}

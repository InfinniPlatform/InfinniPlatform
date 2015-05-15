using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors.Repository;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.PropertyEditors
{
	public interface IPropertyEditor
	{
		RepositoryItem GetRepositoryItem(object value);

		Func<string, dynamic> ItemPropertyFunc { get; set; }
	}
}

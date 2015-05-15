using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InfinniPlatform.DesignControls.Layout
{
	public interface ILayoutProvider
	{
		dynamic GetLayout();

	    void SetLayout(dynamic value);

		string GetPropertyName();

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.DesignControls.ObjectInspector
{
	public interface IInspectedItem
	{
		ObjectInspectorTree ObjectInspector { get; set; }
	}
}

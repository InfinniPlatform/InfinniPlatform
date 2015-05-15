using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.DesignControls.ObjectInspector
{
	public sealed class FocusedNodeMemento
	{
		private readonly ObjectInspectorTree _inspector;
		private PropertiesNode _state;

		public FocusedNodeMemento(ObjectInspectorTree inspector)
		{
			_inspector = inspector;
		}

		public void BeginUpdate()
		{
			_state = _inspector.FocusedPropertiesNode;
		}

		public void EndUpdate()
		{
			_inspector.FocusedPropertiesNode = _state;
		}

	}
}

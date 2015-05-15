using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.DesignControls.Controls;

namespace InfinniPlatform.DesignControls.ObjectInspector
{
	public interface IControlHost
	{
		CompositPanel GetHost();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.Controls.Alignment
{
    public class ResizerFixed
    {
        private readonly PropertiesControl _control;

        public ResizerFixed(PropertiesControl control)
        {
            _control = control;
        }

        public int GetSize()
        {
            return (_control as IClientHeightProvider).GetClientHeight();
        }

        public PropertiesControl GetControl()
        {
            return _control;
        }
    }
}

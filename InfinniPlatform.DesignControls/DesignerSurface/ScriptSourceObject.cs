using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.DesignerSurface
{
    public sealed class ScriptSourceObject : ILayoutProvider
    {
        public string ScriptSourceName
        {
            get { return ScriptSource != null ? ObjectHelper.GetProperty(ScriptSource, "Name") : string.Empty; }
        }

        public dynamic ScriptSource { get; set; }

        public string ScriptSourceString
        {
            get { return ScriptSource != null ? ScriptSource.ToString() : string.Empty; }
        }

        public dynamic GetLayout()
        {
            var layoutProvider = ScriptSource as ILayoutProvider;
            if (layoutProvider != null)
            {
                return layoutProvider.GetLayout();
            }
            return new DynamicWrapper();
        }

        public void SetLayout(dynamic value)
        {
            //no layout
        }

        public string GetPropertyName()
        {
            var layoutProvider = ScriptSource as ILayoutProvider;
            if (layoutProvider != null)
            {
                return layoutProvider.GetPropertyName();
            }
            return string.Empty;
        }

    }
}

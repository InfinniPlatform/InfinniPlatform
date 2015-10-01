using System.Collections.Generic;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.Controls.Properties
{
    public static class LayoutPanel
    {
        public static Dictionary<string, IControlProperty> GetLayoutPanels()
        {
            return new Dictionary<string, IControlProperty>
            {
                {
                    "TabPanel", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {"Image", new SimpleProperty(null)},
                        {"Collapsible", new SimpleProperty(false)},
                        {"Collapsed", new SimpleProperty(false)}
                    }.InheritBaseElementSimpleProperties(),
                        new Dictionary<string, CollectionProperty>())
                },
                {
                    "GridPanel", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {"Image", new SimpleProperty(null)},
                        {"Collapsible", new SimpleProperty(false)},
                        {"Collapsed", new SimpleProperty(false)}
                    }.InheritBaseElementSimpleProperties(),
                        new Dictionary<string, CollectionProperty>())
                }
            };
        }
    }
}
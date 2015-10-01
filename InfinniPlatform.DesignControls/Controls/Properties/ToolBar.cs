using System.Collections.Generic;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.Controls.Properties
{
    public static class ToolBar
    {
        public static Dictionary<string, CollectionProperty> InheritToolBarButtonCollectionProperties(
            this Dictionary<string, CollectionProperty> properties, int recursiveIndex = 0)
        {
            if (recursiveIndex > 3)
            {
                return new Dictionary<string, CollectionProperty>();
            }
            recursiveIndex ++;


            properties.Add(
                "Items", new CollectionProperty(new Dictionary<string, IControlProperty>
                {
                    {
                        "ToolBarButton", new ObjectProperty(
                            new Dictionary<string, IControlProperty>
                            {
                                {
                                    "Image", new SimpleProperty(null)
                                },
                                {
                                    "OnClick",
                                    new ObjectProperty(new Dictionary<string, IControlProperty>
                                    {
                                        {
                                            "Name", new SimpleProperty(string.Empty)
                                        }
                                    }, new Dictionary<string, CollectionProperty>())
                                }
                            }
                                .InheritBaseElementSimpleProperties()
                                .GetActionProperties(),
                            new Dictionary<string, CollectionProperty>())
                    },
                    {
                        "ToolBarPopupButton", new ObjectProperty(
                            new Dictionary<string, IControlProperty>().InheritBaseElementSimpleProperties(),
                            new Dictionary<string, CollectionProperty>()
                                .InheritToolBarButtonCollectionProperties(recursiveIndex))
                    },
                    {
                        "ToolBarSeparator", new ObjectProperty(
                            new Dictionary<string, IControlProperty>().InheritBaseElementSimpleProperties(),
                            new Dictionary<string, CollectionProperty>())
                    }
                }));

            return properties;
        }
    }
}